using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nine2012
{
    public partial class Form1 : Form
    {
        int greenCount = 9;
        int pinkCount = 9;

        bool greenTurn = true;

        bool greenMill = false;
        bool pinkMill = false;

        bool goingMove = false;

        PictureBox focused;

        char[,] board = new char[3, 8];

        public Form1()
        {
            InitializeComponent();
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = 'B';
                }
            }
        }

        private void pieceClick(object sender, MouseEventArgs e)
        {
            PictureBox piece = sender as PictureBox;

            if (greenMill)
            {

            }
            else if (pinkMill)
            {

            }
            else
            {
                if (greenTurn && piece.Tag.ToString() == "Green")
                {
                    focused = piece;
                    goingMove = true;
                }
                else if (!greenTurn && piece.Tag.ToString() == "Pink")
                {
                    focused = piece;
                    goingMove = true;
                }
            }
        }

        private void blankClick(object sender, MouseEventArgs e)
        {
            PictureBox blank = sender as PictureBox;

            if (goingMove)
            {
                focused.Location = blank.Location;
                goingMove = false;

                int row = blank.Tag.ToString()[0] - '0';
                int col = blank.Tag.ToString()[1] - '0';

                if (focused.Tag.ToString() == "Green")
                {
                    board[row, col] = 'G';
                    // if (greenMill) { }
                }
                else if (focused.Tag.ToString() == "Pink")
                {
                    board[row, col] = 'P';
                    // if (pinkMill) { }
                }

                focused = null;
                greenTurn = !greenTurn;

                if (greenTurn)
                {
                    textBoxMessage.Text = "Green's Turn!";
                }
                else
                {
                    textBoxMessage.Text = "Pink's Turn!";
                }
            }
        }
    }
}
