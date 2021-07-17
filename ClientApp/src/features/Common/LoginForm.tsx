import React, { FunctionComponent, useState, useRef } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { Button } from 'primereact/button';
import { InputText } from 'primereact/inputtext';
import { Password } from 'primereact/password';
import { ProgressBar } from 'primereact/progressbar';
import { Toast } from 'primereact/toast';
import {
	selectAuthLoginLoading,
	selectAuthErrorMessage,
	logIn,
	isUserLoggedIn,
	getRs3Rsn,
} from '../Common/commonSlice';
import {
	buttonTextLogin,
	loginFormPlaceholderEmail,
	loginFormPlaceholderPassword,
	formErrorToastLifetime,
} from 'utils/constants';
import { isNullUndefinedOrWhitespace } from 'utils/helperFunctions';
import './common.scss';

type props = {
	handleClose: Function;
};

export const LoginForm: FunctionComponent<props> = ({ handleClose }) => {
	const dispatch = useAppDispatch();
	const loading = useAppSelector(selectAuthLoginLoading);
	const errorMessage = useAppSelector(selectAuthErrorMessage);
	const [email, setEmail] = useState('');
	const [password, setPassword] = useState('');
	const invalid =
		isNullUndefinedOrWhitespace(email) || isNullUndefinedOrWhitespace(password);
	const toast = useRef<Toast>(null);

	const handleLogin = async () => {
		const authData = {
			email,
			password,
		};
		const result = await dispatch(logIn(authData));
		if (result.type.includes('rejected')) {
			setTimeout(() => {
				toast?.current?.show({
					severity: 'error',
					detail: errorMessage,
					life: formErrorToastLifetime,
				});
			}, 100);
		} else {
			dispatch(isUserLoggedIn());
			dispatch(getRs3Rsn());
			handleClose();
		}
	};

	return (
		<div className="p-d-flex p-flex-column">
			<Toast ref={toast} />
			<form onSubmit={(e) => handleLogin()} className="p-d-flex p-flex-column">
				<InputText
					className="p-mb-3"
					placeholder={loginFormPlaceholderEmail}
					value={email}
					onChange={(e) => setEmail(e.target.value)}
				/>
				<Password
					feedback={false}
					className="p-mb-3"
					placeholder={loginFormPlaceholderPassword}
					value={password}
					onChange={(e) => setPassword(e.target.value)}
				/>
			</form>
			<div className="container--login-form-btn">
				{loading ? (
					<ProgressBar
						className="progressbar--login-form p-mt-3"
						mode="indeterminate"
					/>
				) : (
					<Button
						label={buttonTextLogin}
						className="p-button-success btn--login-form"
						onClick={() => handleLogin()}
						disabled={invalid}
					/>
				)}
			</div>
		</div>
	);
};
