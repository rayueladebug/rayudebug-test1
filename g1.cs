using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
using OpenAI;
using OpenAI.Chat;
using OpenQA.Selenium.DevTools.V139.Page;
using project.Models;
using project.Utils;
using project.AI;
using OpenQA.Selenium.DevTools.V137.Runtime;
using projet.Utils;
using System.Drawing;
using OpenQA.Selenium.DevTools.V137.Profiler;
using OpenQA.Selenium.BiDi.BrowsingContext;
using OpenQA.Selenium.DevTools.V137.Debugger;
using OpenQA.Selenium.DevTools.V137.DOM;
using System.Security.Cryptography;
using OpenAI.Realtime;
using Microsoft.VisualBasic;
using System.Data;
using System.Text.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
namespace project.Games;
/*
*
*
// KODUN YARISI AIDAN ALINMISTIR DUZENLI VE VERIMLI HALE GETIRILECEK
*
*
*/
public class Stats{
    public int zaman { get; set; }
    public int highscore { get; set; }
}

public class Game
{
    private static string _direction = "right";
    private static Point _foodPosition;
    private static readonly Random _random = Random.Shared;
    private static CancellationTokenSource? _cancellationTokenSource;
    //private static List<Stats> stat = new List<Stats>();
    //private static System.Timers.Timer timer = new System.Timers.Timer(1000);
    //private int timepast = 0;
    /*private static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
    {
        Console.Write("zaman: ", e.SignalTime);
    }*/
    private static DateTime timenow = new DateTime();
    private static DateTime timeend = new DateTime();
    private const string Savefile = "saved.json";
    private static int a;
    private static Stats stats = new Stats();
    public static List<Point> borders;
    public static async Task Main1()
    {
        Console.Clear();
        Console.CursorVisible = false;
        
        _cancellationTokenSource = new CancellationTokenSource();
        for (int i = 3; i <= Console.WindowHeight - 4; i++)
        {
            Console.SetCursorPosition(3, i);
            Console.Write("|");
            Console.SetCursorPosition(Console.WindowWidth - 4, i);
            Console.Write("|");
        }
        for (int i = 3; i <= Console.WindowWidth - 4; i++){
            Console.SetCursorPosition(i, 3);
            Console.Write("-");
            Console.SetCursorPosition(i, Console.WindowHeight - 4);
            Console.Write("-");
        }

        var snake = new Snake(new Point(Console.WindowWidth / 2, Console.WindowHeight / 2), 3, '@');
        snake.Draw();
        
        SpawnFood(snake);
        
        var inputTask = Task.Run(() => HandleInput(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
        
        try
        {
            //timer.Enabled = true;
            //
            timenow = DateTime.Now;
            DrawMax();
            await GameLoop(snake, _cancellationTokenSource.Token);

        }
        finally
        {
            _cancellationTokenSource.Cancel();
            Console.CursorVisible = true;
        }
    }
    
    private static async Task GameLoop(Snake snake, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var direction = _direction switch
            {
                "up" => Snake.Direction.Up,
                "left" => Snake.Direction.Left,
                "down" => Snake.Direction.Down,
                "right" => Snake.Direction.Right,
                _ => Snake.Direction.Right
            };
            
            snake.Move(direction);
            
            if (snake.HeadPosition == _foodPosition)
            {
                snake.Grow();
                SpawnFood(snake);
                DrawScore(snake.Length);
            }
            if (snake.CheckSelfCollision() || CheckWallCollision(snake.HeadPosition))
            {
                //timer.Enabled = false;
                GameOver(snake.Length);
                break;
            }
            //Console.BackgroundColor = ConsoleColor.White;
            //Console.ResetColor();
            timeend = DateTime.Now;
            long timetick = timenow.Ticks - timeend.Ticks;
            TimeSpan timespan = new TimeSpan(timetick);
            a = (int)-timespan.TotalSeconds;
            Console.SetCursorPosition(0, 1);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write($"zaman: {a}");
            //timer.Elapsed += OnTimedEvent;
            Console.ResetColor();
            await Task.Delay(100, cancellationToken);
        }
    }
    
    private static void HandleInput(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                
                _direction = key switch
                {
                    ConsoleKey.UpArrow when _direction != "down" => "up",
                    ConsoleKey.LeftArrow when _direction != "right" => "left",
                    ConsoleKey.DownArrow when _direction != "up" => "down",
                    ConsoleKey.RightArrow when _direction != "left" => "right",
                    ConsoleKey.Escape => throw new OperationCanceledException(),
                    _ => _direction
                };
            }
            Thread.Sleep(10);
        }
    }
    
    private static void SpawnFood(Snake snake)
    {
        bool validPosition;
        
        do
        {
            var x = _random.Next(4, Console.WindowWidth - 5);
            var y = _random.Next(4, Console.WindowHeight - 5);
            _foodPosition = new Point(x, y);
            validPosition = !snake.IsOnSnake(_foodPosition);
        }
        while (!validPosition);
        
        Console.SetCursorPosition(_foodPosition.X, _foodPosition.Y);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write('#');
        Console.ResetColor();
    }
    
    private static bool CheckWallCollision(Point position) =>
        position.X <= 3 || position.X >= Console.WindowWidth - 4 ||
        position.Y <= 3 || position.Y >= Console.WindowHeight - 4;
    
    private static void DrawScore(int length)
    {
        Console.SetCursorPosition(0, 0);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"skor: {length - 2}");
        Console.ResetColor();
    }
    private static void DrawMax(){
        string readmax = File.ReadAllText(Savefile);
        //var saved2 = JsonConverter
        var saved = JsonSerializer.Deserialize<Stats>(readmax);
        if (saved != null){
            //foreach (var saved1 in saved){
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write($"max skor: {saved.highscore}");
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write($"min zaman: {saved.zaman}");
            Console.ResetColor();
            //}
        }
    }
    
    private static async Task GameOver(int length)
    {
        string json2 = File.ReadAllText(Savefile);
        var saved = JsonSerializer.Deserialize<Stats>(json2);
        //stats.highscore = length-2;
        //stats.zaman = a;
        if (saved != null){
            //foreach (var saved1 in saved){
            if (saved.highscore < length -2){
                stats.highscore = length - 2;
                stats.zaman = a;
                string json = JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(Savefile, json);   
            }
            }
        //}
        //foreach (var stat123 in saved){

        //}
        //File.WriteAllText(Savefile, json);
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("over");
        Console.ResetColor();
        Console.WriteLine($"final: {length - 2}");
        /////////////////////////////////////////////////Console.WriteLine("tekrar baslamak icin enter cikmak icin esc");
        bool validation = Console.ReadKey(true).Key switch
        {
            ConsoleKey.Enter => true,
            ConsoleKey.Escape => false,
            _ => false
        };
        /*switch(Console.ReadKey(true).Key){
            case ConsoleKey.Enter: await Menuservices.Menuac();
            break;
            case ConsoleKey.Escape: break;
            default: break;
        }*/
        //Console.ReadKey();
        if (validation == true) await Task.Run(Main1);
        else System.Environment.Exit(0);
    }
}

