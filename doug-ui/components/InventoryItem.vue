<template>
  <div>
    <div>
      <v-dialog width="300" v-model="dialog">
        <template v-slot:activator="{ on }">
          <v-hover v-slot:default="{ hover }">
            <div>
              <div v-on="on" v-if="item">
                <v-card
                  :color="rarityColors[item.rarity]"
                  width="42"
                  height="42"
                  class="ma-1 d-flex justify-center align-center icon"
                  :elevation="hover ? 9 : 2"
                >
                  <img class="pa-auto" :src="`/sprites/${icon}.png`" />
                </v-card>
                <div class="number-container">
                  <span class="numbering">{{ quantity }}</span>
                </div>
              </div>
              <div v-if="!item">
                <v-card
                  width="42"
                  height="42"
                  class="grey darken-1 ma-1 d-flex justify-center align-center icon"
                  :elevation="hover ? 9 : 2"
                ></v-card>
              </div>
            </div>
          </v-hover>
        </template>

        <ItemStat :item="itemData" :equipmentActions="isEquipment" @dialogClose="closeDialog" />
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
import ItemStat from "~/components/ItemStat.vue";

export default {
  components: {
    Draggable,
    ItemStat
  },
  props: {
    item: {
      type: Object
    },
    isEquipped: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      dialog: false,
      rarityColors: ["#b0b0b0", "#6aad66", "#4c7cba", "#db9851", "#9b5eb8"]
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
    },
    itemData: function() {
      return this.item;
    },
    name: function() {
      return this.item.name;
    },
    description: function() {
      return this.item.description;
    },
    isEquipment: function() {
      return this.isEquipped;
    }
  },
  methods: {
    closeDialog: function() {
      this.dialog = false;
    }
  }
};
</script>