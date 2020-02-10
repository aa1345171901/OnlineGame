using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Reflection;
using GameServer.Servers;

namespace GameServer.Controller
{
    class ControllerManager
    {
        private Server server;
        Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController> ();
        public ControllerManager(Server server)
        {
            this.server = server;
            InitController();
        }

        public void InitController()
        {
            DefaultController defaultController = new DefaultController();
            controllerDict.Add(defaultController.RequestCode, defaultController);
            controllerDict.Add(RequestCode.User, new UserController());
            controllerDict.Add(RequestCode.Room, new RoomController());
            controllerDict.Add(RequestCode.Game, new GameController());
        }

        public void RequestHander(RequestCode requestCode,ActionCode actionCode ,string data,Client client)//data为传递进controller中的参数
        {
            BaseController baseController;
            bool isGet = controllerDict.TryGetValue(requestCode, out baseController);
            if(isGet==false)
            {
                Console.WriteLine("无法得到[" + requestCode + "]所对应的Conrtoller，无法处理");return;
            }
            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo mi = baseController.GetType().GetMethod(methodName);   //反射出方法名，利用方法名执行方法
            if(mi==null)
            {
                Console.WriteLine("[警告]在Controller[" + baseController + "]中没有找到对应的[" + methodName + "]方法");return;
            }
            object[] parameters = new object[]{ data,client ,server };  //函数的参数
            object o= mi.Invoke(baseController, parameters);            //返回的值
            if(o==null||string.IsNullOrEmpty(o as string ))
            {
                return;
            }
            server.SendResponse(actionCode, client, o as string);
        }
    }
}
