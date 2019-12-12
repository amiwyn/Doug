<template>
  <v-container fluid>
    <v-row>
      <v-card>
        <v-card-text>loadout component wiwpwip</v-card-text>
        <v-card-actions>
          <v-col cols="12">
            <v-row>
              <InventoryItem :item="getLoadout('skillbook')" />
              <InventoryItem :item="getLoadout('head')" />
              <InventoryItem :item="getLoadout('neck')" />
            </v-row>
            <v-row>
              <InventoryItem :item="getLoadout('left_hand')" />
              <InventoryItem :item="getLoadout('body')" />
              <InventoryItem :item="getLoadout('right_hand')" />
            </v-row>
            <v-row>
              <InventoryItem :item="getLoadout('left_ring')" />
              <InventoryItem :item="getLoadout('boots')" />
              <InventoryItem :item="getLoadout('right_ring')" />
            </v-row>
          </v-col>
        </v-card-actions>
      </v-card>
    </v-row>
  </v-container>
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