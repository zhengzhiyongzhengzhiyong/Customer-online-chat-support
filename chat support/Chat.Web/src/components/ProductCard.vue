<template>
  <div class="product-card">
    <van-image lazy-load :src="item.merchandisePictures[0].image" />
    <div class="product-card__title">{{ item.name }}</div>
    <div class="product-card__price">
      <span class="product-card__present">￥{{ item.currentPrice }}</span>
      <s class="product-card__original">￥{{ item.originalPrice }}</s>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    item: {
      type: Object,
      default: () => []
    }
  }
}
</script>

<style scoped>

</style>
