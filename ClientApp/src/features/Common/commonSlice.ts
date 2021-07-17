import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { RootState } from '../../app/store';
import {
	checkIfUserLoggedIn,
	logUserIn,
	createUserAccount,
	getPlayerCount,
	getUserRsn,
	updateUserRsn,
	followPlayer,
	unfollowPlayer,
	getFollowed,
} from './commonApi';
import {
	AuthDataCreateAccount,
	AuthDataLogin,
	AuthResponseCreateAccount,
} from 'features/Common/commonTypes';
import { loginFormErrorMessage } from 'utils/constants';
import { AxiosResponse } from 'axios';

export interface CommonState {
	playerCount: {
		loading: boolean;
		count: number;
	};
	auth: {
		error: boolean;
		errorMessage: string | null;
		loading: boolean;
		loginLoading: boolean;
		createLoading: boolean;
		isLoggedIn: boolean;
		username: string;
	};
	account: {
		rs3Rsn: string | null;
		osrsRsn: string;
		loading: boolean;
		following: Array<string>;
	};
}

const initialState: CommonState = {
	playerCount: {
		loading: true,
		count: 0,
	},
	auth: {
		error: false,
		errorMessage: null,
		loading: false,
		loginLoading: false,
		createLoading: false,
		isLoggedIn: false,
		username: '',
	},
	account: {
		rs3Rsn: null,
		osrsRsn: '',
		loading: false,
		following: [],
	},
};

export const getToken = () => {
	const cookie = document.cookie;
	let token;
	try {
		token = cookie.split('authtoken=')[1].split(';')[0];
	} catch (error) {
		token = null;
	}
	return token;
};

export const isUserLoggedIn = createAsyncThunk(
	'common/auth/isLoggedIn',
	async () => {
		const token = getToken();
		if (!token) {
			return;
		}

		const response = await checkIfUserLoggedIn(token);
		return response;
	},
);

export const logIn = createAsyncThunk(
	'common/auth/login',
	async (data: AuthDataLogin) => {
		const response = await logUserIn(data);
		return response;
	},
);

export const createAccount = createAsyncThunk(
	'common/auth/create',
	async (data: AuthDataCreateAccount) => {
		const response: AxiosResponse<AuthResponseCreateAccount> =
			await createUserAccount(data);
		return response;
	},
);

export const getCurrentPlayerCount = createAsyncThunk(
	'rs3/playerCount',
	async () => {
		const response = await getPlayerCount();
		return response;
	},
);

export const getRs3Rsn = createAsyncThunk('common/getrsn', async () => {
	const token = getToken();
	if (!token) {
		return;
	}

	const response = await getUserRsn(token);
	return response;
});

export const updateRs3Rsn = createAsyncThunk(
	'common/updatersn',
	async (data: string) => {
		const token = getToken();
		if (!token) {
			return;
		}

		const response = await updateUserRsn(token, data);
		return response;
	},
);

export const followPlayerRsn = createAsyncThunk(
	'common/follow',
	async (data: string) => {
		const token = getToken();
		if (!token) {
			return;
		}

		const response = await followPlayer(token, data);
		return response;
	},
);

export const unfollowPlayerRsn = createAsyncThunk(
	'common/unfollow',
	async (data: string) => {
		const token = getToken();
		if (!token) {
			return;
		}

		const response = await unfollowPlayer(token, data);
		return response;
	},
);

export const getFollowing = createAsyncThunk('common/following', async () => {
	const token = getToken();
	if (!token) {
		return;
	}

	const response = await getFollowed(token);
	return response;
});

