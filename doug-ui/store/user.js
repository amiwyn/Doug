export const state = () => ({

})

export const actions = {
  async equip(context, position) {
    var result = await this.$axios.$post(`https://0b2bd38f.ngrok.io/ui/user/equip/${position}`)

    context.commit('setInventory', result.items, { root: true })
    context.commit('setLoadout', result.loadout, { root: true })
    return result.action_message
  },
  async unequip(context, slot) {
    var result = await this.$axios.$post(`https://0b2bd38f.ngrok.io/ui/user/unequip/${slot}`)
    
    context.commit('setInventory', result.items, { root: true })
    context.commit('setLoadout', result.loadout, { root: true })
    return result.action_message
  },
  async use(context, position) {
    var result = await this.$axios.$post(`https://0b2bd38f.ngrok.io/ui/user/use/${position}`)
    
    context.commit('setInventory', result.items, { root: true })
    context.commit('setLoadout', result.loadout, { root: true })
    return result.action_message
  },
  async sell(context, position) {
    var result = await this.$axios.$post(`https://0b2bd38f.ngrok.io/ui/user/sell/${position}`)
    
    context.commit('setInventory', result.items, { root: true })
    context.commit('setLoadout', result.loadout, { root: true })
    return result.action_message
  }
}