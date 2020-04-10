import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { environment as env } from 'src/environments/environment';
import { Line } from '../model/Line';
import { Point } from '../model/Point';
import { Subject, BehaviorSubject } from 'rxjs';
import { Message } from '../model/Message';


@Injectable({
	providedIn: 'root'
})
export class GameService {

	private activePlayersSubject = new Subject<string[]>();
	public activePlayers$ = this.activePlayersSubject.asObservable();

	private playerNameSubject = new Subject<string>();
	public playerName$ = this.playerNameSubject.asObservable();

	private receiveLineSubject = new Subject<Line>();
	public receiveLine$ = this.receiveLineSubject.asObservable();

	private clearSubject = new Subject();
	public clear$ = this.clearSubject.asObservable();

	private fillSubject = new Subject<string>();
	public fill$ = this.fillSubject.asObservable();

	private chatMessageSubject = new Subject<Message>();
	public chatMessage$ = this.chatMessageSubject.asObservable();

	private roundFinishedSubject = new Subject<string>();
	public roundFinished$ = this.roundFinishedSubject.asObservable();

	private chooseWordsSubject = new Subject<string[]>();
	public chooseWords$ = this.chooseWordsSubject.asObservable();

	private playerJoinedSubject = new Subject<string>();
	public playerJoined$ = this.playerJoinedSubject.asObservable();

	private playerLeftSubject = new Subject<string>();
	public playerLeft$ = this.playerLeftSubject.asObservable();

	private newRoundSubject = new Subject<{wordCount: number, duration: number}>();
	public newRound$ = this.newRoundSubject.asObservable();


	private connection: signalR.HubConnection;

	constructor() {
		this.connection = new signalR.HubConnectionBuilder().withUrl(env.url + 'hubs/game').build();


		this.connection.on('receiveLine', (line: Line) => {
			this.receiveLineSubject.next(line);
		});


		this.connection.on('receiveClearCanvas', () => {
			this.clearSubject.next();
		});


		this.connection.on('receiveFillCanvas', color => {
			this.fillSubject.next(color);
		});


		this.connection.on('roundFinished', (word: string) => {
			this.roundFinishedSubject.next(word);
		});


		this.connection.on('startChoosing', (words: string[]) => {
			this.chooseWordsSubject.next(words);
		});


		this.connection.on('receiveChatMessage', (name: string, message: string) => {
			this.chatMessageSubject.next({
				sender: name,
				content: message
			});
		});


		this.connection.on('receivePlayerJoined', (playerName: string) => {
			this.playerJoinedSubject.next(playerName);
		});


		this.connection.on('receivePlayerLeft', (playerName: string) => {
			this.playerLeftSubject.next(playerName);
		});


		this.connection.on('newRoundIsStarted', (wordCount: number, duration: number) => {
			this.newRoundSubject.next({ wordCount, duration });
		});
	}


	public async startGame(name: string) {
		await this.connection.start();
		const players = await this.connection.invoke<string[]>('joinGame', name);
		this.activePlayersSubject.next(players);
		this.playerNameSubject.next(name);
	}


	public async sendLine(from: Point, to: Point, color: string, width: number) {
		const line: Line = {
			from,
			to,
			color,
			width
		};

		return this.connection.invoke('sendLine', line);
	}

	public async sendClear() {
		return this.connection.invoke('clearCanvas');
	}


	public async sendFill(color: string) {
		return this.connection.invoke('sendFillCanvas', color);
	}


	public async sendChatMessage(message: string) {
		return this.connection.invoke('SendChatMessage', message);
	}

	public async chooseWord(word: string) {
		return this.connection.invoke('chooseWord', word);
	}


	public setPlayerName(name: string) {
		this.playerNameSubject.next(name);
	}
}
