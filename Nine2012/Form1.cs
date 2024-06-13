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
using System.Windows.Forms.VisualStyles;

namespace Nine2012
{
    public partial class Form1 : Form
    {
        bool greenTurn = true;
        bool greenMill = false; bool pinkMill = false;
        bool goingMove = false;

        PictureBox focused;

        char[,] board = new char[3, 8];
        int[,] greenLoc = new int[9, 2]; // 녹색 피스의 위치
        int[,] pinkLoc = new int[9, 2]; // 핑크 피스의 위치

        int greenCnt = 9; 
        int pinkCnt = 9; 
        int waitGreen = 9; // 놓을 녹색 피스 수
        int waitPink = 9; // 놓을 핑크 피스 수

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
            // 피스 위치 초기화
            for (int i = 0; i < 9; i++)
            {
                greenLoc[i, 0] = -1;
                greenLoc[i, 1] = -1;
                pinkLoc[i, 0] = -1;
                pinkLoc[i, 1] = -1;
            }
        }

        private void pieceClick(object sender, MouseEventArgs e)
        {
            PictureBox piece = sender as PictureBox;
            string color = piece.Tag.ToString();
            string[] tmp = color.Split('-');

            // 밀을 생성한 경우 다른 피스를 선택해 제거
            if (greenMill)
            {
                if (tmp[0] == "Pink")
                {
                    int row = pinkLoc[Int32.Parse(tmp[1]), 0];
                    int col = pinkLoc[Int32.Parse(tmp[1]), 1];

                    // 선택된 피스가 밀에 포함되어 있지 않으면 메시지
                    if (chkMill(row, col) != 1)
                    {
                        textBoxMessage.Text = "Impossible!";
                    }
                    else
                    {
                        // 피스 제거 및 게임 상태 업데이트
                        piece.Visible = false;
                        pinkCnt--;
                        textBoxPinkCount.Text = "P : " + pinkCnt; // 남은 피스 개수 표시

                        // 승리 조건 검사1 : 피스 수가 3개 미만인 경우 
                        if (pinkCnt < 3)
                        {
                            MessageBox.Show("Green Win!");
                            Application.Restart();
                        }
                        board[row, col] = 'B';
                        greenMill = false;
                        textBoxMessage.Text = "Pink's Turn!";
                    }
                }
                else
                {
                    textBoxMessage.Text = "Select Pink!";
                }
            }
            // 밀을 생성한 경우 다른 피스를 선택해 제거
            else if (pinkMill)
            {
                if (tmp[0] == "Green")
                {
                    int row = greenLoc[Int32.Parse(tmp[1]), 0];
                    int col = greenLoc[Int32.Parse(tmp[1]), 1];

                    if (chkMill(row, col) != 1)
                    {
                        textBoxMessage.Text = "Impossible!";
                    }
                    else
                    {
                        // 피스 제거 및 게임 상태 업데이트
                        piece.Visible = false;
                        greenCnt--;
                        textBoxGreenCount.Text = "G : " + greenCnt; // 남은 피스 개수 표시

                        // 승리 조건 검사1 : 피스 수가 3개 미만인 경우 
                        if (pinkCnt < 3)
                        {
                            MessageBox.Show("Pink Win!");
                            Application.Restart();
                        }

                        board[row, col] = 'B';
                        pinkMill = false;
                        textBoxMessage.Text = "Green's Turn!";
                    }
                }
                else
                {
                    textBoxMessage.Text = "Select Green!";
                }
            }
            else
            {
                if (greenTurn && tmp[0] == "Green")
                {
                    focused = piece;
                    goingMove = true;
                }
                else if (!greenTurn && tmp[0] == "Pink")
                {
                    focused = piece;
                    goingMove = true;
                }
            }
        }

