using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium.DevTools.V137.DeviceAccess;

namespace project.Games
{
    /*
    *
    *
    *
    // KODU EN DUZENLI VE OKUNABILIR PROJEM
    *
    *
    *
    */
    public class Game2
    {
        public static void Loop()
        {
            TetrisGame game = new TetrisGame();
            game.Start();
        }
    }

    public class TetrisGame
    {

        // Oyun alanı
        private const int Width = 10;
        private const int Height = 20;
        private int score = 0;

        // Oyun tahtası
        private int[,] board = new int[Height, Width];

        // Taşlar
        private List<Tetromino> tetrominos = new List<Tetromino>
        {
            new Tetromino("I", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(3, 0) }), // I
            new Tetromino("O", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1) }), // O
            new Tetromino("T", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0), new Point(1, 1) }), // T
            new Tetromino("L", new[] { new Point(0, 0), new Point(0, 1), new Point(0, 2), new Point(1, 2) }), // L
            new Tetromino("J", new[] { new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(0, 2) }), // J
            new Tetromino("S", new[] { new Point(1, 0), new Point(2, 0), new Point(0, 1), new Point(1, 1) }), // S
            new Tetromino("Z", new[] { new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(2, 1) })  // Z
        };
        // Hareket edilecek yönler (test)
        private enum Direction{
            left,
            right
        }

        private Tetromino currentTetromino;
        private int currentX, currentY;
        private bool gameOver = false;
        private bool moveLeft = false;
        private bool moveRight = false;
        private bool moveDown = false;
        private bool rotate = false;
        private bool drop = false;
        private readonly object lockobj = new object();

        public void Start()
        {
            Console.CursorVisible = false;
            InitBoard();
            SpawnNewTetromino();
            Task.Run(() => ProcessInput());  // Girişler ayrı thread'de işlenir
            Task.Run(() => MoveTetrominoDown()); // Hareketin akıcı olması için deneme (test)
            while (!gameOver)
            {
                DrawBoard();
                if (rotate){
                    RotateTetromino(); // Eğer mümkünse taşı döndür
                }
                if (moveLeft || moveRight) MoveTetrominoSide(); // X vektöründe hareket
                /*if (moveDown)
                {
                    MoveTetrominoDown(); // Eğer aşağı hareket etmeli ise
                }*/
                Thread.Sleep(30); // Değer düştükçe hızlı tepki
            }
            Console.Clear();
            Console.WriteLine("hayatbiti");
        }

        // Oyun tahtasını başlat
        private void InitBoard()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    board[y, x] = 0;  // Oyun tahtasını sıfırla
                }
            }
        }

        // Taşı spawn et
        private void SpawnNewTetromino()
        {
            Random rand = new Random();
            currentTetromino = tetrominos[rand.Next(tetrominos.Count)];
            currentX = Width / 2 - 1;
            currentY = 0;

            // Eğer başlangıçta çarpışma varsa oyun bitmeli
            if (IsCollision(currentTetromino, currentX, currentY))
            {
                gameOver = true;
            }
        }

        // Ekrana oyun tahtasını yazdır
        private void DrawBoard()
        {
            Console.Clear();  // Tahtayı temizle
            // Oyun tahtasını çiz
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (board[y, x] == 1)
                    {
                        Console.Write("[]");  // Taşları [] ile çiz
                    }
                    else
                    {
                        Console.Write(" .");  // Boş taşlar
                    }
                }
                Console.WriteLine();
            }

            // Şu anki tetrominoyu çiz
            foreach (var block in currentTetromino.Blocks)
            {
                int x = currentX + block.X;
                int y = currentY + block.Y;
                if (y >= 0)  // Taş ekranın üstünde ise yerleştirme
                {
                    Console.SetCursorPosition(x * 2, y);  // Çift boşlukta doğru konumlandırma
                    Console.Write("[]");  // Taşı ekranda çiz
                }
            }

            // Skoru göster
            Console.SetCursorPosition(0, 20);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"skor: {score}");
            Console.ResetColor();

            Console.SetCursorPosition(0, 0);  // Konsolun üst sol köşesine geri dön
        }

        // Kullanıcı girişlerini işler
        private void ProcessInput()
        {
            while (!gameOver)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.LeftArrow)
                    {
                        if (!IsCollision(currentTetromino, currentX - 1, currentY))
                        {
                            //MoveTetrominoSide(Direction.left); // Sol taş hareket
                            moveLeft = true;  // Sol hareketi başlat
                        }
                    }
                    else if (key == ConsoleKey.RightArrow)
                    {
                        if (!IsCollision(currentTetromino, currentX + 1, currentY))
                        {
                            //MoveTetrominoSide(Direction.right); // Sağ taş hareketi
                            moveRight = true;  // Sağ hareketi başlat
                        }
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        moveDown = true;  // Aşağı hareketi başlat
                    }
                    else if (key == ConsoleKey.Spacebar)
                    {
                        drop = true; //Düşme hareketini başlat
                    }
                    else if (key == ConsoleKey.UpArrow)
                    {
                        rotate = true;  // Döndürme işlemi
                    }
                }
            }
        }

        // Taşı y vektörde hareket ettir
        private void MoveTetrominoDown()
        {
            // Void çalıştığında boşa düşmemesi için
            moveDown = true;
            Thread.Sleep(500);

            while (moveDown && !gameOver)
            {
            if (!IsCollision(currentTetromino, currentX, currentY + 1))
            {
                currentY++;  // Taşı aşağıya indir
            }
            else
            {
                PlaceTetromino();  // Taşı sabitle
                SpawnNewTetromino();  // Yeni taş oluştur
                ClearFullLines();  // Tam dolmuş satırları temizle
            }
            // Space basılı tutuluyorsa hızlı düşme
            if (drop)
            {
                drop = false;
                Thread.Sleep(35);
            }
            else if (!drop) Thread.Sleep(195); // Tekrar aşağı inmeden beklenecek süre
            moveDown = true; // Aşağı hareketi sonlandır
            }

        }
        // Taşı x vektörde hareket ettir
        private void MoveTetrominoSide(){
            if (!IsCollision(currentTetromino, currentX + 1, currentY) || !IsCollision(currentTetromino, currentX - 1, currentY)){
                // X vektörünü girdiye göre değiştirme
                /*currentX = dir switch{
                    Direction.right => currentX++,
                    Direction.left => currentX--
                };*/
                // X vektörünü bool ile değiştirme
                if (moveLeft && !moveRight) {currentX--; moveLeft = false;}
                else if (moveRight && !moveLeft) {currentX++; moveRight = false;}
            }
        }

        // Taşı döndür
        private void RotateTetromino()
        {
            var newTetromino = currentTetromino.Rotate();
            if (!IsCollision(newTetromino, currentX, currentY))
            {
                currentTetromino = newTetromino; // Taşı döndür
            }
            rotate = false; // Döndürmeyi sonlandır
        }

        // Taş çarpışma kontrolü
        private bool IsCollision(Tetromino tetromino, int x, int y)
        {
            foreach (var block in tetromino.Blocks)
            {
                int newX = x + block.X;
                int newY = y + block.Y;
                if (newX < 0 || newX >= Width || newY >= Height || (newY >= 0 && board[newY, newX] == 1))
                {
                    return true;
                }
            }
            return false;
        }

        // Taşı tahtaya yerleştir
        private void PlaceTetromino()
        {
            foreach (var block in currentTetromino.Blocks)
            {
                int x = currentX + block.X;
                int y = currentY + block.Y;
                if (y >= 0)  // Taş ekranın üstünde ise yerleştirme
                {
                    board[y, x] = 1;  // Taşı tahtaya yerleştir
                }
            }
        }

        // Tam dolmuş satırları temizle
        private void ClearFullLines()
        {
            for (int y = Height - 1; y >= 0; y--)
            {
                if (IsLineFull(y))
                {
                    RemoveLine(y);
                    ShiftLinesDown(y);
                    score += 100;  // Tahtanın sol altında gösterilecek skora ekle
                    y++;  // Daha önce kaydırılan satırı tekrar kontrol et
                }
            }
        }

        // Satır tamamen dolmuş mu kontrolü
        private bool IsLineFull(int y)
        {
            for (int x = 0; x < Width; x++)
            {
                if (board[y, x] == 0)  // Eğer bir boş taş varsa satır dolmamış demektir
                {
                    return false;
                }
            }
            return true;
        }

        // Satırı temizle
        private void RemoveLine(int y)
        {
            // Satırlar silinmeden önce kkısa süreli yeşil renk patlama efekti(Şu anda tüm tahtanın rengi değişiyor ilerde sadece satırın değişecek)
            Console.ForegroundColor = ConsoleColor.Green;
            for (int a = 0; a < Width; a++){
                Console.SetCursorPosition(a, y);
                Console.Write("[]");
            }
            Thread.Sleep(200);
            Console.ResetColor();
            for (int x = 0; x < Width; x++)
            {
                board[y, x] = 0;// Satırdaki tüm taşları sıfırla
            } 
        }

        // Satırları aşağı kaydır
        private void ShiftLinesDown(int y)
        {
            for (int i = y; i >= 1; i--)
            {
                for (int x = 0; x < Width; x++)
                {
                    board[i, x] = board[i - 1, x];  // Satırları kaydır
                }
            }
        }
    }

    public class Tetromino
    {
        public string Name { get; }
        public List<Point> Blocks { get; }

        public Tetromino(string name, Point[] blocks)
        {
            Name = name;
            Blocks = blocks.ToList();
        }

        // Taşı döndür
        public Tetromino Rotate()
        {
            var rotatedBlocks = Blocks.Select(block => new Point(-block.Y, block.X)).ToArray();
            return new Tetromino(Name, rotatedBlocks);
        }
    }
}
