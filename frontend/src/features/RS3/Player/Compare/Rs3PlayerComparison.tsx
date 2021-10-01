import React, { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../../../app/hooks';
import { useParams } from 'react-router-dom';
import { ProgressBar } from 'primereact/progressbar';
import { Link } from 'react-router-dom';

import { isNullUndefinedOrWhitespace } from 'utils/helperFunctions';
import {
	comparisonNeedTwoNamesText,
	comparisonUnsuccessfulText,
} from 'utils/constants';
import {
	getXpGainsForPlayer1,
	getXpGainsForPlayer2,
	selectComparisonLoading,
	selectPlayer1Skills,
	selectPlayer2Skills,
	selectPlayer1Minigames,
	selectPlayer2Minigames,
	selectPlayer1IronStatus,
	selectPlayer2IronStatus,
	getPlayer1IronStatus,
	getPlayer2IronStatus,
	selectComparisonSuccess,
} from 'features/RS3/rs3Slice';
import { Rs3PlayerComparisonStatTable } from 'features/RS3/Player/Compare/Rs3PlayerComparisonStatTable';
import { Rs3PlayerComparisonMinigameTable } from 'features/RS3/Player/Compare/Rs3PlayerComparisonMinigameTable';
import Rs3PlayerComparisonHome from 'features/RS3/Player/Compare/Rs3PlayerComparisonHome';
import { IronIcon } from 'features/RS3/IronIcon';
import { Avatar } from 'features/RS3/Avatar';
import 'features/RS3/rs3.scss';

interface RouteParams {
	player1: string;
	player2: string;
}

export default function Rs3PlayerComparison() {
	const dispatch = useAppDispatch();
	const isLoading = useAppSelector(selectComparisonLoading);
	const wasSuccessful = useAppSelector(selectComparisonSuccess);
	const player1Gains = useAppSelector(selectPlayer1Skills);
	const player2Gains = useAppSelector(selectPlayer2Skills);
	const player1Minigames = useAppSelector(selectPlayer1Minigames);
	const player2Minigames = useAppSelector(selectPlayer2Minigames);
	const player1IronStatus = useAppSelector(selectPlayer1IronStatus);
	const player2IronStatus = useAppSelector(selectPlayer2IronStatus);
	const { player1, player2 } = useParams<RouteParams>();
	const linkPath = '/rs3/user/';
	const invalidName =
		isNullUndefinedOrWhitespace(player1) ||
		isNullUndefinedOrWhitespace(player2);

	useEffect(() => {
		if (!invalidName) {
			dispatch(getXpGainsForPlayer1(player1));
			dispatch(getXpGainsForPlayer2(player2));
			dispatch(getPlayer1IronStatus(player1));
			dispatch(getPlayer2IronStatus(player2));
		}
	}, [invalidName, player1, player2, dispatch]);

	if (isLoading) {
		return (
			<div className="container--comparison p-d-flex p-jc-center p-p-4">
				<ProgressBar className="progressbar p-my-6" mode="indeterminate" />
			</div>
		);
	}
	if (invalidName) {
		return (
			<div className="container--comparison p-d-flex p-jc-center p-p-4">
				{comparisonNeedTwoNamesText}
			</div>
		);
	}
	if (!isLoading && !wasSuccessful) {
		return (
			<div className="container--comparison-unsuccessful p-d-flex p-flex-column p-ai-center p-p-4">
				<h1>{comparisonUnsuccessfulText}</h1>
				<Rs3PlayerComparisonHome />
			</div>
		);
	}

	return (
		<div className="container--comparison p-d-flex p-flex-column p-ai-center p-p-4">
			<div className="container--player-names p-mb-6 p-mb-lg-0 p-d-flex p-jc-between">
				<div className="container--player p-d-flex p-ai-center p-jc-start">
					<Avatar username={player1} />
					<Link className="link--user" to={linkPath + player1}>
						<h1 className="p-mr-2">{player1}</h1>
					</Link>
					<IronIcon accountType={player1IronStatus} />
				</div>
				<div className="container--player p-d-flex p-ai-center p-jc-end">
					<Link className="link--user" to={linkPath + player2}>
						<h1 className="p-mr-2">{player2}</h1>
					</Link>
					<IronIcon accountType={player2IronStatus} />
					<Avatar username={player2} />
				</div>
			</div>
			<Rs3PlayerComparisonStatTable
				player1Gains={player1Gains}
				player2Gains={player2Gains}
			/>
			<Rs3PlayerComparisonMinigameTable
				player1Minigames={player1Minigames}
				player2Minigames={player2Minigames}
			/>
		</div>
	);
}
