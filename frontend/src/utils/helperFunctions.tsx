import Overall from 'assets/skillIcons/1_overall.png';
import Attack from 'assets/skillIcons/2_attack.png';
import Defence from 'assets/skillIcons/3_defence.png';
import Strength from 'assets/skillIcons/4_strength.png';
import Constitution from 'assets/skillIcons/5_constitution.png';
import Ranged from 'assets/skillIcons/6_ranged.png';
import Prayer from 'assets/skillIcons/7_prayer.png';
import Magic from 'assets/skillIcons/8_magic.png';
import Cooking from 'assets/skillIcons/9_cooking.png';
import Woodcutting from 'assets/skillIcons/10_woodcutting.png';
import Fletching from 'assets/skillIcons/11_fletching.png';
import Fishing from 'assets/skillIcons/12_fishing.png';
import Firemaking from 'assets/skillIcons/13_firemaking.png';
import Crafting from 'assets/skillIcons/14_crafting.png';
import Smithing from 'assets/skillIcons/15_smithing.png';
import Mining from 'assets/skillIcons/16_mining.png';
import Herblore from 'assets/skillIcons/17_herblore.png';
import Agility from 'assets/skillIcons/18_agility.png';
import Thieving from 'assets/skillIcons/19_thieving.png';
import Slayer from 'assets/skillIcons/20_slayer.png';
import Farming from 'assets/skillIcons/21_farming.png';
import Runecrafting from 'assets/skillIcons/22_runecrafting.png';
import Hunter from 'assets/skillIcons/23_hunter.png';
import Construction from 'assets/skillIcons/24_construction.png';
import Summoning from 'assets/skillIcons/25_summoning.png';
import Dungeoneering from 'assets/skillIcons/26_dungeoneering.png';
import Divination from 'assets/skillIcons/27_divination.png';
import Invention from 'assets/skillIcons/28_invention.png';
import Archaeology from 'assets/skillIcons/29_archaeology.png';
import QuestIcon from 'assets/images/questIcon.png';
import {
	archJournal,
	maxCapeXp,
	maxTotalXp,
	all120Xp,
	maxXp,
	regularSkill99Xp,
	regularSkill120Xp,
	eliteSkill99Xp,
	eliteSkill120Xp,
} from 'utils/constants';
import { Rs3Activity, Rs3Skill } from 'features/RS3/rs3Types';

export const isNullUndefinedOrWhitespace = (
	text: string | null | undefined
) => {
	if (text === null || text === undefined) {
		return true;
	}
	return text.trim() === '';
};

export const isDevEnv = () => {
	return process.env.node_env === 'development';
};

export const activityIconHelper = (activity: Rs3Activity) => {
	let icon = null;
	const text = activity.title;

	if (text.includes(' level ')) {
		const skill = text.split(' level ')[0];
		icon = imgArray[skillNameArray.indexOf(skill)];
		return icon;
	}

	if (text.includes(' xp in ')) {
		const skill = text.split(' xp in ')[0];
		icon = imgArray[skillNameArray.indexOf(skill)];
		return icon;
	}

	if (text.includes('Quest')) {
		return QuestIcon;
	}

	if (text.includes('mystery') || text.includes('qualification')) {
		return archJournal;
	}

	return;
};

const imgArray = [
	Overall,
	Attack,
	Defence,
	Strength,
	Constitution,
	Ranged,
	Prayer,
	Magic,
	Cooking,
	Woodcutting,
	Fletching,
	Fishing,
	Firemaking,
	Crafting,
	Smithing,
	Mining,
	Herblore,
	Agility,
	Thieving,
	Slayer,
	Farming,
	Runecrafting,
	Hunter,
	Construction,
	Summoning,
	Dungeoneering,
	Divination,
	Invention,
	Archaeology,
];

export const skillNameArray = [
	'Overall',
	'Attack',
	'Defence',
	'Strength',
	'Constitution',
	'Ranged',
	'Prayer',
	'Magic',
	'Cooking',
	'Woodcutting',
	'Fletching',
	'Fishing',
	'Firemaking',
	'Crafting',
	'Smithing',
	'Mining',
	'Herblore',
	'Agility',
	'Thieving',
	'Slayer',
	'Farming',
	'Runecrafting',
	'Hunter',
	'Construction',
	'Summoning',
	'Dungeoneering',
	'Divination',
	'Invention',
	'Archaeology',
];

export const skillIcon = (id: number) => {
	return <img src={imgArray[id]} alt="skill con" />;
};

export const calcVirtualLevel = (rowData: {
	xp: number;
	name: string;
	level: number;
}) => {
	if (
		rowData.xp < 14391160 ||
		rowData.name === 'Overall' ||
		rowData.name === 'Invention'
	) {
		return rowData.level;
	} else if (rowData.xp >= regularSkill120Xp) {
		return 120;
	} else if (rowData.xp >= 94442737) {
		return 119;
	} else if (rowData.xp >= 85539082) {
		return 118;
	} else if (rowData.xp >= 77474828) {
		return 117;
	} else if (rowData.xp >= 70170840) {
		return 116;
	} else if (rowData.xp >= 63555443) {
		return 115;
	} else if (rowData.xp >= 57563718) {
		return 114;
	} else if (rowData.xp >= 52136869) {
		return 113;
	} else if (rowData.xp >= 47221641) {
		return 112;
	} else if (rowData.xp >= 42769801) {
		return 111;
	} else if (rowData.xp >= 38737661) {
		return 110;
	} else if (rowData.xp >= 35085654) {
		return 109;
	} else if (rowData.xp >= 31777943) {
		return 108;
	} else if (rowData.xp >= 28782069) {
		return 107;
	} else if (rowData.xp >= 26068632) {
		return 106;
	} else if (rowData.xp >= 23611006) {
		return 105;
	} else if (rowData.xp >= 21385073) {
		return 104;
	} else if (rowData.xp >= 19368992) {
		return 103;
	} else if (rowData.xp >= 17542976) {
		return 102;
	} else if (rowData.xp >= 15889109) {
		return 101;
	} else if (rowData.xp >= 14391160) {
		return 100;
	} else {
		console.log('Error calculating virtual level.');
		return 0;
	}
};

