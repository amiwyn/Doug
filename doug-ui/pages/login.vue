<template>
  <v-layout column justify-center align-center>
    <v-flex xs12 sm8 md6>
      <v-card>
        <v-card-title class="headline">Welcome to Doug/Ui</v-card-title>
        <v-card-actions>
          <v-spacer />
          <a
            href="https://slack.com/oauth/authorize?scope=identity.basic,identity.email,identity.team,identity.avatar&client_id=373519414260.374791955143&state=login"
          >
            <img
              alt="Sign in with Slack"
              height="40"
              width="172"
              src="https://platform.slack-edge.com/img/sign_in_with_slack.png"
              srcset="https://platform.slack-edge.com/img/sign_in_with_slack.png 1x, https://platform.slack-edge.com/img/sign_in_with_slack@2x.png 2x"
            />
          </a>
          <v-spacer />
        </v-card-actions>
      </v-card>
    </v-flex>
  </v-layout>
</template>

<script>
import { mapState, mapGetters } from "vuex";

export default {
  layout: "empty",
  computed: mapState({
    authenticated: state => state.user.authenticated
  }),
  head() {
    return {
      title: "Login"
    };
  },
  mounted() {
    if (this.$route.query.code) {
      this.$nextTick(() => {
        this.$nuxt.$loading.start();
        this.$store.dispatch("authenticate", this.$route.query.code).then(() => {
          this.$router.push("/");
          this.$nuxt.$loading.finish();
        });
      });
    }
  }
};
</script>
