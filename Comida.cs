using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;


namespace JuegoSnake
{
    internal class Comida
    {
        public Point Posicion { get; set; }
        public ConsoleColor Color { get; set; }
        public Ventana VentanaC { get; set; }
        public Comida(ConsoleColor color, Ventana ventana)
        {
            this.Color = color;
            this.VentanaC = ventana;
           
        }
        public bool GenerarPosicion(Snake snake)
        {
            int longSnake = snake.Cuerpo.Count + 1;
            if ((VentanaC.Area - longSnake) <= 0)
            {
                return false; 
            }

            Random random = new Random();
            int x = random.Next(VentanaC.LimiteSuperio.X + 1, VentanaC.LimiteInferior.X);
            int y = random.Next(VentanaC.LimiteSuperio.Y + 1, VentanaC.LimiteInferior.Y);
            this.Posicion = new Point(x, y);

            foreach(Point item in snake.Cuerpo)
            {
                if((x==item.X && y == item.Y) || (x == snake.Cabeza.X && y == snake.Cabeza.Y))
                {
                    if (GenerarPosicion(snake))
                    {
                        return true;
                    }
                       

                    
                }
            }
            Dibujar();
            return true;
        }
        public void Dibujar()
        {
            Console.SetCursorPosition(Posicion.X, Posicion.Y);
            Console.ForegroundColor = Color;
            Console.Write("█");
        }
    }
}
