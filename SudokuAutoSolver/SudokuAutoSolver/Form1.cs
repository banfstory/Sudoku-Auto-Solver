using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace SudokuAutoSolver
{
    public partial class Form1 : Form
    {
        bool solved = false;
        private int[][] LogicalBoard = new int[9][];
        private Button[][] PhysicalBoard = new Button[9][];      

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < PhysicalBoard.Length; i++)
            {
                LogicalBoard[i] = new int[9];
                PhysicalBoard[i] = new Button[9];
            }
            FillBoard(PhysicalBoard);
        }

        private void testBoard(int[][] board) // used for testing purposes
        {
            board[0][0] = 5; board[0][1] = 3; board[0][4] = 7; board[1][0] = 6; board[1][3] = 1; board[1][4] = 9;
            board[1][5] = 5; board[2][1] = 9; board[2][2] = 8; board[2][7] = 6; board[3][0] = 8; board[3][4] = 6;
            board[3][8] = 3; board[4][0] = 4; board[4][3] = 8; board[4][5] = 3; board[4][8] = 1; board[5][0] = 7;
            board[5][4] = 2; board[5][8] = 6; board[6][1] = 6; board[6][6] = 2; board[6][7] = 8; board[7][3] = 4;
            board[7][4] = 1; board[7][5] = 9; board[7][8] = 5; board[8][4] = 8; board[8][7] = 7; board[8][8] = 9;
            for (int i = 0; i < LogicalBoard.Length; i++)
            {
                for (int j = 0; j < LogicalBoard[i].Length; j++)
                {
                    if (board[i][j] == 0) continue;
                    PhysicalBoard[i][j].Text = board[i][j].ToString();
                }
            }
        }

        private void Click_Block(object sender, EventArgs e)
        {
            if (!solved)
            {
                Button b = (Button)sender;
                Select s = new Select(LogicalBoard, b, this);
                DialogResult show = s.ShowDialog();
            }
        }

        private void Solve_Click(object sender, EventArgs e) // Solve the current Sudoku Problem
        {
            if (!solved)
            {
                if (Solve(LogicalBoard, CountTurns(LogicalBoard), LogicalBoard))
                {
                    for (int i = 0; i < LogicalBoard.Length; i++)
                    {
                        for (int j = 0; j < LogicalBoard[i].Length; j++)
                        {
                            if (PhysicalBoard[i][j].Text == "")
                            {
                                if (LogicalBoard[i][j] == 0) continue;
                                PhysicalBoard[i][j].Text = LogicalBoard[i][j].ToString();
                                PhysicalBoard[i][j].ForeColor = Color.Red;
                            }
                        }
                    }
                    solved = true;
                }
                else
                    MessageBox.Show("This Sudoku puzzle is unsolvable!");
            }
        }

        private void Reset_Click(object sender, EventArgs e) // Reset current board state
        {
            Reset();
        }

        private bool Solve(int[][] board, int turn, int[][] origBoard)
        {
            if (turn == 81) 
            {
                for (int y = 0; y < origBoard.Length; y++) 
                {
                    for (int x = 0; x < origBoard[y].Length; x++) 
                        origBoard[y][x] = board[y][x];
                }
                return true;
            }
            for (int y = 0; y < board.Length; y++) 
            {
                for (int x = 0; x < board[y].Length; x++) 
                {
                    if (board[y][x] == 0)
                    {
                        for (int i = 1; i <= 9; i++)
                        {
                            if (ValidChoice(board, y, x, i))
                            {
                                int[][] newBoard = new int[9][];
                                for (int j = 0; j < newBoard.Length; j++) 
                                {
                                    newBoard[j] = new int[9];
                                    for (int k = 0; k < newBoard[j].Length; k++) 
                                        newBoard[j][k] = board[j][k];
                                }
                                newBoard[y][x] = i;
                                if (Solve(newBoard, turn + 1, origBoard))
                                    return true;
                            }
                        }
                        return false; // if a selected area cannot use any of its numbers, this board state is impossible break out early to reduce processing 
                    }
                }
            }
            return false;
        }

        public bool ValidChoice(int[][] board, int y, int x, int num) // determine if selected choice does not have any repeated values within its own row, column and box areas
        {
            for (int i = 0; i < board.Length; i++)
            {
                if (board[y][i] == num || board[i][x] == num)
                    return false;
            }
            int Y = BoxCoord(y);
            int X = BoxCoord(x);
            for (int i = Y; i < Y + 3; i++)
            {
                for (int j = X; j < X + 3; j++)
                {
                    if (board[i][j] == num)
                        return false;
                }
            }
            return true;
        }

        private int BoxCoord(int point) // used to determine which box the selected choice is located to check if it is a valid choice or not
        {
            switch (point)
            {
                case 0:
                case 1:
                case 2:
                    return 0;
                case 3:
                case 4:
                case 5:
                    return 3;
                default:
                    return 6;
            }
        }

        private int CountTurns(int[][] board) 
        {
            int count = 0;
            for (int i = 0; i < board.Length; i++) 
            {
                for (int j = 0; j < board[i].Length; j++) 
                {
                    if (board[i][j] != 0)
                        count++;
                }
            }
            return count;
        }

        private void Reset()
        {
            for (int i = 0; i < LogicalBoard.Length; i++)
            {
                for (int j = 0; j < LogicalBoard[i].Length; j++)
                {
                    LogicalBoard[i][j] = 0;
                    PhysicalBoard[i][j].Text = "";
                    PhysicalBoard[i][j].ForeColor = Color.Black;
                }
            }
            solved = false;
        }

        private void FillBoard(Button[][] b) // Initalize all buttons into an array
        {
            b[0][0] = a1; b[0][1] = a2; b[0][2] = a3; b[0][3] = a4; b[0][4] = a5; b[0][5] = a6; b[0][6] = a7; b[0][7] = a8; b[0][8] = a9;
            b[1][0] = b1; b[1][1] = b2; b[1][2] = b3; b[1][3] = b4; b[1][4] = b5; b[1][5] = b6; b[1][6] = b7; b[1][7] = b8; b[1][8] = b9;
            b[2][0] = c1; b[2][1] = c2; b[2][2] = c3; b[2][3] = c4; b[2][4] = c5; b[2][5] = c6; b[2][6] = c7; b[2][7] = c8; b[2][8] = c9;
            b[3][0] = d1; b[3][1] = d2; b[3][2] = d3; b[3][3] = d4; b[3][4] = d5; b[3][5] = d6; b[3][6] = d7; b[3][7] = d8; b[3][8] = d9;
            b[4][0] = e1; b[4][1] = e2; b[4][2] = e3; b[4][3] = e4; b[4][4] = e5; b[4][5] = e6; b[4][6] = e7; b[4][7] = e8; b[4][8] = e9;
            b[5][0] = f1; b[5][1] = f2; b[5][2] = f3; b[5][3] = f4; b[5][4] = f5; b[5][5] = f6; b[5][6] = f7; b[5][7] = f8; b[5][8] = f9;
            b[6][0] = g1; b[6][1] = g2; b[6][2] = g3; b[6][3] = g4; b[6][4] = g5; b[6][5] = g6; b[6][6] = g7; b[6][7] = g8; b[6][8] = g9;
            b[7][0] = h1; b[7][1] = h2; b[7][2] = h3; b[7][3] = h4; b[7][4] = h5; b[7][5] = h6; b[7][6] = h7; b[7][7] = h8; b[7][8] = h9;
            b[8][0] = i1; b[8][1] = i2; b[8][2] = i3; b[8][3] = i4; b[8][4] = i5; b[8][5] = i6; b[8][6] = i7; b[8][7] = i8; b[8][8] = i9;
        }
    }
}
