//引入安装的signalr包
import * as signalR from '@aspnet/signalr'
const signal = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:52970/chathub')//服务器地址
    .build()
export default {
    install: function (Vue) {
        Vue.prototype.signalr = signal
    }
}