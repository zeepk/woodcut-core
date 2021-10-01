import React, { useState } from 'react';
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { CreateForm } from 'features/Common/Account/CreateForm';
import { buttonTextCreateAccount } from 'utils/constants';
import 'features/Common/common.scss';

export function CreateAccountButton() {
	const [open, setOpen] = useState(false);

	return (
		<div className="p-d-flex">
			<Dialog
				header={buttonTextCreateAccount}
				visible={open}
				onHide={() => setOpen(false)}
			>
				<CreateForm handleClose={() => setOpen(false)} />
			</Dialog>
			<Button
				label={buttonTextCreateAccount}
				className="p-button-info btn--create p-p-2"
				onClick={() => setOpen(true)}
			/>
		</div>
	);
}
