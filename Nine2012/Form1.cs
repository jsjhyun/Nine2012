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

        PictureBox lastClicked;
        Image lastImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void outerClick(object sender, MouseEventArgs e)
        {
            PictureBox current = sender as PictureBox;
            lastClicked = current;
            lastImage = lastClicked.Image;

            current.Image = null;
        }

        private void innerClick(object sender, MouseEventArgs e)
        {
            PictureBox current = sender as PictureBox;

            if (current.Image == Properties.Resources.Green)
            {
                current.Image = Properties.Resources.Blank;
                lastImage = Properties.Resources.Green;
            }
            else if (current.Image == Properties.Resources.Pink)
            {
                current.Image = Properties.Resources.Blank;
                lastImage = Properties.Resources.Pink;
            }
        }

        private void doubleClick(object sender, MouseEventArgs e)
        {
            PictureBox current = sender as PictureBox;
            current.Image = lastImage;
        }
    }
}
