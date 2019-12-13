export const state = () => ({
  authenticated: null,
  token: null
})

export const mutations = {
  authenticate(state, token) {
    state.token = token
    state.authenticated = true
  }
}

export const actions = {
  authenticate(context, code) {
    this.$auth.loginWith('local', { data: code })
  }
}