import React, { useState, useRef } from 'react';
import { useLocation, useHistory } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../../../app/hooks';
import { selectAuthLoading } from 'features/Common/commonSlice';
import { Button } from 'primereact/button';
import { Password } from 'primereact/password';
import { ProgressBar } from 'primereact/progressbar';
import { Toast } from 'primereact/toast';
import {
	buttonTextResetPassword,
	resetFormPlaceholderPassword,
	resetFormPlaceholderConfirmPassword,
	passwordMatchErrorMessage,
	formErrorToastLifetime,
	passwordResetErrorMessage,
	passwordResetSuccessMessage,
} from 'utils/constants';
import { resetUserPassword } from 'features/Common/commonSlice';
import { isNullUndefinedOrWhitespace } from 'utils/helperFunctions';
import 'features/Common/common.scss';

function useQuery() {
	return new URLSearchParams(useLocation().search);
}

export const ResetPasswordForm = () => {
	const history = useHistory();
	const query = useQuery();
	const token = query.get('token')?.split(' ').join('+');
	const email = query.get('email')?.split(' ').join('+');
	const dispatch = useAppDispatch();
	const loading = useAppSelector(selectAuthLoading);
	const [password, setPassword] = useState('');
	const [confirmPassword, setConfirmPassword] = useState('');

	const passwordsMatch = password === confirmPassword;
	const fieldsBlank =
		isNullUndefinedOrWhitespace(confirmPassword) ||
		isNullUndefinedOrWhitespace(password);
	const invalid = fieldsBlank || !passwordsMatch;
	const showPasswordMatchError = !fieldsBlank && !passwordsMatch;

	const toast = useRef<Toast>(null);

	const handleReset = async (e?: any) => {
		if (e) {
			e.preventDefault();
		}
		if (invalid) {
			return;
		}

		const payload = {
			email,
			password,
			token,
		};
		const result: any = await dispatch(resetUserPassword(payload));
		const errors = result.payload?.data.errors;
		if (errors) {
			errors.forEach((err: any) => {
				toast?.current?.show({
					severity: 'error',
					detail: err.toString(),
					life: formErrorToastLifetime * errors.length,
				});
			});
			return;
		}

		if (result.type.includes('rejected')) {
			toast?.current?.show({
				severity: 'error',
				detail: passwordResetErrorMessage,
				life: formErrorToastLifetime,
			});
		} else {
			toast?.current?.show({
				severity: 'success',
				detail: passwordResetSuccessMessage,
				life: formErrorToastLifetime,
			});
			setPassword('');
			setConfirmPassword('');
			setTimeout(() => {
				history.push('');
			}, 3000);
		}
	};
	return (
		<div className="p-d-flex p-flex-column p-ai-center container--reset-form">
			<Toast ref={toast} />
			<div
				className={`text--error p-mb-3 ${!showPasswordMatchError && 'hidden'}`}
			>
				{passwordMatchErrorMessage}
			</div>
			<form onSubmit={(e) => handleReset(e)} className="p-d-flex p-flex-column">
				<Password
					className="p-mb-3"
					placeholder={resetFormPlaceholderPassword}
					value={password}
					onChange={(e) => setPassword(e.target.value)}
				/>
			</form>
			<form
				onSubmit={(e) => handleReset(e)}
				className="p-d-flex p-flex-column p-mb-6"
			>
				<Password
					className="p-mb-3"
					placeholder={resetFormPlaceholderConfirmPassword}
					value={confirmPassword}
					onChange={(e) => setConfirmPassword(e.target.value)}
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
						label={buttonTextResetPassword}
						className="p-button-success btn--create-form"
						onClick={() => handleReset()}
						disabled={invalid}
					/>
				)}
			</div>
		</div>
	);
};
