import React, { FunctionComponent, useState } from 'react';
import { avatarUrlPre, avatarUrlPost } from 'utils/constants';
import { ProgressBar } from 'primereact/progressbar';
import 'features/RS3/rs3.scss';

type props = {
	username: string;
};

export const Avatar: FunctionComponent<props> = ({ username }) => {
	const [avatarLoading, updateAvatarLoading] = useState(true);

	return (
		<div className="container--avatar">
			<img
				className="img--avatar"
				src={`${avatarUrlPre}${username.split(' ').join('+')}${avatarUrlPost}`}
				alt={'player avatar'}
				onLoad={() => updateAvatarLoading(false)}
			/>
			{avatarLoading ? (
				<ProgressBar className="progressbar--avatar" mode="indeterminate" />
			) : (
				<div />
			)}
		</div>
	);
};
