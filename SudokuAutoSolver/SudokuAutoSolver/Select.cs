using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuAutoSolver
{
    public partial class Select : Form
    {
        int[][] Board;
        Button button;
        Form1 Main;

        public Select(int[][] board, Button b, Form1 main)
        {
            Board = board;
            button = b;
            Main = main;
            InitializeComponent();
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            if (value.Text.Trim() != "" && Char.IsDigit(Convert.ToChar(value.Text)) && SelectNum(Convert.ToInt32(value.Text))) 
            {
                int y = translateCharToCoord(button.Name[0]);
                int x = Convert.ToInt32(button.Name[1].ToString()) - 1;
                if (value.Text == "0")
                {
                    Board[y][x] = 0;
                    button.Text = "";
                    Close();
                }
                else
                {
                    if (button.Text != value.Text)
                    {
                        if (Main.ValidChoice(Board, y, x, Convert.ToInt32(value.Text)))
                        {
                            Board[y][x] = Convert.ToInt32(value.Text);
                            button.Text = value.Text;
                        }
                        else
                            MessageBox.Show("Invalid value. Please try again!");
                    }
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Value is not between 1 to 9. Please try again!");
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Value_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Enter)
            {
                Confirm_Click(sender, e);
            }
        }

        private bool SelectNum(int num) // number is between 0 to 9
        {
            switch (num)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return true;
                default:
                    return false;
            }
        }

        private int translateCharToCoord(char c) //translate the character within button name to y coordinate
        {
            switch (c) 
            {
                case 'a':
                    return 0;
                case 'b':
                    return 1;
                case 'c':
                    return 2;
                case 'd':
                    return 3;
                case 'e':
                    return 4;
                case 'f':
                    return 5;
                case 'g':
                    return 6;
                case 'h':
                    return 7;
                default:
                    return 8;
            }
        }
    }
}
