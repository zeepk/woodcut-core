import React, { FunctionComponent } from 'react';
import { Chip } from 'primereact/chip';
import { badgeTypes } from 'utils/constants';
import 'features/Common/common.scss';

type props = {
	badge: number;
};

export const Badge: FunctionComponent<props> = ({ badge }) => {
	let badgeType = badgeTypes.find((bt) => bt.id === badge);

	if (!badgeType) {
		return <div />;
	}

	return (
		<Chip
			className="p-ml-2"
			style={{ backgroundColor: badgeType.color }}
			label={badgeType.text}
			image={badgeType.icon}
		/>
	);
};