public record SnakeBody(char Symbol, Point Position)
{
    public Point Position { get; init; } = Position;
    
    public void Erase()
    {
        Console.SetCursorPosition(Position.X, Position.Y);
        Console.Write(' ');
    }
    
    public void Draw()
    {
        Console.SetCursorPosition(Position.X, Position.Y);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(Symbol);
        Console.ResetColor();
    }
}

public class Snake
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    
    public char Symbol { get; init; }
    private readonly List<SnakeBody> _body;
    private SnakeBody Head => _body[0];
    public Point HeadPosition => Head.Position;
    public int Length => _body.Count;
    private bool _shouldGrow;
    
    public Snake(Point startingPoint, int length, char symbol)
    {
        Symbol = symbol;
        _body = Enumerable
            .Range(0, length)
            .Select(i => new SnakeBody(symbol, new Point(startingPoint.X - i, startingPoint.Y)))
            .ToList();
    }
    
    public void Draw()
    {
        foreach (var segment in _body)
        {
            segment.Draw();
        }
    }
    
    public void Grow() => _shouldGrow = true;
    
    public bool IsOnSnake(Point position) => 
        _body.Any(segment => segment.Position == position);
    
    public bool CheckSelfCollision() =>
        _body.Skip(1).Any(segment => segment.Position == HeadPosition);
    
    public void Move(Direction direction)
    {
        var newPosition = direction switch
        {
            Direction.Up => new Point(Head.Position.X, Head.Position.Y - 1),
            Direction.Down => new Point(Head.Position.X, Head.Position.Y + 1),
            Direction.Left => new Point(Head.Position.X - 1, Head.Position.Y),
            Direction.Right => new Point(Head.Position.X + 1, Head.Position.Y),
            _ => Head.Position
        };
        
        var newHead = new SnakeBody(Symbol, newPosition);
        _body.Insert(0, newHead);
        
        if (!_shouldGrow)
        {
            _body[^1].Erase();
            _body.RemoveAt(_body.Count - 1);
        }
        else
        {
            _shouldGrow = false;
        }
        
        newHead.Draw();
    }
}