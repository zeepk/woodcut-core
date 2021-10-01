import React, { FunctionComponent } from 'react';

import '../rs3.scss';
import { Rs3Skill } from 'features/RS3/rs3Types';
import { skillIcon, skillNameArray } from 'utils/helperFunctions';
type props = {
	gain: Rs3Skill;
};

export const Gain: FunctionComponent<props> = ({ gain }) => {
	const levelsString = gain.levelGain > 1 ? 'levels' : 'level';
	const levelGain =
		gain.levelGain > 0 ? `+${gain.levelGain} ${levelsString}` : '';
	return (
		<div className="container--gain p-d-flex p-jc-between p-ai-center p-p-3">
			<div className="container--skill p-d-flex p-ai-center">
				<div className="container--icon">{skillIcon(gain.skillId)}</div>
				<div className="text--skill-name p-ml-2">
					{skillNameArray[gain.skillId]}
				</div>
			</div>
			<div className="p-d-flex p-flex-column p-ai-end">
				<div className="text--gain">{`+${gain.dayGain.toLocaleString()} xp`}</div>
				<div className="text--gain">{levelGain}</div>
			</div>
		</div>
	);
};