export const rs3DataArray = [
	'Overall',
	'Attack',
	'Defence',
	'Strength',
	'Constitution',
	'Ranged',
	'Prayer',
	'Magic',
	'Cooking',
	'Woodcutting',
	'Fletching',
	'Fishing',
	'Firemaking',
	'Crafting',
	'Smithing',
	'Mining',
	'Herblore',
	'Agility',
	'Thieving',
	'Slayer',
	'Farming',
	'Runecrafting',
	'Hunter',
	'Construction',
	'Summoning',
	'Dungeoneering',
	'Divination',
	'Invention',
	'Archaeology',
	'Bounty Hunter',
	'B.H. Rogues',
	'Dominion Tower',
	'The Crucible',
	'Castle Wars games',
	'B.A. Attackers',
	'B.A. Defenders',
	'B.A. Collectors',
	'B.A. Healers',
	'Duel Tournament',
	'Mobilising Armies',
	'Conquest',
	'Fist of Guthix',
	'GG: Athletics',
	'GG: Resource Race',
	'WE2: Armadyl Lifetime Contribution',
	'WE2: Bandos Lifetime Contribution',
	'WE2: Armadyl PvP kills',
	'WE2: Bandos PvP kills',
	'Heist Guard Level',
	'Heist Robber Level',
	'CFP: 5 game average',
	'AF15: Cow Tipping',
	'AF15: Rats killed after the miniquest',
	'RuneScore',
	'Clue Scrolls Easy',
	'Clue Scrolls Medium',
	'Clue Scrolls Hard',
	'Clue Scrolls Elite',
	'Clue Scrolls Master',
];

export const calcSkillPercentage = (flag: string, data: Array<Rs3Skill>) => {
	const skillsAt120 = [
		'Invention',
		'Slayer',
		'Dungeoneering',
		'Herblore',
		'Farming',
		'Archaeology',
	];
	var runningTotal = 0;
	switch (flag) {
		case 'max':
			for (const skill in data) {
				const xp = +data[skill].xp;
				if (skillNameArray[data[skill].skillId] !== 'Invention') {
					// not invention, add normally
					if (xp >= regularSkill99Xp) {
						runningTotal += regularSkill99Xp;
					} else {
						runningTotal += xp;
					}
				} else {
					// use invention xp
					if (xp >= eliteSkill99Xp) {
						runningTotal += eliteSkill99Xp;
					} else {
						runningTotal += xp;
					}
				}
			}
			if (runningTotal >= maxCapeXp) {
				return {
					remainder: 0,
					xp: maxCapeXp,
				};
			}
			return {
				remainder: maxCapeXp - runningTotal,
				xp: runningTotal,
			};
		case 'maxtotal':
			for (const skill in data) {
				const xp = +data[skill].xp;
				if (skillNameArray[data[skill].skillId] === 'Invention') {
					// use invention xp
					if (xp >= eliteSkill120Xp) {
						runningTotal += eliteSkill120Xp;
					} else {
						runningTotal += xp;
					}
				} else if (skillsAt120.includes(skillNameArray[data[skill].skillId])) {
					// not invention, add normally
					if (xp >= regularSkill120Xp) {
						runningTotal += regularSkill120Xp;
					} else {
						runningTotal += xp;
					}
				} else {
					// not invention, add normally
					if (xp >= regularSkill99Xp) {
						runningTotal += regularSkill99Xp;
					} else {
						runningTotal += xp;
					}
				}
			}
			if (runningTotal >= maxTotalXp) {
				return {
					remainder: 0,
					xp: maxTotalXp,
				};
			}
			return {
				remainder: maxTotalXp - runningTotal,
				xp: runningTotal,
			};
		case '120all':
			for (const skill in data) {
				const xp = +data[skill].xp;
				if (skillNameArray[data[skill].skillId] !== 'Invention') {
					// not invention, add normally
					if (xp >= regularSkill120Xp) {
						runningTotal += regularSkill120Xp;
					} else {
						runningTotal += xp;
					}
				} else {
					// use invention xp
					if (xp >= eliteSkill120Xp) {
						runningTotal += eliteSkill120Xp;
					} else {
						runningTotal += xp;
					}
				}
			}
			if (runningTotal >= all120Xp) {
				return {
					remainder: 0,
					xp: all120Xp,
				};
			}
			return {
				remainder: all120Xp - runningTotal,
				xp: runningTotal,
			};
		case 'maxxp':
			runningTotal = data.slice(1).reduce((a, b) => a + +b.xp, 0);
			return {
				remainder: maxXp - runningTotal,
				xp: runningTotal,
			};
		default:
			return {
				remainder: 0,
				xp: 0,
			};
	}
};
