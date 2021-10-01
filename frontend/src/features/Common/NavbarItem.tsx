import React, { FunctionComponent } from 'react';
import { Link } from 'react-router-dom';
import 'features/Common/common.scss';

type props = {
	path: string;
	text: string;
};

export const NavbarItem: FunctionComponent<props> = ({ path, text }) => {
	return (
		<Link className="navbar-item" to={path}>
			{text}
		</Link>
	);
};
