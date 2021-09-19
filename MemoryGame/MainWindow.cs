using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace MemoryGame
{
    public partial class MainWindow : Form
    {
        public Button[] LevelButtons;   // An array of button for levels
        private Panel ScoresPanel;   // A panel to contain the scores' labels
        private Label ScoresWord;         // A label to show the word "Scores : "
        public Label ScoresValue;         // A label to show the scores
        private LevelsWindow fr;

        public MainWindow()
        {
            InitializeComponent();
            
            this.AutoSize = true;   // To make the size of window fit the number of it's controls automatically 

            CreateWindow();
        }

        #region ChangeScores method
        private void ChangeScores()
        {
            ScoresValue.Text = fr.CountClicks.ToString();
        }
        #endregion

        #region CallLevelWindow method
        // A method to call level form and send it's properties
        private void CallLevelWindow(int Level)
        {
            fr = new LevelsWindow(Level, this);
            System.Threading.Thread TH = new Thread(ChangeScores);
            TH.Start();
            fr.ShowDialog();
            fr.Dispose();
        }
        #endregion

        #region LevelButClick event
        // Buttons Click method
        private void LevelButClick(object sender, EventArgs e)
        {
            Button tempBut = sender as Button;

            CallLevelWindow(Convert.ToInt32(tempBut.Tag) + 1);
        }
        #endregion

        #region CreateButtons method
        // Creating level buttons method
        private void CreateButtons()
        {
            LevelButtons = new Button[16];
            for (int i = 0; i < 16; i++)
            {
                LevelButtons[i] = new Button
                {
                    Size = new Size(100, 50),
                    Location = new Point(i % 4 * 100, i / 4 * 50 + ScoresPanel.Height),
                    Tag = i,
                    Text = ("Level " + (i + 1).ToString())
                };
                LevelButtons[i].Click += new EventHandler(LevelButClick);
                // To disable buttons except level 1 button 
                if (i > 0)
                    LevelButtons[i].Enabled = false;
                this.Controls.Add(LevelButtons[i]);
            }
        }
        #endregion

        #region CreateWindow
        private void CreateWindow()
        {
            InitializeWindow();

            CreateButtons();
        }
        #endregion

        #region InitializeWindow method
        // Initialize panel and it's label for scores
        private void InitializeWindow()
        {
            this.AutoSize = true;
            this.ScoresPanel = new Panel();
            this.ScoresWord = new Label();
            this.ScoresValue = new Label();
            // 
            // ScoresPanel
            // 
            this.ScoresPanel.Controls.Add(this.ScoresValue);
            this.ScoresPanel.Controls.Add(this.ScoresWord);
            this.ScoresPanel.Dock = DockStyle.Top;
            this.ScoresPanel.Location = new Point(0, 0);
            this.ScoresPanel.Name = "ScoresPanel";
            this.ScoresPanel.Size = new Size(147, 40);
            // 
            // ScoresWord
            // 
            this.ScoresWord.AutoSize = true;
            this.ScoresWord.Location = new Point(13, 13);
            this.ScoresWord.Name = "ScoresWord";
            this.ScoresWord.Size = new Size(49, 13);
            this.ScoresWord.Text = "Scores : ";
            // 
            // ScoresValue
            // 
            this.ScoresValue.AutoSize = true;
            this.ScoresValue.Location = new Point(68, 13);
            this.ScoresValue.Name = "ScoresValue";
            this.ScoresValue.Size = new Size(13, 13);
            this.ScoresValue.TabIndex = 1;
            this.ScoresValue.Text = "0";

            this.Controls.Add(this.ScoresPanel);
        }
        #endregion
    }
}