import React, { FunctionComponent, useState } from 'react';
import { useAppSelector } from 'app/hooks';

import { Doughnut } from 'react-chartjs-2';
import { Dropdown } from 'primereact/dropdown';

import { milestones } from 'utils/constants';
import { calcSkillPercentage } from 'utils/helperFunctions';
import { selectSkills } from 'features/RS3/rs3Slice';
import 'features/RS3/rs3.scss';

type props = {
	widthString: string;
};

export const MilestoneChart: FunctionComponent<props> = ({ widthString }) => {
	const skills = useAppSelector(selectSkills);
	const [milestone, setMilestone] = useState(milestones[0]);
	const metrics = calcSkillPercentage(milestone.value, skills);
	const chartData = {
		labels: [
			'Current Xp',
			`${metrics.remainder.toLocaleString()} xp remaining`,
		],
		datasets: [
			{
				data: [metrics.xp, metrics.remainder],
				backgroundColor: ['#72be83', '#2d4e31'],
			},
		],
	};

	const options = {
		animation: {
			duration: 0,
		},
		tooltips: {
			callbacks: {
				label: (data: any) => {
					if (data.index === 0) {
						return `${metrics.xp.toLocaleString()} current xp`;
					}
					return `${metrics.remainder.toLocaleString()} xp remaining`;
				},
			},
		},
		plugins: {
			legend: {
				labels: {
					color: 'white',
				},
			},
		},
	};

	return (
		<div className="container--chart p-d-flex p-flex-column p-ai-center p-mb-6">
			<Doughnut
				id="milestone-chart"
				data={chartData}
				options={options}
				style={{ position: 'relative', width: widthString }}
			/>
			<Dropdown
				className="p-mt-4"
				value={milestone}
				options={milestones}
				placeholder={milestone.label}
				onChange={(e: any) =>
					setMilestone(
						milestones.find((ms) => ms.value === e.value) || milestones[0]
					)
				}
			/>
		</div>
	);
};
