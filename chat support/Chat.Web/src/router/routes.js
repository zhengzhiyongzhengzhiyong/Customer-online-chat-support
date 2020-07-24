const Login = resolve => require(['@/views/login/index'], resolve);
const chatroom = resolve => require(['@/views/chatroom/chatroom'], resolve);

const frameOut = [
  {
    path: '/login',
    name: 'Login',
    meta: { auth: 0, title: '用户登录' },
    component: Login
  },
  {
    path: '/',
    name: 'ToLogin',
    redirect: '/login'
    },
    {
        path: '/chatroom',
        name: '聊天室',
        meta: { auth: 1, title: '聊天室' },
        component: chatroom
    }
]

const errorPage = [
  {
    path: '*',
    name: '404',
    component: () => import('@/views/errors/404')
  }
]

export default [
  ...frameOut,
  ...errorPage
]
