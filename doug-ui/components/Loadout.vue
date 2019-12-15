<template>
  <v-card class="ma-2 px-2" max-height="300">
    <v-card-text>loadout</v-card-text>
    <v-container>
      <v-row justify="center">
        <v-col align="center">
          <v-row>
            <LoadoutSlot :item="getLoadout('skillbook')" />
            <LoadoutSlot :item="getLoadout('head')" />
          </v-row>
          <v-row>
            <LoadoutSlot :item="getLoadout('right_ring')" />
            <LoadoutSlot :item="getLoadout('neck')" />
            <LoadoutSlot :item="getLoadout('gloves')" />
          </v-row>
          <v-row>
            <LoadoutSlot :item="getLoadout('left_hand')" />
            <LoadoutSlot :item="getLoadout('body')" />
            <LoadoutSlot :item="getLoadout('right_hand')" />
          </v-row>
          <v-row>
            <LoadoutSlot :item="getLoadout('left_ring')" />
            <LoadoutSlot :item="getLoadout('boots')" />
          </v-row>
        </v-col>
      </v-row>
    </v-container>
  </v-card>
</template>

<script>
import LoadoutSlot from "~/components/LoadoutSlot.vue";

export default {
  components: {
    LoadoutSlot
  },
  methods: {
    getItem: function(x, y) {
      let items = [...this.$store.state.auth.user.user.inventory_items];
      items.sort((a, b) => a.inventory_position - b.inventory_position);
      let pos = x - 1 + 4 * (y - 1);
      return items.find(itm => itm.inventory_position === pos);
    },
    getLoadout: function(slot) {
      let item = this.$store.state.auth.user.user.loadout[slot];
      return { item: item, slot: slot };
    }
  }
};
</script>