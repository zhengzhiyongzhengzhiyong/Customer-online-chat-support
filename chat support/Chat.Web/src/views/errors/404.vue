<template>
  <div class="error">
    <p>页面被噬元兽吞噬了</p>
    <el-button icon="arrow-left" type="info" @click="onBack">返回</el-button>
    <el-button icon="home-o" type="info" to="/">首页</el-button>
  </div>
</template>

<script>
export default {
  methods: {
    onBack() {
      this.$router.go(-1)
    }
  }
}
</script>

<style scoped>
</style>
