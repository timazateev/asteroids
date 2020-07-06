using System;
using System.Windows.Forms;
using System.Drawing;
using static c_2_1.LoggingMethods;

namespace c_2_1
{
    static class Game
    {
        private static Timer _timer = new Timer();
        public static Random Rnd = new Random();
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;

        static public Image background = Image.FromFile("Images\\fon.jpg");

        private static Bullet _bullet;
        private static Asteroid[] _asteroids;
        private static AidKit[] _aidKits;
        public static BaseObject[] _objs;
        private static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(10, 10));

        private static readonly FileLogger fileOutput = new FileLogger("log.txt");

        public static void Load()
        {
            _objs = new BaseObject[30];
            _bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(4, 1));
            _asteroids = new Asteroid[30];
            _aidKits = new AidKit[10];

            var rnd = new Random();
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _objs[i] = new Star(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
            }
            for (var i = 0; i < _asteroids.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _asteroids[i] = new Asteroid(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r));
            }
            for (var i = 0; i < _aidKits.Length; i++)
            {
                _aidKits[i] = new AidKit(new Point(1000, rnd.Next(0, Game.Height)), new Point(5, 0), new Size(20, 10));
            }
        }

        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }
        static Game()
        {
        }
        public static void Init(Form form)
        {
            // Графическое устройство для вывода графики            
            Graphics g;
            // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            if (Width < 0 || Height < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();
            form.KeyDown += Form_KeyDown;
            Ship.MessageDie += Finish;
            //Добавим в Init таймер и обработчик таймера, в котором заставим вызываться Draw и Update.
            Timer timer = new Timer { Interval = 100 };
            timer.Start();
            timer.Tick += Timer_Tick;
        }

        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) _bullet = new Bullet(new Point(_ship.Rect.X + 10, _ship.Rect.Y + 4), new Point(4, 0), new Size(4, 1));
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
            if (e.KeyCode == Keys.Right) _ship.Right();
            if (e.KeyCode == Keys.Left) _ship.Left();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        public static void Draw()
        {
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (Asteroid a in _asteroids)
            {
                a?.Draw();
            }
            foreach (AidKit a in _aidKits)
            {
                a?.Draw();
            }
            _bullet?.Draw();
            _ship?.Draw();
            if (_ship != null)
            {
                Buffer.Graphics.DrawString("Energy:" + _ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);
                //add destroyed asteroids count
                Buffer.Graphics.DrawString("Destroyed asteroids:" + _ship.AsteroidDestroyed, SystemFonts.DefaultFont, Brushes.White, 0, 25);
            }
            Buffer.Render();
        }

        public static void Update()
        {
            //Turn Console logging.
            //Logger.WriteMessage += LoggingMethods.LogToConsole;
            foreach (BaseObject obj in _objs) obj.Update();
            _bullet?.Update();
            
            for (var i = 0; i < _asteroids.Length; i++)
            {
                if (_asteroids[i] == null) continue;
                _asteroids[i].Update();
                if (_asteroids[i] == null) continue;
                if (_bullet != null && _bullet.Collision(_asteroids[i]))
                {
                    System.Media.SystemSounds.Hand.Play();
                    _asteroids[i] = null;
                    _bullet = null;
                    Logger.WriteMessage($"Asteroid {_asteroids[i]} destroid");
                    continue;
                }
                if (!_ship.Collision(_asteroids[i])) continue;
                var rnd = new Random();
                int damage = rnd.Next(1, 10);
                _ship?.EnergyLow(damage);
                _asteroids[i] = null;
                _ship?.DestroyCount();
                Logger.WriteMessage($"Ship damage is {damage}. Asteroid destroid");
                System.Media.SystemSounds.Asterisk.Play();
                if (_ship.Energy <= 0) _ship?.Die();
            }

            for (var i = 0; i < _aidKits.Length; i++)
            {
                if (_aidKits[i] == null) continue;
                _aidKits[i].Update();
                if (_aidKits[i] == null) continue;
                if (_bullet != null && _bullet.Collision(_aidKits[i]))
                {
                    System.Media.SystemSounds.Hand.Play();
                    _aidKits[i] = null;
                    _bullet = null;
                    Logger.WriteMessage($"Aid kit {_aidKits[i]} destroid :-( ");
                    continue;
                }
                if (!_ship.Collision(_aidKits[i])) continue;
                var rnd = new Random();
                int repair = rnd.Next(1, 10);
                _ship?.EnergyGrow(repair);
                _aidKits[i] = null;
                Logger.WriteMessage($"Ship repaired HP {repair}. Aid kit picked up");
                System.Media.SystemSounds.Beep.Play();
            }

        }   
        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("The End", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            Buffer.Render();
        }


    }
}