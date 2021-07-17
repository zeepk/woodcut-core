import React, { useState, useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { useAppDispatch, useAppSelector } from '../../app/hooks';

import { Menubar } from 'primereact/menubar';
import { InputText } from 'primereact/inputtext';
import { ProgressBar } from 'primereact/progressbar';

import {
	selectPlayerCount,
	selectPlayerCountLoading,
	getCurrentPlayerCount,
	getRs3Rsn,
	getFollowing,
} from 'features/Common/commonSlice';
import { NavbarItem } from './NavbarItem';
import { AuthButton } from './AuthButton';
import { navbarMenuItems, playerCountText } from 'utils/constants';
import Logo from '../../assets/images/logo.png';
import 'features/Common/common.scss';

export function Navbar() {
	const history = useHistory();
	const dispatch = useAppDispatch();
	const playerCount = useAppSelector(selectPlayerCount);
	const playerCountLoading = useAppSelector(selectPlayerCountLoading);
	const [value, setValue] = useState('');
	const currentPlayerCount = `${playerCount.toLocaleString()} ${playerCountText}`;

	useEffect(() => {
		dispatch(getCurrentPlayerCount());
		dispatch(getRs3Rsn());
		dispatch(getFollowing());
	}, [dispatch]);

	const handleSearch = (e: any) => {
		e.preventDefault();
		if (value.toString().trim() !== '') {
			history.push(`/rs3/${value.split(' ').join('+')}`);
		}
	};

	const navbarMenuItemComponents = navbarMenuItems.map((item) => {
		return {
			template: () => <NavbarItem text={item.text} path={item.path} />,
		};
	});

	const start = (
		<Link to="/">
			<img className="img--logo p-ml-1" src={Logo} alt="logo" />
		</Link>
	);

	const playerCountItem = playerCountLoading ? (
		<ProgressBar className="progressbar--player-count" mode="indeterminate" />
	) : (
		<div className="player-count">{currentPlayerCount}</div>
	);

	const navbarItems = [
		...navbarMenuItemComponents,
		{
			template: () => playerCountItem,
			className: 'container--player-count',
		},
		{
			template: () => (
				<form onSubmit={(e) => handleSearch(e)}>
					<span className="p-input-icon-right">
						<i className="pi icon--search" />
						<InputText
							value={value}
							onChange={(e) => setValue(e.target.value)}
						/>
					</span>
				</form>
			),
			className: 'container--search-form',
		},
		{
			template: () => <AuthButton />,
			className: 'container--auth-buttons',
		},
	];

	return <Menubar className="navbar" model={navbarItems} start={start} />;
}
