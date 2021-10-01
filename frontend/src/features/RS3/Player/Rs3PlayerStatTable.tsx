import React, { useState } from 'react';
import '../rs3.scss';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { useAppSelector } from '../../../app/hooks';
import { selectSkills } from 'features/RS3/rs3Slice';
import { skillIcon, skillNameArray } from 'utils/helperFunctions';
import { gainPeriods, isDxpActive } from 'utils/constants';
import { Rs3Skill } from '../rs3Types';
import { Dropdown } from 'primereact/dropdown';

const iconTemplate = (rowData: Rs3Skill) => {
	const icon = skillIcon(rowData.skillId);
	return (
		<span className="body--icon p-d-flex p-ai-center">
			{icon}
			{skillNameArray[rowData.skillId]}
		</span>
	);
};

const dayGainTemplate = (rowData: Rs3Skill) => {
	const gainClass = rowData.dayGain > 0 ? 'gain' : '';
	return <div className={gainClass}>{rowData.dayGain.toLocaleString()}</div>;
};

export default function Rs3PlayerStatTable() {
	const defaultIndex = isDxpActive ? gainPeriods.length - 1 : 0;
	const [gainPeriod, setGainPeriod] = useState(gainPeriods[defaultIndex]);
	const skills = useAppSelector(selectSkills);

	const otherGainsHeaderTemplate = (
		<Dropdown
			className="header--other-gains"
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

	const otherGainsTemplate = (rowData: Rs3Skill) => {
		const gain = rowData[gainPeriod.data];
		if (gain === null || gain === undefined) {
			return;
		}
		const gainClass = gain > 0 ? 'gain' : '';
		return <div className={gainClass}>{gain.toLocaleString()}</div>;
	};

	const levelTemplate = (rowData: Rs3Skill) => {
		const levelGain = rowData.levelGain > 0 ? `+${rowData.levelGain}` : '';
		return (
			<div className="p-d-flex">
				<div>{rowData.level}</div>
				<div className="p-ml-2 gain">{levelGain}</div>
			</div>
		);
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
		<DataTable className="p-datatable-striped" value={skills}>
			<Column className="column--icon" body={iconTemplate} header="Skill" />
			<Column
				className="column--rank"
				body={(rowData: Rs3Skill) => rowData.rank.toLocaleString()}
				field="rank"
				header="Rank"
				sortable
			/>
			<Column
				className="column--level"
				field="level"
				header="Level"
				sortable
				body={levelTemplate}
			/>
			<Column
				className="column--xp"
				body={(rowData: Rs3Skill) => rowData.xp.toLocaleString()}
				field="xp"
				header="XP"
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
