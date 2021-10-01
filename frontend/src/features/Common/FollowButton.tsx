import React, { FunctionComponent } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hooks';

import { Button } from 'primereact/button';
import { ProgressBar } from 'primereact/progressbar';

import {
	followPlayerRsn,
	unfollowPlayerRsn,
	selectPlayersFollowed,
	selectUserLoading,
} from '../Common/commonSlice';
import { followButtonText, unfollowButtonText } from 'utils/constants';
import 'features/Common/common.scss';

type props = {
	playerToFollow: string;
};

export const FollowButton: FunctionComponent<props> = ({ playerToFollow }) => {
	const dispatch = useAppDispatch();

	const loading = useAppSelector(selectUserLoading);
	const followed = useAppSelector(selectPlayersFollowed);

	const isFollowing = followed ? followed.includes(playerToFollow) : false;
	const buttonText = isFollowing ? unfollowButtonText : followButtonText;

	const handleFollowButtonClick = () => {
		if (isFollowing) {
			dispatch(unfollowPlayerRsn(playerToFollow));
		} else {
			dispatch(followPlayerRsn(playerToFollow));
		}
	};

	return (
		<div className="p-d-flex p-jc-center p-ml-2">
			{loading ? (
				<ProgressBar className="progressbar--follow" mode="indeterminate" />
			) : (
				<Button
					label={buttonText}
					className="p-button-info btn--follow p-p-2"
					onClick={() => handleFollowButtonClick()}
				/>
			)}
		</div>
	);
};
