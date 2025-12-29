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
    using project.Utils;
    namespace project.Utils
    {
        public class Csvservice
        {
            //List<string> hizmetler = new List<string>{"csvekle", "csvtemizle", "csvgoster"};
            public static void Askmenu()
            {
                //List<Bilgiler> veriler = Csvservice.Csvekle();
                List<string> hizmetler = new List<string> { "csvekle", "csvtemizle", "csvgoster" };
                foreach (var secenek in hizmetler)
                {
                    Console.WriteLine($"-{secenek}");
                }
                string userinput = Console.ReadLine().ToLower();
            if (userinput == "csvekle")
            {
                var csvekle = Csvekle();
                Kaydet(csvekle);
                Menuservices.Menuac();
            }
            else if (userinput == "csvtemizle")
            {
                Csvtemizle();
                Menuservices.Menuac();
            }
            else if (userinput == "csvgoster")
            {
                Csvgoster();
                Menuservices.Menuac();    
            }
            else
            {
                Menuservices.Menuac();
            }
            }
            public static List<Bilgiler> Csvekle()
            {
                List<Bilgiler> veriler = new List<Bilgiler>();
                while (true)
                {
                    Console.WriteLine("ad");
                    string ad = Console.ReadLine();
                    Console.WriteLine("soyad");
                    string soyad = Console.ReadLine();
                    Console.WriteLine("yas");
                    int yas = int.Parse(Console.ReadLine());
                    veriler.Add(new Bilgiler { Ad = ad, Soyad = soyad, Yas = yas });
                    Console.WriteLine("başka kişi? y/n");
                    string devam = Console.ReadLine().ToLower();
                    if (devam != "y")
                    {
                        break;
                    }
                    //filename = "veriler.csv";

                }
                return veriler;
            }
            public static void Kaydet(List<Bilgiler> veriler)
            {
                //List<Bilgiler> veriler = Csvservice.Csvekle();
                string filename = "girisler.csv";
                var satirlar = new List<string>();
                if (!File.Exists(filename))
                {
                    satirlar.Add("Ad, Soyad, Yaş");
                    File.WriteAllLines(filename, satirlar);
                }
                foreach (var giris in veriler)
                {
                    satirlar.Add($"{giris.Ad}, {giris.Soyad}, {giris.Yas}");
                    File.AppendAllLines(filename, satirlar);
                }
                
                //File.AppendAllLines(filename, satirlar);
                Console.WriteLine("kaydedildi");
            }
            public static void Csvtemizle()
            {
                string filename = "girisler.csv";
                File.Delete(filename);
            }
            public static void Csvgoster()
            {
                string filename = "girisler.csv";
                if (!File.Exists(filename))
                {
                    Console.WriteLine("dosyan bulunmuyor");
                }
                else
                {
                    var metinler = File.ReadAllLines(filename);
                    foreach (var satirlar in metinler)
                    {
                        Console.WriteLine(satirlar);
                    }
                }
            }
        }
    }