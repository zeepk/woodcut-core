import React, { FunctionComponent } from 'react';
import '../rs3.scss';
import { Rs3Skill } from 'features/RS3/rs3Types';
import { Gain } from 'features/RS3/Dashboard/Gain';
type props = {
	gains: Array<Rs3Skill>;
};

export const GainsList: FunctionComponent<props> = ({ gains }) => {
	return (
		<div className="container--list gains">
			{gains.map((gain, index) => (
				<Gain key={index} gain={gain} />
			))}
		</div>
	);
};
