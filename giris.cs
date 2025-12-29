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
using project.Utils;
using project.Models;
namespace menu
{
    public class Giris
    {

        static async Task Main(string[] args)
        {
            Bruteword.Run();
            await Menuservices.Menuac();

            Ekipmanbilgi yeniEkipman = new Ekipmanbilgi("daggerbroken", "dagger", 1, 1);
            Ekran.Karaktereekle(yeniEkipman);
        }
    }
}