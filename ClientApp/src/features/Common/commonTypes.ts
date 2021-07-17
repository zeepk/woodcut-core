export type AuthDataLogin = {
	email: string;
	password: string;
};

export type AuthDataCreateAccount = {
	email: string;
	username: string;
	password: string;
};

export type AuthResponseCreateAccount = {
	errors: Array<string>;
};

// export type AuthResponseCreateAccount = {
// 	data: {
// 		errors: Array<string>;
// 	};
// 	config: object;
// 	headers: object;
// 	request: object;
// };
