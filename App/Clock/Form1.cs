using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Clock
{
    public partial class Form1 : Form
    {
        float clockRad = 240;
        int shortLine = 8;
        Point[] second = new Point[]
        {
            new Point(0, -60),
            new Point(-3, 3),
            new Point(0, 0),
            new Point(3, 3),
        };
        Point[] minut = new Point[]
        {
            new Point(0, -80),
            new Point(-5, 5),
            new Point(0, 0),
            new Point(5, 5),
        };
        Point[] hour = new Point[]
        {
            new Point(0, -95),
            new Point(-7, 7),
            new Point(0, 0),
            new Point(7, 7),
        };

        public Form1()
        {
            InitializeComponent();
            Paint += drawFace;
            var timer = new Timer
            {
                Interval = 1000
            };
            timer.Tick += new EventHandler(Tick);
            timer.Start();
        }

        private void drawFace(Graphics graphics)
        {
            SolidBrush fillPen = new SolidBrush(Color.Black);
            graphics.FillRectangle(fillPen, 0, 0, Width, Height);
            graphics.TranslateTransform(((float)Width / 2) - 10, ((float)Height / 2) - 20);
            float dim = Width > Height ? Height : Width;
            dim -= 50;
            float scale = dim / clockRad;
            graphics.ScaleTransform(scale, scale);
            Pen cir_pen = new Pen(Color.White, 2);
            graphics.DrawEllipse(cir_pen, -(clockRad / 2), -(clockRad / 2), clockRad, clockRad);
            GraphicsState gs = graphics.Save();
            int lineAdd = 7;
            int line = shortLine;
            for(int i = 0; i < 36; ++i)
            {
                line += lineAdd;
                lineAdd *= -1;
                graphics.DrawLine(cir_pen, new Point(0, -((int)clockRad / 2)), new Point(0, -((int)clockRad / 2) + line));
                graphics.RotateTransform(15);
            }
            graphics.Restore(gs);
        }

        private void drawFace(object sender, PaintEventArgs e)
        {
            drawFace(e.Graphics);
        }

        private void drawArrows(Graphics graphics)
        {
            DateTime dt = DateTime.Now;
            graphics.TranslateTransform(((float)Width / 2) - 10, ((float)Height / 2) - 20);
            float dim = Width > Height ? Height : Width;
            dim -= 50;
            float scale = dim / clockRad;
            graphics.ScaleTransform(scale, scale);
            SolidBrush fillPen = new SolidBrush(Color.Black);
            int arrowRad = (int)clockRad - 2 * (shortLine + 7);
            graphics.FillEllipse(fillPen, -(arrowRad / 2), -(arrowRad / 2), arrowRad, arrowRad);
            GraphicsState gs = graphics.Save();
            graphics.RotateTransform(6 * dt.Second);
            graphics.DrawPolygon(new Pen(new SolidBrush(Color.Aqua), 1), second);
            graphics.Restore(gs);
            gs = graphics.Save();
            graphics.RotateTransform(6 * (dt.Minute + (float)dt.Second / 60));
            graphics.DrawPolygon(new Pen(new SolidBrush(Color.Yellow), 1), minut);
            graphics.Restore(gs);
            gs = graphics.Save();
            graphics.RotateTransform(30 * (dt.Hour + (float)dt.Minute / 60));
            graphics.DrawPolygon(new Pen(new SolidBrush(Color.Red), 1), hour);
            graphics.Restore(gs);
        }

        private void Tick(object sender, EventArgs e)
        {
            drawArrows(CreateGraphics());
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            var graph = CreateGraphics();
            drawFace(graph);
            drawArrows(graph);
        }
    }
}
