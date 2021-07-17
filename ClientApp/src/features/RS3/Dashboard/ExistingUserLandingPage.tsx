import React, { useEffect, useState } from 'react';
import { useAppSelector, useAppDispatch } from 'app/hooks';

import { ProgressBar } from 'primereact/progressbar';

import {
	getRs3FollowingActivities,
	getXpGains,
	getPlayerQuests,
	getPlayerDetails,
	getPlayerMetrics,
	selectActivities,
	selectActivitiesLoading,
	selectClanname,
} from 'features/RS3/rs3Slice';
import {
	getRs3Rsn,
	getFollowing,
	selectUserRs3Rsn,
} from 'features/Common/commonSlice';
import LoadingIcon from 'features/Common/LoadingIcon';
import { ActivityList } from 'features/RS3/Dashboard/ActivityList';
import '../rs3.scss';
import {
	avatarUrlPre,
	avatarUrlPost,
	followingActivityFeedTitleText,
} from 'utils/constants';

export default function ExistingUserLandingPage() {
	const [avatarLoading, updateAvatarLoading] = useState(true);
	const dispatch = useAppDispatch();

	const username = useAppSelector(selectUserRs3Rsn);
	const activities = useAppSelector(selectActivities);
	const loading = useAppSelector(selectActivitiesLoading);
	const clanname = useAppSelector(selectClanname);

	useEffect(() => {
		dispatch(getRs3Rsn());
		dispatch(getFollowing());
		dispatch(getRs3FollowingActivities());
		if (username) {
			const formattedUsername = username.split('+').join(' ');
			dispatch(getXpGains(formattedUsername));
			dispatch(getPlayerQuests(formattedUsername));
			dispatch(getPlayerDetails(formattedUsername));
			dispatch(getPlayerMetrics(formattedUsername));
		}
	}, [dispatch, username]);

	if (!username) {
		return <h2>{'Please set your Runescape Username'}</h2>;
	}

	const activityList =
		activities?.length > 0 ? (
			<ActivityList activities={activities} />
		) : (
			<h3>No following activities</h3>
		);

	return (
		<div className="container--logged-in-user p-d-flex p-jc-between p-p-5 p-flex-wrap p-flex-md-nowrap">
			<div className="container--player-info">
				<div className="container--home-avatar p-my-2">
					<img
						className="img--home-avatar"
						src={`${avatarUrlPre}${username
							.split(' ')
							.join('+')}${avatarUrlPost}`}
						alt={username}
						onLoad={() => updateAvatarLoading(false)}
					/>
					{avatarLoading ? (
						<div className="icon--loading-avatar">
							<LoadingIcon fullScreen={false} />
						</div>
					) : (
						<div />
					)}
				</div>
				<h2>{username}</h2>
				<h3>{clanname}</h3>
			</div>
			<div className="container--home-activities p-d-flex p-flex-column p-ai-center">
				<h2>{followingActivityFeedTitleText}</h2>
				{loading ? (
					<ProgressBar
						className="progressbar--activity-list p-mt-6"
						mode="indeterminate"
					/>
				) : (
					activityList
				)}
			</div>
		</div>
	);
}
