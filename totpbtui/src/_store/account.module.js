import { userService } from '../_services';
import { router } from '../_helpers';

const user = JSON.parse(localStorage.getItem('user'));
const userToken = JSON.parse(localStorage.getItem('userToken'));

// const state = user
//     ? { status: { loggedIn: false }, user }
//     : { status: {}, user: null };

const state = user && userToken ? { status: { loggedIn: true }, user, userToken } : (user && !userToken) ? { status: { loggedIn: false }, user, userToken: null } : { status: {}, user: null, userToken: null };

// const state;

// if(user && userToken) {
//     state = { status: { loggedIn: true }, user, userToken }
// } else if (user && !userToken) {
//     state = { status: { loggedIn: false }, user, userToken: null }
// } else {
//     state = { status: {}, user: null, userToken: null };
// }

const actions = {
    login({ dispatch, commit }, { username, password }) {
        commit('loginRequest', { username });
    
        userService.login(username, password)
            .then(
                user => {
                    commit('loginSuccess', user);
                    router.push('/totp');
                },
                error => {
                    commit('loginFailure', error);
                    dispatch('alert/error', error, { root: true });
                }
            );
    },
    authenticate({ dispatch, commit }, { totppassword }) {
        commit('authenticateRequest', { totppassword });

        userService.authenticate(totppassword)
            .then(
                userToken => {
                    commit('authenticateSuccess', userToken);
                    router.push('/');
                },
                error => {
                    commit('authenticateFailure', error);
                    dispatch('alert/error', error, { root: true });
                }
            );
    },
    logout({ commit }) {
        userService.logout();
        commit('logout');
    },
    register({ dispatch, commit }, user) {
        commit('registerRequest', user);
    
        userService.register(user)
            .then(
                user => {
                    commit('registerSuccess', user);
                    router.push('/login');
                    setTimeout(() => {
                        // display success message after route change completes
                        dispatch('alert/success', 'Registration successful', { root: true });
                    })
                },
                error => {
                    commit('registerFailure', error);
                    dispatch('alert/error', error, { root: true });
                }
            );
    }
};

const mutations = {
    loginRequest(state, user) {
        state.status = { loggingIn: true };
        state.user = user;
        state.userToken = null;
    },
    loginSuccess(state, user) {
        state.status = { loggedIn: false };
        state.user = user;
        state.userToken = null;
    },
    loginFailure(state) {
        state.status = {};
        state.user = null;
        state.userToken = null;
    },
    authenticateRequest(state, userToken, user) {
        state.status = { loggingIn: true };
        state.user = user;
        state.userToken = userToken;
    },
    authenticateSuccess(state, user, userToken) {
        state.status = { loggedIn: true };
        state.user = user;
        state.userToken = userToken;
    },
    authenticateFailure(state) {
        state.status = {};
        state.user = null;
        state.userToken = null;
    },
    logout(state) {
        state.status = {};
        state.user = null;
    },
    registerRequest(state, user) {
        state.status = { registering: true };
    },
    registerSuccess(state, user) {
        state.status = {};
    },
    registerFailure(state, error) {
        state.status = {};
    }
};

export const account = {
    namespaced: true,
    state,
    actions,
    mutations
};