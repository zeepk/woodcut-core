import React, { useEffect } from 'react';
import { useAppSelector, useAppDispatch } from 'app/hooks';

import { ProgressBar } from 'primereact/progressbar';

import {
	selectActivities,
	selectActivitiesLoading,
	getRs3Activities,
} from 'features/RS3/rs3Slice';
import { ActivityList } from 'features/RS3/Dashboard/ActivityList';
import { LoginButton } from 'features/Common/Account/LoginButton';
import { CreateAccountButton } from 'features/Common/Account/CreateAccountButton';
import '../rs3.scss';
import {
	homeContentTextPre,
	homeContentTextPost,
	activityFeedTitleText,
	rs3HomePageActivities,
	twitterUsername,
	twitterUrl,
} from 'utils/constants';
import Logo from 'assets/images/logo.png';

export default function NewUserLandingPage() {
	const dispatch = useAppDispatch();

	const activities = useAppSelector(selectActivities);
	const loading = useAppSelector(selectActivitiesLoading);

	useEffect(() => {
		dispatch(getRs3Activities(rs3HomePageActivities));
	}, [dispatch]);

	return (
		<div className="container--logged-out-user p-d-flex p-jc-between p-p-5 p-flex-wrap p-flex-md-nowrap">
			<div className="container--half p-pl-1 p-pl-lg-5">
				<img className="img--home-logo" src={Logo} alt="logo" />
				<div className="container--home-text p-my-2">
					<span className="text--home-content">{homeContentTextPre}</span>
					<a className="text--home-content twitter p-mx-1" href={twitterUrl}>
						{twitterUsername}
					</a>
					<span className="text--home-content">{homeContentTextPost}</span>
				</div>
				<div className="p-d-flex container--home-buttons p-mt-4">
					<LoginButton />
					<CreateAccountButton />
				</div>
			</div>
			<div className="container--half p-d-flex p-flex-column p-ai-center">
				<h2>{activityFeedTitleText}</h2>
				{loading ? (
					<ProgressBar
						className="progressbar--activity-list p-mt-6"
						mode="indeterminate"
					/>
				) : (
					<ActivityList activities={activities} />
				)}
			</div>
		</div>
	);
}
