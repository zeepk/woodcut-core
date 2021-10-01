import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import '../rs3.scss';
import { useAppDispatch, useAppSelector } from '../../../app/hooks';
import {
	getXpGains,
	getPlayerQuests,
	getPlayerIronStatus,
	getPlayerDetails,
	getPlayerMetrics,
	selectStatus,
	selectPlayerSuccess,
} from '../../RS3/rs3Slice';
import { TabMenu } from 'primereact/tabmenu';
import Rs3PlayerStatTable from './Rs3PlayerStatTable';
import Rs3PlayerMinigameTable from './Rs3PlayerMinigameTable';
import PlayerDetails from 'features/RS3/Player/Details/PlayerDetails';
import Rs3PlayerInfo from './Rs3PlayerInfo';
import LoadingIcon from '../../Common/LoadingIcon';
import { userNotFoundText } from 'utils/constants';
interface RouteParams {
	username: string;
}

export default function Rs3PlayerLandingPage() {
	const [tabIndex, updateTabIndex] = useState(1);
	const dispatch = useAppDispatch();
	const status = useAppSelector(selectStatus);
	const playerSuccess = useAppSelector(selectPlayerSuccess);
	const { username } = useParams<RouteParams>();
	let activitiesLoaded = false;

	const tabs = [
		{ label: 'Info', icon: 'pi pi-fw pi-home' },
		{ label: 'Stats', icon: 'tab--skills-icon' },
		{ label: 'Minigames', icon: 'tab--quest-icon' },
	];

	useEffect(() => {
		if (username.trim() !== '') {
			const formattedUsername = username.split('+').join(' ');
			dispatch(getXpGains(formattedUsername));
			dispatch(getPlayerQuests(formattedUsername));
			dispatch(getPlayerIronStatus(formattedUsername));
			dispatch(getPlayerDetails(formattedUsername));
			updateTabIndex(1);
		}
	}, [username, dispatch]);

	if (status === 'loading') {
		return <LoadingIcon fullScreen={true} />;
	}

	if (!playerSuccess) {
		return <h1 className="p-text-center">{userNotFoundText}</h1>;
	}

	let content;
	switch (tabIndex) {
		case 0:
			content = <PlayerDetails />;
			break;
		case 1:
			content = <Rs3PlayerStatTable />;
			break;
		case 2:
			content = <Rs3PlayerMinigameTable />;
			break;
		default:
			break;
	}

	const updateTab = (index: number) => {
		if (index === 0 && activitiesLoaded === false) {
			activitiesLoaded = true;
			const formattedUsername = username.split('+').join(' ');
			dispatch(getPlayerMetrics(formattedUsername));
		}

		updateTabIndex(index);
	};

	return (
		<div className="p-p-2">
			<div className="p-mb-6 p-mb-lg-0">
				<Rs3PlayerInfo />
			</div>
			<TabMenu
				model={tabs}
				activeIndex={tabIndex}
				onTabChange={(e) => updateTab(e.index)}
			/>
			{content}
		</div>
	);
}
