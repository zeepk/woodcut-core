import React, { useState, useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import {
	selectAuthIsLoggedIn,
	selectAuthLoading,
	isUserLoggedIn,
	logOut,
} from '../Common/commonSlice';
import { Button } from 'primereact/button';
import { ProgressBar } from 'primereact/progressbar';
import { Dialog } from 'primereact/dialog';
import { AccountSettings } from 'features/Common/AccountSettings';
import { LoginButton } from 'features/Common/LoginButton';
import { CreateAccountButton } from 'features/Common/CreateAccountButton';
import { buttonTextLogout, buttonTextAccountSettings } from 'utils/constants';
import './common.scss';

export function AuthButton() {
	const dispatch = useAppDispatch();
	const loading = useAppSelector(selectAuthLoading);
	const isLoggedIn = useAppSelector(selectAuthIsLoggedIn);
	const [open, setOpen] = useState(false);
	const [header, setHeader] = useState('');
	const [content, setContent] = useState(<div />);

	const handleLogout = () => {
		dispatch(logOut());
	};

	const handleAccountSettings = () => {
		setContent(<AccountSettings />);
		setHeader(buttonTextAccountSettings);
		setOpen(true);
	};

	useEffect(() => {
		dispatch(isUserLoggedIn());
	}, [dispatch]);

	if (loading) {
		return <ProgressBar className="progressbar" mode="indeterminate" />;
	}

	let buttons = (
		<div className="p-d-flex">
			<CreateAccountButton />
			<LoginButton />
		</div>
	);

	if (isLoggedIn) {
		buttons = (
			<div className="p-d-flex p-jc-sm-end p-jc-start p-ml-2">
				<Button
					label={buttonTextAccountSettings}
					className="p-button-info btn--account-settings p-p-2"
					onClick={() => handleAccountSettings()}
				/>
				<Button
					label={buttonTextLogout}
					className="p-button-danger btn--logout p-p-2 p-ml-2"
					onClick={() => handleLogout()}
				/>
			</div>
		);
	}

	return (
		<div>
			<Dialog header={header} visible={open} onHide={() => setOpen(false)}>
				{content}
			</Dialog>
			{buttons}
		</div>
	);
}
