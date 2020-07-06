using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_2_1
{
    class AidKit : BaseObject, ICloneable, IComparable
    {
        public int Power { get; set; } = 3; // Начиная с версии C# 6.0 была добавлена инициализация автосвойств

        public AidKit (Point pos, Point dir, Size size) : base(pos, dir, size)
        {
            Power = 1;
        }

        public object Clone()
        {
            AidKit aidKit = new AidKit(new Point(Pos.X, Pos.Y), new Point(Dir.X, Dir.Y),
                new Size(Size.Width, Size.Height))
            { Power = Power };
            return aidKit;
        }
        public override void Draw()
        {
            Game.Buffer.Graphics.FillEllipse(Brushes.Green, Pos.X, Pos.Y, Size.Width, Size.Height);
        }
        int IComparable.CompareTo(object obj)
        {
            if (obj is AidKit temp)
            {
                if (Power > temp.Power)
                    return 1;
                if (Power < temp.Power)
                    return -1;
                else
                    return 0;
            }
            throw new ArgumentException("Parameter is not а aid kit!");
        }

        static public Image aidKit = Image.FromFile("Images\\fon.jpg");
        public virtual void DrawImage()
        {
            Game.Buffer.Graphics.DrawImage(aidKit, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            Pos.X = Pos.X - Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }
    }
}
