import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { RootState } from '../../app/store';
import {
	getStats,
	startTrackingUser,
	getQuests,
	getDetails,
	getMetrics,
	getActivities,
	getFollowingActivities,
	likeActivity,
	unlikeActivity,
	getIronStatus,
} from './rs3Api';
import { Rs3Skill, Rs3Minigame, Rs3Activity } from './rs3Types';
import {
	questBadgeId,
	rs3HomePageActivities,
	accountTypes,
} from 'utils/constants';
import { getToken } from 'features/Common/commonSlice';

export interface Rs3State {
	player: {
		username: string;
		clanname: string;
		isTracking: boolean;
		success: boolean;
		quests: {
			totalQuests: number;
			completedQuests: number;
			questPoints: number;
			totalQuestPoints: number;
		};
		skills: Array<Rs3Skill>;
		minigames: Array<Rs3Minigame>;
		badges: Array<number>;
		activities: Array<Rs3Activity>;
		runemetricsEnabled: boolean;
		playerLoading: number;
		ironmanStatus: accountTypes;
	};
	compare: {
		player1: {
			skills: Array<Rs3Skill>;
			minigames: Array<Rs3Minigame>;
			playerLoading: boolean;
			ironmanStatus: accountTypes;
			success: boolean;
		};
		player2: {
			skills: Array<Rs3Skill>;
			minigames: Array<Rs3Minigame>;
			playerLoading: boolean;
			ironmanStatus: accountTypes;
			success: boolean;
		};
	};
	dashboard: {
		activitiesLoading: boolean;
		activities: Array<Rs3Activity>;
	};
	status: 'idle' | 'loading' | 'failed';
}

const initialState: Rs3State = {
	status: 'idle',
	player: {
		username: '',
		clanname: '',
		success: true,
		isTracking: false,
		quests: {
			totalQuests: 0,
			completedQuests: 0,
			questPoints: 0,
			totalQuestPoints: 0,
		},
		skills: [],
		minigames: [],
		badges: [],
		activities: [],
		runemetricsEnabled: true,
		playerLoading: 0,
		ironmanStatus: accountTypes.MAIN,
	},
	compare: {
		player1: {
			skills: [],
			minigames: [],
			playerLoading: false,
			ironmanStatus: accountTypes.MAIN,
			success: true,
		},
		player2: {
			skills: [],
			minigames: [],
			playerLoading: false,
			ironmanStatus: accountTypes.MAIN,
			success: true,
		},
	},
	dashboard: {
		activitiesLoading: false,
		activities: [],
	},
};

export const getXpGains = createAsyncThunk(
	'rs3/totalXp',
	async (username: string) => {
		const response = await getStats(username);
		// The value we return becomes the `fulfilled` action payload
		return response;
	},
);

export const getPlayerQuests = createAsyncThunk(
	'rs3/quests',
	async (username: string) => {
		const response = await getQuests(username);
		// The value we return becomes the `fulfilled` action payload
		return response;
	},
);

export const getPlayerIronStatus = createAsyncThunk(
	'rs3/ironstatus',
	async (username: string) => {
		const response = await getIronStatus(username);
		// The value we return becomes the `fulfilled` action payload
		return response;
	},
);

export const getPlayer1IronStatus = createAsyncThunk(
	'rs3/ironstatus1',
	async (username: string) => {
		const response = await getIronStatus(username);
		// The value we return becomes the `fulfilled` action payload
		return response;
	},
);

export const getPlayer2IronStatus = createAsyncThunk(
	'rs3/ironstatus2',
	async (username: string) => {
		const response = await getIronStatus(username);
		// The value we return becomes the `fulfilled` action payload
		return response;
	},
);

export const getPlayerDetails = createAsyncThunk(
	'rs3/details',
	async (username: string) => {
		const response = await getDetails(username);
		// The value we return becomes the `fulfilled` action payload
		return response;
	},
);

export const getPlayerMetrics = createAsyncThunk(
	'rs3/metrics',
	async (username: string) => {
		const response = await getMetrics(username);
		// The value we return becomes the `fulfilled` action payload
		return response;
	},
);

export const startTrackingForUser = createAsyncThunk(
	'rs3/tracking',
	async (arg, { getState, requestId }) => {
		// not using other params, but function won't work without them
		const state: any = getState();
		const username = state.rs3.player.username;
		const response = await startTrackingUser(username);
		return response;
	},
);

export const getRs3Activities = createAsyncThunk(
	'rs3/activities',
	async (size?: number) => {
		const response = await getActivities(size);
		// The value we return becomes the `fulfilled` action payload
		return response;
	},
);

