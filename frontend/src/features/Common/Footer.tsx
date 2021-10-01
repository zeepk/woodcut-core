import React from 'react';
import { footerLinks } from 'utils/constants';

import 'features/Common/common.scss';

const links = footerLinks.map((f) => (
	<a key={f.id} className={`footer-link p-mr-3 p-p-2 ${f.class}`} href={f.link}>
		{f.text}
	</a>
));

export function Footer() {
	return (
		<div className="container--footer">
			<div className="p-d-flex flex-column flex-wrap p-jc-start">{links}</div>
		</div>
	);
}
