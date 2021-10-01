import React, { FunctionComponent, useRef } from 'react';
import { useAppDispatch, useAppSelector } from 'app/hooks';
import { Link } from 'react-router-dom';

import { Toast } from 'primereact/toast';
import { DateTime } from 'luxon';

import '../rs3.scss';
import { Rs3Activity } from 'features/RS3/rs3Types';
import { likeWithoutLoginText, formErrorToastLifetime } from 'utils/constants';
import { activityIconHelper } from 'utils/helperFunctions';
import { likeRs3Activity, unlikeRs3Activity } from 'features/RS3/rs3Slice';
import { selectAuthIsLoggedIn } from 'features/Common/commonSlice';
import { IronIcon } from '../IronIcon';
import { Avatar } from 'features/RS3/Avatar';

type props = {
	activity: Rs3Activity;
};

export const Activity: FunctionComponent<props> = ({ activity }) => {
	const dispatch = useAppDispatch();
	const toast = useRef<Toast>(null);

	const isLoggedIn = useAppSelector(selectAuthIsLoggedIn);
	const iconUri = activity.iconUri || activityIconHelper(activity);
	const linkPath = `/rs3/user/${activity.player.displayName
		.split(' ')
		.join('+')}`;
	const price =
		!activity.price || activity.price <= 0
			? ''
			: `+${activity.price.toLocaleString()} gp`;

	const dt = DateTime.fromJSDate(new Date(activity.dateRecorded)).plus({
		hours: 16,
		days: -1,
	});
	const dateRecorded = dt.toLocaleString(DateTime.DATETIME_SHORT);

	const handleLikeButtonClick = async () => {
		if (!isLoggedIn || !activity.id) {
			toast?.current?.show({
				severity: 'info',
				detail: likeWithoutLoginText,
				life: formErrorToastLifetime,
			});
			return;
		}
		let result: any;
		if (activity.currentUserLiked) {
			result = await dispatch(unlikeRs3Activity(Number(activity.id)));
		} else {
			result = await dispatch(likeRs3Activity(Number(activity.id)));
		}
		return result;
	};

	return (
		<div className="container--activity-outer p-d-flex p-jc-end p-px-2 p-py-3">
			<Toast ref={toast} />
			<Link className="link--user" to={linkPath}>
				<div className="container--activity p-d-flex p-jc-between">
					<div className="container--user">
						<Avatar username={activity.player.displayName} />
						<div className="text--username p-d-flex p-ai-end">
							<div className="p-mr-1">{activity.player.displayName}</div>
							<IronIcon accountType={activity.player.ironmanStatus} />
						</div>
					</div>
					<div className="container--entry p-ml-2 p-d-flex">
						<div>
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
				</div>
			</Link>
			<div
				onClick={() => handleLikeButtonClick()}
				className={`container--likes p-d-flex p-flex-column p-jc-center p-ai-center p-ml-4 p-mr-2 ${
					activity.currentUserLiked ? 'active' : ''
				}`}
			>
				<i className="pi pi-caret-up"></i>
				<div className="text--likes">{activity.likes}</div>
			</div>
		</div>
	);
};
