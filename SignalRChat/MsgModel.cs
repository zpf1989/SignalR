using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRChat
{
    public class MsgModel
    {
        public MsgType Type { get; set; }
        /// <summary>
        /// 发送者固定唯一标识
        /// Type：
        ///     Broadcast时，From为空
        ///     其他，From为发送者
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 接收者固定唯一标识
        /// Type：
        ///     Broadcast时，To为空
        ///     Group时，To为群标识
        ///     P2P时，To为接收者
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 发送时间：yyyy-MM-dd HH:mm:ss
        /// </summary>
        public string SendTime { get; set; }
    }

    public enum MsgType
    {
        /// <summary>
        /// 广播
        /// </summary>
        Broadcast,
        /// <summary>
        /// 群聊
        /// </summary>
        Group,
        /// <summary>
        /// 点对点（私聊）
        /// </summary>
        P2P
    }
}