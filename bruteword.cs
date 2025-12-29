using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.DevTools.V136.Overlay;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using OpenAI;
using OpenAI.Chat;
using System.Threading.Tasks;
//using OpenQA.Selenium.DevTools.V136.Debugger;
using System.Security.Cryptography.X509Certificates;
using OpenQA.Selenium.DevTools.V139.Page;
using project.Models;
namespace project.Utils
{
    public class Bruteword
    {
        public static void Run()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ";
            var result = "";
            var i = 0;
            var target = "DENEME uygulamama hos geldin";
            while (result != target)
            {
                foreach (char c in alphabet)
                {
                    string guess = result + c;
                    Console.WriteLine(guess + "\r");
                    Thread.Sleep(5);
                    if (c == target[i])
                    {
                        result += c;
                        i++;
                        Thread.Sleep(5);
                        break;
                    }
                }
            }
        }
    }
}