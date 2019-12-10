<template>
  <div>
    <div>
      <v-dialog width="244">
        <template v-slot:activator="{ on }">
          <v-hover v-slot:default="{ hover }">
            <div v-on="on">
              <v-card
                width="42"
                height="42"
                class="grey darken-1 ma-1 d-flex justify-center align-center icon"
                :elevation="hover ? 12 : 2"
              >
                <img class="pa-auto" :src="`/sprites/${icon}.png`" :alt="icon" />
              </v-card>
              <div class="number-container">
                <span class="numbering">{{ quantity }}</span>
              </div>
            </div>
          </v-hover>
        </template>

        <v-card>
          <v-card-title>{{ name }}</v-card-title>
          <v-card-text class="caption mt-2">{{ description }}</v-card-text>
          <v-divider></v-divider>
          <v-card-text class="caption mt-2">stats coming soon</v-card-text>
        </v-card>
      </v-dialog>
    </div>

    <div class="text-center"></div>
  </div>
</template>

<style scoped>
.icon {
  position: relative;
}
.numbering {
  position: relative;
  bottom: 26px;
  left: 8px;
  font-size: 12px;
  color: rgb(231, 211, 28);
  text-align: right;
}
.number-container {
  position: absolute;
}
</style>

<script>
export default {
  props: {
    itemSlot: {
      type: Object
    }
  },
  data() {
    return {
      name: this.itemSlot ? this.itemSlot.item.name : undefined,
      description: this.itemSlot ? this.itemSlot.item.description : undefined,
    };
  },
  computed: {
    quantity: function() {
      return this.itemSlot && this.itemSlot.quantity > 1
        ? this.itemSlot.quantity
        : undefined;
    },
    icon: function() {
      if (this.itemSlot) {
        let iconName = this.itemSlot.item.icon;
        let last = iconName.length - 1;
        return iconName.substring(1, last);
      }
      return undefined;
    }
  }
};
</script>