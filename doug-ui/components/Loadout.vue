<template>
  <v-card class="ma-2 px-2" max-height="300">
    <v-card-text>loadout</v-card-text>
    <v-container>
      <v-row justify="center">
        <v-col align="center">
          <v-row>
            <InventoryItem :item="getLoadout('skillbook')" />
            <InventoryItem :item="getLoadout('head')" />
          </v-row>
          <v-row>
            <InventoryItem :item="getLoadout('right_ring')" />
            <InventoryItem :item="getLoadout('neck')" />
            <InventoryItem :item="getLoadout('gloves')" />
          </v-row>
          <v-row>
            <InventoryItem :item="getLoadout('left_hand')" />
            <InventoryItem :item="getLoadout('body')" />
            <InventoryItem :item="getLoadout('right_hand')" />
          </v-row>
          <v-row>
            <InventoryItem :item="getLoadout('left_ring')" />
            <InventoryItem :item="getLoadout('boots')" />
          </v-row>
        </v-col>
      </v-row>
    </v-container>
  </v-card>
</template>

<script>
import InventoryItem from "~/components/InventoryItem.vue";

export default {
  components: {
    InventoryItem
  },
  methods: {
    getItem: function(x, y) {
      let items = [...this.$store.state.auth.user.user.inventory_items];
      items.sort((a, b) => a.inventory_position - b.inventory_position);
      let pos = x - 1 + 4 * (y - 1);
      return items.find(itm => itm.inventory_position === pos);
    },
    getLoadout: function(slot) {
      return this.$store.state.auth.user.user.loadout[slot];
    }
  }
};
</script>