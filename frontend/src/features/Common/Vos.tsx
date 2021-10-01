import React, { useEffect, useState } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { Button } from 'primereact/button';
import { ProgressBar } from 'primereact/progressbar';

import Amlodd from 'assets/images/vos/Amlodd_Clan.png';
import Cadarn from 'assets/images/vos/Cadarn_Clan.png';
import Crwys from 'assets/images/vos/Crwys_Clan.png';
import Hefin from 'assets/images/vos/Hefin_Clan.png';
import Iorwerth from 'assets/images/vos/Iorwerth_Clan.png';
import Ithell from 'assets/images/vos/Ithell_Clan.png';
import Meilyr from 'assets/images/vos/Meilyr_Clan.png';
import Trahaearn from 'assets/images/vos/Trahaearn_Clan.png';

import {
	getCurrentVos,
	selectCurrentVos,
	selectCurrentVosLoading,
} from 'features/Common/commonSlice';
import 'features/Common/common.scss';
import { isNullUndefinedOrWhitespace } from 'utils/helperFunctions';
import {
	lastUpdatedText,
	updateVosButtonText,
	vosRefreshTimer,
} from 'utils/constants';
import { DateTime } from 'luxon';

export function Vos() {
	const dispatch = useAppDispatch();
	const currentVos = useAppSelector(selectCurrentVos);
	const loading = useAppSelector(selectCurrentVosLoading);
	const [lastUpdated, setLastUpdated] = useState(
		DateTime.fromJSDate(new Date()),
	);
	const [refreshAvailable, setRefreshAvailable] = useState(false);

	const clanImage = (name: string) => {
		switch (name) {
			case 'Amlodd':
				return <img src={Amlodd} alt="Amlodd" />;
			case 'Cadarn':
				return <img src={Cadarn} alt="Cadarn" />;
			case 'Crwys':
				return <img src={Crwys} alt="Crwys" />;
			case 'Hefin':
				return <img src={Hefin} alt="Hefin" />;
			case 'Iorwerth':
				return <img src={Iorwerth} alt="Iorwerth" />;
			case 'Ithell':
				return <img src={Ithell} alt="Ithell" />;
			case 'Meilyr':
				return <img src={Meilyr} alt="Meilyr" />;
			case 'Trahaearn':
				return <img src={Trahaearn} alt="Trahaearn" />;

			default:
				return <div>{name}</div>;
		}
	};

	const handleRefresh = () => {
		if (refreshAvailable) {
			setRefreshAvailable(false);
			dispatch(getCurrentVos());
			setLastUpdated(DateTime.now());
			setTimeout(() => {
				setRefreshAvailable(true);
			}, vosRefreshTimer);
		}
	};

	useEffect(() => {
		setTimeout(() => {
			setRefreshAvailable(true);
		}, vosRefreshTimer);
		if (
			!isNullUndefinedOrWhitespace(currentVos.clan1) &&
			!isNullUndefinedOrWhitespace(currentVos.clan2)
		) {
			return;
		}
		dispatch(getCurrentVos());
	}, [dispatch, currentVos.clan1, currentVos.clan2]);

	const content = loading ? (
		<ProgressBar className="progressbar--vos" mode="indeterminate" />
	) : (
		<div className="container--vos-button p-d-flex p-jc-between p-ai-center">
			<div>
				{clanImage(currentVos.clan1)}
				{clanImage(currentVos.clan2)}
			</div>
			<Button
				disabled={!refreshAvailable}
				label={updateVosButtonText}
				className="p-button-info p-px-2 btn--vos"
				onClick={() => handleRefresh()}
			/>
		</div>
	);

	return (
		<div className="container--vos">
			<div className="container--vos-content p-d-flex p-ai-center">
				{content}
			</div>
			<p>{`${lastUpdatedText} ${lastUpdated.toLocaleString(
				DateTime.DATETIME_FULL,
			)}`}</p>
		</div>
	);
}
