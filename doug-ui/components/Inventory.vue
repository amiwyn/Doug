<template>
  <v-card class="ma-2" max-width="224">
    <v-card-text>WIP inventory component</v-card-text>
    <v-container>
      <v-col>
        <v-row v-for="y in 5" :key="y">
          <InventoryItem :item="getItem(x,y)" v-for="x in 4" :key="x" />
        </v-row>
      </v-col>
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
      let items = [...this.$store.getters.user.inventory_items];
      items.sort((a, b) => a.inventory_position - b.inventory_position);
      let pos = x - 1 + 4 * (y - 1);
      let invItem = items.find(itm => itm.inventory_position === pos);

      if (!invItem) {
        return null;
      }

      return { ...invItem.item, quantity: invItem.quantity, pos: invItem.inventory_position };
    }
  }
};
</script>