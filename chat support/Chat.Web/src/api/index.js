import { getAxios } from '@/plugins/axios'

const axios = getAxios()

// 用户登录
export const userLogin = (data) => { return axios.post('/api/Auth/Login', data).then(res => res.data) }
