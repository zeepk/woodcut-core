import requestWrapper from '../../utils/requestWrapper';
import {
	gainsUrl,
	startTrackingUrl,
	questsUrl,
	detailsUrl,
	metricsUrl,
	activitiesUrl,
	followingActivitiesUrl,
} from '../../utils/constants';

export async function getStats(username: string) {
	const requestOptions = {
		url: `${gainsUrl}/${username}`,
		method: 'GET',
	};
	return await requestWrapper(requestOptions);
}

export async function getQuests(username: string) {
	const requestOptions = {
		url: `${questsUrl}/${username}`,
		method: 'GET',
	};
	return await requestWrapper(requestOptions);
}

export async function getDetails(username: string) {
	const requestOptions = {
		url: `${detailsUrl}/${username}`,
		method: 'GET',
	};
	return await requestWrapper(requestOptions);
}

export async function getMetrics(username: string) {
	const requestOptions = {
		url: `${metricsUrl}/${username}`,
		method: 'GET',
	};
	return await requestWrapper(requestOptions);
}

export async function startTrackingUser(username: string) {
	const requestOptions = {
		url: `${startTrackingUrl}/${username}`,
		method: 'PUT',
	};
	return await requestWrapper(requestOptions);
}

export async function getActivities(size?: number) {
	const params = {
		size,
	};

	const requestOptions = {
		url: activitiesUrl,
		method: 'GET',
		params,
	};
	return await requestWrapper(requestOptions);
}

export async function getFollowingActivities(token: string, size: number) {
	const params = {
		size,
	};

	const requestOptions = {
		url: followingActivitiesUrl,
		method: 'GET',
		params,
		token,
	};
	return await requestWrapper(requestOptions);
}
