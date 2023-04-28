using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PK_07_Snake
{
    public partial class FormSnake : Form
    {
        private BoardSnake myGame;

        private Graphics graphics;
        private const int fieldSize = 25;


        public FormSnake()
        {
            InitializeComponent();
            myGame = new BoardSnake();
            myGame.BoardChanged += DrawBoard;
            myGame.GameEnd += MyGame_GameEnd;


            pictureBoxVisualization.Image = new Bitmap(myGame.Size.Width * fieldSize,
                                                       myGame.Size.Height * fieldSize);

            pictureBoxVisualization.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBoxVisualization.Margin = new Padding(0);
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            graphics = Graphics.FromImage(pictureBoxVisualization.Image);

            DrawBoard();
        }

        private void MyGame_GameEnd(string info)
        {
            MessageBox.Show("Koniec gry: " + info);
        }

        private void DrawBoard()
        {
            graphics.Clear(Color.White);
            for (int col = 0; col < myGame.Size.Width; col++)
            {
                for (int row = 0; row < myGame.Size.Height; row++)
                {
                    switch (myGame.GetFiledType(col, row))
                    {
                        case BoardSnake.FieldType.SnakeHead:

                            graphics.DrawImage(Properties.Resources.ImageSnakeHead,
                                                   col * fieldSize,
                                                   row * fieldSize);
                            break;
                        case BoardSnake.FieldType.SnakeTail:

                            graphics.DrawImage(Properties.Resources.ImageSnakeTail,
                                                    col * fieldSize,
                                                    row * fieldSize);
                            break;
                        case BoardSnake.FieldType.Apple:

                            graphics.DrawImage(Properties.Resources.ImageApple,
                                                   col * fieldSize,
                                                   row * fieldSize);
                            break;
                        case BoardSnake.FieldType.Wall:

                            graphics.DrawImage(Properties.Resources.ImageWall,
                                                   col * fieldSize,
                                                   row * fieldSize);
                            break;
                    }
                    graphics.DrawRectangle(new Pen(Color.Gray),
                                                   col * fieldSize,
                                                   row * fieldSize,
                                                   fieldSize,
                                                   fieldSize);
                }
            }
            pictureBoxVisualization.Refresh();
        }

        private void FormSnake_KeyDown(object sender, KeyEventArgs e)
        {
            myGame.KeyDown(e.KeyCode);
        }
    }
}
