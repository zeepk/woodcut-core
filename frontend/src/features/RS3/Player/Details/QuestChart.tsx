import React, { FunctionComponent, useState } from 'react';
import { useAppSelector } from 'app/hooks';

import { Doughnut } from 'react-chartjs-2';
import { InputSwitch } from 'primereact/inputswitch';

import {
	questPointsText,
	pointsRemainingText,
	questsCompleteText,
	questsRemainingText,
} from 'utils/constants';
import { selectQuestData } from 'features/RS3/rs3Slice';
import 'features/RS3/rs3.scss';

type props = {
	widthString: string;
};

export const QuestChart: FunctionComponent<props> = ({ widthString }) => {
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
		defaultFontColor: 'white',
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
				id="quest-chart"
				data={chartData}
				options={options}
				style={{ position: 'relative', width: widthString }}
			/>
			<div className="container--switch p-d-flex p-jc-center p-ai-center">
				<div
					className={`text--switch-label p-text-right p-mr-3 p-mt-4 ${
						!useQuestPoints ? 'active' : ''
					}`}
				>
					{questsCompleteText}
				</div>
				<InputSwitch
					className="p-mt-4"
					checked={useQuestPoints}
					onChange={(e) => setUseQuestPoints(e.value)}
				/>
				<div
					className={`text--switch-label p-text-left p-ml-3 p-mt-4 ${
						useQuestPoints ? 'active' : ''
					}`}
				>
					{questPointsText}
				</div>
			</div>
		</div>
	);
};
