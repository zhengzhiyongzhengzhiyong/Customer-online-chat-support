//引入安装的signalr包
import * as signalR from '@aspnet/signalr';
import * as auth from '@/utils/auth';

let options = {
    accessTokenFactory: function () { return auth.getToken(); }
};

const connection  = new signalR.HubConnectionBuilder()
    .withUrl('http://localhost:51709/chathub', options)
    .configureLogging(signalR.LogLevel.Information)
    .build();
export default {
    install: function (Vue) {
        Vue.prototype.signalr = connection
    }
}