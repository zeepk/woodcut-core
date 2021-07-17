import React, { useState } from 'react';
import '../rs3.scss';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { useAppSelector } from '../../../app/hooks';
import { selectMinigames } from 'features/RS3/rs3Slice';
import { rs3DataArray } from 'utils/helperFunctions';
import { gainPeriods } from 'utils/constants';
import { Rs3Minigame } from '../rs3Types';
import { Dropdown } from 'primereact/dropdown';

const nameTemplate = (rowData: Rs3Minigame) => (
	<div>{rs3DataArray[rowData.minigameId]}</div>
);
const dayGainTemplate = (rowData: Rs3Minigame) => {
	const gainClass = rowData.dayGain > 0 ? 'gain' : '';
	return <div className={gainClass}>{rowData.dayGain.toLocaleString()}</div>;
};

export default function Rs3PlayerStatTable() {
	const [gainPeriod, setGainPeriod] = useState(gainPeriods[0]);
	const minigames = useAppSelector(selectMinigames);

	const otherGainsHeaderTemplate = (
		<Dropdown
			value={gainPeriod}
			options={gainPeriods}
			placeholder={gainPeriod.label}
			onChange={(e) =>
				setGainPeriod(
					gainPeriods.find((gp) => gp.value === e.value) || gainPeriods[0],
				)
			}
		/>
	);

	const otherGainsTemplate = (rowData: Rs3Minigame) => {
		const gain = rowData[gainPeriod.data];
		if (gain === undefined) {
			return;
		}
		const gainClass = gain > 0 ? 'gain' : '';
		return <div className={gainClass}>{gain.toLocaleString()}</div>;
	};

	const OtherGainsColumn = (
		<Column
			className="column--other-gains"
			field={gainPeriod.data}
			body={otherGainsTemplate}
			header={otherGainsHeaderTemplate}
		/>
	);

	return (
		<DataTable className="p-datatable-striped" value={minigames}>
			<Column className="column--icon" body={nameTemplate} header="Skill" />
			<Column className="column--level" field="score" header="Score" sortable />
			<Column
				className="column--rank"
				body={(rowData: Rs3Minigame) => rowData.rank.toLocaleString()}
				field="rank"
				header="Rank"
				sortable
			/>
			<Column
				className="column--day"
				field="dayGain"
				body={dayGainTemplate}
				header="Day"
				sortable
			/>
			{OtherGainsColumn}
		</DataTable>
	);
}
