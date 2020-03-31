import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { environment as env } from 'src/environments/environment';
import { Line } from '../model/Line';
import { Point } from '../model/Point';
import { Subject } from 'rxjs';


@Injectable({
	providedIn: 'root'
})
export class GameService {

	private receiveLineSubject = new Subject<Line>();
	public receiveLine$ = this.receiveLineSubject.asObservable();

	private receiveClearSubject = new Subject();
	public receiveClear$ = this.receiveClearSubject.asObservable();


	private connection: signalR.HubConnection;

	constructor() {
		this.connection = new signalR.HubConnectionBuilder().withUrl(env.url + 'hubs/game').build();

		this.connection.on('receiveLine', (line: Line) => {
			this.receiveLineSubject.next(line);
		});

		this.connection.on('clearCanvas', () => {
			this.receiveClearSubject.next();
		});


		this.startConnection();
	}


	public async startConnection() {
		await this.connection.start();
		await this.connection.invoke('joinGame', 'Mario');
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
}
