import { Component, OnInit } from '@angular/core';
import { GameService } from '../services/game.service';
import { Observable } from 'rxjs';

@Component({
	selector: 'app-player-list',
	templateUrl: './player-list.component.html',
	styleUrls: ['./player-list.component.scss']
})
export class PlayerListComponent implements OnInit {

	players: string[] = [];
	playerName: Observable<string>;

	constructor(private game: GameService) { }

	ngOnInit(): void {
		this.game.playerJoined$.subscribe(player => {
			this.players.push(player);
		});

		this.game.playerLeft$.subscribe(player => {
			this.players.splice(this.players.findIndex(p => p === player), 1);
		});

		this.playerName = this.game.playerName$;
	}
}
