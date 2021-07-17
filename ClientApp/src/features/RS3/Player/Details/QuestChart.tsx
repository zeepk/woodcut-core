import React, { useState } from 'react';
import { useAppSelector } from 'app/hooks';

import { Chart } from 'primereact/chart';
import { InputSwitch } from 'primereact/inputswitch';

import {
	questPointsText,
	pointsRemainingText,
	questsCompleteText,
	questsRemainingText,
} from 'utils/constants';
import { selectQuestData } from 'features/RS3/rs3Slice';
import 'features/RS3/rs3.scss';

export default function QuestChart() {
	const questData = useAppSelector(selectQuestData);
	const [useQuestPoints, setUseQuestPoints] = useState(false);

	const chartData = {
		labels: [questsCompleteText, questsRemainingText],
		datasets: [
			{
				data: [
					questData.completedQuests,
					questData.totalQuests - questData.completedQuests,
				],
				backgroundColor: ['#36A2EB', '#8bc1e6'],
			},
		],
	};

	if (useQuestPoints) {
		chartData.labels = [questPointsText, pointsRemainingText];
		chartData.datasets[0].data = [
			questData.questPoints,
			questData.totalQuestPoints - questData.questPoints,
		];
	}

	const options = {
		animation: {
			duration: 0,
		},
	};

	return (
		<div className="container--chart p-d-flex p-flex-column p-ai-center p-mb-6">
			<Chart
				type="doughnut"
				data={chartData}
				options={options}
				style={{ position: 'relative', width: '40rem' }}
			/>
			<div className="container--switch p-d-flex p-jc-center p-ai-center">
				<div
					className={`text--switch-label p-text-right p-mr-3 p-mt-1 ${
						!useQuestPoints ? 'active' : ''
					}`}
				>
					{questsCompleteText}
				</div>
				<InputSwitch
					className="p-mt-2"
					checked={useQuestPoints}
					onChange={(e) => setUseQuestPoints(e.value)}
				/>
				<div
					className={`text--switch-label p-text-left p-ml-3 p-mt-1 ${
						useQuestPoints ? 'active' : ''
					}`}
				>
					{questPointsText}
				</div>
			</div>
		</div>
	);
}
