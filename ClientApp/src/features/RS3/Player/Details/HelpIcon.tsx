import React from 'react';
import { Button } from 'primereact/button';
// import { Tooltip } from 'primereact/tooltip';
import 'features/RS3/rs3.scss';
import { helpIconRunemetricsPrivateText } from 'utils/constants';

export default function HelpIcon() {
	return (
		<Button
			type="button"
			icon="pi pi-bell"
			className="p-button-rounded p-button-warning p-ml-2"
			tooltip={helpIconRunemetricsPrivateText}
		/>
	);
}
