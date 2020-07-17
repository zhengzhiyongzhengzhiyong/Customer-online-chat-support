'use strict'

import Vue from 'vue'
import axios from 'axios'
import { getToken } from '@/utils/auth'

const config = {
  baseURL: 'http://luoyangc.cn:22330',
  timeout: 60 * 1000
}

const _axios = axios.create(config)

_axios.interceptors.request.use(
  config => {
    const token = getToken()
    if (token) config.headers['token'] = token
    return config
  },
  error => {
    return Promise.reject(error)
  }
)

_axios.interceptors.response.use(
  response => {
    if (response.data.code === 20000) {
      return response.data
    } else {
      console.log(response.data.message)
      return response.data
    }
  },
  error => {
    const status = error.response.status
    switch (status) {
      case 400:
        window.console.log('请求参数错误')
        break
      default:
        window.console.log(status)
        break
    }
    const message = error.response.data.message || '未知错误'
  }
)

export const getAxios = () => { return _axios }

Plugin.install = function(Vue) {
  Vue.axios = _axios
  window.axios = _axios
  Object.defineProperties(Vue.prototype, {
    axios: {
      get() {
        return _axios
      }
    },
    $axios: {
      get() {
        return _axios
      }
    }
  })
}

Vue.use(Plugin)

export default Plugin
