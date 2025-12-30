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
using OpenQA.Selenium.DevTools.V137.Debugger;
namespace projet.Utils{
    public class Chatclient{
        public static readonly object lockobj = new object();
        public static TcpClient thisclient = new TcpClient();
        public static NetworkStream thisstream;
        public static async Task Main1()
        {
            byte[] buffer = new byte[1024];
            string name = Ask();
            int port = 6660;
            thisclient.Connect("", port);
            thisstream = thisclient.GetStream();
            byte[] deneme = Encoding.UTF8.GetBytes(name);
            thisstream.Write(deneme, 0, deneme.Length);
            //Ask();
            //Thread t = new Thread(Handlemessage);
            Task.Run(() => Handlemessage());
            //t.Start();
            while(true){
                try{
                    //int port = 6660;
                    //thisclient.Connect(***"172.18.21.30"***, port);
                    //thisstream = thisclient.GetStream();
                    // byte[] buffer = new byte[1024];???
                    string userinput = Console.ReadLine();
                    byte[] data = Encoding.UTF8.GetBytes(userinput);
                    thisstream.Write(data, 0, data.Length);
                }
                catch(Exception ex){
                    Console.WriteLine($"hata {ex}");
                }
            }
        }
        public static string Ask(){
            //byte[] buffer = new byte[1024];
            Console.WriteLine("nickname gir");
            string nickname = Console.ReadLine();
            return nickname;
        }
        public static void Handlemessage(){
            byte[] buffer = new byte[1024];
            while (true){
                int bytesred = thisstream.Read(buffer, 0, buffer.Length);
                if (bytesred == 0){
                    break;
                }
                string message = Encoding.UTF8.GetString(buffer, 0, bytesred);
                Console.WriteLine(message);
            }
        }
    }
}