export const state = () => ({
  authenticated: null,
  username: null,
  userId: null,
  image: null
})

export const mutations = {
  authenticate(state, identity) {
    state.username = identity.user.name
    state.userId = identity.user.id
    state.image = identity.user.image_48
    state.authenticated = true
  }
}

export const actions = {
  async authenticate(context) {
    if (localStorage.token) {
      const identity = await this.$axios.$get(`https://slack.com/api/users.identity?token=${localStorage.token}`)
      context.commit('authenticate', identity)
    }
  },
  async signIn(context, code) {
    const token = await this.$axios.$get(`api/authorization/${code}`)
    localStorage.token = token
    context.dispatch('authenticate')
  }
}