export const commonSlice = createSlice({
	name: 'common',
	initialState,
	reducers: {
		logOut(state) {
			document.cookie = 'authtoken=';
			state.auth.isLoggedIn = false;
			state.auth.username = '';
			state.account = {
				rs3Rsn: null,
				osrsRsn: '',
				loading: false,
				following: [],
			};
		},
	},
	extraReducers: (builder) => {
		builder
			.addCase(getFollowing.pending, (state) => {
				state.account.loading = true;
			})
			.addCase(getFollowing.fulfilled, (state, action) => {
				state.account.loading = false;
				state.account.following = action.payload?.data.data;
			})
			.addCase(followPlayerRsn.pending, (state) => {
				state.account.loading = true;
			})
			.addCase(followPlayerRsn.fulfilled, (state, action) => {
				state.account.loading = false;
				state.account.following.push(action.payload?.data.data);
			})
			.addCase(unfollowPlayerRsn.pending, (state) => {
				state.account.loading = true;
			})
			.addCase(unfollowPlayerRsn.fulfilled, (state, action) => {
				state.account.loading = false;
				state.account.following = state.account.following.filter(
					(u) => u !== action.payload?.data.data,
				);
			})
			.addCase(getCurrentPlayerCount.pending, (state) => {
				state.playerCount.loading = true;
			})
			.addCase(getRs3Rsn.pending, (state) => {
				state.account.loading = true;
			})
			.addCase(getRs3Rsn.fulfilled, (state, action) => {
				state.account.loading = false;
				state.account.rs3Rsn = action.payload?.data;
			})
			.addCase(updateRs3Rsn.pending, (state) => {
				state.account.loading = true;
			})
			.addCase(updateRs3Rsn.fulfilled, (state, action) => {
				state.account.loading = false;
				state.account.rs3Rsn = action.payload?.data.data;
			})
			.addCase(getCurrentPlayerCount.fulfilled, (state, action) => {
				state.playerCount.count = action.payload.data;
				state.playerCount.loading = false;
			})
			.addCase(isUserLoggedIn.pending, (state) => {
				state.auth.loading = true;
			})
			.addCase(isUserLoggedIn.rejected, (state) => {
				state.auth.loading = false;
				state.auth.isLoggedIn = false;
			})
			.addCase(isUserLoggedIn.fulfilled, (state, action) => {
				state.auth.loading = false;
				if (action.payload?.data) {
					state.auth.isLoggedIn = action.payload.data.success;
					state.auth.username = action.payload.data.data;
				}
			})
			.addCase(logIn.pending, (state) => {
				state.auth.loginLoading = true;
			})
			.addCase(logIn.rejected, (state) => {
				state.auth.loginLoading = false;
				state.auth.error = true;
				state.auth.errorMessage = loginFormErrorMessage;
			})
			.addCase(logIn.fulfilled, (state, action) => {
				state.auth.loginLoading = false;
				if (action.payload.data) {
					state.auth.error = false;
					state.auth.isLoggedIn = action.payload.data.success;
					document.cookie = `authtoken=${action.payload.data.token}`;
				} else {
					state.auth.error = true;
					state.auth.errorMessage = loginFormErrorMessage;
				}
			})
			.addCase(createAccount.pending, (state) => {
				state.auth.createLoading = true;
			})
			.addCase(createAccount.rejected, (state) => {
				state.auth.createLoading = false;
				state.auth.error = true;
				state.auth.errorMessage = loginFormErrorMessage;
			})
			.addCase(createAccount.fulfilled, (state, action) => {
				state.auth.createLoading = false;
				if (action.payload.data.errors) {
					state.auth.error = true;
					state.auth.errorMessage =
						action.payload.data.errors[0] || 'Invalid form submission';
				} else {
					state.auth.error = false;
					state.auth.isLoggedIn = true;
					// document.cookie = `authtoken=${action.payload.data.token}`;
				}
			});
	},
});

export const { logOut } = commonSlice.actions;

export const selectPlayersFollowed = (state: RootState) =>
	state.common.account.following;
export const selectUserRs3Rsn = (state: RootState) =>
	state.common.account.rs3Rsn;
export const selectUserLoading = (state: RootState) =>
	state.common.account.loading;
export const selectPlayerCount = (state: RootState) =>
	state.common.playerCount.count;
export const selectPlayerCountLoading = (state: RootState) =>
	state.common.playerCount.loading;
export const selectAuthIsLoggedIn = (state: RootState) =>
	state.common.auth.isLoggedIn;
export const selectAuthUsername = (state: RootState) =>
	state.common.auth.username;
export const selectAuthLoginLoading = (state: RootState) =>
	state.common.auth.loginLoading;
export const selectAuthCreateLoading = (state: RootState) =>
	state.common.auth.createLoading;
export const selectAuthLoading = (state: RootState) =>
	state.common.auth.loading;
export const selectAuthError = (state: RootState) => state.common.auth.error;
export const selectAuthErrorMessage = (state: RootState) =>
	state.common.auth.errorMessage;

export default commonSlice.reducer;
