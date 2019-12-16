export const state = () => ({

})

export const actions = {
  async equip(context, position) {
    console.log(this.$axios)
    var result = await this.$axios.$post(`https://doug-bot.azurewebsites.net/ui/user/equip/${position}`)

    context.commit('setInventory', result.items, { root: true })
    context.commit('setLoadout', result.loadout, { root: true })
    return result.action_message
  },
  async unequip(context, slot) {
    var result = await this.$axios.$post(`https://doug-bot.azurewebsites.net/ui/user/unequip/${slot}`)
    
    context.commit('setInventory', result.items, { root: true })
    context.commit('setLoadout', result.loadout, { root: true })
    return result.action_message
  },
  async use(context, position) {
    var result = await this.$axios.$post(`https://doug-bot.azurewebsites.net/ui/user/use/${position}`)
    
    context.commit('setInventory', result.items, { root: true })
    context.commit('setLoadout', result.loadout, { root: true })
    return result.action_message
  },
  async sell(context, position) {
    var result = await this.$axios.$post(`https://doug-bot.azurewebsites.net/ui/user/sell/${position}`)
    
    context.commit('setInventory', result.items, { root: true })
    context.commit('setLoadout', result.loadout, { root: true })
    return result.action_message
  }
}