import React, { useState } from 'react';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { LoginForm } from 'features/Common/Account/LoginForm';
import { buttonTextLogin } from 'utils/constants';
import 'features/Common/common.scss';

export function LoginButton() {
	const [open, setOpen] = useState(false);

	return (
		<div className="p-d-flex">
			<Dialog
				className="dialog"
				header={buttonTextLogin}
				visible={open}
				onHide={() => setOpen(false)}
			>
				<LoginForm handleClose={() => setOpen(false)} />
			</Dialog>
			<Button
				label={buttonTextLogin}
				className="p-button-success btn--login p-p-2 p-ml-2"
				onClick={() => setOpen(true)}
			/>
		</div>
	);
}
