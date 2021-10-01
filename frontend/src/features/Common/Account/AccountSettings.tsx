import React, { FunctionComponent, useEffect, useState, useRef } from 'react';
import { useAppDispatch, useAppSelector } from '../../../app/hooks';
import { Button } from 'primereact/button';
import { InputText } from 'primereact/inputtext';
import { Toast } from 'primereact/toast';
import { ProgressBar } from 'primereact/progressbar';

import {
	selectUserRs3Rsn,
	selectUserLoading,
	getRs3Rsn,
	updateRs3Rsn,
} from 'features/Common/commonSlice';
import {
	accountSettingsRs3RsnText,
	accountSettingsRs3RsnUpdatePlaceholder,
	accountSettingsRs3RsnUpdateButtonText,
	formErrorToastLifetime,
	updateRsnErrorMessage,
	rsnMaxLength,
} from 'utils/constants';
import { isNullUndefinedOrWhitespace } from 'utils/helperFunctions';
import 'features/Common/common.scss';

type props = {
	handleClose: Function;
};

export const AccountSettings: FunctionComponent<props> = ({ handleClose }) => {
	const dispatch = useAppDispatch();
	const rs3Rsn = useAppSelector(selectUserRs3Rsn);
	const loading = useAppSelector(selectUserLoading);
	const [newRs3Rsn, setNewRs3Rsn] = useState('');
	const invalid =
		isNullUndefinedOrWhitespace(newRs3Rsn) || newRs3Rsn.length > rsnMaxLength;
	const toast = useRef<Toast>(null);

	useEffect(() => {
		if (isNullUndefinedOrWhitespace(rs3Rsn)) {
			dispatch(getRs3Rsn());
		}
	}, [rs3Rsn, dispatch]);

	const handleUpdateRs3Rsn = async () => {
		const result: any = await dispatch(updateRs3Rsn(newRs3Rsn));
		if (!result.payload?.data.success) {
			toast?.current?.show({
				severity: 'error',
				detail: updateRsnErrorMessage,
				life: formErrorToastLifetime,
			});
		}
		handleClose();
	};

	const rs3RsnString = isNullUndefinedOrWhitespace(rs3Rsn)
		? 'Error: Invalid RSN'
		: `${accountSettingsRs3RsnText} ${rs3Rsn}`;

	const rs3AccountDetails = loading ? (
		<ProgressBar
			className="progressbar--create-form p-mt-3"
			mode="indeterminate"
		/>
	) : (
		<form
			onSubmit={(e) => handleUpdateRs3Rsn()}
			className="p-d-flex p-flex-column"
		>
			<div className="container--rs3-account">
				<h2>{rs3RsnString}</h2>
				<InputText
					className="p-mb-3"
					placeholder={accountSettingsRs3RsnUpdatePlaceholder}
					value={newRs3Rsn}
					onChange={(e) => setNewRs3Rsn(e.target.value)}
				/>
				{loading ? (
					<ProgressBar
						className="progressbar--create-form p-mt-3"
						mode="indeterminate"
					/>
				) : (
					<Button
						label={accountSettingsRs3RsnUpdateButtonText}
						className="p-button-success btn--login-form"
						onClick={() => handleUpdateRs3Rsn()}
						disabled={invalid}
					/>
				)}
			</div>
		</form>
	);

	return (
		<div className="container--account-settings p-d-flex p-flex-column">
			<Toast ref={toast} />
			{rs3AccountDetails}
		</div>
	);
};
