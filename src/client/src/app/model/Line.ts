import { Point } from './Point';

export interface Line {
	from: Point;
	to: Point;
	color: string;
	width: number;
}
