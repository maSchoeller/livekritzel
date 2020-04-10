export interface Message {
	sender?: string;
	content: string;
	type: MessageType;
}

export enum MessageType { ChatMessage, GameMessage, GuessedMessage }
