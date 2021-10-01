import React, { FunctionComponent } from 'react';
import Iron from 'assets/images/ironIcon.png';
import Hcim from 'assets/images/hcimIcon.png';
import { accountTypes } from 'utils/constants';
import 'features/RS3/rs3.scss';

type props = {
	accountType: accountTypes;
};

export const IronIcon: FunctionComponent<props> = ({ accountType }) => {
	if (
		accountType === accountTypes.IRON ||
		accountType === accountTypes.DEADIRON
	) {
		return (
			<img className="icon--iron im" src={Iron} alt="Ironman" title="Ironman" />
		);
	}

	if (accountType === accountTypes.HCIM) {
		return (
			<img
				className="icon--iron hcim"
				src={Hcim}
				alt="Hardcore Ironman"
				title="Hardcore Ironman"
			/>
		);
	}

	return <div />;
};
