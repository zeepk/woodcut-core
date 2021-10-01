import React, { useState, useEffect } from 'react';
import { useHistory } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { useAppDispatch } from '../../app/hooks';

import { Menubar } from 'primereact/menubar';
import { InputText } from 'primereact/inputtext';
import { Sidebar } from 'primereact/sidebar';

import {
	getCurrentPlayerCount,
	getRs3Rsn,
	getFollowing,
} from 'features/Common/commonSlice';
import { NavbarItem } from './NavbarItem';
import { SidebarContent } from './SidebarContent';
import { AuthButton } from 'features/Common/Account/AuthButton';
import { navbarMenuItems } from 'utils/constants';
import Logo from '../../assets/images/logo.png';
import 'features/Common/common.scss';

export function Navbar() {
	const history = useHistory();
	const dispatch = useAppDispatch();
	const [value, setValue] = useState('');
	const [sidebarVisible, setSidebarVisible] = useState(false);

	useEffect(() => {
		dispatch(getCurrentPlayerCount());
		dispatch(getRs3Rsn());
		dispatch(getFollowing());
	}, [dispatch]);

	const handleSearch = (e: any) => {
		e.preventDefault();
		if (value.toString().trim() !== '') {
			history.push(`/rs3/user/${value.split(' ').join('+')}`);
		}
	};

	const navbarMenuItemComponents = navbarMenuItems.map((item) => {
		return {
			template: () => <NavbarItem text={item.text} path={item.path} />,
		};
	});

	const start = (
		<div className="p-d-flex p-jc-start p-ai-center">
			<i
				className="icon--sidebar pi pi-bars p-mr-3"
				onClick={() => setSidebarVisible(true)}
			/>
			<Link to="/">
				<img className="img--logo p-ml-1" src={Logo} alt="logo" />
			</Link>
		</div>
	);

	const navbarItems = [
		...navbarMenuItemComponents,
		{
			template: () => (
				<form
					className="container--search p-mr-2"
					onSubmit={(e) => handleSearch(e)}
				>
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
			template: () => <AuthButton sidebar={false} />,
			className: 'container--auth-buttons',
		},
	];

	return (
		<div>
			<Sidebar visible={sidebarVisible} onHide={() => setSidebarVisible(false)}>
				<SidebarContent />
			</Sidebar>
			<Menubar className="navbar" model={navbarItems} start={start} />;
		</div>
	);
}
