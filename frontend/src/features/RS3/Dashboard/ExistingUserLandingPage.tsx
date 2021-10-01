import React, { useEffect, useState } from 'react';
import { useAppSelector, useAppDispatch } from 'app/hooks';

import { ProgressBar } from 'primereact/progressbar';
import { TabMenu } from 'primereact/tabmenu';

import {
	getRs3FollowingActivities,
	getXpGains,
	getPlayerQuests,
	getPlayerIronStatus,
	getPlayerDetails,
	getPlayerMetrics,
	selectActivities,
	selectActivitiesLoading,
	selectPlayerLoading,
	selectTopTenGains,
} from 'features/RS3/rs3Slice';
import {
	getRs3Rsn,
	getFollowing,
	selectUserRs3Rsn,
	selectUserLoading,
} from 'features/Common/commonSlice';
import { ActivityList } from 'features/RS3/Dashboard/ActivityList';
import { GainsList } from 'features/RS3/Dashboard/GainsList';
import LoadingIcon from 'features/Common/LoadingIcon';
import Rs3PlayerStatTable from 'features/RS3/Player/Rs3PlayerStatTable';
import Rs3PlayerMinigameTable from 'features/RS3/Player/Rs3PlayerMinigameTable';
import { QuestChart } from 'features/RS3/Player/Details/QuestChart';
import { MilestoneChart } from 'features/RS3/Player/Details/MilestoneChart';
import '../rs3.scss';
import {
	followingActivityFeedTitleText,
	loggedInUserGainsFeedTitleText,
	homePageNoFollowingActivitiesText,
	homePageNoGainsText,
	homePageNoRsn,
	homePageNoRsnDetails,
} from 'utils/constants';
import DashboardUserInfo from './DashboardUserInfo';

export default function ExistingUserLandingPage() {
	const [tabIndex, updateTabIndex] = useState(0);
	const dispatch = useAppDispatch();

	const username = useAppSelector(selectUserRs3Rsn);
	const activities = useAppSelector(selectActivities);
	const activityLoading = useAppSelector(selectActivitiesLoading);
	const gains = useAppSelector(selectTopTenGains);
	const playerLoading = useAppSelector(selectPlayerLoading);
	const userLoading = useAppSelector(selectUserLoading);

	useEffect(() => {
		dispatch(getRs3Rsn());
		dispatch(getFollowing());
		dispatch(getRs3FollowingActivities());
		if (username) {
			const formattedUsername = username.split('+').join(' ');
			dispatch(getXpGains(formattedUsername));
			dispatch(getPlayerQuests(formattedUsername));
			dispatch(getPlayerIronStatus(formattedUsername));
			dispatch(getPlayerDetails(formattedUsername));
			dispatch(getPlayerMetrics(formattedUsername));
		}
	}, [dispatch, username]);

	if (userLoading) {
		return <LoadingIcon fullScreen={true} />;
	}

	if (!username) {
		return (
			<div className="container--no-rsn p-d-flex p-flex-column p-jc-center p-ai-center">
				<h2 className="text--no-rsn">{homePageNoRsn}</h2>;
				<h3 className="text--no-rsn">{homePageNoRsnDetails}</h3>;
			</div>
		);
	}

	const activityList =
		activities?.length > 0 ? (
			<ActivityList activities={activities} />
		) : (
			<h3>{homePageNoFollowingActivitiesText}</h3>
		);

	const tabs = [
		{ label: 'Stats', icon: 'tab--skills-icon' },
		{ label: 'Minigames', icon: 'tab--quest-icon' },
	];

	const gainsList =
		gains.length > 0 ? (
			<GainsList gains={gains} />
		) : (
			<h3>{homePageNoGainsText}</h3>
		);

	let content;
	switch (tabIndex) {
		case 0:
			content = <Rs3PlayerStatTable />;
			break;
		case 1:
			content = <Rs3PlayerMinigameTable />;
			break;
		default:
			break;
	}

	return (
		<div className="container--logged-in-user p-d-flex p-jc-center p-jc-xl-between p-ai-start p-p-1 p-p-md-5 p-flex-wrap">
			<div className="p-mb-2">
				<DashboardUserInfo />
			</div>
			<div className="container--home-list gains p-d-flex p-flex-column p-ai-center p-mx-2 p-mb-4">
				<h2 className="text--list-title p-m-0 p-p-3">
					{loggedInUserGainsFeedTitleText}
				</h2>
				{playerLoading ? (
					<ProgressBar
						className="progressbar--activity-list p-my-6"
						mode="indeterminate"
					/>
				) : (
					gainsList
				)}
			</div>
			<div className="container--home-list activities p-d-flex p-flex-column p-ai-center p-mx-2 p-mb-4">
				<h2 className="text--list-title p-m-0 p-p-3">
					{followingActivityFeedTitleText}
				</h2>
				{activityLoading ? (
					<ProgressBar
						className="progressbar--activity-list p-my-6"
						mode="indeterminate"
					/>
				) : (
					activityList
				)}
			</div>
			<div className="container--home-bottom p-d-flex p-ai-center p-flex-column">
				{playerLoading ? (
					<ProgressBar className="progressbar--gains" mode="indeterminate" />
				) : (
					<div className="container--home-gains">
						<TabMenu
							model={tabs}
							activeIndex={tabIndex}
							onTabChange={(e) => updateTabIndex(e.index)}
						/>
						{content}
					</div>
				)}
				<div className="container--home-charts p-d-flex p-jc-center p-ai-center p-flex-wrap">
					<MilestoneChart widthString="30rem" />
					<QuestChart widthString="30rem" />
				</div>
			</div>
		</div>
	);
}
