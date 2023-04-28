using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PK_07_Snake
{
    public partial class BoardSnake
    {
        public enum Direction { Up, Down, Left, Right };
        public enum FieldType { Empty, SnakeHead, SnakeTail, Apple, Wall };
        public Size Size
        {
            get
            {
                return new Size(myMap.First().Length, myMap.Length);
            }
        }
        private Snake mySnake;
        private Timer timer;
        private Point apple;
        private Random generator;
        private String[] myMap;
        private int level = 0;


        public BoardSnake()
        {

            generator = new Random();

            prepareLevel();
        }

        private void prepareLevel()
        {
            if (timer != null)
            {
                timer.Tick -= Timer_Tick;
            }
            level++;
            if (level == 1)
            {
                myMap = Properties.Resources.Map1.Replace("\r", "").Split('\n');
            }
            else if (level == 2)
            {
                myMap = Properties.Resources.Map2.Replace("\r", "").Split('\n');
            }
            else if (level == 3)
            {
                myMap = Properties.Resources.Map3.Replace("\r", "").Split('\n');
            }
            else
            {
                GameEnd?.Invoke("Wygrana");
            }


            List<Point> tmp = new List<Point>();
            tmp.Add(new Point(this.Size.Width / 2, this.Size.Height - 2));
            tmp.Add(new Point(this.Size.Width / 2, this.Size.Height - 1));
            tmp.Add(new Point(this.Size.Width / 2, this.Size.Height));
            tmp.Add(new Point(this.Size.Width / 2, this.Size.Height + 1));
            tmp.Add(new Point(this.Size.Width / 2, this.Size.Height + 2));
            tmp.Add(new Point(this.Size.Width / 2, this.Size.Height + 3));
            mySnake = new Snake(tmp);


            generateApple();

            timer = new Timer();
            timer.Interval = 400;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void generateApple()
        {
            do
            {
                apple = new Point(generator.Next(Size.Width),
                              generator.Next(Size.Height));
            } while (mySnake.Segments.Contains(apple) ||
                     GetFiledType(apple.X, apple.Y) == FieldType.Wall);

        }

        public event Action BoardChanged;
        public delegate void ActionInfo(String info);
        public event ActionInfo GameEnd;
        private void Timer_Tick(object sender, EventArgs e)
        {
            Point newHead = Point.Empty;
            switch (mySnake.direction)
            {
                case Direction.Up:
                    newHead = new Point(mySnake.Segments.First().X, mySnake.Segments.First().Y - 1);
                    break;
                case Direction.Down:
                    newHead = new Point(mySnake.Segments.First().X, mySnake.Segments.First().Y + 1);
                    break;
                case Direction.Left:
                    newHead = new Point(mySnake.Segments.First().X - 1, mySnake.Segments.First().Y);
                    break;
                case Direction.Right:
                    newHead = new Point(mySnake.Segments.First().X + 1, mySnake.Segments.First().Y);
                    break;
            }

            if (mySnake.Segments.Contains(newHead) ||
                GetFiledType(newHead.X, newHead.Y) == FieldType.Wall)

            {
                timer.Stop();
                timer.Tick -= Timer_Tick;
                GameEnd?.Invoke("Przegrana");
            }

            mySnake.Segments.Insert(0, newHead);

            if (newHead == apple)
            {
                prepareLevel();
                generateApple();
            }
            else
            {
                mySnake.Segments.Remove(mySnake.Segments.Last());
            }

            BoardChanged?.Invoke();
        }

        public FieldType GetFiledType(int col, int row)
        {
            if (mySnake.Segments.First() == new Point(col, row))
            {
                return FieldType.SnakeHead;
            }
            else if (mySnake.Segments.Contains(new Point(col, row)))
            {
                return FieldType.SnakeTail;
            }
            else if (apple == new Point(col, row))
            {
                return FieldType.Apple;
            }
            else if (myMap[row][col] == '#')
            {
                return FieldType.Wall;
            }
            else
            {
                return FieldType.Empty;
            }
        }

        internal void KeyDown(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                case Keys.W:
                    mySnake.direction = Direction.Up;
                    break;
                case Keys.Down:
                case Keys.S:
                    mySnake.direction = Direction.Down;
                    break;
                case Keys.Left:
                case Keys.A:
                    mySnake.direction = Direction.Left;
                    break;
                case Keys.Right:
                case Keys.D:
                    mySnake.direction = Direction.Right;
                    break;
                case Keys.Space:
                    timer.Enabled = !timer.Enabled;
                    break;
            }
        }
    }
}
