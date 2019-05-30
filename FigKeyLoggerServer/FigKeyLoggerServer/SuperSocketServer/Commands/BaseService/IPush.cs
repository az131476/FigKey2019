using System;
using System.Linq;
using SuperSocketServer.AppBase;

namespace SuperSocketServer.Commands.BaseService
{
  public  interface IPush
    {
       
    }

    public static class ExtendPush
    {
        public static void Push<T>(this T exampl, MySession session,ushort action, string text) where T : IPush
        {
            var response = BitConverter.GetBytes(action).Reverse().ToList();
            var arr = System.Text.Encoding.UTF8.GetBytes(text);
            response.AddRange(BitConverter.GetBytes((ushort)arr.Length).Reverse().ToArray());
            response.AddRange(arr);

            session.Send(response.ToArray(), 0, response.Count);
        }
    }
}
