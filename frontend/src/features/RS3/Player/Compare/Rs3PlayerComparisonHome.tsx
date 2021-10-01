import React, { useState, useRef } from 'react';
import { useHistory } from 'react-router-dom';
import { InputText } from 'primereact/inputtext';
import { Button } from 'primereact/button';
import { Toast } from 'primereact/toast';

import {
	comparisonSearchText,
	compareSearchErrorText,
	formErrorToastLifetime,
	usernameMaxLength,
} from 'utils/constants';
import 'features/RS3/rs3.scss';

export default function Rs3PlayerComparisonHome() {
	const history = useHistory();
	const toast = useRef<Toast>(null);

	const [name1, setName1] = useState('');
	const [name2, setName2] = useState('');

	const handleSearch = (e: any) => {
		e.preventDefault();

		const invalid =
			name1.trim().length > usernameMaxLength ||
			name1.trim().length < 1 ||
			name2.trim().length > usernameMaxLength ||
			name2.trim().length < 1;

		if (invalid) {
			toast?.current?.show({
				severity: 'error',
				detail: compareSearchErrorText,
				life: formErrorToastLifetime,
			});

			return;
		}

		history.push(`/rs3/compare/${name1}/${name2}`);
	};

	return (
		<div className="container--comparison-home p-d-flex p-flex-column p-ai-center">
			<Toast ref={toast} />
			<form
				className="p-d-flex p-flex-column p-ai-center"
				onSubmit={(e) => handleSearch(e)}
			>
				<InputText
					placeholder="Player 1"
					className="p-mt-6"
					value={name1}
					onChange={(e) => setName1(e.target.value)}
				/>
			</form>
			<form
				className="p-d-flex p-flex-column p-ai-center"
				onSubmit={(e) => handleSearch(e)}
			>
				<InputText
					placeholder="Player 2"
					className="p-mt-2"
					value={name2}
					onChange={(e) => setName2(e.target.value)}
				/>
			</form>
			<Button
				label={comparisonSearchText}
				className="p-button-info btn--compare-search p-py-2 p-px-6 p-mt-2"
				onClick={(e) => handleSearch(e)}
			/>
		</div>
	);
}
