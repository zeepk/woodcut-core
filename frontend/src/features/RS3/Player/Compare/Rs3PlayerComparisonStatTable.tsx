import React, { FunctionComponent, useState } from 'react';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Dropdown } from 'primereact/dropdown';
import 'features/RS3/rs3.scss';
import { skillIcon } from 'utils/helperFunctions';
import { Rs3Skill } from 'features/RS3/rs3Types';
import { comparisonGainPeriods, isDxpActive } from 'utils/constants';
type props = {
	player1Gains: Array<Rs3Skill>;
	player2Gains: Array<Rs3Skill>;
};

export const Rs3PlayerComparisonStatTable: FunctionComponent<props> = ({
	player1Gains,
	player2Gains,
}) => {
	const defaultIndex = isDxpActive ? comparisonGainPeriods.length - 1 : 0;
	const [gainPeriod, setGainPeriod] = useState(
		comparisonGainPeriods[defaultIndex],
	);

	const combinedGains = player1Gains.map((g, i) => {
		return {
			skillId: g.skillId,
			player1Xp: g.xp,
			player1Level: g.level,
			player1LevelGain: g.levelGain,
			player1Rank: g.rank,
			dayGain1: g.dayGain,
			weekGain1: g.weekGain,
			monthGain1: g.monthGain,
			yearGain1: g.yearGain,
			dxpGain1: g.dxpGain,
			player2Xp: player2Gains[i].xp,
			player2Level: player2Gains[i].level,
			player2LevelGain: player2Gains[i].levelGain,
			player2Rank: player2Gains[i].rank,
			dayGain2: player2Gains[i].dayGain,
			weekGain2: player2Gains[i].weekGain,
			monthGain2: player2Gains[i].monthGain,
			yearGain2: player2Gains[i].yearGain,
			dxpGain2: player2Gains[i].dxpGain,
		};
	});

	const levelTemplate = (rowData: any) => {
		const higher = rowData.player1Level > rowData.player2Level;
		return (
			<div className="p-d-flex">
				<div className={higher ? 'gain' : ''}>{rowData.player1Level}</div>
			</div>
		);
	};

	const level2Template = (rowData: any) => {
		const higher = rowData.player1Level < rowData.player2Level;
		return (
			<div className="p-d-flex p-jc-end">
				<div className={higher ? 'gain' : ''}>{rowData.player2Level}</div>
			</div>
		);
	};

	const iconTemplate = (rowData: any) => {
		const icon = skillIcon(rowData.skillId);
		const player1Higher = rowData.player1Xp > rowData.player2Xp;

		return (
			<span className="body--icon p-d-flex p-ai-center">
				<span
					className={`${player1Higher ? 'gain' : 'hidden'} icon--higher p-mr-2`}
				>
					<i className="pi pi-caret-left" />
				</span>
				{icon}
				<span
					className={`${
						!player1Higher ? 'gain' : 'hidden'
					} icon--higher p-ml-2`}
				>
					<i className="pi pi-caret-right" />
				</span>
			</span>
		);
	};

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

	const xpTemplate = (xp: Number, higher: boolean) => {
		return <div className={higher ? 'gain' : ''}>{xp.toLocaleString()}</div>;
	};

	const XpPlayer1 = (rowData: any) => {
		const gain = rowData.player1Xp;
		const higher = gain > rowData.player2Xp;
		return xpTemplate(gain, higher);
	};

	const XpPlayer2 = (rowData: any) => {
		const gain = rowData.player2Xp;
		const higher = gain > rowData.player1Xp;
		return xpTemplate(gain, higher);
	};

	const rankTemplate = (rank: Number, higher: boolean) => {
		return <div className={higher ? 'gain' : ''}>{rank.toLocaleString()}</div>;
	};

	const RankPlayer1 = (rowData: any) => {
		const gain = rowData.player1Rank;
		const higher = gain < rowData.player2Rank;
		return rankTemplate(gain, higher);
	};

	const RankPlayer2 = (rowData: any) => {
		const gain = rowData.player2Rank;
		const higher = gain < rowData.player1Rank;
		return rankTemplate(gain, higher);
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

	return (
		<DataTable className="p-datatable-striped" value={combinedGains}>
			<Column
				className="column--rank"
				body={RankPlayer1}
				field="player1Rank"
				header="Rank"
			/>
			<Column
				className="column--xp"
				body={XpPlayer1}
				field="player1Xp"
				header="XP"
			/>
			<Column
				className="column--level"
				field="player1Level"
				header="Level"
				body={levelTemplate}
			/>
			{OtherGainsColumn(true)}
			<Column className="column--icon" body={iconTemplate} header="Skill" />
			{OtherGainsColumn(false)}
			<Column
				className="column--level right"
				field="player2Level"
				header="Level"
				body={level2Template}
			/>
			<Column
				className="column--xp right"
				body={XpPlayer2}
				field="player2Xp"
				header="XP"
			/>
			<Column
				className="column--rank right"
				body={RankPlayer2}
				field="player2Rank"
				header="Rank"
			/>
		</DataTable>
	);
};
