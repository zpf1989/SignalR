using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading;

[assembly: XmlConfigurator(Watch = true, ConfigFile = "log4net.config")]
namespace ServiceMonitor
{
    [HubName("MonitorHub")]
    public class MonitorHub : Hub
    {
        static object locker = new object();
        static List<string> connections = new List<string>();
        static ILog _logger = LogManager.GetLogger(typeof(MonitorHub));
        static bool _collecting = false;
        static CancellationTokenSource _tokenSrc;
        [HubMethodName("CollectData")]
        public void CollectData()
        {
            string connId = Context.ConnectionId;
            if (!connections.Contains(connId))
            {
                lock (locker)
                {
                    if (!connections.Contains(connId))
                    {
                        connections.Add(connId);
                    }
                }
            }
            _logger.Info("new client connected,connection id:" + connId);
            if (_collecting)
            {
                return;
            }
            _collecting = true;

            _tokenSrc = new CancellationTokenSource();
            Task.Run(() =>
                 {
                     _logger.Info("client connected,monitor server start work");
                     MachineInfo info = new MachineInfo();
                     while (true)
                     {
                         if (_tokenSrc.IsCancellationRequested)
                         {
                             break;
                         }
                         try
                         {
                             MachineInfoCollector.Collect(ref info);
                         }
                         catch (Exception ex)
                         {
                             _logger.Error("收集服务器性能数据错误", ex);
                             break;
                         }
                         Clients.All.updateData(info);
                         Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
                     }
                 }, _tokenSrc.Token);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string connId = Context.ConnectionId;
            _logger.Info("one client disconnected,connection id:" + connId);
            if (connections.Contains(connId))
            {
                lock (locker)
                {
                    if (connections.Contains(connId))
                    {
                        connections.Remove(Context.ConnectionId);
                    }
                }
            }
            if (connections.Count < 1)
            {
                lock (locker)
                {
                    //所有客户端都断开了
                    if (connections.Count < 1)
                    {
                        _logger.Info("all clients disconnected,monitor server shutdown");
                        _tokenSrc.Cancel();
                        _collecting = false;
                    }
                }
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}