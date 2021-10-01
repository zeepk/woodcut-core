import React from 'react';
import { useAppSelector } from '../../app/hooks';

import { selectAuthIsLoggedIn, selectAuthLoading } from '../Common/commonSlice';
import NewUserLandingPage from 'features/RS3/Dashboard/NewUserLandingPage';
import ExistingUserLandingPage from 'features/RS3/Dashboard/ExistingUserLandingPage';
import LoadingIcon from 'features/Common/LoadingIcon';
import 'features/Common/common.scss';

export function LandingPage() {
	const isLoggedIn = useAppSelector(selectAuthIsLoggedIn);
	const loading = useAppSelector(selectAuthLoading);

	if (loading) {
		return <LoadingIcon fullScreen={true} />;
	}

	if (isLoggedIn) {
		return <ExistingUserLandingPage />;
	}

	return <NewUserLandingPage />;
}
