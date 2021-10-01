import { accountTypes } from 'utils/constants';

export type Rs3Skill = {
	[index: string]: number | undefined;
	skillId: number;
	xp: number;
	level: number;
	levelGain: number;
	rank: number;
	dayGain: number;
	weekGain: number;
	monthGain: number;
	yearGain: number;
	dxpGain?: number;
};
export type Rs3Minigame = {
	[index: string]: number | undefined;
	minigameId: number;
	score: number;
	rank: number;
	dayGain: number;
	weekGain: number;
	monthGain: number;
	yearGain: number;
	dxpGain?: number;
};
export type Rs3Activity = {
	[id: string]: number | undefined | string | object;
	userId: number;
	player: {
		displayName: string;
		ironmanStatus: accountTypes;
	};
	title: string;
	details: string;
	iconUri?: string;
	price?: number;
	dateRecorded: Date;
	currentUserLiked: Boolean;
	likes: number;
};
