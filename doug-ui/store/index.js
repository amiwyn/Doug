export const state = () => ({
  token: null
})

export const getters = {
  user: state => {
    return state.auth ? state.auth.user.user : {}
  }
}

export const mutations = {
  setInventory(state, items) {
    state.auth.user.user.inventory_items = items
  },
  setLoadout(state, loadout) {
    state.auth.user.user.loadout = loadout
  }
}

export const actions = {
  authenticate(context, code) {
    this.$auth.loginWith('local', { data: code })
  }
}