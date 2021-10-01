import React, { FunctionComponent } from 'react';
import '../rs3.scss';
import { Rs3Activity } from 'features/RS3/rs3Types';
import { Activity } from 'features/RS3/Dashboard/Activity';
import { noActivitiesFoundText } from 'utils/constants';
type props = {
	activities: Array<Rs3Activity>;
};

export const ActivityList: FunctionComponent<props> = ({ activities }) => {
	if (
		activities === undefined ||
		activities === null ||
		activities.length === 0
	) {
		return <h3>{noActivitiesFoundText}</h3>;
	}

	return (
		<div className="container--list activities">
			{activities.map((activity, index) => (
				<Activity key={index} activity={activity} />
			))}
		</div>
	);
};
