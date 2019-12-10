export const state = () => ({
  authenticated: null,
  token: null,
  user: {}
})

export const mutations = {
  authenticate(state, token) {
    state.token = token
    state.authenticated = true
  },
  setUser(state, user) {
    state.user = user
  }
}

export const actions = {
  authenticate(context, code) {
    this.$auth.loginWith('local', { data: code })
  }
}