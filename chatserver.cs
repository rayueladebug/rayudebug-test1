using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.DevTools.V136.Overlay;
using OpenQA.Selenium.Support.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using OpenAI;
using OpenAI.Chat;
using System.Threading.Tasks;
//using OpenQA.Selenium.DevTools.V136.Debugger;
using System.Security.Cryptography.X509Certificates;
using OpenQA.Selenium.DevTools.V139.Page;
using project.Models;
using project.Utils;
using project.AI;
using OpenQA.Selenium.DevTools.V137.Runtime;
using projet.Utils;
namespace project.Utils
{
    public class Chatserver
    {
        public static TcpListener listener;
        public static List<TcpClient> clients = new List<TcpClient>();
        public static readonly object lockobj = new object();

        public static async Task Main1()
        {
            int port = 6660;
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"sunucu {port} portunda basladi");
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                lock (lockobj)
                {
                    clients.Add(client);
                }
                //await Handlenickname(client);
                Console.WriteLine("yeni istemci baglandi");
                Thread t = new Thread(Handleclient);
                t.Start(client);
            }
        }
        /*public static async Task Handlenickname(TcpClient obj){
            TcpClient client = obj;
            NetworkStream networkstream = client.GetStream();
            byte[] buffer = new byte[1024];
            try{
                Chatclient.Main1.thisnamebyte
            }
            catch(Exception ex){
                Console.WriteLine($"hata {ex}");
            }
            
        }*/
        public static void Handleclient(object obj)
        {
            Thread.Sleep(500);
            TcpClient client = (TcpClient)obj;
            NetworkStream networkstream = client.GetStream();
            byte[] buffer = new byte[1024];
            try
            {
                byte[] nicknamebyte = new byte[1024];
                int nameread = networkstream.Read(nicknamebyte, 0, nicknamebyte.Length);
                string name = Encoding.UTF8.GetString(nicknamebyte, 0, nameread);
                while (true)
                {
                    int bytesread = networkstream.Read(buffer, 0, buffer.Length);
                    if (bytesread == 0)
                    {
                        break;
                    }
                    string msg = Encoding.UTF8.GetString(buffer, 0, bytesread);
                    
                    Broadcast(name, msg, client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"hata {ex}");
            }
            /*finally
            {
                lock (lockobj)
                {
                    clients.Remove(client);
                }
                client.Close();
                Console.WriteLine("client ayrildi");
            }*/
        }
        public static void Broadcast(string nickname, string msg, TcpClient sender)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            byte[] namedata = Encoding.UTF8.GetBytes(nickname);
            string full = ($"{nickname}: {msg}");
            byte[] fullmsg = Encoding.UTF8.GetBytes(full);
            lock (lockobj)
            {
                foreach (var c in clients)
                {
                    if (c == sender)
                    {
                        continue;
                    }
                    try
                    {
                        NetworkStream networkstream = c.GetStream();
                        networkstream.Write(fullmsg, 0, fullmsg.Length);
                        //networkstream.Write(namedata, 0, namedata.Length);
                        //networkstream.Write(data, 0, data.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"hata {ex}");
                    }
                }
            }
        }
        // public static TcpListener listener;
        // public static List<TcpClient> clients = new List<TcpClient>();
        // public static readonly object lockobj = new object();
        // public static void main()
        // int port = 6660;
        // listener = new TcpListener(IPAdress.Any, port);
        // listener.Start();
        // Console.WriteLine($"sunucu {port} portunda basladi");
        // while (true)
        // TcpClient client = listener.AcceptTcpClient();
        // lock (lockobj)
        // clients.Add(client);
        // Console.WriteLine("yeni istemci baglandi");
        // Thread t = new Thread(Handleclient);
        // t.Start(client)
        // public static void Handleclient(object obj)
        // TcpClient client = (TcpClient)obj;
        // NetworkStream networkstream = client.GetStream();
        // byte[] buffer = new byte[1024];
        // try{}
        // while(true){}
        // int bytesread = networkstream.Read(buffer, 0, buffer.Length);
        // if (bytesread == 0){}
        // break;
        // string msg = Encoding.UTF8.GetString(buffer, 0, bytesread.Length);
        // Broadcast(msg, client);
        // catch(Exception ex){}
        // Console.WriteLine($"hata {ex}");
        // finally{}
        // lock(lockobj){}
        // clients.Remove(client);
        // client.Close();
        // Console.WriteLine($"client cikarildi");
        // public static void Broadcast(string msg, TcpClient sender){}
        // byte[] data = Encoding.UTF8.GetBytes(msg);
        // lock (lockobj){}
        // foreach (var c in clients){}
        // if (c == sender){}
        // continue;
        // try{}
        // NetworkStream networkstream = c.GetStream;
        // networkstream.Write(data, 0, data.Length);
        // catch{Exception ex}
        // Console.WriteLine($"hata {ex}");
    }
}