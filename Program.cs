using JuegoSnake;
using System.Drawing;

Ventana ventana;
Snake snake;
Comida comida;
bool jugar = false;
bool ejecucion = true;
void Iniciar()
{
    int ancho = 65;
    int alto = 20;
    Point limSup = new Point(5, 3);
    Point limInf = new Point(59, 18);
    ventana = new Ventana("Juego Snake", ancho, alto, ConsoleColor.Black, ConsoleColor.White, limSup, limInf);
    ventana.DibujarLimites();
    comida = new Comida(ConsoleColor.Red, ventana);
    snake = new Snake(new Point(8, 4), ConsoleColor.Green, ConsoleColor.DarkGreen, ventana, comida);
    
}
void Game()
{
    while (ejecucion)
    {
        ventana.Menu();
        ventana.Teclado(ref ejecucion, ref jugar, snake);
        while (jugar)
        {
            snake.Informacion(0, 34);
            snake.Mover();
            if(!snake.Vivo)
            {
                jugar = false;
                snake.Puntaje = 0;
            }
            Thread.Sleep(100);
        }

        Thread.Sleep(100);
    }
   
}

Iniciar();
Game();

