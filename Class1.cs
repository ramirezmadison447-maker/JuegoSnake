using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace JuegoSnake
{
    internal class Ventana
    {
        public string Titulo { get; set; }
        public int Ancho { get; set; }
        public int Alto { get; set; }

        public ConsoleColor ColorFondo { get; set; }

        public ConsoleColor ColorLetra { get; set; }

        public Point LimiteSuperio { get; set; }

        public Point LimiteInferior { get; set; }

        public int Area { get; set; }

        public Snake SnakeC { get; set; }

        

        public Ventana(string Titulo, int Ancho, int Alto, ConsoleColor ColorFondo, ConsoleColor ColorLetra, Point LimiteSuperio, Point LimiteInferior)
        {
            this.Titulo = Titulo;
            this.Ancho = Ancho;
            this.Alto = Alto;
            this.ColorFondo = ColorFondo;
            this.ColorLetra = ColorLetra;
            this.LimiteSuperio = LimiteSuperio;
            this.LimiteInferior = LimiteInferior;
            Area = ((this.LimiteInferior.X-this.LimiteSuperio.X)-1) * ((this.LimiteInferior.Y - this.LimiteSuperio.Y) - 1);
            Init();
        }

        public void Init()
        {
            Console.SetWindowSize(this.Ancho, this.Alto);

            Console.Title = this.Titulo;

            Console.CursorVisible = false;
            Console.BackgroundColor = this.ColorFondo;
            Console.Clear();
            SnakeC = new Snake(new Point(this.LimiteInferior.X/2, this.LimiteInferior.Y-3), ConsoleColor.Magenta, ConsoleColor.White, this, null);
            SnakeC.IniciarCuerpo(4);
        }

        public void DibujarLimites()
        {
            Console.ForegroundColor = this.ColorLetra;

            for(int i = LimiteSuperio.X; i < LimiteInferior.X; i++)
            {
                Console.SetCursorPosition(i, LimiteSuperio.Y);
                Console.Write("═");
                Console.SetCursorPosition(i, LimiteInferior.Y);
                Console.Write("═");
            }

            for(int i = LimiteSuperio.Y; i < LimiteInferior.Y; i++)
            {
                Console.SetCursorPosition(LimiteSuperio.X, i);
                Console.Write("║");
                Console.SetCursorPosition(LimiteInferior.X, i);
                Console.Write("║");
            }
            Console.SetCursorPosition(LimiteSuperio.X, LimiteSuperio.Y);
            Console.Write("╔");
            Console.SetCursorPosition(LimiteInferior.X, LimiteSuperio.Y);
            Console.Write("╗");
            Console.SetCursorPosition(LimiteSuperio.X, LimiteInferior.Y);
            Console.Write("╚");
            Console.SetCursorPosition(LimiteInferior.X, LimiteInferior.Y);
            Console.Write("╝");
        }

        public void Menu()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(this.LimiteSuperio.X+(this.LimiteInferior.X/2)-12, this.LimiteSuperio.Y+(this.LimiteInferior.Y/2)-4);
            Console.Write("J U E G O  S N A K E");
            Console.SetCursorPosition(this.LimiteSuperio.X + (this.LimiteInferior.X / 2) - 8, this.LimiteSuperio.Y + (this.LimiteInferior.Y / 2) - 2);
            Console.Write("Enter - Jugar");
            Console.SetCursorPosition(this.LimiteSuperio.X + (this.LimiteInferior.X / 2) - 8, this.LimiteSuperio.Y + (this.LimiteInferior.Y / 2) - 1);
            Console.Write("Esc - SALIR");

            SnakeC.MoverMenu();

        }

        public void Teclado(ref bool ejecucion, ref bool jugar, Snake snake)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if(tecla.Key == ConsoleKey.Enter)
                {
                    jugar = true;
                    Console.Clear();
                    DibujarLimites();
                    snake.Init();
                }
                if (tecla.Key == ConsoleKey.Escape)
                {
                    ejecucion = false;
                }
            }
        }

        public void GameOver(string text)
        {
            Console.Clear();
            DibujarLimites();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(this.LimiteSuperio.X + (this.LimiteInferior.X / 2) - 10, this.LimiteSuperio.Y + (this.LimiteInferior.Y / 2) - 2);
            Console.Write(text);
            Thread.Sleep(3000);
            Console.Clear();
            DibujarLimites();


        }
    }
}
