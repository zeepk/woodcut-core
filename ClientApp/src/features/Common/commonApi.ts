import requestWrapper from 'utils/requestWrapper';
import {
	checkUrl,
	loginUrl,
	createUrl,
	playerCountUrl,
	userRsnUrl,
	followUrl,
	unfollowUrl,
	followedUrl,
} from 'utils/constants';
import {
	AuthDataLogin,
	AuthDataCreateAccount,
} from 'features/Common/commonTypes';

export async function checkIfUserLoggedIn(token: string) {
	const requestOptions = {
		url: checkUrl,
		method: 'GET',
		token,
	};
	return await requestWrapper(requestOptions);
}

export async function logUserIn(data: AuthDataLogin) {
	const requestOptions = {
		url: loginUrl,
		method: 'POST',
		data,
	};
	return await requestWrapper(requestOptions);
}

export async function createUserAccount(data: AuthDataCreateAccount) {
	const requestOptions = {
		url: createUrl,
		method: 'POST',
		data,
	};
	return await requestWrapper(requestOptions);
}

export async function getPlayerCount() {
	const requestOptions = {
		url: playerCountUrl,
		method: 'GET',
	};
	return await requestWrapper(requestOptions);
}

export async function getUserRsn(token: string) {
	const requestOptions = {
		url: userRsnUrl,
		method: 'GET',
		token,
	};
	return await requestWrapper(requestOptions);
}

export async function getFollowed(token: string) {
	const requestOptions = {
		url: followedUrl,
		method: 'GET',
		token,
	};
	return await requestWrapper(requestOptions);
}

export async function updateUserRsn(token: string, data: string) {
	const requestOptions = {
		url: `${userRsnUrl}/${data}`,
		method: 'PUT',
		token,
	};
	return await requestWrapper(requestOptions);
}

export async function followPlayer(token: string, data: string) {
	const requestOptions = {
		url: `${followUrl}/${data}`,
		method: 'PUT',
		token,
	};
	return await requestWrapper(requestOptions);
}

export async function unfollowPlayer(token: string, data: string) {
	const requestOptions = {
		url: `${unfollowUrl}/${data}`,
		method: 'PUT',
		token,
	};
	return await requestWrapper(requestOptions);
}
