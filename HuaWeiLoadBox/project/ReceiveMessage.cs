//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LoadBoxControl
//{
//    class ReceiveMessage
//    {
//        ///<summary>
//        ///接收消息
//        ///</summary>
//        private static void ReceiveMessage()
//        {
//            while (true)
//            {
//                //接受消息头（消息校验码4字节 + 消息长度4字节 + 身份ID8字节 + 主命令4字节 + 子命令4字节 + 加密方式4字节 = 28字节）
//                int HeadLength = 28;
//                //存储消息头的所有字节数
//                byte[] recvBytesHead = new byte[HeadLength];
//                //如果当前需要接收的字节数大于0，则循环接收
//                while (HeadLength > 0)
//                {
//                    byte[] recvBytes1 = new byte[28];
//                    //将本次传输已经接收到的字节数置0
//                    int iBytesHead = 0;
//                    //如果当前需要接收的字节数大于缓存区大小，则按缓存区大小进行接收，相反则按剩余需要接收的字节数进行接收
//                    if (HeadLength >= recvBytes1.Length)
//                    {
//                        iBytesHead = socketClient.Receive(recvBytes1, recvBytes1.Length, 0);
//                    }
//                    else
//                    {
//                        iBytesHead = socketClient.Receive(recvBytes1, HeadLength, 0);
//                    }
//                    //将接收到的字节数保存
//                    recvBytes1.CopyTo(recvBytesHead, recvBytesHead.Length - HeadLength);
//                    //减去已经接收到的字节数
//                    HeadLength -= iBytesHead;
//                }
//                //接收消息体（消息体的长度存储在消息头的4至8索引位置的字节里）
//                byte[] bytes = new byte[4];
//                Array.Copy(recvBytesHead, 4, bytes, 0, 4);
//                int BodyLength = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0));
//                //存储消息体的所有字节数
//                byte[] recvBytesBody = new byte[BodyLength];
//                //如果当前需要接收的字节数大于0，则循环接收
//                while (BodyLength > 0)
//                {
//                    byte[] recvBytes2 = new byte[BodyLength < 1024 ? BodyLength : 1024];
//                    //将本次传输已经接收到的字节数置0
//                    int iBytesBody = 0;
//                    //如果当前需要接收的字节数大于缓存区大小，则按缓存区大小进行接收，相反则按剩余需要接收的字节数进行接收
//                    if (BodyLength >= recvBytes2.Length)
//                    {
//                        iBytesBody = socketClient.Receive(recvBytes2, recvBytes2.Length, 0);
//                    }
//                    else
//                    {
//                        iBytesBody = socketClient.Receive(recvBytes2, BodyLength, 0);
//                    }
//                    //将接收到的字节数保存
//                    recvBytes2.CopyTo(recvBytesBody, recvBytesBody.Length - BodyLength);
//                    //减去已经接收到的字节数
//                    BodyLength -= iBytesBody;
//                }
//                //一个消息包接收完毕，解析消息包
//                UnpackData(recvBytesHead, recvBytesBody);
//            }
//        }
//        /// <summary>
//        /// 解析消息包
//        /// </summary>
//        /// <param name="Head">消息头</param>
//        /// <param name="Body">消息体</param>
//        public static void UnpackData(byte[] Head, byte[] Body)
//        {
//            byte[] bytes = new byte[4];
//            Array.Copy(Head, 0, bytes, 0, 4);
//            Debug.Log("接收到数据包中的校验码为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0)));

//            bytes = new byte[8];
//            Array.Copy(Head, 8, bytes, 0, 8);
//            Debug.Log("接收到数据包中的身份ID为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt64(bytes, 0)));

//            bytes = new byte[4];
//            Array.Copy(Head, 16, bytes, 0, 4);
//            Debug.Log("接收到数据包中的数据主命令为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0)));

//            bytes = new byte[4];
//            Array.Copy(Head, 20, bytes, 0, 4);
//            Debug.Log("接收到数据包中的数据子命令为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0)));

//            bytes = new byte[4];
//            Array.Copy(Head, 24, bytes, 0, 4);
//            Debug.Log("接收到数据包中的数据加密方式为：" + IPAddress.NetworkToHostOrder(BitConverter.ToInt32(bytes, 0)));

//            bytes = new byte[Body.Length];
//            for (int i = 0; i < Body.Length;)
//            {
//                byte[] _byte = new byte[4];
//                Array.Copy(Body, i, _byte, 0, 4);
//                i += 4;
//                int num = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(_byte, 0));

//                _byte = new byte[num];
//                Array.Copy(Body, i, _byte, 0, num);
//                i += num;
//                Debug.Log("接收到数据包中的数据有：" + Encoding.UTF8.GetString(_byte, 0, _byte.Length));
//            }
//        }
//    }
//}
