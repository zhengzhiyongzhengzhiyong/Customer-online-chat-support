<template>
    <div>
    </div>
</template>
<script>
    import api from '@/api';
    export default{
        name: 'chatroom',
        data() {
            return {
            }
        },
        created() {

        },
        sockets:{
        },
        components: {
        },
        watch: {
        },
        computed: {
        },
        methods: {
            CreateOn() {
                //ReceiveMessage和Clients.All.SendAsync中的第一个参数对应
                this.signalr.on('ReceiveMessage', res => {
                    //可以写业务逻辑
                    //res 返回的是后台传过来的数据
                    console.log(res);
                })
            },
            SendMessage() {
                var user = "123";
                var message = "123";
                //SendMessage和后台方法对应
                this.signalr.invoke("SendMessage", user, message).catch(function (err) {
                    console.error(err.toString());
                });
            }
        }
        },
        mounted() {
            this.signalr.start().then(() => {
                console.log('连接成功');
            })
        }
    }
</script>

<style scoped>
</style>