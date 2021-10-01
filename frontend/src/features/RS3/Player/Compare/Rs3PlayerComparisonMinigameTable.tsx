import React, { FunctionComponent, useState } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Dropdown } from 'primereact/dropdown';
import 'features/RS3/rs3.scss';
import { Rs3Minigame } from 'features/RS3/rs3Types';
import { rs3DataArray } from 'utils/helperFunctions';
import { comparisonGainPeriods } from 'utils/constants';
type props = {
	player1Minigames: Array<Rs3Minigame>;
	player2Minigames: Array<Rs3Minigame>;
};

export const Rs3PlayerComparisonMinigameTable: FunctionComponent<props> = ({
	player1Minigames,
	player2Minigames,
}) => {
	const [gainPeriod, setGainPeriod] = useState(comparisonGainPeriods[0]);

	const combinedGains = player1Minigames.map((g, i) => {
		return {
			minigameId: g.minigameId,
			player1Score: g.score,
			player1Rank: g.rank,
			dayGain1: g.dayGain,
			weekGain1: g.weekGain,
			monthGain1: g.monthGain,
			yearGain1: g.yearGain,
			dxpGain1: g.dxpGain,
			player2Score: player2Minigames[i].score,
			player2Rank: player2Minigames[i].rank,
			dayGain2: player2Minigames[i].dayGain,
			weekGain2: player2Minigames[i].weekGain,
			monthGain2: player2Minigames[i].monthGain,
			yearGain2: player2Minigames[i].yearGain,
			dxpGain2: player2Minigames[i].dxpGain,
		};
	});

	const otherGainsHeaderTemplate = (
		<Dropdown
			value={gainPeriod}
			options={comparisonGainPeriods}
			placeholder={gainPeriod.label}
			onChange={(e) =>
				setGainPeriod(
					comparisonGainPeriods.find((gp) => gp.value === e.value) ||
						comparisonGainPeriods[0],
				)
			}
		/>
	);

	const otherGainsTemplate = (gain: Number) => {
		if (gain === null || gain === undefined) {
			return;
		}
		const gainClass = gain > 0 ? 'gain' : '';
		return <div className={gainClass}>{gain.toLocaleString()}</div>;
	};

	const OtherGainsPlayer1 = (rowData: any) => {
		const gain = rowData[gainPeriod.data + '1'];
		return otherGainsTemplate(gain);
	};

	const OtherGainsPlayer2 = (rowData: any) => {
		const gain = rowData[gainPeriod.data + '2'];
		return otherGainsTemplate(gain);
	};

	const OtherGainsColumn = (isPlayer1: boolean) => {
		return (
			<Column
				className={`column--other-gains ${isPlayer1 ? '' : 'right'}`}
				field={gainPeriod.data}
				body={isPlayer1 ? OtherGainsPlayer1 : OtherGainsPlayer2}
				header={otherGainsHeaderTemplate}
			/>
		);
	};

	const nameTemplate = (rowData: any) => (
		<div>{rs3DataArray[rowData.minigameId]}</div>
	);

	return (
		<DataTable className="p-datatable-striped" value={combinedGains}>
			<Column
				className="column--rank"
				body={(rowData: any) => rowData.player1Rank.toLocaleString()}
				field="player1Rank"
				header="Rank"
			/>
			<Column
				className="column--score"
				body={(rowData: any) => rowData.player1Score.toLocaleString()}
				field="player1Xp"
				header="Score"
			/>
			{OtherGainsColumn(true)}
			<Column className="column--name" body={nameTemplate} header="Minigame" />
			{OtherGainsColumn(false)}
			<Column
				className="column--score right"
				body={(rowData: any) => rowData.player2Score.toLocaleString()}
				field="player2Xp"
				header="Score"
			/>
			<Column
				className="column--rank right"
				body={(rowData: any) => rowData.player2Rank.toLocaleString()}
				field="player2Rank"
				header="Rank"
			/>
		</DataTable>
	);
};
