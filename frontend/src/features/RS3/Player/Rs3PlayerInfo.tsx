import React from 'react';
import '../rs3.scss';
import { useAppSelector } from 'app/hooks';

import {
	selectUsername,
	selectTotalXp,
	selectRunescore,
	selectBadges,
	selectClanname,
	selectQuestPoints,
	selectIronStatus,
} from 'features/RS3/rs3Slice';
import { Badge } from 'features/Common/Badge';
import { Avatar } from 'features/RS3/Avatar';
import TotalXpIcon from 'assets/skillIcons/1_overall.png';
import RunescoreIcon from 'assets/images/RuneScore.png';
import QuestIcon from 'assets/images/questIcon.png';
import { IronIcon } from '../IronIcon';

export default function Rs3PlayerInfo() {
	const username = useAppSelector(selectUsername);
	const clanname = useAppSelector(selectClanname);
	const totalXp = useAppSelector(selectTotalXp);
	const runescore = useAppSelector(selectRunescore);
	const badges = useAppSelector(selectBadges);
	const questPoints = useAppSelector(selectQuestPoints);
	const ironStatus = useAppSelector(selectIronStatus);

	return (
		<div className="container--player-info p-d-flex p-ai-center p-flex-wrap">
			<div className="container--avatar p-my-2">
				<Avatar username={username} />
			</div>
			<div className="container--text-info">
				<div className="p-d-flex p-ai-center">
					<h1 className="p-mr-2 p-my-0">{username}</h1>
					<IronIcon accountType={ironStatus} />
				</div>
				<p className="p-m-0">{clanname}</p>
			</div>
			<div className="container--totalxp p-ml-5">
				<img
					className="icon--player-info p-mr-2"
					src={TotalXpIcon}
					alt="total xp"
				/>
				<span>{totalXp.toLocaleString()}</span>
			</div>
			<div className="container--runescore p-ml-5">
				<img
					className="icon--player-info p-mr-2"
					src={RunescoreIcon}
					alt="total xp"
				/>
				<span>{runescore.toLocaleString()}</span>
			</div>
			<div className="container--qp p-ml-5">
				<img
					className="icon--player-info p-mr-2"
					src={QuestIcon}
					alt="quest points"
				/>
				<span>{questPoints}</span>
			</div>
			<div className="container--badges p-ml-5">
				{badges.map((b) => (
					<Badge key={b} badge={b} />
				))}
			</div>
		</div>
	);
}