        private void blankClick(object sender, MouseEventArgs e)
        {
            PictureBox blank = sender as PictureBox;

            try
            {
                string color = focused.Tag.ToString();
                string[] tmp = color.Split('-');

                if (goingMove)
                {
                    goingMove = false;

                    int row = blank.Tag.ToString()[0] - '0';
                    int col = blank.Tag.ToString()[1] - '0';

                    if (tmp[0] == "Green")
                    {
                        int oldRow = greenLoc[Int32.Parse(tmp[1]), 0];
                        int oldCol = greenLoc[Int32.Parse(tmp[1]), 1];

                        if (oldRow != -1 && oldCol != -1)
                        {
                            if (!chkToGo(oldRow, oldCol, row, col)) return;
                            board[oldRow, oldCol] = 'B';
                        }
                        else waitGreen--;

                        focused.Location = blank.Location;
                        greenLoc[Int32.Parse(tmp[1]), 0] = row;
                        greenLoc[Int32.Parse(tmp[1]), 1] = col;
                        board[row, col] = 'G';

                        if (chkMill(row, col) == 0) greenMill = true;

                        if (greenMill)
                        {
                            textBoxMessage.Text = "Green's Mill!";
                        }
                        else
                        {
                            if (chkWin())
                            {
                                MessageBox.Show("Green Win!");
                                Application.Restart();
                            }
                            else textBoxMessage.Text = "Pink's Turn!";
                        }
                    }
                    else if (tmp[0] == "Pink")
                    {
                        board[row, col] = 'P';
                        int oldRow = pinkLoc[Int32.Parse(tmp[1]), 0];
                        int oldCol = pinkLoc[Int32.Parse(tmp[1]), 1];

                        if (oldRow != -1 && oldCol != -1)
                        {
                            if (!chkToGo(oldRow, oldCol, row, col)) return;
                            board[oldRow, oldCol] = 'B';
                        }
                        else waitPink--;

                        focused.Location = blank.Location;
                        pinkLoc[Int32.Parse(tmp[1]), 0] = row;
                        pinkLoc[Int32.Parse(tmp[1]), 1] = col;

                        if (chkMill(row, col) == 0) pinkMill = true;

                        if (pinkMill)
                        {
                            textBoxMessage.Text = "Pink's Mill!";
                        }
                        else
                        {
                            if (chkWin())
                            {
                                MessageBox.Show("Pink Win!");
                                Application.Restart();
                            }
                            else textBoxMessage.Text = "Green's Turn!";
                        }
                    }
                    greenTurn = !greenTurn;
                }
            }
            catch (Exception ex) { }
        }

        // 주어진 위치에서 밀을 형성하는지 검사하는 로직
        private int chkMill(int row, int col)
        {
            if (row < 0 || col < 0 || row > 2 || col > 7) return -1;

            char c = 'G';

            if (!greenTurn)
            {
                c = 'P';
            }

            if (col == 0)
            {
                if (board[row, 1] == c && board[row, 2] == c) return 0;
                if (board[row, 6] == c && board[row, 7] == c) return 0;
                return 1;
            }
            else if (col == 1)
            {
                if (board[row, 0] == c && board[row, 2] == c) return 0;
                if (board[0, 1] == c && board[1, 1] == c && board[2, 1] == c) return 0;
                return 1;
            }
            else if (col == 2)
            {
                if (board[row, 0] == c && board[row, 1] == c) return 0;
                if (board[row, 3] == c && board[row, 4] == c) return 0;
                return 1;
            }
            else if (col == 3)
            {
                if (board[row, 4] == c && board[row, 2] == c) return 0;
                if (board[0, 3] == c && board[1, 3] == c && board[2, 3] == c) return 0;
                return 1;
            }
            else if (col == 4)
            {
                if (board[row, 2] == c && board[row, 3] == c) return 0;
                if (board[row, 5] == c && board[row, 6] == c) return 0;
                return 1;
            }
            else if (col == 5)
            {
                if (board[row, 4] == c && board[row, 6] == c) return 0;
                if (board[0, 5] == c && board[1, 5] == c && board[2, 5] == c) return 0;
                return 1;
            }
            else if (col == 6)
            {
                if (board[row, 4] == c && board[row, 5] == c) return 0;
                if (board[row, 0] == c && board[row, 7] == c) return 0;
                return 1;
            }
            else if (col == 7)
            {
                if (board[row, 6] == c && board[row, 0] == c) return 0;
                if (board[0, 7] == c && board[1, 7] == c && board[2, 7] == c) return 0;
                return 1;
            }
            else return -1;
        }

        // 승리 조건 검사 로직2 : 더 이상 움직이지 못할 경우
        private bool chkWin()
        {
            if (greenTurn)
            {
                if (waitPink != 0) return false;

                for (int i = 0; i < 9; i++)
                {
                    if (board[pinkLoc[i, 0], pinkLoc[i, 1]] != 'P') continue;

                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            if (board[j, k] != 'B') continue;
                            if (chkToGo(pinkLoc[i, 0], pinkLoc[i, 1], j, k)) return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                if (waitGreen != 0) return false;

                for (int i = 0; i < 9; i++)
                {
                    if (board[greenLoc[i, 0], greenLoc[i, 1]] != 'G') continue;

                    for (int j = 0; j < 3; j++)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            if (board[j, k] != 'B') continue;
                            if (chkToGo(greenLoc[i, 0], greenLoc[i, 1], j, k)) return false;
                        }
                    }
                }
                return true;
            }
        }

        // 이동 가능 여부 검사
        private bool chkToGo(int oldRow, int oldCol, int row, int col)
        {
            if ((oldCol == 0 && col == 7) || (oldCol == 7 && col == 0))
            {
                if (row == oldRow) return true;
                else return false;
            }

            int rowSub = oldRow - row;
            int colSub = oldCol - col;

            if (rowSub == 1 || rowSub == -1)
            {
                if (oldCol != col) return false;
                if (col == 0 || col == 2 || col == 4 || col == 6) return false;
                return true;
            }

            if (colSub == 1 || colSub == -1)
            {
                if (row == oldRow) return true;
                else return false;
            }
            return false;
        }
    }
}