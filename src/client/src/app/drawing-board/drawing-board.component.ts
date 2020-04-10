import { Component, OnInit, ElementRef, ViewChild, Input, AfterViewInit } from '@angular/core';
import { fromEvent } from 'rxjs';
import { switchMap, takeUntil, pairwise, takeWhile } from 'rxjs/operators';
import { GameService } from '../services/game.service';
import { isDevMode } from '@angular/core';

interface Color {
	name: string;
	value: string;
}

interface Point {
	x: number;
	y: number;
}

@Component({
	selector: 'app-drawing-board',
	templateUrl: './drawing-board.component.html',
	styleUrls: ['./drawing-board.component.scss']
})
export class DrawingBoardComponent implements OnInit, AfterViewInit {

	isDev: boolean = isDevMode();

	canDraw = false;
	isChoosing = false;

	colors: Color[] = [
		{ name: 'black', value: 'rgb(0, 0, 0)' },
		{ name: 'white', value: 'rgb(255, 255, 255)' },
		{ name: 'red', value: 'rgb(255, 0, 0)' },
		{ name: 'orange', value: 'rgb(255, 150, 0)' },
		{ name: 'yellow', value: 'rgb(255, 255, 0)' },
		{ name: 'green', value: 'rgb(0, 255, 0)' },
		{ name: 'turquoise', value: 'rgb(0, 200, 255)' },
		{ name: 'blue', value: 'rgb(0, 0, 255)' },
		{ name: 'purple', value: 'rgb(255, 0, 255)' },
	];

	words: string[] = [
		'Balloon',
		'Gameboy',
		'Watermelon'
	];
	chosenWord: string;

	guessWord: string;

	private _selectedColor: Color = this.colors[0];
	public get selectedColor() {
		return this._selectedColor;
	}
	public set selectedColor(color: Color) {
		this._selectedColor = color;
		this.cx.strokeStyle = color.value;
	}


	readonly maxBrushSize = 42;
	readonly brushStep = 8;
	private _brushSize = 2;
	public get brushSize() {
		return this._brushSize;
	}
	public set brushSize(value: number) {
		this._brushSize = value;
		this.cx.lineWidth = value;
	}



	@ViewChild('canvas') public canvas: ElementRef;
	private cx: CanvasRenderingContext2D;


	constructor(private game: GameService) {
		this.game.receiveLine$.subscribe(line => {
			this.drawOnCanvas(line.from, line.to, line.color, line.width);
		});

		this.game.clear$.subscribe(() => {
			this.cx.clearRect(0, 0, this.canvas.nativeElement.width, this.canvas.nativeElement.height);
		});

		this.game.fill$.subscribe(color => {
			this.cx.fillStyle = color;
			this.cx.fillRect(0, 0, this.canvas.nativeElement.width, this.canvas.nativeElement.height);
		});

		this.game.newRound$.subscribe(({word, duration}) => {
			const mutedWord = word.replace(/\S/g, '_');
			if (!this.canDraw) {
				this.chosenWord = undefined;
			}
			this.guessWord = mutedWord;
			this.clear();
		});

		this.game.roundFinished$.subscribe(word => {
			this.canDraw = false;
		});
		this.game.chooseWords$.subscribe((words) => {
			this.words = words;
			this.isChoosing = true;
		});
	}


	ngOnInit(): void { }


	ngAfterViewInit() {
		const canvasEl: HTMLCanvasElement = this.canvas.nativeElement;
		this.cx = canvasEl.getContext('2d');

		this.cx.lineCap = 'round';

		this.captureEvents(canvasEl);
	}

	selectColor(color: Color) {
		this.selectedColor = color;
	}


	incrementBrushSize() {
		if (this.brushSize + this.brushStep <= this.maxBrushSize) {
			this.brushSize += this.brushStep;
		} else {
			this.brushSize = this.maxBrushSize;
		}
	}
	decrementBrushSize() {
		if (this.brushSize - this.brushStep > 0) {
			this.brushSize -= this.brushStep;
		} else {
			this.brushSize = 2;
		}
	}


	private captureEvents(canvasEl: HTMLCanvasElement) {
		// this will capture all mousedown events from the canvas element
		fromEvent(canvasEl, 'mousedown')
			.pipe(
				switchMap((e) => {
					// after a mouse down, we'll record all mouse moves
					return fromEvent(canvasEl, 'mousemove')
						.pipe(
							// we'll stop (and unsubscribe) once the user releases the mouse
							// this will trigger a 'mouseup' event
							takeUntil(fromEvent(canvasEl, 'mouseup')),
							// we'll also stop (and unsubscribe) once the mouse leaves the canvas (mouseleave event)
							// takeUntil(fromEvent(canvasEl, 'mouseleave')),
							// pairwise lets us get the previous value to draw a line from
							// the previous point to the current point
							pairwise()
						);
				})
			)
			.subscribe((res: [MouseEvent, MouseEvent]) => {
				if (this.canDraw) {
					const rect = canvasEl.getBoundingClientRect();

					// previous and current position with the offset
					const prevPos = {
						x: res[0].clientX - rect.left,
						y: res[0].clientY - rect.top
					};

					const currentPos = {
						x: res[1].clientX - rect.left,
						y: res[1].clientY - rect.top
					};

					// this method we'll implement soon to do the actual drawing
					this.drawOnCanvas(prevPos, currentPos, this.selectedColor.value, this.brushSize);
					this.game.sendLine({ x: prevPos.x, y: prevPos.y }, { x: currentPos.x, y: currentPos.y }, this.selectedColor.value, this.brushSize);
				}
			});

	}

	private drawOnCanvas(prevPos: Point, currentPos: Point, color: string, width: number) {
		if (!this.cx) { return; }


		this.cx.beginPath();

		this.cx.lineWidth = width;
		this.cx.strokeStyle = color;

		if (prevPos) {
			this.cx.moveTo(prevPos.x, prevPos.y); // from
			this.cx.lineTo(currentPos.x, currentPos.y);
			this.cx.stroke();
		}
	}

	public async fill() {
		this.cx.fillStyle = this.selectedColor.value;
		this.cx.fillRect(0, 0, this.canvas.nativeElement.width, this.canvas.nativeElement.height);
		await this.game.sendFill(this.selectedColor.value);
	}

	public async clear() {
		this.cx.clearRect(0, 0, this.canvas.nativeElement.width, this.canvas.nativeElement.height);
		await this.game.sendClear();
	}


	public async chooseWord(word: string) {
		await this.game.chooseWord(word);
		console.log('chose word: ', word);
		this.isChoosing = false;
		this.canDraw = true;
		this.chosenWord = word;
	}

}
