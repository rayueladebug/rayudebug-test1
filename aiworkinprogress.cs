    using System.IO;
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
    //using OpenAI.Chat.ChatMessage;
    using System.Threading.Tasks;
    //using OpenQA.Selenium.DevTools.V136.Debugger;
    using System.Security.Cryptography.X509Certificates;
    using OpenQA.Selenium.DevTools.V139.Page;
    using project.Utils;
    using project.Models;
    using project.AI;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Text;
    using System.Net.Http.Headers;
    namespace project.Utils
    {
        public class Aibot
        {
            private class SavedChatMessage
            {
                public string Role { get; set; }
                public string Content { get; set; }
                public DateTime Timestamp { get; set; }
            }

            private const string ConfigFile = "config.txt";
            private const string ChatLogFile = "chatlog.json";
            private static string apiKey;
            private static ChatClient chatClient;
            private static List<ChatMessage> messages = new();

            public static async Task Run()
            {
                LoadApiKey();

                // ChatClient oluştur, apiKey kullanılarak
                chatClient = new ChatClient(model: "gpt-3.5-turbo", apiKey: apiKey);

                LoadChatHistory();
                Console.WriteLine("=== AI Chat Bot (OpenAI 2.3.0) ===");
                Console.WriteLine("komutlar: 'exit' = çıkış, 'clear' = geçmişi temizle, 'help' = yardım\n");

                while (true)
                {
                    Console.Write("Sen: ");
                    string userInput = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(userInput)) continue;

                    switch (userInput.ToLower())
                    {
                        case "exit":
                            Console.WriteLine("Hoşçakal!");
                            return;

                        case "clear":
                            ClearChat();
                            Console.WriteLine("Geçmiş temizlendi.\n");
                            continue;

                        case "help":
                            Console.WriteLine("Komutlar:\n  exit  - Programdan çıkış\n  clear - Konuşma geçmişini temizle\n  help  - Yardım menüsü\n");
                            continue;
                    }

                    try
                    {
                        messages.Add(new UserChatMessage(userInput));

                        string aiResponse = await GetAIResponse();
                        Console.WriteLine("AI: " + aiResponse + "\n");

                        messages.Add(new AssistantChatMessage(aiResponse));

                        SaveChatHistory();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("hata: " + ex.Message + "\n");
                    }
                }
            }

            private static void LoadApiKey()
            {
                if (!File.Exists(ConfigFile))
                {
                    Console.Write("openai api key gir: ");
                    apiKey = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(apiKey))
                        File.WriteAllText(ConfigFile, apiKey);
                }
                else
                {
                    apiKey = File.ReadAllText(ConfigFile).Trim();
                }

                if (string.IsNullOrEmpty(apiKey))
                {
                    Console.WriteLine("api key bulunamadi.");
                    Environment.Exit(1);
                }
            }

            private static void LoadChatHistory()
            {
                messages = new List<ChatMessage>();

                if (!File.Exists(ChatLogFile)) return;

                try
                {
                    string json = File.ReadAllText(ChatLogFile);
                    var saved = JsonSerializer.Deserialize<List<SavedChatMessage>>(json);
                    if (saved != null)
                    {
                        foreach (var item in saved)
                        {
                            if (item.Role == "user")
                                messages.Add(new UserChatMessage(item.Content));
                            else if (item.Role == "assistant")
                                messages.Add(new AssistantChatMessage(item.Content));
                        }

                        Console.WriteLine($"önceki konusmalar yuklendi ({messages.Count} mesaj).\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("gecmis yuklenirken hata olustu: " + ex.Message + "\n");
                }

                if (messages.Count == 0)
                {
                    messages.Add(new SystemChatMessage("Sen Türkçe konuşan yardımcı bir AI asistanısın."));
                }
            }

            private static void SaveChatHistory()
            {
                try
                {
                    var saved = new List<SavedChatMessage>();
                    foreach (var msg in messages)
                    {
                        if (msg is SystemChatMessage) continue;

                        string role = msg switch
                        {
                            UserChatMessage => "user",
                            AssistantChatMessage => "assistant",
                            _ => "unknown"
                        };

                        saved.Add(new SavedChatMessage
                        {
                            Role = role,
                            Content = msg.Content.ToString(),
                            Timestamp = DateTime.Now
                        });
                    }

                    string jsonOut = JsonSerializer.Serialize(saved, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(ChatLogFile, jsonOut);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Geçmiş kaydedilirken hata oluştu: " + ex.Message + "\n");
                }
            }

            private static void ClearChat()
            {
                messages.Clear();
                messages.Add(new SystemChatMessage("Sen Türkçe konuşan yardımcı bir AI asistanısın."));
                if (File.Exists(ChatLogFile))
                    File.Delete(ChatLogFile);
            }

            private static async Task<string> GetAIResponse()
    {
            try
            {
                var completion = await chatClient.CompleteChatAsync(messages);
                var typepropertymessage = completion.GetType().GetProperty("Message");
                var typepropertychoices = completion.GetType().GetProperty("Choices");
                if (typepropertychoices != null)
                {
                    var choices = typepropertychoices.GetValue(completion) as System.Collections.IList;
                    if (choices != null && choices.Count > 0)
                    {

                        var msgProp = choices[0].GetType().GetProperty("Message");
                        if (msgProp != null)
                        {
                            var messageobj = msgProp.GetValue(choices[0]);
                            var contentproperty = messageobj.GetType().GetProperty("Content");
                            var content = contentproperty?.GetValue(messageobj) as string;
                            if (!string.IsNullOrEmpty(content))
                                return content.Trim();
                        }
                    }
                }
                if (typepropertymessage != null)
                {
                    var message = typepropertymessage.GetValue(completion);
                    var contentproperty = message.GetType().GetProperty("Content");
                    var content = contentproperty?.GetValue(message) as string;
                    if (!string.IsNullOrEmpty(content))
                        return content.Trim();
                }
                // If no response found, return a default message
                return "AI'dan yanıt alınamadı.";
            }
            catch (Exception ex)
            {
                Console.WriteLine("AI Hatası: " + ex.Message);
                return "Bir hata oluştu.";
            }
    }
        }
    }