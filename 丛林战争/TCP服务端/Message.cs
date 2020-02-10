using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP服务端
{
    class Message
    {
        private byte[] data = new byte[1024];

        private int startIndex=0;//存取了字节数

        public  byte[] Data
        {
            get { return data; }
        }

        public int Length
        {
            get { return startIndex; }
        }

        public void AddCount(int count)
        {
            startIndex += count;
        }
        public int RemainSize
        {
            get { return data.Length - startIndex; }
        }

        /// <summary>
        ///读取数据，解析 
        /// </summary>
        public void ReadMessage()
        {
            //Console.WriteLine(startIndex);
            while (true)
            {
                if (startIndex <= 4) return;
                int count = BitConverter.ToInt32(data, 0);
                if (startIndex - 4 >= count)
                {
                    string s = Encoding.UTF8.GetString(data, 4, count);
                    Console.WriteLine("解析出一条数据：" + s);
                    Array.Copy(data, count + 4, data, 0, startIndex - count - 4);
                    startIndex -= (4 + count);
                }
                else
                {
                    break;
                }
            }
        }

     }
}
