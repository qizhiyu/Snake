using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Snake
{

    class ArenaView
    {
		public static readonly int SIZE = 30;

        public static void Render(Graphics graphics, Arena arena)
        {
			graphics.FillRectangle(Brushes.AliceBlue, 0, 0, arena.Width * SIZE, arena.Height * SIZE);
            for (int x = 0; x < arena.Width; x++)
            {
                for (int y = 0; y < arena.Height; y++)
                {
                    if (arena.Cells[x, y] == Food.Apple)
                    {
						graphics.FillRectangle(Brushes.GreenYellow, x * SIZE, y * SIZE, SIZE, SIZE);
                    }
                    else if (arena.Cells[x, y] == Food.Orange)
                    {
						graphics.FillRectangle(Brushes.Orange, x * SIZE, y * SIZE, SIZE, SIZE);
                    }
                }
            }

            bool up, down, left, right;

            LinkedListNode<Point> lastSegment = null;
            LinkedListNode<Point> currentSegment = arena.Snake.Body.First;
            LinkedListNode<Point> nextSegment;
            while (null != currentSegment)
            {
                up = down = left = right = false;
                nextSegment = currentSegment.Next;

                CompareSegment(currentSegment, lastSegment, ref up, ref down, ref left, ref right);
                CompareSegment(currentSegment, nextSegment, ref up, ref down, ref left, ref right);

                DrawSegment(graphics, currentSegment.Value.X, currentSegment.Value.Y, up, down, left, right);

                lastSegment = currentSegment;
                currentSegment = nextSegment;
            }

        }

        private static void CompareSegment(LinkedListNode<Point> currentSegment, LinkedListNode<Point> otherSegment,
            ref bool up, ref bool down, ref bool left, ref bool right)
        {
            if (currentSegment != null && otherSegment != null)
            {
                if (currentSegment.Value.Y > otherSegment.Value.Y)
                {
                    up = true;
                }

                if (currentSegment.Value.Y < otherSegment.Value.Y)
                {
                    down = true;
                }

                if (currentSegment.Value.X > otherSegment.Value.X)
                {
                    left = true;
                }

                if (currentSegment.Value.X < otherSegment.Value.X)
                {
                    right = true;
                }
            }
        }

        private static void DrawSegment(Graphics graphics, int x, int y, bool up, bool down, bool left, bool right)
        {
            // possible patterns:
            // D,R  D,U  D,L  R,U  R,L  U,L
            // +++  + +  +++  + +  +++  + +
            // +    + +    +  +           + 
            // + +  + +  + +  +++  +++  +++

			graphics.FillRectangle(Brushes.RosyBrown, x * SIZE, y * SIZE, SIZE, SIZE);

			Point upLeft = new Point(x * SIZE, y * SIZE);
			Point upRight = new Point(x * SIZE + SIZE-1, y * SIZE);
			Point downLeft = new Point(x * SIZE, y * SIZE + SIZE - 1);
			Point downRight = new Point(x * SIZE + SIZE - 1, y * SIZE + SIZE - 1);

            if ((down && right) || (down && left) || (right && left))
            {
                graphics.DrawLine(Pens.Black, upLeft, upRight);
            }

            if ((down && right) || (down && up) || (right && up))
            {
                graphics.DrawLine(Pens.Black, upLeft, downLeft);
            }

            if ((down && up) || (down && left) || (up && left))
            {
                graphics.DrawLine(Pens.Black, upRight, downRight);
            }

            if ((right && up) || (right && left) || (up && left))
            {
                graphics.DrawLine(Pens.Black, downLeft, downRight);
            }
        }
    }
}
