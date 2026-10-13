using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace JuegoSnake
{
    internal class Snake
    {
        public enum Direccion
        {
            Arriba,
            Abajo,
            Izquierda,
            Derecha
        }
        public bool Vivo { get; set; }

        public ConsoleColor ColorCabeza { get; set; }

        public ConsoleColor ColorCuerpo { get; set; }

        public Ventana VentanaC { get; set; }

        public List<Point> Cuerpo { get; set; }

        public Point Cabeza { get; set; }

        public Comida ComidaC { get; set; }

        public int Puntaje { get; set; }

        public int PuntajeMaximo { get; set; }

        public Point PosicionInicial { get; set; }

        public Direccion _direccion { get; set; }
        private bool _comiendo;

        public Snake(Point posicion, ConsoleColor colorCabeza, ConsoleColor colorCuerpo, Ventana ventana, Comida comida)
        {
            this.Vivo = true;
            this.ColorCabeza = colorCabeza;
            this.ColorCuerpo = colorCuerpo;
            this.VentanaC = ventana;
            this.Cuerpo = new List<Point>();
            this.Cabeza = posicion;
            ComidaC = comida;
            Puntaje = 0;
            PuntajeMaximo = 0;
            PosicionInicial = posicion;

            


            _direccion = Direccion.Derecha;
        }

        public void Init()
        {
            this.Cuerpo.Clear();
            this.Cabeza = PosicionInicial;
            IniciarCuerpo(2);
            this.Vivo = true;
            _direccion = Direccion.Derecha;
            ComidaC.GenerarPosicion(this);

        }
        public void IniciarCuerpo(int longitud)
        {
            int x = this.Cabeza.X - 1;
            for (int i = 0; i < longitud; i++)
            {
                Console.SetCursorPosition(x, this.Cabeza.Y);
                Console.Write("█");
               this.Cuerpo.Add(new Point(x, this.Cabeza.Y));
                x--;
            }
        }
        public void Mover()
        {
            Teclado();
            Point posCabezaAnterior = this.Cabeza;
            MoverCabeza();                       // modo juego: atravesarParedes = false

            if (!this.Vivo)                      // chocó con la pared
            {
                Muerte();
                VentanaC.GameOver("G A M E  O V E R");
                return;                          // no mover el cuerpo ni revisar comida
            }

            MoverCuerpo(posCabezaAnterior);
            ColisionesComida();
            if (ColisionesCuerpo())
            {
                Muerte();
                VentanaC.GameOver("G A M E  O V E R");
            }
        }
            
        
        public void MoverCabeza(bool atravesarParedes = false)
        {
            // 1) Calcular la nueva posición SIN moverla todavía
            Point nueva = this.Cabeza;
            switch (_direccion)
            {
                case Direccion.Arriba: nueva = new Point(nueva.X, nueva.Y - 1); break;
                case Direccion.Abajo: nueva = new Point(nueva.X, nueva.Y + 1); break;
                case Direccion.Izquierda: nueva = new Point(nueva.X - 1, nueva.Y); break;
                case Direccion.Derecha: nueva = new Point(nueva.X + 1, nueva.Y); break;
            }

            // 2) Decidir qué pasa con la pared
            if (atravesarParedes)
            {
                nueva = AtravesarMarco(nueva);       // menú: sale por el otro lado
            }
            else if (ChocaConMarco(nueva))
            {
                this.Vivo = false;                   // juego: muere y NO pisa el borde
                return;
            }

            // 3) Mover y dibujar
            Console.ForegroundColor = this.ColorCabeza;
            Console.SetCursorPosition(this.Cabeza.X, this.Cabeza.Y);
            Console.Write(" ");
            this.Cabeza = nueva;
            Console.SetCursorPosition(this.Cabeza.X, this.Cabeza.Y);
            Console.Write("█");
        }

        

        private void MoverCuerpo(Point posCabezaAnterior)
        {
            Console.ForegroundColor = this.ColorCuerpo;
            Console.SetCursorPosition(posCabezaAnterior.X, posCabezaAnterior.Y);
            Console.Write("█");
            this.Cuerpo.Insert(0, posCabezaAnterior);

            if(_comiendo)
            {
                _comiendo = false;
                return;
            }

            Console.SetCursorPosition(this.Cuerpo[this.Cuerpo.Count-1].X, this.Cuerpo[this.Cuerpo.Count - 1].Y);
            Console.Write(" ");
            this.Cuerpo.RemoveAt(this.Cuerpo.Count - 1);
        }

        private void ColisionesComida()
        {
            if(this.Cabeza == ComidaC.Posicion)
            {
                if (!ComidaC.GenerarPosicion(this))
                {
                    this.Vivo = false;
                    VentanaC.GameOver("JUEGO COMPLETADO"); 
                }
                _comiendo = true;
                Puntaje++;
                if(Puntaje > PuntajeMaximo)
                {
                    PuntajeMaximo = Puntaje;
                }
            }
        }
        private void Teclado()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                switch (tecla.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (_direccion != Direccion.Abajo)
                            _direccion = Direccion.Arriba;
                        break;
                    case ConsoleKey.DownArrow:
                        if (_direccion != Direccion.Arriba)
                            _direccion = Direccion.Abajo;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (_direccion != Direccion.Derecha)
                            _direccion = Direccion.Izquierda;
                        break;
                    case ConsoleKey.RightArrow:
                        if (_direccion != Direccion.Izquierda)
                            _direccion = Direccion.Derecha;
                        break;
                }
            }
        }

        private bool ChocaConMarco(Point p)
        {
            return p.X <= VentanaC.LimiteSuperio.X || p.X >= VentanaC.LimiteInferior.X ||
                   p.Y <= VentanaC.LimiteSuperio.Y || p.Y >= VentanaC.LimiteInferior.Y;
        }

        private Point AtravesarMarco(Point p)
        {
            // Interior jugable: un espacio dentro de cada borde
            int minX = VentanaC.LimiteSuperio.X + 1;
            int maxX = VentanaC.LimiteInferior.X - 1;
            int minY = VentanaC.LimiteSuperio.Y + 1;
            int maxY = VentanaC.LimiteInferior.Y - 1;

            int x = p.X, y = p.Y;
            if (x > maxX) x = minX;      // salió por la derecha  → entra por la izquierda
            else if (x < minX) x = maxX; // salió por la izquierda → entra por la derecha
            if (y > maxY) y = minY;      // salió por abajo  → entra por arriba
            else if (y < minY) y = maxY; // salió por arriba → entra por abajo

            return new Point(x, y);
        }

        private bool ColisionesCuerpo()
        {
            foreach(Point item in Cuerpo)
            {
                if(Cabeza == item)
                {
                    this.Vivo = false;
                    return true; 
                }
            }
            return false;
        }

        public void Muerte()
        {
            Console.ForegroundColor = ColorCuerpo;
            foreach(Point item in Cuerpo)
            {
                if (item == Cabeza)
                    continue;

                Console.SetCursorPosition(item.X, item.Y);
                Console.Write("░");
                Thread.Sleep(120);
            }
        }

        
        public void Informacion(int distanciaX1, int distanciaX2)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(VentanaC.LimiteSuperio.X + distanciaX1, VentanaC.LimiteSuperio.Y - 1);
            Console.Write("Puntaje: " +Puntaje+ " ");
            Console.SetCursorPosition(VentanaC.LimiteSuperio.X + distanciaX2, VentanaC.LimiteSuperio.Y - 1);
            Console.Write("Puntaje Maximo: " +PuntajeMaximo+ " ");

        }

        public void MoverMenu()
        {
            _direccion = Direccion.Derecha;
            Point posCabezaAnterior = this.Cabeza;
            MoverCabeza(atravesarParedes: true); 
            MoverCuerpo(posCabezaAnterior);

        }
    }
}