export const getRs3FollowingActivities = createAsyncThunk(
	'rs3/followingactivities',
	async () => {
		const token = getToken();
		if (!token) {
			return;
		}

		const response = await getFollowingActivities(token, rs3HomePageActivities);
		// The value we return becomes the `fulfilled` action payload
		return response;
	},
);

export const likeRs3Activity = createAsyncThunk(
	'rs3/likeactivity',
	async (id: number) => {
		const token = getToken();
		if (!token) {
			return;
		}

		const response = await likeActivity(token, id);
		// The value we return becomes the `fulfilled` action payload
		return response;
	},
);

export const unlikeRs3Activity = createAsyncThunk(
	'rs3/unlikeactivity',
	async (id: number) => {
		const token = getToken();
		if (!token) {
			return;
		}

		const response = await unlikeActivity(token, id);
		// The value we return becomes the `fulfilled` action payload
		return response;
	},
);

export const getXpGainsForPlayer1 = createAsyncThunk(
	'rs3/totalXpPlayer1',
	async (username: string) => {
		const response = await getStats(username);
		return response;
	},
);

export const getXpGainsForPlayer2 = createAsyncThunk(
	'rs3/totalXpPlayer2',
	async (username: string) => {
		const response = await getStats(username);
		return response;
	},
);

// Loading is controlled by the number of loading calls currently active
// the loading count is increased when a call is made, and decreased when the call completes

const incrementPlayerLoading = (state: Rs3State) => {
	state.player.playerLoading = state.player.playerLoading + 1;
};

const decrementPlayerLoading = (state: Rs3State) => {
	state.player.playerLoading = Math.max(state.player.playerLoading - 1, 0);
};

export const rs3Slice = createSlice({
	name: 'rs3',
	initialState,
	reducers: {},
	extraReducers: (builder) => {
		builder
			.addCase(getXpGains.pending, (state) => {
				state.status = 'loading';
				incrementPlayerLoading(state);
				// reset player state
				state.player.badges = [];
			})
			.addCase(getXpGains.fulfilled, (state, action) => {
				state.status = 'idle';
				decrementPlayerLoading(state);
				state.player.success = action.payload.data.success;
				if (state.player.success) {
					state.player.skills = action.payload.data.data.skillGains;
					state.player.minigames = action.payload.data.data.minigameGains;
					state.player.badges = [
						...state.player.badges,
						...action.payload.data.data.badges,
					];
					state.player.username = action.payload.data.data.displayName;
					state.player.isTracking = action.payload.data.data.isTracking;
				}
			})
			.addCase(getXpGainsForPlayer1.pending, (state) => {
				state.compare.player1.playerLoading = true;
			})
			.addCase(getXpGainsForPlayer2.pending, (state) => {
				state.compare.player2.playerLoading = true;
			})
			.addCase(getXpGainsForPlayer1.fulfilled, (state, action) => {
				state.compare.player1.playerLoading = false;
				state.compare.player1.success = action.payload.data.success;
				if (state.compare.player1.success) {
					state.compare.player1.skills = action.payload.data.data.skillGains;
					state.compare.player1.minigames =
						action.payload.data.data.minigameGains;
				}
			})
			.addCase(getXpGainsForPlayer2.fulfilled, (state, action) => {
				state.compare.player2.playerLoading = false;
				state.compare.player2.success = action.payload.data.success;
				if (state.compare.player2.success) {
					state.compare.player2.skills = action.payload.data.data.skillGains;
					state.compare.player2.minigames =
						action.payload.data.data.minigameGains;
				}
			})
			.addCase(startTrackingForUser.fulfilled, (state, action) => {
				state.player.isTracking = action.payload.data.data;
			})
			.addCase(getPlayerDetails.fulfilled, (state, action) => {
				state.player.clanname = action.payload.data.data.clanName;
			})
			.addCase(getPlayerIronStatus.fulfilled, (state, action) => {
				state.player.ironmanStatus = action.payload.data.data;
			})
			.addCase(getPlayer1IronStatus.fulfilled, (state, action) => {
				state.compare.player1.ironmanStatus = action.payload.data.data;
			})
			.addCase(getPlayer2IronStatus.fulfilled, (state, action) => {
				state.compare.player2.ironmanStatus = action.payload.data.data;
			})
			.addCase(getPlayerMetrics.pending, (state) => {
				incrementPlayerLoading(state);
				state.player.activities = [];
			})
			.addCase(getPlayerMetrics.fulfilled, (state, action) => {
				decrementPlayerLoading(state);
				state.player.activities = action.payload.data.data.activities;
			})
			.addCase(getPlayerQuests.fulfilled, (state, action) => {
				state.player.runemetricsEnabled = action.payload.data.success;
				state.player.quests.questPoints = action.payload.data.data.questPoints;
				state.player.quests.totalQuestPoints =
					action.payload.data.data.totalQuestPoints;
				state.player.quests.totalQuests = action.payload.data.data.totalQuests;
				state.player.quests.completedQuests =
					action.payload.data.data.completedQuests;
				if (action.payload.data.data.questCape) {
					state.player.badges = [...state.player.badges, questBadgeId];
				}
			})
			.addCase(getRs3Activities.pending, (state) => {
				state.dashboard.activitiesLoading = true;
			})
			.addCase(getRs3Activities.fulfilled, (state, action) => {
				state.dashboard.activitiesLoading = false;
				state.dashboard.activities = action.payload.data;
			})
			.addCase(getRs3FollowingActivities.pending, (state) => {
				state.dashboard.activitiesLoading = true;
			})
			.addCase(getRs3FollowingActivities.fulfilled, (state, action) => {
				state.dashboard.activitiesLoading = false;
				state.dashboard.activities = action.payload?.data.data;
			})
			.addCase(likeRs3Activity.fulfilled, (state, action) => {
				const activityId = action.payload?.data.data.id;
				const activity = [
					...state.dashboard.activities,
					...state.player.activities,
				].find((a) => a.id === activityId);
				if (activity) {
					activity.likes += 1;
					activity.currentUserLiked = true;
				}
				state.player.activities = [...state.player.activities];
				state.dashboard.activities = [...state.dashboard.activities];
			})
			.addCase(unlikeRs3Activity.fulfilled, (state, action) => {
				const activityId = action.payload?.data.data.id;
				const activity = [
					...state.dashboard.activities,
					...state.player.activities,
				].find((a) => a.id === activityId);
				if (activity) {
					activity.likes -= 1;
					activity.currentUserLiked = false;
				}
				state.player.activities = [...state.player.activities];
				state.dashboard.activities = [...state.dashboard.activities];
			});
	},
});

