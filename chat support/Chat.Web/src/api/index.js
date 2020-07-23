import { getAxios } from '@/plugins/axios'

const axios = getAxios()

// 用户登录
export const userLogin = (data) => { return axios.post('/dev-api/user/login', data).then(res => res.data) }
