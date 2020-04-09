import { Component, OnInit } from '@angular/core';
import { GameService } from './services/game.service';
import swal from 'sweetalert2';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
	title = 'LiveKritzel';

	constructor(private game: GameService) {
	}


	async ngOnInit() {

		let name;

		do {
			const { value } = await swal.fire({
				title: 'Enter your name',
				input: 'text',
				allowOutsideClick: false
			});

			name = value;

		} while (!name);

		await this.game.startGame(name);

	}
}
