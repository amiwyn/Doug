<template>
<div>
  <v-card v-if="!item.item" width="42" height="42" class="grey darken-1 ma-1 d-flex justify-center align-center icon">
    <img class="pa-auto" :src="`/loadout/${item.slot}.png`" />
  </v-card>
  <InventoryItem v-else :item="this.item.item" :isEquipped="true" />
</div>

</template>

<script>
import InventoryItem from "~/components/InventoryItem.vue";

export default {
  components: {
    InventoryItem
  },
  props: {
    item: {
      type: Object
    }
  }
};
</script>