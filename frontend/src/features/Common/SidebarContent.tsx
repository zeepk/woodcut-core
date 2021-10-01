import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { useAppSelector } from '../../app/hooks';

import { InputText } from 'primereact/inputtext';
import { ProgressBar } from 'primereact/progressbar';

import {
	selectPlayerCount,
	selectPlayerCountLoading,
} from 'features/Common/commonSlice';
import { NavbarItem } from './NavbarItem';
import { AuthButton } from 'features/Common/Account/AuthButton';
import { Vos } from './Vos';
import { Timers } from './Timers';
import { navbarMenuItems, playerCountText } from 'utils/constants';
import Logo from '../../assets/images/logo.png';
import 'features/Common/common.scss';

export function SidebarContent() {
	const history = useHistory();
	const playerCount = useAppSelector(selectPlayerCount);
	const playerCountLoading = useAppSelector(selectPlayerCountLoading);
	const [value, setValue] = useState('');
	const currentPlayerCount = `${playerCount.toLocaleString()} ${playerCountText}`;

	const handleSearch = (e: any) => {
		e.preventDefault();
		if (value.toString().trim() !== '') {
			history.push(`/rs3/user/${value.split(' ').join('+')}`);
		}
	};

	const navbarMenuItemComponents = navbarMenuItems.map((item) => (
		<div className="p-my-3" key={item.text}>
			<NavbarItem text={item.text} path={item.path} />
		</div>
	));

	const playerCountItem = playerCountLoading ? (
		<ProgressBar
			className="progressbar--player-count p-my-3"
			mode="indeterminate"
		/>
	) : (
		<div className="player-count p-my-3">{currentPlayerCount}</div>
	);

	return (
		<div className="container--sidebar p-d-flex p-flex-column p-ai-start p-jc-center">
			<div className="p-px-3">
				<Link to="/">
					<img className="img--logo p-ml-1 p-my-3" src={Logo} alt="logo" />
				</Link>
				<div className="p-my-3">
					<AuthButton sidebar={true} />
				</div>
				<form onSubmit={(e) => handleSearch(e)} className="p-my-3">
					<span className="p-input-icon-right">
						<i className="pi icon--search" />
						<InputText
							value={value}
							onChange={(e) => setValue(e.target.value)}
						/>
					</span>
				</form>
				{navbarMenuItemComponents}
			</div>
			<div className="space p-my-6" />
			<div className="p-px-3 nav-item stripe">{playerCountItem}</div>
			<div className="p-px-3 nav-item">
				<Timers />
			</div>
			<div className="p-px-3 nav-item stripe">
				<Vos />
			</div>
		</div>
	);
}
