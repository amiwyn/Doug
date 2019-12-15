<template>
  <v-container fluid>
    <v-col cols="12">
      <v-row>
        <Inventory />
        <Loadout />
      </v-row>
    </v-col>
  </v-container>
</template>

<script>
import Inventory from "~/components/Inventory.vue";
import Loadout from "~/components/Loadout.vue";

export default {
  components: {
    Inventory,
    Loadout
  }
};
</script>