// export const { increment, decrement, incrementByAmount } = rs3Slice.actions;

export const selectActivities = (state: RootState) =>
	state.rs3.dashboard.activities;
export const selectActivitiesLoading = (state: RootState) =>
	state.rs3.dashboard.activitiesLoading;
export const selectPlayerLoading = (state: RootState) =>
	state.rs3.player.playerLoading > 0;
export const selectPlayerSuccess = (state: RootState) =>
	state.rs3.player.success;
export const selectSkills = (state: RootState) => state.rs3.player.skills;
export const selectMinigames = (state: RootState) => state.rs3.player.minigames;
export const selectPlayer1Skills = (state: RootState) =>
	state.rs3.compare.player1.skills;
export const selectPlayer1Minigames = (state: RootState) =>
	state.rs3.compare.player1.minigames;
export const selectPlayer1IronStatus = (state: RootState) =>
	state.rs3.compare.player1.ironmanStatus;
export const selectPlayer2IronStatus = (state: RootState) =>
	state.rs3.compare.player2.ironmanStatus;
export const selectPlayer2Skills = (state: RootState) =>
	state.rs3.compare.player2.skills;
export const selectPlayer2Minigames = (state: RootState) =>
	state.rs3.compare.player2.minigames;
export const selectComparisonLoading = (state: RootState) =>
	state.rs3.compare.player2.playerLoading ||
	state.rs3.compare.player1.playerLoading;
export const selectComparisonSuccess = (state: RootState) =>
	state.rs3.compare.player2.success && state.rs3.compare.player1.success;
export const selectBadges = (state: RootState) => state.rs3.player.badges;
export const selectTotalXp = (state: RootState) =>
	state.rs3.player.skills?.length > 0 ? state.rs3.player.skills[0].xp : 0;
export const selectRunescore = (state: RootState) =>
	state.rs3.player.minigames?.length > 0
		? state.rs3.player.minigames[24].score
		: 0;
export const selectUsername = (state: RootState) => state.rs3.player.username;
export const selectClanname = (state: RootState) => state.rs3.player.clanname;
export const selectIronStatus = (state: RootState) =>
	state.rs3.player.ironmanStatus;
export const selectIsTracking = (state: RootState) =>
	state.rs3.player.isTracking;
export const selectPlayerActivities = (state: RootState) =>
	state.rs3.player.activities;
export const selectStatus = (state: RootState) => state.rs3.status;
export const selectQuestPoints = (state: RootState) =>
	state.rs3.player.quests.questPoints;
export const selectQuestData = (state: RootState) => state.rs3.player.quests;
export const selectRuneMetricsEnabled = (state: RootState) =>
	state.rs3.player.runemetricsEnabled;
export const selectTopTenGains = (state: RootState) =>
	state.rs3.player.skills
		.filter((s) => s.dayGain > 0)
		.sort((a, b) => b.dayGain - a.dayGain);

export default rs3Slice.reducer;
