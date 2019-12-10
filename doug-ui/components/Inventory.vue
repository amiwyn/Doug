<template>
  <v-container fluid>
    <v-row>
      <v-card>
        <v-card-text>WIP inventory component</v-card-text>
        <v-card-actions>
          <v-col cols="12">
            <v-row v-for="y in 5" :key="y">
              <InventoryItem :itemSlot="getItem(x,y)" v-for="x in 4" :key="x" />
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
    }
  }
};
</script>