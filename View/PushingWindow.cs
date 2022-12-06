using PushingGame.Eventargs;
using PushingGame.Model;
using PushingGame.Persistance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.MessageBox;

namespace PushingGame.View
{
    public partial class PushingWindow : Form
    {
        private PushingModel model;
        private bool isSelection;
        private TableLayoutPanelCellPosition selectedButton;

        public PushingWindow()
        {
            isSelection = false;
           // TableLayoutPanelCellPosition selectedButton = new TableLayoutPanelCellPosition();
            InitializeComponent();
            threeByThreeToolStripMenuItem.Click += (sender, args) => model.StartNewGame(3);
            fourByFourToolStripMenuItem.Click += (sender, args) => model.StartNewGame(4);
            sixBySixToolStripMenuItem.Click += (sender, args) => model.StartNewGame(6);

            model = new PushingModel(new DataAccess());
            model.GameStarted += onGameStarted;
            model.Refresh += Model_Refresh;
            model.Finish += onGameFinished;

            model.StartNewGame(3);
        }

        private void Model_Refresh(object sender, GameStartedEventArgs e)
        {
            var size = e.BoardSize;
            var board = e.Board;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {

                    setButtonColor((Button)buttonTableLayoutPanel.GetControlFromPosition(j, i), board[i, j]);
                }
            }
        }

        private void onGameStarted(object sender, GameStartedEventArgs e)
        {
            var size = e.BoardSize;
            var board = e.Board;

            buttonTableLayoutPanel.RowCount = size+1;
            buttonTableLayoutPanel.ColumnCount = size+1;
            buttonTableLayoutPanel.Controls.Clear();
            buttonTableLayoutPanel.RowStyles.Clear();
            buttonTableLayoutPanel.ColumnStyles.Clear();

            for (int i = 0; i < size; i++)
            {
               buttonTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent,1/Convert.ToSingle(size)));
               buttonTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1 / Convert.ToSingle(size)));
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var button = new Button();
                    button.AutoSize = true;
                    button.Dock = DockStyle.Fill;
                    setButtonColor(button, board[i, j]);
                    buttonTableLayoutPanel.Controls.Add(button, j, i);
                    button.MouseClick += Button_MouseClick;
                    button.KeyUp += PushingWindow_KeyUp;
                }
            }
        }

        private void Button_MouseClick(object sender, MouseEventArgs e)
        {
            isSelection = true;
            selectedButton = buttonTableLayoutPanel.GetPositionFromControl((Button)sender);
            this.Focus();

        }

        public void setButtonColor(Button b, FieldState state)
        {
            switch (state)
            {
                case FieldState.White:
                    b.BackColor = Color.White;
                    break;
                case FieldState.Black:
                    b.BackColor = Color.Black;
                    break;
                case FieldState.Gray:
                    b.BackColor = Color.Gray;
                    break;
            }
                
        }

        private void PushingWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (isSelection)
            {
                switch (e.KeyData)
                {
                    case Keys.Up:
                        model.Move(selectedButton, Keys.Up);
                        break;
                    case Keys.Down:
                        model.Move(selectedButton, Keys.Down);
                        break;
                    case Keys.Right:
                        model.Move(selectedButton, Keys.Right);
                        break;
                    case Keys.Left:
                        model.Move(selectedButton, Keys.Left);
                        break;
                        
                }
            }
        }

        private void onGameFinished(object sender,GameFinishedEvenetArgs e)
        {
            int w = e.W;
            int b = e.B;
            if (w > b)
            {
                MessageBox.Show
                (
                    "White wins the game",
                    "Game over",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else if (w < b)
            {
                MessageBox.Show
                (
                    "Black wins the game",
                    "Game over",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            { 
                MessageBox.Show
                (
                    "The game ended in a tie",
                    "Game over",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
        }

        private void onSaveMenuItemClicked(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            var result =  saveFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                string path = saveFileDialog.FileName;
                model.SaveGame(path);
            }
        }

        private void onLoadMenuItemClicked(object sender, EventArgs e)
        {
            var OpenFileDialog = new OpenFileDialog();
            var result = OpenFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = OpenFileDialog.FileName;
                model.LoadGame(path);
            }
        }
    }
}
