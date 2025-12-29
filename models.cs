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
namespace project.Models
{
    /*
    *
    *
    // OGRENME AMACLI AMACSIZ CLASSLAR OLUSTURULDU KALDIRILACAK
    *
    *
    */
    public class Program
    {
        /*static void Main(string[] args)
        {
            Ekipmanbilgi yeniEkipman = new Ekipmanbilgi("daggerbroken", "dagger", 1, 1);
            Ekran.Karaktereekle(yeniEkipman);
        }*/
    }
    public class Listnode
    {
        public int val;
        public Listnode next;
        public Listnode(int val = 0, Listnode next = null)
        {
            this.val = val;
            this.next = next;
        }
    }

    public class Bilgiler
    {
        public string Ad { get; set; }

        public string Soyad { get; set; }
        public int Yas { get; set; }
    }
    public class Ekipmanbilgi
    {
        public bool isbuffed;
        public string ISIM { get; set; }
        public string TIP { get; set; }

        public int HASAR { get; set; }
        public int BLOK { get; set; }
        public Ekipmanbilgi(string isim, string tip, int hasar, int blok)
        {
            ISIM = isim;
            TIP = tip;
            HASAR = hasar;
            BLOK = blok;
        }
    }
    public class Karakterbilgi
    {
        public string SINIF { get; set; }
        public Karakterbilgi(string sinif)
        {
            SINIF = sinif;
        }

    }
    public class Ekran
    {
        Bilgiler asdd = new Bilgiler();
        //while (true){}
        //List<Ekipmanbilgi> ekipmanBilgi = new List<Ekipmanbilgi>();
        //List<Karakterbilgi> karakterBilgi = new List<Karakterbilgi>();
        public static List<Ekipmanbilgi> ekipmanBilgi = new();
        Ekipmanbilgi asd = new ("asd", "x", 10, 10);
        public static List<Karakterbilgi> karakterBilgi = new();
        public static void Karaktereekle(Ekipmanbilgi ekipmanS)
        {
            ekipmanBilgi.Add(ekipmanS);
        }


    }
}
namespace project.AI
{
    public class Response
    {

    }
    /*public class ChatMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }*/
}