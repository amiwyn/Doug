<template>
  <v-card v-if="item">
    <v-card-title >{{ item.name }}</v-card-title>
    <v-card-text class="caption">{{ item.description }}</v-card-text>
    <v-divider class="mb-2"></v-divider>
    <v-card-text>
      <p v-if="isDisplayable(item.level_requirement)" class="text-center font-weight-bold caption">Level {{ item.level_requirement }}</p>
      <p v-if="isDisplayable(item.strength_requirement)" class="text-center caption">Str required {{ item.strength_requirement }}</p>
      <p v-if="isDisplayable(item.agility_requirement)" class="text-center caption">Agi required {{ item.agility_requirement }}</p>
      <p v-if="isDisplayable(item.intelligence_requirement)" class="text-center caption">Int required {{ item.intelligence_requirement }}</p>
      
      <p v-if="isDisplayable(item.max_attack)" class="text-center caption">Attack {{ item.min_attack }}/{{ item.max_attack }}</p>
      <p v-if="isDisplayable(item.health)" class="text-center caption">Health {{ formatStat(item.health) }}</p>
      <p v-if="isDisplayable(item.energy)" class="text-center caption">Mana {{ formatStat(item.energy) }}</p>
      <p v-if="isDisplayable(item.attack_speed)" class="text-center caption">Atk Spd {{ formatStat(item.attack_speed) }}</p>
      <p v-if="isDisplayable(item.hitrate)" class="text-center caption">Hit rate {{ formatStat(item.hitrate) }}</p>
      <p v-if="isDisplayable(item.dodge)" class="text-center caption">Dodge {{ formatStat(item.dodge) }}</p>
      <p v-if="isDisplayable(item.defense)" class="text-center caption">Defense {{ formatStat(item.defense) }}</p>
      <p v-if="isDisplayable(item.resistance)" class="text-center caption">Resistance {{ formatStat(item.resistance) }}</p>
      <p v-if="isDisplayable(item.health_regen)" class="text-center caption">Health Regen {{ formatStat(item.health_regen) }}</p>
      <p v-if="isDisplayable(item.energy_regen)" class="text-center caption">Mana Regen {{ formatStat(item.energy_regen) }}</p>
      <p v-if="isDisplayable(item.luck)" class="text-center caption">Luck {{ formatStat(item.luck) }}</p>
      <p v-if="isDisplayable(item.agility)" class="text-center caption">Agility {{ formatStat(item.agility) }}</p>
      <p v-if="isDisplayable(item.strength)" class="text-center caption">Strength {{ formatStat(item.strength) }}</p>
      <p v-if="isDisplayable(item.constitution)" class="text-center caption">Constitution {{ formatStat(item.constitution) }}</p>
      <p v-if="isDisplayable(item.intelligence)" class="text-center caption">Intelligence {{ formatStat(item.intelligence) }}</p>
      <p class="text-center font-weight-bold caption">Value {{ item.price }}</p>
    </v-card-text>
    <v-divider></v-divider>
    <v-card-actions v-if="equipmentActions">
      <v-btn @click="unequip()" text>Unequip</v-btn>
    </v-card-actions>
    <v-card-actions v-if="!equipmentActions" >
      <v-btn @click="use()" text>Use</v-btn>
      <v-btn @click="equip()" text>Equip</v-btn>
      <v-btn @click="sell()" text>Sell</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
export default {
  props: {
    item: {
      type: Object
    },
    equipmentActions: {
      type: Boolean
    }
  },
  methods: {
    formatStat: function(stat) {
      return stat >= 0 ? '+' + stat : '-' + stat
    },
    isDisplayable: function(stat) {
      return stat && stat !== 0
    },
    equip: function() {
      this.$store.dispatch("user/equip", this.item.pos)
        .then(result => this.$notifier.showMessage({ content: result }))
      this.$emit('dialogClose')
    },
    unequip: function() {
      this.$store.dispatch("user/unequip", this.item.slot)
        .then(result => this.$notifier.showMessage({ content: result }))
      this.$emit('dialogClose')
    },
    use: function() {
      this.$store.dispatch("user/use", this.item.pos)
        .then(result => this.$notifier.showMessage({ content: result }))
      this.$emit('dialogClose')
    },
    sell: function() {
      this.$store.dispatch("user/sell", this.item.pos)
        .then(result => this.$notifier.showMessage({ content: result }))
      this.$emit('dialogClose')
    }
  }
}
</script>