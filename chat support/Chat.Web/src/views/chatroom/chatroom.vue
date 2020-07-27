<template>
    <div>
        <beautiful-chat :participants="participants"
                        :titleImageUrl="titleImageUrl"
                        :onMessageWasSent="onMessageWasSent"
                        :messageList="messageList"
                        :newMessagesCount="newMessagesCount"
                        :isOpen="isChatOpen"
                        :close="closeChat"
                        :icons="icons"
                        :open="openChat"
                        :showEmoji="true"
                        :showFile="true"
                        :showEdition="true"
                        :showDeletion="true"
                        :showTypingIndicator="showTypingIndicator"
                        :showLauncher="true"
                        :showCloseButton="true"
                        :colors="colors"
                        :alwaysScrollToBottom="alwaysScrollToBottom"
                        :messageStyling="messageStyling"
                        :placeholder="txtplaceholder"
                        @onType="handleOnType"
                        @edit="editMessage">
            <template v-slot:header>
                XX公司
            </template>
            <template v-slot:user-avatar="{ message, user }">
                <div style="border-radius:50%; color: pink; font-size: 15px; line-height:25px; text-align:center;background: tomato; width: 25px !important; height: 25px !important; min-width: 30px;min-height: 30px;margin: 5px; font-weight:bold" v-if="message.type === 'text' && user && user.name">
                    {{user.name.toUpperCase()[0]}}
                </div>
            </template>
        </beautiful-chat>
    </div>
</template>
<script>
    import CloseIcon from 'vue-beautiful-chat/src/assets/close-icon.png'
    import OpenIcon from 'vue-beautiful-chat/src/assets/logo-no-bg.svg'
    import FileIcon from 'vue-beautiful-chat/src/assets/file.svg'
    import CloseIconSvg from 'vue-beautiful-chat/src/assets/close.svg'
    import api from '@/api';
    export default{
        name: 'chatroom',
        components: {
        },
        data() {
            return {
                icons: {
                    open: {
                        img: OpenIcon,
                        name: 'default',
                    },
                    close: {
                        img: CloseIcon,
                        name: 'default',
                    },
                    file: {
                        img: FileIcon,
                        name: 'default',
                    },
                    closeSvg: {
                        img: CloseIconSvg,
                        name: 'default',
                    },
                },
                participants: [
                    {
                        id: 'user1',
                        name: 'text',
                        imageUrl: 'https://avatars3.githubusercontent.com/u/1915989?s=230&v=4'
                    }
                ], 
                titleImageUrl: 'https://a.slack-edge.com/66f9/img/avatars-teams/ava_0001-34.png',
                messageList: [
                    { type: 'text', author: `me`, data: { text: `Say yes!` } },
                    { type: 'text', author: `user1`, data: { text: `` } }
                ], 
                newMessagesCount: 0,
                isChatOpen: false,
                txtplaceholder:'请输入信息',
                showTypingIndicator: '', 
                colors: {
                    header: {
                        bg: '#4e8cff',
                        text: '#ffffff'
                    },
                    launcher: {
                        bg: '#4e8cff'
                    },
                    messageList: {
                        bg: '#ffffff'
                    },
                    sentMessage: {
                        bg: '#4e8cff',
                        text: '#ffffff'
                    },
                    receivedMessage: {
                        bg: '#eaeaea',
                        text: '#222222'
                    },
                    userInput: {
                        bg: '#f4f7f9',
                        text: '#565867'
                    }
                }, 
                alwaysScrollToBottom: false, 
                messageStyling: true
            }
        },
        created() {
            Header
        },
        sockets:{
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
            },
            sendMessage(text) {
                if (text.length > 0) {
                    this.newMessagesCount = this.isChatOpen ? this.newMessagesCount : this.newMessagesCount + 1
                    this.onMessageWasSent({ author: 'support', type: 'text', data: { text } })
                }
            },
            onMessageWasSent(message) {
                // called when the user sends a message
                this.messageList = [...this.messageList, message]
            },
            openChat() {
                // called when the user clicks on the fab button to open the chat
                this.isChatOpen = true
                this.newMessagesCount = 0
            },
            closeChat() {
                // called when the user clicks on the botton to close the chat
                this.isChatOpen = false
            },
            handleScrollToTop() {
                // called when the user scrolls message list to top
                // leverage pagination for loading another page of messages
            },
            handleOnType() {
                console.log('Emit typing event')
            },
            editMessage(message) {
                const m = this.messageList.find(m => m.id === message.id);
                m.isEdited = true;
                m.data.text = message.data.text;
            }
        },
        mounted() {
            this.signalr.start().then(() => {
                console.log('连接成功');
            }).catch(error =>{
                if (error.statusCode === 401) {
                    this.$message({
                        message: '登录已过期,请重新登录',
                        type: 'warning'
                    });
                    setTimeout(() => {
                        this.$router.push("/login");
                    }, 1500);
                }
                else {
                    this.$message.error('连接服务器失败');
                    console.error(error.message);
                }
            });
        }
    }
</script>

<style scoped>
</style>