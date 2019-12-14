<template>
  <v-card>
    <v-card-title >{{ item.name }}</v-card-title>
    <v-card-text class="caption">{{ item.description }}</v-card-text>
    <v-divider class="mb-2"></v-divider>
    <v-card-text>
      <p v-if="isDisplayable(item.level_requirement)" class="text-center font-weight-bold caption">Level {{ item.level_requirement }}</p>
      <p v-if="isDisplayable(item.strength_requirement)" class="text-center caption">Str requirement {{ item.strength_requirement }}</p>
      <p v-if="isDisplayable(item.agility_requirement)" class="text-center caption">Agi requirement {{ item.agility_requirement }}</p>
      <p v-if="isDisplayable(item.intelligence_requirement)" class="text-center caption">Int requirement {{ item.intelligence_requirement }}</p>
      
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
    </v-card-text>
    <v-divider></v-divider>
    <v-card-text class="text-center caption mt-2">Value {{ item.price }}</v-card-text>
  </v-card>
</template>

<script>
export default {
  props: {
    item: {
      type: Object
    }
  },
  methods: {
    formatStat: function(stat) {
      return stat >= 0 ? '+' + stat : '-' + stat
    },
    isDisplayable: function(stat) {
      return stat && stat !== 0
    }
  }
}
</script>