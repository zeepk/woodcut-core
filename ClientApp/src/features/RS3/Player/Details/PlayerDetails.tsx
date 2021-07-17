import React from 'react';
import { useAppSelector } from 'app/hooks';

import TrackingButton from 'features/RS3/Player/Details/TrackingButton';
import QuestChart from 'features/RS3/Player/Details/QuestChart';
import MilestoneChart from 'features/RS3/Player/Details/MilestoneChart';
import { FollowButton } from 'features/Common/FollowButton';
import { ActivityList } from 'features/RS3/Dashboard/ActivityList';
import HelpIcon from 'features/RS3/Player/Details/HelpIcon';
import { selectAuthIsLoggedIn } from 'features/Common/commonSlice';
import { activityFeedTitleText } from 'utils/constants';
import {
	selectPlayerActivities,
	selectRuneMetricsEnabled,
	selectUsername,
	selectIsTracking,
} from 'features/RS3/rs3Slice';
import 'features/RS3/rs3.scss';

export default function PlayerDetails() {
	const username = useAppSelector(selectUsername);
	const isTracking = useAppSelector(selectIsTracking);
	const isLoggedIn = useAppSelector(selectAuthIsLoggedIn);
	const activities = useAppSelector(selectPlayerActivities);
	const metricsEnabled = useAppSelector(selectRuneMetricsEnabled);

	const showFollowButton = isTracking && isLoggedIn && username;

	return (
		<div className="container--player-details p-p-2 p-d-flex p-ai-start p-jc-between p-flex-wrap">
			<div className="container--details-data p-mb-6">
				<div className="container--details-buttons p-d-flex p-ai-start p-jc-md-start p-jc-center p-mb-6">
					<TrackingButton />
					{showFollowButton ? (
						<FollowButton playerToFollow={username} />
					) : (
						<div />
					)}
					{!metricsEnabled && <HelpIcon />}
				</div>
				<div className="container--details-charts p-d-flex p-ai-between p-ai-md-start p-jc-center p-flex-wrap p-mb-3">
					{metricsEnabled && <QuestChart />}
					<MilestoneChart />
				</div>
			</div>
			{metricsEnabled && (
				<div className="container--details-activities p-d-flex p-flex-column p-ai-center">
					<h2>{activityFeedTitleText}</h2>
					<ActivityList activities={activities} />
				</div>
			)}
		</div>
	);
}
