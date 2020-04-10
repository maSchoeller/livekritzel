import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Message, MessageType } from '../model/Message';
import { GameService } from '../services/game.service';

@Component({
	selector: 'app-chat-component',
	templateUrl: './chat-component.component.html',
	styleUrls: ['./chat-component.component.scss']
})
export class ChatComponentComponent implements OnInit {

	MessageType = MessageType;

	chat: Message[] = [];
	playerName: string;

	@ViewChild('chatContainer')
	chatContainer: ElementRef;

	constructor(private game: GameService) {
		this.game.playerName$ .subscribe((n) => {
			this.playerName = n;
		});
	}

	ngOnInit(): void {
		this.game.chatMessage$.subscribe(m => {
			this.addMessage(m);
		});

		this.game.playerGuessedWord$.subscribe(playerName => {
			this.addMessage({
				content: `${playerName} guessed the word`,
				type: MessageType.GuessedMessage
			});
		});

		this.game.playerJoined$.subscribe(playerName => {
			this.addMessage({
				type: MessageType.GameMessage,
				content: `${playerName} joined the game`
			});
		});

		this.game.playerLeft$.subscribe(playerName => {
			this.addMessage({
				type: MessageType.GameMessage,
				content: `${playerName} left the game`
			});
		});
	}


	addMessage(message: Message) {
		this.chat.push(message);
		setTimeout(() => this.chatContainer.nativeElement.scrollTo(0, this.chatContainer.nativeElement.scrollHeight), 0);
	}


	async sendMessage(input: HTMLInputElement) {
		this.addMessage({
			sender: this.playerName,
			content: input.value,
			type: MessageType.ChatMessage
		});
		await this.game.sendChatMessage(input.value);
		input.value = '';
	}
}
