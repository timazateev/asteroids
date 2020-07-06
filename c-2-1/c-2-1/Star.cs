﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_2_1
{
    class Star : BaseObject
    {
        //static Image Image { get; } = Image.FromFile("Images\\star.png");
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X, Pos.Y, Pos.X + Size.Width, Pos.Y + Size.Height);
            Game.Buffer.Graphics.DrawLine(Pens.White, Pos.X + Size.Width, Pos.Y, Pos.X, Pos.Y + Size.Height);
        }
        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }
        public Star(Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }
        public static BaseObject[] _objs;
        public static void Load()
        {
            _objs = new BaseObject[30];

            for (int i = 0; i < _objs.Length; i++)
            {
                _objs[i] = new Star(new Point(600, i * 20), new Point(-i, 0), new Size(5, 5));
            }
        }
    }
}
