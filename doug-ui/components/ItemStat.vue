<template>
  <v-card>
    <v-card-title>{{ item.name }}</v-card-title>
    <v-card-text class="caption mt-2">{{ item.description }}</v-card-text>
    <v-divider class="mb-2"></v-divider>
    <v-card-text>
      <p v-if="isDisplayable(item.max_attack)" class="text-center caption">Attack [{{ item.min_attack }}/{{ item.max_attack }}]</p>
      <p v-if="isDisplayable(item.defense)" class="text-center caption">Defense {{ formatStat(item.defense) }}</p>
      <p v-if="isDisplayable(item.level_requirement)" class="text-center caption">Level {{ item.level_requirement }}</p>
    </v-card-text>

    <span v-if="isDisplayable(item.strength_requirement)" class="text-center caption">Str Requirement {{ item.strength_requirement }}</span>
    <span v-if="isDisplayable(item.agility_requirement)" class="text-center caption">Agi Requirement {{ item.agility_requirement }}</span>
    <span v-if="isDisplayable(item.intelligence_requirement)" class="text-center caption">Int Requirement {{ item.intelligence_requirement }}</span>
    
    <span v-if="isDisplayable(item.defense)" class="text-center caption">Defense {{ formatStat(item.defense) }}</span>
    <span v-if="isDisplayable(item.defense)" class="text-center caption">Defense {{ formatStat(item.defense) }}</span>
    <span v-if="isDisplayable(item.defense)" class="text-center caption">Defense {{ formatStat(item.defense) }}</span>
    <span v-if="isDisplayable(item.defense)" class="text-center caption">Defense {{ formatStat(item.defense) }}</span>
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
      return stat !== 0
    }
  }
}
</script>