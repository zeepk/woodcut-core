import React, { FunctionComponent, useState } from 'react';
import { Link } from 'react-router-dom';

import { ProgressBar } from 'primereact/progressbar';
import { DateTime } from 'luxon';

import '../rs3.scss';
import { Rs3Activity } from 'features/RS3/rs3Types';
import { avatarUrlPre, avatarUrlPost } from 'utils/constants';
import { activityIconHelper } from 'utils/helperFunctions';

type props = {
	activity: Rs3Activity;
};

export const Activity: FunctionComponent<props> = ({ activity }) => {
	const [avatarLoading, updateAvatarLoading] = useState(true);
	const iconUri = activity.iconUri || activityIconHelper(activity);
	const joinedDisplayName = activity.player
		? activity.player.displayName.split(' ').join('+')
		: '';
	const linkPath = `/rs3/${joinedDisplayName}`;
	const price =
		!activity.price || activity.price <= 0
			? ''
			: `+${activity.price.toLocaleString()} gp`;

	const dt = DateTime.fromJSDate(new Date(activity.dateRecorded)).plus({
		hours: -1,
	});
	const dateRecorded = dt.toLocaleString(DateTime.DATETIME_SHORT);

	return (
		<Link className="link--activity-user" to={linkPath}>
			<div className="container--activity p-d-flex p-jc-between p-px-2 p-py-3">
				<div className="container--user">
					<img
						className="img--activity-avatar"
						src={`${avatarUrlPre}${joinedDisplayName}${avatarUrlPost}`}
						alt={'player avatar'}
						onLoad={() => updateAvatarLoading(false)}
					/>
					{avatarLoading ? (
						<ProgressBar
							className="progressbar--activity"
							mode="indeterminate"
						/>
					) : (
						<div />
					)}
					<div className="text--username">{activity.player?.displayName}</div>
				</div>
				<div className="container--entry p-ml-2">
					<div className="p-d-flex p-ai-center">
						<div className="text--title p-mr-2">{activity.title}</div>
						{iconUri ? (
							<img
								className="img--activity-icon"
								src={iconUri}
								alt="activity"
							/>
						) : (
							<div />
						)}
					</div>
					<div className="container--details p-d-flex p-ai-center p-jc-end p-mr-2 p-mt-1">
						<div className="text--price">{price}</div>
						<div className="text--date">{dateRecorded}</div>
					</div>
				</div>
			</div>
		</Link>
	);
};
