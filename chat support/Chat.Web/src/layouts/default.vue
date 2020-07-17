<template>
  <div class="layout" :class="getThemeName">
    <router-view></router-view>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
export default {
  name: 'Layout',
  components: {
  },
  computed: {
    ...mapGetters([
      'theme',
      'cache'
    ]),
    isTableNav() {
      const currentPath = this.$route.name
      const tablePaths = this.cache.map(item => item.toLowerCase())
      return tablePaths.indexOf(currentPath) === -1
    },
    getThemeName() {
      return `theme--${this.theme}`
    }
  },
  methods: {
    onClickLeft() { // 返回上级路由
      this.$router.go(-1)
    },
    onClickRight() {
      this.$toast('搜索')
    }
  }
}
</script>

<style scoped>

</style>
