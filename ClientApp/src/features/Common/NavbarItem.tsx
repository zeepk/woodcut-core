import React, { FunctionComponent } from 'react'; // importing FunctionComponent
import { Link } from 'react-router-dom';
import './common.scss';

type props = {
	path: string;
	text: string;
};

export const NavbarItem: FunctionComponent<props> = ({ path, text }) => {
	return (
		<Link className="navbar-item p-mx-4" to={path}>
			{text}
		</Link>
	);
};
