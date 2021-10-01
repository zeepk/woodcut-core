import React, { FunctionComponent } from 'react';
import { useHistory } from 'react-router-dom';
import { useAppSelector } from '../../app/hooks';

import { Button } from 'primereact/button';

import { selectUserRs3Rsn } from '../Common/commonSlice';
import { compareMeButtonText } from 'utils/constants';
import 'features/Common/common.scss';

type props = {
	playerToCompare: string;
};

export const CompareButton: FunctionComponent<props> = ({
	playerToCompare,
}) => {
	const history = useHistory();
	const currentUserRsn = useAppSelector(selectUserRs3Rsn);
	const linkPath = `/rs3/compare/${currentUserRsn}/${playerToCompare}`;

	const handleCompareClick = () => {
		history.push(linkPath);
	};

	return (
		<div className="p-d-flex p-jc-center p-ml-2">
			<Button
				label={compareMeButtonText}
				className="btn--compare p-p-2"
				onClick={() => handleCompareClick()}
			/>
		</div>
	);
};
