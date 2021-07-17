import MaxCape from 'assets/images/maxCape.png';
import SkillIcon from 'assets/skillIcons/1_overall.png';
import QuestIcon from 'assets/images/questIcon.png';

// urls

export const apiBaseUrl = process.env.REACT_APP_API_URL;
export const gainsUrl = `${apiBaseUrl}/users/gains`;
export const playerCountUrl = `${apiBaseUrl}/users/playercount`;
export const startTrackingUrl = `${apiBaseUrl}/users/track`;
export const questsUrl = `${apiBaseUrl}/users/quests`;
export const detailsUrl = `${apiBaseUrl}/users/details`;
export const metricsUrl = `${apiBaseUrl}/users/metrics`;
export const userRsnUrl = `${apiBaseUrl}/users/rs3rsn`;
export const activitiesUrl = `${apiBaseUrl}/users/activities`;
export const followUrl = `${apiBaseUrl}/users/follow`;
export const unfollowUrl = `${apiBaseUrl}/users/unfollow`;
export const followedUrl = `${apiBaseUrl}/users/following`;
export const followingActivitiesUrl = `${apiBaseUrl}/users/following/activities`;
export const checkUrl = `${apiBaseUrl}/authmanagement/check`;
export const loginUrl = `${apiBaseUrl}/authmanagement/login`;
export const createUrl = `${apiBaseUrl}/authmanagement/register`;
export const avatarUrlPre = 'https://secure.runescape.com/m=avatar-rs/';
export const avatarUrlPost = '/chat.png';
export const itemPriceUrl =
	'https://api.weirdgloop.org/exchange/history/rs/latest?name=';
export const itemIconUrl =
	'https://secure.runescape.com/m=itemdb_rs/api/catalogue/detail.json?item=';

// icon urls
export const archJournal =
	'https://runescape.wiki/images/thumb/b/be/Archaeology_journal_detail.png/100px-Archaeology_journal_detail.png?9ffe0';

// verbiage

export const logoTitleText = 'Woodcut';
export const homeTitleText = 'Woodcut';
export const playerCountText = 'players online';
export const trackingButtonTextDisabled = 'Track';
export const trackingButtonTextEnabled = 'Tracking Enabled';
export const userNotFoundText = 'User not found in the official Hiscores';
export const buttonTextLogin = 'Login';
export const buttonTextLogout = 'Logout';
export const buttonTextAccountSettings = 'Account';
export const buttonTextCreateAccount = 'Create Account';
export const loginFormPlaceholderUsername = 'Username';
export const loginFormPlaceholderPassword = 'Password';
export const loginFormPlaceholderEmail = 'Email';
export const loginFormErrorMessage =
	'Email or Password is incorrect. Please try again.';
export const homeContentText =
	'Site currently in beta. Start tracking your stats by searching for your player, and then find the Start Tracking button. Feel free to report any bugs/suggestions to @zeepkrs on Twitter!';
export const accountSettingsRs3RsnText = 'RS3 Name:';
export const accountSettingsRs3RsnUpdatePlaceholder = 'New rsn';
export const accountSettingsRs3RsnUpdateButtonText = 'Update';
export const updateRsnErrorMessage =
	'RSN not valid. Try searching for it first, and enable tracking from the Details tab.';
export const activityFeedTitleText = 'Activity Feed';
export const noActivitiesFoundText = 'No Activities Found';
export const followingActivityFeedTitleText = 'Followed';
export const followButtonText = 'Follow';
export const unfollowButtonText = 'Unfollow';
export const questPointsText = 'Quest Points';
export const pointsRemainingText = 'Points Remaining';
export const questsCompleteText = 'Quests Complete';
export const questsRemainingText = 'Quests Remaining';
export const helpIconRunemetricsPrivateText =
	"This player has their official RuneScape RuneMetrics profile set to 'private'. This means data such as quest progress and recent activity is unavailable.";

// thresholds

export const formErrorToastLifetime = 5000;
export const rsnMaxLength = 12;
export const rs3HomePageActivities = 30;

// types

export const gainPeriods = [
	{ label: 'Week', value: 'week', data: 'weekGain' },
	{ label: 'Month', value: 'month', data: 'monthGain' },
	{ label: 'Year', value: 'year', data: 'yearGain' },
	// { label: 'DXP', value: 'dxp', data: 'dxpGain' },
];

export const milestones = [
	{ label: 'Max', value: 'max', maxValue: 0 },
	{ label: 'Max Total', value: 'maxtotal', maxValue: 0 },
	{ label: '120 All', value: '120all', maxValue: 0 },
	{ label: 'Max Xp', value: 'maxxp', maxValue: 0 },
];

export const navbarMenuItems = [
	{
		text: 'Runescape 3',
		path: '/rs3',
	},
	{
		text: 'Old School',
		path: '/osrs',
	},
];

export const questBadgeId = 5;

export const badgeTypes = [
	{
		id: 1,
		text: 'Maxed',
		icon: MaxCape,
		color: 'rgb(72, 46, 46)',
	},
	{
		id: 2,
		text: 'Max Total',
		icon: SkillIcon,
		color: 'rgb(72, 46, 46)',
	},
	{
		id: 3,
		text: '120 All',
		icon: SkillIcon,
		color: 'rgb(72, 46, 46)',
	},
	{
		id: 4,
		text: 'Max XP',
		icon: SkillIcon,
		color: 'rgb(72, 46, 46)',
	},
	{
		id: questBadgeId,
		text: 'Quest Cape',
		icon: QuestIcon,
		color: 'rgb(25, 98, 134)',
	},
	{
		id: 10,
		text: 'Base 10',
		icon: 'https://runescape.wiki/images/3/30/Milestone_cape_(10).png',
	},
	{
		id: 20,
		text: 'Base 20',
		icon: 'https://runescape.wiki/images/9/97/Milestone_cape_(20).png',
	},
	{
		id: 30,
		text: 'Base 30',
		icon: 'https://runescape.wiki/images/1/1a/Milestone_cape_%2830%29.png?bb78f',
	},
	{
		id: 40,
		text: 'Base 40',
		icon: 'https://runescape.wiki/images/6/6e/Milestone_cape_%2840%29.png?2c6a5',
	},
	{
		id: 50,
		text: 'Base 50',
		icon: 'https://runescape.wiki/images/8/87/Milestone_cape_%2850%29.png?b988c',
	},
	{
		id: 60,
		text: 'Base 60',
		icon: 'https://runescape.wiki/images/5/58/Milestone_cape_%2860%29.png?3610e',
	},
	{
		id: 70,
		text: 'Base 70',
		icon: 'https://runescape.wiki/images/3/3b/Milestone_cape_%2870%29.png?90dd8',
	},
	{
		id: 80,
		text: 'Base 80',
		icon: 'https://runescape.wiki/images/0/0a/Milestone_cape_%2880%29.png?72a03',
	},
	{
		id: 90,
		text: 'Base 90',
		icon: 'https://runescape.wiki/images/5/55/Milestone_cape_%2890%29.png?20c6a',
	},
];

export const usernameTakenErrorMessage = 'is already taken';
export const emailTakenErrorMessage = 'Email already in use';
