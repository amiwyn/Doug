<template>
  <v-container fluid>
    <v-row>
      <v-col>
        <Inventory />
        <Loadout />
      </v-col>
    </v-row>
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
