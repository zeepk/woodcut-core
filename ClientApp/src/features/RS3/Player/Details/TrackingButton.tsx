import React from 'react';
import { Button } from 'primereact/button';
import 'features/RS3/rs3.scss';
import { useAppSelector, useAppDispatch } from 'app/hooks';
import {
	selectIsTracking,
	selectUsername,
	startTrackingForUser,
} from '../../rs3Slice';
import {
	trackingButtonTextDisabled,
	trackingButtonTextEnabled,
} from 'utils/constants';

export default function TrackingButton() {
	const dispatch = useAppDispatch();

	const isTracking = useAppSelector(selectIsTracking);
	const username = useAppSelector(selectUsername);

	const trackingButtonText = isTracking
		? trackingButtonTextEnabled
		: `${trackingButtonTextDisabled} ${username}`;
	const trackingButtonClassName = isTracking
		? 'p-button-success'
		: 'p-button-warning';

	const handleButtonClick = () => {
		if (!isTracking) {
			dispatch(startTrackingForUser());
		}
	};

	return (
		<Button
			label={trackingButtonText}
			className={`${trackingButtonClassName} btn--tracking`}
			onClick={() => handleButtonClick()}
			disabled={isTracking}
		/>
	);
}
