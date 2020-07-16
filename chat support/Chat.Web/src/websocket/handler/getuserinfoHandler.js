import AbstractMessageHandler from "./abstractmessagehandler";
import { PUB_ACK, UPUI, SUCCESS_CODE } from "../../constant";
import UserInfo from "../model/userInfo";
import FutureResult from "../future/futureResult";

export default class GetUserInfoHandler extends AbstractMessageHandler{
    match(proto){
        return proto.signal == PUB_ACK && proto.subSignal == UPUI;
    }

    processMessage(proto){
       if(proto.content != null && proto.content != ''){
           var userInfoList = JSON.parse(proto.content);
           var stateFriendList = [];
           var userInfos = [];
           for(var i in userInfoList){
               var displayName = userInfoList[i].displayName === '' ? userInfoList[i].mobile : userInfoList[i].displayName;
               stateFriendList.push({
                id: parseInt(i) + 1,
                wxid: userInfoList[i].uid, //微信号
                initial: "ABC", //姓名首字母
                img: userInfoList[i].portrait, //头像
                signature: displayName, //个性签名
                nickname: displayName,  //昵称
                sex: 0,   //性别 1为男，0为女
                remark: displayName,  //备注
                area: userInfoList[i].address,  //地区
               });
               userInfos.push(UserInfo.convert2UserInfo(userInfoList[i]));
           }
           if(this.vueWebsocket.resolvePromiseMap.has(proto.messageId)){
               
              var promiseReslove = this.vueWebsocket.resolvePromiseMap.get(proto.messageId);
              clearTimeout(promiseReslove.timeoutId);
               displayName = userInfoList[0].displayName === '' ? userInfoList[0].mobile : userInfoList[0].displayName;
              promiseReslove.resolve(new FutureResult(SUCCESS_CODE,displayName));
              this.vueWebsocket.resolvePromiseMap.delete(proto.messageId);
           }
        
           this.vueWebsocket.sendAction("updateUserInfos",userInfos);
           this.vueWebsocket.sendAction("updateFriendList",stateFriendList);
       }
    }
}