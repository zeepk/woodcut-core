import React, { useState } from 'react';
import 'features/Common/common.scss';
import { DateTime, Duration } from 'luxon';
import { dailyResetText, gainsResetText } from 'utils/constants';
import { setInterval } from 'timers';

export function Timers() {
	const defaultDiff = Duration.fromObject({
		hours: 0,
		minutes: 0,
		seconds: 0,
	}).toObject();
	const [timeTilReset, setTimeTilReset] = useState(defaultDiff);
	const [timeTilGainsReset, setTimeTilGainsReset] = useState(defaultDiff);
	const [timer, setTimer] = useState(false);
	let nextReset = DateTime.now().setZone('GMT').endOf('day');
	let nextGainsReset = DateTime.now().setZone('US/Mountain').endOf('day');

	const updateTime = () => {
		let now = DateTime.now();
		setTimeTilReset(
			nextReset.diff(now, ['hours', 'minutes', 'seconds']).toObject(),
		);
		setTimeTilGainsReset(
			nextGainsReset.diff(now, ['hours', 'minutes', 'seconds']).toObject(),
		);
	};

	const formattedSeconds = (seconds: number | undefined) => {
		if (!seconds) {
			return '0';
		}
		const roundedSeconds = Math.floor(seconds);
		return roundedSeconds < 10 ? `0${roundedSeconds}` : `${roundedSeconds}`;
	};

	if (!timer) {
		setTimer(true);
		setInterval(updateTime, 1000);
	}

	return (
		<div className="container--vos p-py-2">
			<div className="p-d-flex p-jc-between p-py-1">
				<div>{dailyResetText}</div>
				<div>
					{`${timeTilReset.hours}:${timeTilReset.minutes}:${formattedSeconds(
						timeTilReset.seconds,
					)}`}
				</div>
			</div>
			<div className="p-d-flex p-jc-between p-py-1">
				<div>{gainsResetText}</div>
				<div>
					{`${timeTilGainsReset.hours}:${
						timeTilGainsReset.minutes
					}:${formattedSeconds(timeTilGainsReset.seconds)}`}
				</div>
			</div>
		</div>
	);
}
