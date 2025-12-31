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
    public class Aibot2
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
        private static string aibehavior = "Sen Türkçe konuşan yardımcı bir AI asistanısın";

        public static async Task Run()
        {
            LoadApiKey();
            
            try
            {
                chatClient = new ChatClient("gpt-3.5-turbo", apiKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"client olusturulamadi: {ex.Message}");
                Console.ReadLine();
                return;
            }

            LoadChatHistory();
            Console.WriteLine("ai chat bot");
            Console.WriteLine("komutlar 'exit' = cikis, 'clear' = gecmis temizle, 'help' = yardim\n");
            Console.WriteLine(aibehavior,"\n");

            while (true)
            {
                Console.Write("sen: ");
                string userInput = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(userInput)) continue;

                switch (userInput.ToLower())
                {
                    case "exit":
                        Console.WriteLine("kapaniyor");
                        return;
                    case "clear":
                        ClearChat();
                        Console.WriteLine("gecmis temizlendi\n");
                        continue;
                    /*case "help":
                        Console.WriteLine("");
                        continue;*/
                }

                await ProcessUserMessage(userInput);
            }
        }

        private static async Task ProcessUserMessage(string userInput)
        {
            try
            {
                messages.Add(new UserChatMessage(userInput));

                Console.WriteLine("dusunuyor");

                ChatCompletion response;
                try
                {
                    response = await chatClient.CompleteChatAsync(messages);
                }
                catch (Exception)
                {

                    response = await chatClient.CompleteChatAsync(messages.ToArray());
                }

                string aiResponse = ExtractContent(response);

                Console.WriteLine($"AI: {aiResponse}\n");

                messages.Add(new AssistantChatMessage(aiResponse));
                SaveChatHistory();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"hata: {ex.Message}");
                Console.WriteLine("devam etmek icin enter bas");
                Console.ReadLine();
            }
            // try
            // messages.Add(new UserChatMessage(userinput));
            // Console.WriteLine("AI düşünüyor...");
            // ChatCompletion response;
            // try
            // response = await chatClient.CompleteChatAsync(messages);
            // catch (Exception)
            // response = await chatClient.CompleteChatAsync(messages.ToArray());
            // string airesponse = ExtractContent(response);
            // Console.WriteLine($"AI: {airesponse}\n");
            // messages.Add(new AssistantChatMessage(response));
            // SaveChatHistory();
            // catch (Exception ex)
            // Console.WriteLine($"Hata oluştu: {ex.Message}");
        }
        //!!!
        /*
        *
        *
        // EXTRACTCONTENT VOIDI KARISIK VE AIDAN ALINMISTIR DUZELTILIP VERIMLI HALE GETIRILECEK
        *
        *
        */
        //!!!

        private static string ExtractContent(ChatCompletion response)
        {
            try
            {
                if (response?.Content?.Count > 0)
                {
                    return response.Content[0].Text ?? "yanit bos";
                }
            }
            catch { }

            try
            {
                var responseType = response.GetType();
                var choicesProperty = responseType.GetProperty("Choices");
                if (choicesProperty != null)
                {
                    var choices = choicesProperty.GetValue(response) as System.Collections.IList;
                    if (choices != null && choices.Count > 0)
                    {
                        var choice = choices[0];
                        var messageProperty = choice.GetType().GetProperty("Message");
                        if (messageProperty != null)
                        {
                            var message = messageProperty.GetValue(choice);
                            var contentProperty = message?.GetType().GetProperty("Content");
                            if (contentProperty != null)
                            {
                                var content = contentProperty.GetValue(message);
                                if (content != null)
                                {
                                    if (content is System.Collections.IList contentList && contentList.Count > 0)
                                    {
                                        var firstContent = contentList[0];
                                        var textProperty = firstContent.GetType().GetProperty("Text");
                                        return textProperty?.GetValue(firstContent)?.ToString() ?? content.ToString();
                                    }
                                    return content.ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            try
            {
                return response?.ToString() ?? "yanit cikmadi";
            }
            catch { }

            return "yanit cikmadi";
        }

        private static void LoadApiKey()
        {

            if (!File.Exists(ConfigFile))
            {
                Console.Write("openai api key girin: ");
                apiKey = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(apiKey))
                {
                    File.WriteAllText(ConfigFile, apiKey);
                }
            }
            else
            {
                apiKey = File.ReadAllText(ConfigFile).Trim();
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                Console.WriteLine("api key gerekli!");
                Environment.Exit(1);
            }
            // if (!File.Exists(configFile)){
            // Console.Write("OpenAI API key girin: ");
            // apiKey = Console.ReadLine()?.Trim();
            // if (!string.IsNullOrEmpty(apiKey))
            // File.WriteAllText(configFile, apikey);
            // else if (string.IsNullOrEmpty(apikey))
            // Console.WriteLine("apikey gerekli");
            // Environment.Exit(1);
            // else (apiKey = File.ReadAllText(configFile).Trim());
            //}
        }

        private static void LoadChatHistory()
        {
            messages.Clear();
            //messages.Clear();
            messages.Add(new SystemChatMessage(aibehavior));
            //messages.Add(new SystemChatMessage("Sen Türkçe konuşan yardımcı bir AI asistanısın."));

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
                    Console.WriteLine($"gecmis yuklendi ({saved.Count} mesaj).\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"gecmis yuklenmedi: {ex.Message}");
            }
            //messages.Clear();
            //messages.Add(new SystemChatMessage("Sen Türkçe konuşan yardımcı bir AI asistanısın."));
            // if (!File.Exists(chatlogfile)) return;
            // try
            // read = File.ReadAllText(ChatLogFile);
            // json = JsonDeserializer.Deserialize<List<SavedChatMessage>>(read);
            // foreach (var item in jason)
            // if (item.Role == "user")
            // messages.Add(new UserChatMessage(item.Content));
            // else if (item.Role == "assistant")
            // messages.Add(new AssistantChatMessage(item.Content));
            // catch (Exception ex)
            // Console.WriteLine($"ex.Message");
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
                        _ => null
                    };

                    if (role != null)
                    {
                        string content = "";
                        try
                        {
                            if (msg.Content?.Count > 0)
                            {
                                content = msg.Content[0].Text ?? "";
                            }
                        }
                        catch
                        {
                            content = msg.ToString() ?? "";
                        }

                        saved.Add(new SavedChatMessage
                        {
                            Role = role,
                            Content = content,
                            Timestamp = DateTime.Now
                        });
                    }
                }
                File.WriteAllText(ChatLogFile, String.Empty);
                string json = JsonSerializer.Serialize(saved, new JsonSerializerOptions { WriteIndented = true });
                File.AppendAllText(ChatLogFile, json);
                File.AppendAllText(ChatLogFile, "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"gecmis kaydedilmedi: {ex.Message}");
            }
            // var saved = new List<SavedChatMessage>();
            // foreach (var item in messages)
            // if (item is SystemChatMessage) continue;
            // string role = item switch
            // UserChatMessage => "user",
            // AssistantChatMessage => "assistant",
            // _ => null;
            // if (role != null)
            // string content = "";
            // try
            // if (item.Content?.Count > 0)
            // content = item.content[0].Text ?? "";
            // catch
            // content = item.ToString() ?? "";
            // saved.Add(new SavedMessage
            // {Role = role
            // Content = content
            // Timestamp = DateTime.Now
            // });
            //string json = JsonSerializer.Serialize(saved, new JsonSerializerOptions { WriteIndented = true });
            // File.WriteAllText(ChatLogFile, json);
        }

        private static void ClearChat()
        {
            messages.Clear();
            messages.Add(new SystemChatMessage("Sen Türkçe konuşan yardımcı bir AI asistanısın"));

            try
            {
                if (File.Exists(ChatLogFile))
                    File.Delete(ChatLogFile);
                    File.Create(ChatLogFile).Close();
            }
            catch { }
        }
    }
}