<template>
</template>

<script>
import { mapState, mapGetters } from "vuex";

export default {
  layout: "empty",
  head() {
    return {
      title: "Logout"
    };
  },
  mounted() {
    this.$auth.logout()
  }
};
</script>
