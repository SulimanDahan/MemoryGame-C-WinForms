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
    public partial class LevelsWindow : Form
    {
        private int SelectedCells, AllCells;
        public int CountClicks { get; set; } // count the number of clicks the user have been done to buttons
        

        Thread th1;
        Thread th2;

        Button[] btns;

        List<int> RandomIndexes;

        MainWindow MW;

        public LevelsWindow(int Level, MainWindow MW)
        {
            this.MW = new MainWindow();
            this.MW = MW;

            ChangeLevel(Level);

            ReloadWindow();
        }

        #region CreateButtons method
        // Create Buttons of level and add them to level window
        private void CreateButtons()
        {
            btns = new Button[AllCells];
            int TempCells = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(AllCells)));

            for (int i = 0; i < AllCells; i++)
            {
                btns[i] = new Button
                {
                    Size = new Size(50, 50),
                    Location = new Point(i % TempCells * 50, i / TempCells * 50 + this.ScoresPanel.Height + 5),
                    Tag = i
                };
                this.Controls.Add(btns[i]);
            }
        }
        #endregion

        #region StartPlay method
        // Enable buttons to play and return the selected buttons to the default one
        private void StartPlay()
        {
            Thread.Sleep(3000);

            // Add event to all buttons after the end of waiting time
            for (int j = 0; j < AllCells; j++)
                btns[j].Click += new EventHandler(BtnClick);

            foreach (int i in RandomIndexes)
                this.Controls.OfType<Button>().ToList()[i].BackColor = SystemColors.ControlLight;
        }
        #endregion

        #region StartColor method
        // Disable buttons and color the selected buttons
        private void StartColor()
        {
            foreach (int i in RandomIndexes)
                this.Controls.OfType<Button>().ToList()[i].BackColor = Color.White;

            //Thread.Sleep(3000);
        }
        #endregion

        #region Buttons click event
        // Buttons click method and game algorithm
        private void BtnClick(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.BackColor == SystemColors.ControlLight)
            {
                CountClicks++;
                if (RandomIndexes.Contains((int)btn.Tag))
                {
                    ScoresValue.Text = CountClicks.ToString();
                    btn.BackColor = Color.Green;
                    System.Media.SystemSounds.Beep.Play();
                    MW.ScoresValue.Text = CountClicks.ToString();

                    // Check the number of clicks the user have been done
                    if (CountClicks == SelectedCells)
                    {
                        Thread.Sleep(500);
                        MessageBox.Show($"You win\nResult = {CountClicks}", "Result");

                        MW.LevelButtons[SelectedCells - 1].Enabled = true;
                        ChangeLevel(1);     // Go to the next level
                        ReloadWindow();     // Restart the window with new components
                    }
                }
                if (!RandomIndexes.Contains((int)btn.Tag))
                {
                    btn.BackColor = Color.Red;

                    Thread.Sleep(750);
                    MessageBox.Show($"You lose\nResult = {ScoresValue.Text}", "Result");

                    MW.LevelButtons[SelectedCells - 1].Enabled = false;
                    ChangeLevel(-1);        // Go to the previous level
                    ReloadWindow();
                }
            }
        }
        #endregion

        #region SetRanIndexes method
        // Generate random right buttons (green)
        private void SetRanIndexes(int n)
        {
            RandomIndexes = new List<int>();
            Random r = new Random();
            
            while(n>0)
            {
                int Index = r.Next(0, AllCells-1);
                if(!RandomIndexes.Contains(Index))
                {
                    n--;
                    RandomIndexes.Add(Index);
                }
            }
        }
        #endregion

        #region ReloadWindow method
        private void ReloadWindow()
        {
            CountClicks = 0;

            MW.ScoresValue.Text = "0";

            this.Controls.Clear();

            InitializeWindow();
            CreateButtons();
            SetRanIndexes(SelectedCells);

            th1 = new Thread(StartColor);
            th2 = new Thread(StartPlay);

            th1.Start();
            th2.Start();
        }
        #endregion

        #region ChangeLevel method
        // A method to change the level
        private void ChangeLevel(int Level)
        {
            SelectedCells += Level;

            if (SelectedCells == 0) SelectedCells = 1;

            if (SelectedCells >= 1 && SelectedCells <= 7) // Level 1 to 7
                AllCells = 25;
            if (SelectedCells >= 8 && SelectedCells <= 10) // Level 8 to 10
                AllCells = 36;
            if (SelectedCells >= 11 && SelectedCells <= 12) // Level 11 and 12
                AllCells = 49;
            if (SelectedCells >= 13 && SelectedCells <= 15) // Level 13 to 15
                AllCells = 64;
            else if (SelectedCells == 16) //Level 16
                AllCells = 81;

            this.Text = "Level " + SelectedCells.ToString();
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
            
            this.ScoresPanel.Controls.Add(this.ScoresValue);
            this.ScoresPanel.Controls.Add(this.ScoresWord);
            this.ScoresPanel.Dock = DockStyle.Top;
            this.ScoresPanel.Location = new Point(0, 0);
            this.ScoresPanel.Name = "ScoresPanel";
            this.ScoresPanel.Size = new Size(147, 40);
            
            this.ScoresWord.AutoSize = true;
            this.ScoresWord.Location = new Point(13, 13);
            this.ScoresWord.Name = "ScoresWord";
            this.ScoresWord.Size = new Size(49, 13);
            this.ScoresWord.Text = "Scores : ";

            this.ScoresValue.AutoSize = true;
            this.ScoresValue.Location = new Point(68, 13);
            this.ScoresValue.Name = "ScoresValue";
            this.ScoresValue.Size = new Size(13, 13);
            this.ScoresValue.Text = "0";

            this.Controls.Add(this.ScoresPanel);
        }
        #endregion
    }
}