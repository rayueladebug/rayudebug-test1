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
using OpenAI.Graders;
namespace project.Utils
{
    public class Typingbot
    {
        public static void runmenu()
        {
            Console.WriteLine("anonim icin 1 hesabinizla giris yapmak icin 2");
            string userinput = Console.ReadLine();
            if (userinput == "1")
            {
                Run2();
            }
            else if (userinput == "2")
            {
                Run1();
            }
            else
            {
                //return;
                Typingbot.runmenu();
            }
        }
        public static void Run1()
        {
            var index = 0;
            var options = new ChromeOptions();
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddExcludedArgument("enable-automation");
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36");
            options.AddArgument("--disable-notifications");
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            var driver = new ChromeDriver(options);
            ((IJavaScriptExecutor)driver).ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => false})");
            driver.Navigate().GoToUrl("https://10fastfingers.com/login");
            var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(10));
            driver.Manage().Window.Maximize();
            try
            {
                var cookiebutton = driver.FindElement(By.Id("CybotCookiebotDialogBodyLevelButtonLevelOptinAllowallSelection"));
            }
            catch
            {

            }
            Console.WriteLine("hazir olduugnda enter bas");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                wait.Until(driver => driver.FindElement(By.ClassName("highlight")));
                var inputfield = wait.Until(driver => driver.FindElement(By.Id("inputfield")));
                if (inputfield != null)
                {
                    inputfield.Click();
                }
                while (true)
                {
                    var wordlist = driver.FindElement(By.XPath($"//span[@wordnr='{index}']"));
                    string word = wordlist.Text;
                    foreach (char c in word)
                    {
                        inputfield.SendKeys(c.ToString());
                        Thread.Sleep(5);
                    }
                    inputfield.SendKeys(" ");
                    index++;
                    Thread.Sleep(20);
                }
                //return;
            }
        }
        public static void Run2()
        {
            var index = 0;
            var options = new ChromeOptions();
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddExcludedArgument("enable-automation");
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36");
            options.AddArgument("--disable-notifications");
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            var driver = new ChromeDriver(options);
            ((IJavaScriptExecutor)driver).ExecuteScript("Object.defineProperty(navigator, 'webdriver', {get: () => false})");
            driver.Navigate().GoToUrl("https://10fastfingers.com/typing-test/turkish");
            driver.Manage().Window.Maximize();
            var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(10));
            Console.WriteLine("hazir oldugunda enter bas");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Enter)
            {
                wait.Until(driver => driver.FindElement(By.ClassName("highlight")));
                var inputfield = wait.Until(driver => driver.FindElement(By.Id("inputfield")));
                if (inputfield != null)
                {
                    inputfield.Click();
                }
                while (true)
                {
                    var wordlist = driver.FindElement(By.XPath($"//span[@wordnr='{index}']"));
                    var word = wordlist.Text;
                    foreach (char c in word)
                    {
                        inputfield.SendKeys(c.ToString());
                        Thread.Sleep(5);
                    }
                    inputfield.SendKeys(" ");
                    index++;
                    Thread.Sleep(20);
                }
            }
        }
    }
}