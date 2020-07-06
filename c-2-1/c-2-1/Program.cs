using System;
using System.Windows.Forms;

namespace c_2_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Form form = new Form
            {
                Width = Screen.PrimaryScreen.Bounds.Width, 
                Height = Screen.PrimaryScreen.Bounds.Height
            };
            Game.Init(form);
            form.Show();
            Game.Load();
            Game.Draw();
            Application.Run(form);
        }
    }
}