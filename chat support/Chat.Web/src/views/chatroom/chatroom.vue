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
            <template v-slot:system-message-body="{ message }">
              提示： {{message.text}}
            </template>
        </beautiful-chat>
    </div>
</template>
<script>
    import CloseIcon from 'vue-beautiful-chat/src/assets/close-icon.png'
    import OpenIcon from 'vue-beautiful-chat/src/assets/logo-no-bg.svg'
    import FileIcon from 'vue-beautiful-chat/src/assets/file.svg'
    import CloseIconSvg from 'vue-beautiful-chat/src/assets/close.svg'
    import { getSupport } from '@/api';
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
                        id: 'me',
                        name: 'text',
                        imageUrl: 'https://avatars3.githubusercontent.com/u/1915989?s=230&v=4'
                    },
                    {
                        id: '1',
                        name: 'Support',
                        imageUrl: 'https://avatars3.githubusercontent.com/u/1915989?s=230&v=4'
                    }
                ], 
                titleImageUrl: 'https://a.slack-edge.com/66f9/img/avatars-teams/ava_0001-34.png',
                messageList: [
                    { type: 'text', author: `me`, data: { text: `Say yes!` } },

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
        beforeCreate() {
            
        },
        created() {
            this.ComputeCount();
            this.CreateOn();
            this.GetSupport();
            //this.sendMessage("请问您有什么要咨询的吗？");
        },
        sockets: {       
        },
        watch: {
        },
        computed: {
        },
        methods: {
            GetSupport() {
                getSupport({
                }).then(res => {                  
                    this.participants = res.data;
                });
            },
            CreateOn() {               
                this.signalr.on('ReceiveMessage', res => {
                    console.log(res);
                    this.newMessagesCount = this.isChatOpen ? this.newMessagesCount : this.newMessagesCount + 1
                    this.messageList = [...this.messageList, res]
                })
            },
            Sendsdfdfe() {
                var user = "2";
                var message = "test";
                this.signalr.invoke("SendChatMessage", user, message).catch(function (err) {
                    console.error(err.toString());
                });
            },
            ComputeCount() {
                this.signalr.on('OnlineCount', res => {
                    console.log(res);
                })
            },
            sendMessage(text) {
                if (text.length > 0) {
                    this.newMessagesCount = this.isChatOpen ? this.newMessagesCount : this.newMessagesCount + 1
                    this.onMessageWasSent({ type: 'text', author: `Jackson`, data: { text: '<a>123123</a>' } })
                }
            },
            onMessageWasSent(message) {
                let userid = this.participants[0].id;
                this.signalr.invoke("SendChatMessage", userid, message).catch(function (err) {
                    console.error(err);
                });
                this.messageList = [...this.messageList, message]
            },
            openChat() {
                this.isChatOpen = true
                this.newMessagesCount = 0
            },
            closeChat() {
                this.isChatOpen = false
            },
            handleScrollToTop() {
            },
            handleOnType() {
                
            },
            editMessage(message) {
                const m = this.messageList.find(m => m.id === message.id);
                m.isEdited = true;
                m.data.text = message.data.text;
            },
            start() {
                this.signalr.start().then(() => {
                    console.log('connect successfully');
                }).catch(error => {
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
                        console.error(error);
                    }
                });
            },
            ConnectSignlr() {
                this.start();
                this.signalr.onclose(()=>{
                    setTimeout( ()=> {
                        console.info('restart connection');
                        this.start();
                    }, 500); 
                });
            }
        },
        mounted() {
            this.ConnectSignlr();
        }
    }
</script>

<style scoped>
</style>