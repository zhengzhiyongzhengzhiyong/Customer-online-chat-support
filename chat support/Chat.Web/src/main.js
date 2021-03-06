import Vue from 'vue'
import App from './App.vue'
import store from './store'
import i18n from './locale'
import router from './router'
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import signalr from '@/utils/signalR';
import Chat from 'vue-beautiful-chat'

Vue.config.productionTip = false

import '@/plugins/axios'
import '@/plugins/filters'
import '@/utils/permission.js'

Vue.use(signalr)
Vue.use(ElementUI);
Vue.use(Chat)

new Vue({
  store,
  i18n,
  router,
  render: h => h(App)
}).$mount('#app')
