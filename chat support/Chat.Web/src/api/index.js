import { getAxios } from '@/plugins/axios'

const axios = getAxios()

// 用户登录
export const userLogin = (data) => { return axios.post('/api/Auth/Login', data); }

export const getUserInfo = (data) => { return axios.post('/api/Auth/getUserInfo', data); }

export const getSupport = (data) => { return axios.post('/api/Auth/GetSupport', data); }
