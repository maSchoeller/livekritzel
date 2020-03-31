import { Component, OnInit, ElementRef, ViewChild, Input, AfterViewInit } from '@angular/core';
import { fromEvent } from 'rxjs';
import { switchMap, takeUntil, pairwise } from 'rxjs/operators';

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
export class DrawingBoardComponent implements AfterViewInit {

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
	private _selectedColor: Color;
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


	constructor() { }


	ngAfterViewInit() {
		const canvasEl: HTMLCanvasElement = this.canvas.nativeElement;
		this.cx = canvasEl.getContext('2d');

		this.cx.lineWidth = this.brushSize;
		this.cx.lineCap = 'round';
		this.cx.strokeStyle = this.selectedColor?.value ?? 'black';

		this.captureEvents(canvasEl);
		this.selectedColor = this.colors[0];
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
				this.drawOnCanvas(prevPos, currentPos);
			});

	}

	private drawOnCanvas(prevPos: Point, currentPos: { x: number, y: number }) {
		if (!this.cx) { return; }

		this.cx.beginPath();

		if (prevPos) {
			this.cx.moveTo(prevPos.x, prevPos.y); // from
			this.cx.lineTo(currentPos.x, currentPos.y);
			this.cx.stroke();
		}
	}

	public fill() {
		this.cx.fillStyle = this.selectedColor.value;
		this.cx.fillRect(0, 0, this.canvas.nativeElement.width, this.canvas.nativeElement.height);
	}

	public reset() {
		this.cx.clearRect(0, 0, this.canvas.nativeElement.width, this.canvas.nativeElement.height);
	}

}
