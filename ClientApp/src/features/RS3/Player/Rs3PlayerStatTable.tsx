import React, { useState } from 'react';
import '../rs3.scss';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { useAppSelector } from '../../../app/hooks';
import { selectSkills } from '../../RS3/rs3Slice';
import { skillIcon, skillNameArray } from '../../../utils/helperFunctions';
import { gainPeriods } from '../../../utils/constants';
import { Rs3Skill } from '../rs3Types';
import { Dropdown } from 'primereact/dropdown';

const iconTemplate = (rowData: Rs3Skill) => {
	const icon = skillIcon(rowData.skillId);
	return (
		<span className="body--icon">
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
	const [gainPeriod, setGainPeriod] = useState(gainPeriods[0]);
	const skills = useAppSelector(selectSkills);

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

	const otherGainsTemplate = (rowData: Rs3Skill) => {
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
		<DataTable className="p-datatable-striped" value={skills}>
			<Column className="column--icon" body={iconTemplate} header="Skill" />
			<Column
				className="column--xp"
				body={(rowData: Rs3Skill) => rowData.xp.toLocaleString()}
				field="xp"
				header="XP"
				sortable
			/>
			<Column className="column--level" field="level" header="Level" sortable />
			<Column
				className="column--rank"
				body={(rowData: Rs3Skill) => rowData.rank.toLocaleString()}
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
