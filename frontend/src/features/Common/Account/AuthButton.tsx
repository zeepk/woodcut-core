import React, { FunctionComponent, useState, useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../../app/hooks';
import {
	selectAuthIsLoggedIn,
	selectAuthLoading,
	isUserLoggedIn,
	logOut,
} from 'features/Common/commonSlice';
import { Button } from 'primereact/button';
import { ProgressBar } from 'primereact/progressbar';
import { Dialog } from 'primereact/dialog';
import { AccountSettings } from 'features/Common/Account/AccountSettings';
import { LoginButton } from 'features/Common/Account/LoginButton';
import { CreateAccountButton } from 'features/Common/Account/CreateAccountButton';
import { buttonTextLogout, buttonTextAccountSettings } from 'utils/constants';
import 'features/Common/common.scss';

type props = {
	sidebar: boolean;
};

export const AuthButton: FunctionComponent<props> = ({ sidebar }) => {
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
		setContent(<AccountSettings handleClose={() => setOpen(false)} />);
		setHeader(buttonTextAccountSettings);
		setOpen(true);
	};

	useEffect(() => {
		if (sidebar) {
			return;
		}
		dispatch(isUserLoggedIn());
	}, [dispatch, sidebar]);

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
			<div
				className={`p-d-flex p-jc-sm-${sidebar ? 'start' : 'end'} p-jc-start`}
			>
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
};
