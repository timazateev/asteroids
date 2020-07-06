using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_2_1
{
    class Ship : BaseObject
    {
        public static event Message MessageDie;
        private int _energy = 100;
        public int Energy => _energy;
        private int _destroyed = 0;
        public int AsteroidDestroyed => _destroyed;
        public void DestroyCount()
        {
            _destroyed++;
        }
        public void Die()
        {
            MessageDie?.Invoke();
        }
        public void EnergyLow(int n)
        {
            _energy -= n;
        }
        public void EnergyGrow(int n)
        {
            _energy += n;
        }
        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.Wheat, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        public override void Update()
        {
        }
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }
        public void Down()
        {
            if (Pos.Y < Game.Height) Pos.Y = Pos.Y + Dir.Y;
        }
        public void Right()
        {
            if (Pos.Y > 0) Pos.X = Pos.X + Dir.Y;
        }
        public void Left()
        {
            if (Pos.Y < Game.Height) Pos.X = Pos.X - Dir.Y;
        }
    }

}
