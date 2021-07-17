import React, { FunctionComponent, useState, useRef } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { selectAuthCreateLoading, createAccount } from '../Common/commonSlice';
import { Button } from 'primereact/button';
import { InputText } from 'primereact/inputtext';
import { Password } from 'primereact/password';
import { ProgressBar } from 'primereact/progressbar';
import { Toast } from 'primereact/toast';
import {
	buttonTextCreateAccount,
	loginFormPlaceholderUsername,
	loginFormPlaceholderPassword,
	loginFormPlaceholderEmail,
	formErrorToastLifetime,
} from 'utils/constants';
import { isNullUndefinedOrWhitespace } from 'utils/helperFunctions';
import './common.scss';

type props = {
	handleClose: Function;
};

export const CreateForm: FunctionComponent<props> = ({ handleClose }) => {
	const dispatch = useAppDispatch();
	const loading = useAppSelector(selectAuthCreateLoading);
	const [email, setEmail] = useState('');
	const [username, setUsername] = useState('');
	const [password, setPassword] = useState('');
	const invalid =
		isNullUndefinedOrWhitespace(email) ||
		isNullUndefinedOrWhitespace(password) ||
		isNullUndefinedOrWhitespace(username);
	const toast = useRef<Toast>(null);

	const handleCreate = async () => {
		const authData = {
			email,
			password,
			username,
		};
		const result: any = await dispatch(createAccount(authData));
		if (result.payload.data.errors) {
			toast?.current?.show({
				severity: 'error',
				detail: result.payload.data.errors[0],
				life: formErrorToastLifetime,
			});
		} else {
			handleClose();
		}
	};

	return (
		<div className="p-d-flex p-flex-column">
			<Toast ref={toast} />
			<form onSubmit={(e) => handleCreate()} className="p-d-flex p-flex-column">
				<InputText
					className="p-mb-3"
					placeholder={loginFormPlaceholderUsername}
					value={username}
					onChange={(e) => setUsername(e.target.value)}
				/>
				<InputText
					className="p-mb-3"
					placeholder={loginFormPlaceholderEmail}
					value={email}
					onChange={(e) => setEmail(e.target.value)}
				/>
				<Password
					className="p-mb-3"
					placeholder={loginFormPlaceholderPassword}
					value={password}
					onChange={(e) => setPassword(e.target.value)}
				/>
			</form>
			<div className="container--create-form-btn">
				{loading ? (
					<ProgressBar
						className="progressbar--create-form p-mt-3"
						mode="indeterminate"
					/>
				) : (
					<Button
						label={buttonTextCreateAccount}
						className="p-button-success btn--create-form"
						onClick={() => handleCreate()}
						disabled={invalid}
					/>
				)}
			</div>
		</div>
	);
};
