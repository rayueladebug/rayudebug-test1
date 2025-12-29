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
using System.Threading.Channels;
using OpenQA.Selenium.Internal;
using System.Text;
using System.Diagnostics.Metrics;
namespace project.Utils
{
    /*
    *
    // LEETCODE SORULARIMIN BAZILARINI PRATIK ICIN BURADA TEKRAR YAZIYORUM
    *
    */
    public class Leetcode
    {
        public static Listnode Listtoplama(Listnode l1, Listnode l2)
        {
            Listnode dummy = new Listnode(0);
            Listnode current = dummy;
            int carry = 0;
            while (l1 != null || l2 != null || carry != 0)
            {
                //Listnode l1 = new Listnode(2, new Listnode(4, new Listnode(3)));
                int val1 = (l1 != null) ? l1.val : 0;
                int val2 = (l2 != null) ? l2.val : 0;
                int sum = val1 + val2 + carry;
                int digit = sum % 10;
                carry = sum / 10;
                current.next = new Listnode(digit);
                current = current.next;
                if (l1 != null) l1 = l1.next;
                if (l2 != null) l2 = l2.next;
            }
            return dummy.next;
        }
        public static Listnode Mergelist(Listnode list1, Listnode list2)
        {
            Listnode dummy = new Listnode(0);
            Listnode current = dummy;
            while (list1 != null || list2 != null)
            {
                int? val1 = (list1 != null) ? list1.val : null;
                int? val2 = (list2 != null) ? list2.val : null;
                if (val1 != null && (val2 == null || val2 > val1))
                {
                    current.next = new Listnode(val1 ?? default(int));
                    list1 = list1.next;
                }
                else if (val2 != null && (val1 == null || val1 > val2))
                {
                    current.next = new Listnode(val2 ?? default(int));
                    list2 = list2.next;
                }
                else if (val1 == val2 && val1 != null && val2 != null)
                {
                    current.next = new Listnode(val1 ?? default(int));
                    current = current.next;
                    list1 = list1.next;
                    list2 = list2.next;
                    current.next = new Listnode(val2 ?? default(int));
                }
                current = current.next;
                //if (list1 != null) list1 = list1.next;
            }
            return dummy.next;
        }
    }
    public class Twosum
    {
        public static void Toplambul()
        {
            Console.WriteLine("sayilari arasinda bosluk olacak sekilde gir");
            string userinput1 = Console.ReadLine();
            int[] nums = userinput1.Split(' ').Select(int.Parse).ToArray();
            Console.WriteLine("hedef sayiyi giriniz");
            string userinput2 = Console.ReadLine();
            int num = int.Parse(userinput2);
            Dictionary<int, int> numindex = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length; i++)
            {
                int needed = num - nums[i];
                if (numindex.ContainsKey(needed))
                {
                    Console.WriteLine($"gerekli sayilar {nums[i]}, {needed}");
                    return;
                }
                if (!numindex.ContainsKey(needed))
                {
                    numindex.Add(nums[i], i);
                }
            }
        }
        public static void Romantoint()
        {
            List<string> harfler = new List<string> { "I", "V", "X", "L", "C", "D", "M" };
            Console.WriteLine("romen rakamlari gir");
            foreach (var v in harfler)
            {
                Console.WriteLine($"-{v}");
            }
            string userinput = Console.ReadLine().ToUpper();
            int numbers(char c)
            {
                switch (c)
                {
                    case 'I': return 1;
                    case 'V': return 5;
                    case 'X': return 10;
                    case 'L': return 50;
                    case 'C': return 100;
                    case 'D': return 500;
                    case 'M': return 100;
                    default: return 0;
                }
            }
            string chars = userinput;
            var arrayed = userinput.ToCharArray();
            int result = 0;
            int currentvalue = 0;
            for (int i = 0; i < arrayed.Length - 1; i++)
            {
                currentvalue = numbers(arrayed[i]);
                result += (currentvalue < numbers(arrayed[i + 1]) ? -1 : +1) * currentvalue;
            }
            Console.WriteLine(result + numbers(arrayed[arrayed.Length - 1]));
        }
        public class Palindrome
        {
            public static void Tersesit()
            {
                Console.WriteLine("Sayı gir:");
                int x = int.Parse(Console.ReadLine());
                bool ispalindrome;

                if (x < 0)
                {
                    ispalindrome = false;
                }
                else
                {
                    int original = x;
                    int reversed = 0;
                    while (x != 0)
                    {
                        int digit = x % 10;
                        if (reversed > (int.MaxValue - digit) / 10)
                        {
                            ispalindrome = false;
                            Console.WriteLine("Taşma hatası");
                            return;
                        }

                        reversed = reversed * 10 + digit;
                        x /= 10;
                    }

                    ispalindrome = (original == reversed);
                }

                if (ispalindrome)
                {
                    Console.WriteLine("palindrome");
                }
                else
                {
                    Console.WriteLine("palindrome degil");
                }

            }
        }
    }
    public class Validparentheses{
        public static Stack<char> stack = new Stack<char>();
        public static void Result(){
            string checksymbol = Console.ReadLine();
            if (Isvalid(checksymbol)){
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("uyumlu");
                Console.ResetColor();
                Console.CursorVisible = true;
            }
            else {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("uyumsuz");
                Console.ResetColor();
                Console.CursorVisible = true;
            }
            Console.WriteLine("devam etmek icin bir tusa bas");
            Console.ReadKey();
            Menuservices.Menuac();
        }
        public static bool Isvalid(string symbols){
            foreach (char c in symbols){
                if (Isopening(c)) stack.Push(c);
                else {
                    char last = stack.Pop();
                    if (!Ismatching(last, c)) return false;
                }
            }
            return stack.Count == 0;
        }
        public static bool Isopening(char a)
        {
            bool s = a switch
            {
                '{' => true,
                '[' => true,
                '(' => true,
                _ => false
            };
            return s;
        }
        public static bool Ismatching(char a, char b)
        {
        switch (a, b)
        {
            case ('(', ')'): return true;
            case ('{', '}'): return true;
            case ('[', ']'): return true;
            default: return false;
        }
        }
    }
    public class Palindromesubstring{
        public static void Runsubstring(){
            Console.WriteLine("harf dizisi gir");
            string userinput3 = /*int.Parse*/Console.ReadLine();
            Console.WriteLine(Stringpalindrome(userinput3));
        }
        private static string Stringpalindrome(string inp){
            bool caseswitch = false;
            int len = inp.Length;
            int low = 0;
            int high = 1;
            if (len == 1) return inp;
            for (int i = 0; i < len; i++){
                for (int z = 1; z < len; z++){
                    if (Isstringpalindrome(i, z, inp) && high - low > 2) low = i; high = z - i + 1; caseswitch = true;
                }
            }
            return /*inp.Substring(low, high)*/ caseswitch switch
            {
                false => "palindrome degil",
                true => inp.Substring(low, high)
            };
        }
        private static bool Isstringpalindrome(int s, int e, string word){
            while (e > s){
                if (word[e] != word[s]){
                    return false;
                }
                e--;
                s++;
            }
            return true;
        }
    }
    public class Zigzag{
        public static void Zigzagvoid(){
            string k = Console.ReadLine();
            int r = int.Parse(Console.ReadLine());
            Console.WriteLine(Convert(k, r));
        }
        public static string Convert(string w, int row){
            if (row == 1) return w;
            StringBuilder builder = new StringBuilder();
            int len = w.Length;
            int cycle = row * 2 - 2;
            for (int i = 0; i < row; i++)
            {
                for (int z = 0; z + i< len; z += cycle)
                {
                    builder.Append(w[i + z]);
                    if (i != 0 && i != row - 1 && cycle + i - z < len){
                        builder.Append(w[cycle + i - z]);
                    }
                }
            }
            //builder.ToString();
            return builder.ToString();
            //return
        }
    }
    public class Addbinary{
        private void Addbin(){
            Console.WriteLine("1 ve 0 dan oluşan 2 adet sayı gir");
            string a1 = Console.ReadLine();
            string b1 = Console.ReadLine();
            Console.WriteLine(Addb(a1, b1));
        }
        public string Addb(string a, string b){
            var num = new List<int>();
            for (int i = a.Length - 1, x = b[^1], carry = 0; i >= 0 || x >= 0 || carry > 0;){
                int idigit = i >= 0 ? a[i--] - '0' : 0;
                int xdigit = x >= 0 ? b[x--] - '0' : 0;
                int sum = idigit + xdigit + carry;
                carry = sum / 2;
                num.Add(sum % 2); 
            }
            num.Reverse();
            return string.Concat(num);
        }
    }
}