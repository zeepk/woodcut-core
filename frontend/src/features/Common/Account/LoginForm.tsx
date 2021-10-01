import React, { FunctionComponent, useState, useRef } from 'react';
import { useAppDispatch, useAppSelector } from '../../../app/hooks';
import { Button } from 'primereact/button';
import { InputText } from 'primereact/inputtext';
import { Password } from 'primereact/password';
import { ProgressBar } from 'primereact/progressbar';
import { Toast } from 'primereact/toast';
import {
	selectAuthLoginLoading,
	logIn,
	isUserLoggedIn,
	getRs3Rsn,
	getFollowing,
	requestPasswordReset,
	selectAuthResetLoading,
} from 'features/Common/commonSlice';
import {
	buttonTextLogin,
	loginFormPlaceholderEmail,
	loginFormPlaceholderPassword,
	formErrorToastLifetime,
	loginFormErrorMessage,
	buttonTextRequestReset,
	buttonTextForgotPassword,
	buttonTextHaveAccount,
	checkEmailText,
	passwordRequestResetErrorMessage,
	passwordResetRequestInvalidUserMessage,
} from 'utils/constants';
import { AuthDataLogin } from 'features/Common/commonTypes';
import { isNullUndefinedOrWhitespace } from 'utils/helperFunctions';
import 'features/Common/common.scss';

type props = {
	handleClose: Function;
};

export const LoginForm: FunctionComponent<props> = ({ handleClose }) => {
	const dispatch = useAppDispatch();
	const loading = useAppSelector(selectAuthLoginLoading);
	const resetLoading = useAppSelector(selectAuthResetLoading);
	const [email, setEmail] = useState('');
	const [password, setPassword] = useState('');
	const [showForgot, setShowForgot] = useState(false);
	const [showCheckEmail, setShowCheckEmail] = useState(false);
	const emailInvalid =
		isNullUndefinedOrWhitespace(email) ||
		!email.includes('@') ||
		!email.includes('.');
	const invalid = emailInvalid || isNullUndefinedOrWhitespace(password);
	const toast = useRef<Toast>(null);

	const handleLogin = async (e?: any) => {
		if (e) {
			e.preventDefault();
		}
		const authData = {
			email,
			password,
		};
		const result = await dispatch(logIn(authData));
		if (result.type.includes('rejected')) {
			// setTimeout(() => {
			toast?.current?.show({
				severity: 'error',
				detail: loginFormErrorMessage,
				life: formErrorToastLifetime,
			});
			// }, 1000);
		} else {
			dispatch(isUserLoggedIn());
			dispatch(getRs3Rsn());
			dispatch(getFollowing());

			handleClose();
		}
	};

	const handleForgot = async (e?: any) => {
		if (e) {
			e.preventDefault();
		}

		const payload: AuthDataLogin = {
			email,
			password: '',
		};
		const result: any = await dispatch(requestPasswordReset(payload));
		const errors = result.payload?.data.errors;
		if (errors) {
			toast?.current?.show({
				severity: 'error',
				detail: passwordResetRequestInvalidUserMessage,
				life: formErrorToastLifetime,
			});
			return;
		}
		if (result.type.includes('rejected')) {
			// setTimeout(() => {
			toast?.current?.show({
				severity: 'error',
				detail: passwordRequestResetErrorMessage,
				life: formErrorToastLifetime,
			});
			// }, 1000);
		} else {
			setShowCheckEmail(true);
		}
	};

	if (showCheckEmail) {
		return (
			<div className="p-d-flex p-flex-column p-text-center">
				<h4>{checkEmailText}</h4>
			</div>
		);
	}

	if (showForgot) {
		return (
			<div className="p-d-flex p-flex-column">
				<Toast ref={toast} />
				<form
					onSubmit={(e) => handleForgot(e)}
					className="p-d-flex p-flex-column"
				>
					<InputText
						className="p-mb-3"
						placeholder={loginFormPlaceholderEmail}
						value={email}
						onChange={(e) => setEmail(e.target.value)}
					/>
				</form>
				<div className="container--login-form-btn">
					{resetLoading ? (
						<ProgressBar
							className="progressbar--login-form p-mt-3"
							mode="indeterminate"
						/>
					) : (
						<Button
							label={buttonTextRequestReset}
							className="p-button-success btn--login-form"
							onClick={(e) => handleForgot(e)}
							disabled={emailInvalid}
						/>
					)}
				</div>
				<Button
					label={buttonTextHaveAccount}
					className="p-button-info p-button-outlined btn--toggle p-mt-6"
					onClick={() => setShowForgot(false)}
				/>
			</div>
		);
	}

	return (
		<div className="p-d-flex p-flex-column">
			<Toast ref={toast} />
			<form onSubmit={(e) => handleLogin(e)} className="p-d-flex p-flex-column">
				<InputText
					className="input--login p-mb-3"
					placeholder={loginFormPlaceholderEmail}
					value={email}
					onChange={(e) => setEmail(e.target.value)}
				/>
			</form>
			<form onSubmit={(e) => handleLogin(e)} className="p-d-flex p-flex-column">
				<Password
					feedback={false}
					className="input--login p-mb-3"
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
						onClick={(e) => handleLogin(e)}
						disabled={invalid}
					/>
				)}
			</div>
			<Button
				label={buttonTextForgotPassword}
				className="p-button-info p-button-outlined btn--toggle p-mt-6"
				onClick={() => setShowForgot(true)}
			/>
		</div>
	);
};
