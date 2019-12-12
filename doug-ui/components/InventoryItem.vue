<template>
  <div>
    <div>
      <v-dialog width="284">
        <template v-slot:activator="{ on }">
          <v-hover v-slot:default="{ hover }">
            <div v-on="on">
              <v-card
                width="42"
                height="42"
                class="grey darken-1 ma-1 d-flex justify-center align-center icon"
                :elevation="hover ? 9 : 2"
              >
                <img v-if="item" class="pa-auto" :src="`/sprites/${icon}.png`" :alt="icon" />
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
import Draggable from "vuedraggable";

export default {
  components: {
    Draggable
  },
  props: {
    item: {
      type: Object
    }
  },
  data() {
    return {
      name: this.item ? this.item.name : undefined,
      description: this.item ? this.item.description : undefined
    };
  },
  computed: {
    quantity: function() {
      return this.item && this.item.quantity > 1
        ? this.item.quantity
        : undefined;
    },
    icon: function() {
      if (this.item) {
        let iconName = this.item.icon;
        let last = iconName.length - 1;
        return iconName.substring(1, last);
      }
      return undefined;
    }
  }
};
</script>