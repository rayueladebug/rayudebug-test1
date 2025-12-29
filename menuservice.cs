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
using projet.Utils;
using project.Games;
//namespace ekle
/*
*
*
// !!! BURASI COK ACEMICE BILIYORUM PROJENIN ILK ZAMANLARINDAN KALDI DEGISTIRMEYE USENDIGIM ICIN IF ILE HALLETTIM HEPSINI DAHA SONRA SWITCHE(?) GECECEGIM
*
*
*/
public class Menuservices
{
    //public string userinput = Console.ReadLine().ToLower();
    /*static void Main(string[] args)
    {

    }*/
    public static async Task Menuac()
    {
        List<string> secenek = new List<string> { "-csvmenu ", "-typebot(su anda linuxta calismiyor yakinda uyumluluk gelecek windowsta yazmistim)", "-openai", "-toplambul", "-palindrome", "-chatserver", "-chatclient", "-snake", "-uyumluparantez", "-kelimepalindrome(su an duzgun calismiyor)", "-zigzag(su an duzgun calismiyor)", "-tetris"};
        Console.WriteLine("SECENEKLER");
        foreach (var sece in secenek)
        {
            Console.WriteLine(sece);
        }
        string userinput = Console.ReadLine().ToLower();
        if (userinput == "-zigzag"){
            Zigzag.Zigzagvoid();
        }
        if (userinput == "-kelimepalindrome"){
            Palindromesubstring.Runsubstring();
        }
        if (userinput == "-tetris"){
            Game2.Loop();
        }
        if (userinput == "-uyumluparantez"){
            Validparentheses.Result();
        }
        if (userinput == "-snake"){
            await Game.Main1();
        }
        if (userinput == "-chatclient"){
            await Chatclient.Main1();
        }
        if (userinput == "-chatserver"){
            await Chatserver.Main1();
        }
        if (userinput == "-csvmenu")
        {
            Csvservice.Askmenu();
        }
        if (userinput == "-typebot")
        {
            Typingbot.runmenu();

        }
        if (userinput == "-romantoint")
        {
            Twosum.Romantoint();
        }
        if (userinput == "-toplambul")
        {
            Twosum.Toplambul();
        }
        if (userinput == "-palindrome")
        {
            Twosum.Palindrome.Tersesit();
        }
        if (userinput == "-openai")
        {
            await Aibot2.Run();
        }
    }
}