import React, { useState } from 'react';
import { useAppSelector } from 'app/hooks';
import { Link } from 'react-router-dom';

import { ProgressBar } from 'primereact/progressbar';

import { selectClanname, selectBadges } from 'features/RS3/rs3Slice';
import { selectUserRs3Rsn } from 'features/Common/commonSlice';
import { Badge } from 'features/Common/Badge';
import { Avatar } from 'features/RS3/Avatar';
import '../rs3.scss';
import { clanFlagUrlPre, clanFlagUrlPost } from 'utils/constants';

export default function DashboardUserInfo() {
	const [clanFlagLoading, updateClanFlagLoading] = useState(true);

	const username = useAppSelector(selectUserRs3Rsn);
	const clanname = useAppSelector(selectClanname);
	const badges = useAppSelector(selectBadges);

	const formattedUsername = username ? username.split(' ').join('+') : '';

	const linkPath = `/rs3/user/${formattedUsername}`;

	return (
		<div className="container--player-info p-d-flex p-flex-column p-ai-md-start p-ai-center">
			<div className="container--name-clan p-d-flex p-my-2 p-jc-between">
				<Link className="link--user" to={linkPath}>
					<div className="container--home-avatar container--home-info p-d-flex p-flex-column p-jc-between p-ai-center">
						<Avatar username={formattedUsername} />
						<h2 className="p-m-0">{username}</h2>
					</div>
				</Link>
				{clanname && (
					<div className="container--clan container--home-info p-d-flex p-flex-column p-jc-between p-ai-center">
						<img
							className="img--home-clan-flag p-pt-2"
							src={`${clanFlagUrlPre}${clanname}${clanFlagUrlPost}`}
							alt={`${username} flag`}
							onLoad={() => updateClanFlagLoading(false)}
						/>

						<h2 className="p-m-0">{clanname}</h2>
					</div>
				)}
				{clanFlagLoading && clanname ? (
					<ProgressBar
						className="progressbar--home-avatar p-mt-6"
						mode="indeterminate"
					/>
				) : null}
			</div>
			<div className="container--badges">
				{badges.map((b) => (
					<Badge key={b} badge={b} />
				))}
			</div>
		</div>
	);
}
