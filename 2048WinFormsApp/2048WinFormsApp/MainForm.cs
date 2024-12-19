namespace _2048WinFormsApp
{
    public partial class MainForm : Form
    {
        private Label[,] labelsMap;
        private const int mapSize = 4;
        private static Random random = new Random();
        private int score = 0;
        private int bestScore = 0;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitMap();
            GenerateNumber();
            ShowScore();
            CalculateBestScore();
        }

        private void InitMap()
        {
            labelsMap = new Label[mapSize, mapSize];

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    var newLabel = GetNewLabel(i, j);
                    labelsMap[i, j] = newLabel;
                    Controls.Add(labelsMap[i, j]);
                }
            }
        }

        private void GenerateNumber()
        {
            var mapHasEmptyLabels = MapHasEmptyLabels();
            while (mapHasEmptyLabels)
            {
                var randomLabelNumber = random.Next(mapSize * mapSize);
                var rowIndex = randomLabelNumber / mapSize;
                var columnIndex = randomLabelNumber % mapSize;
                if (labelsMap[rowIndex, columnIndex].Text == string.Empty)
                {
                    //randomly generate either 2 or 4... in 75% of times generate 2 and in 25% of times generate 4
                    labelsMap[rowIndex, columnIndex].Text = random.Next(1, 101) <= 75 ? "2" : "4";
                    break;
                }
            }
        }

        private bool MapHasEmptyLabels()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (labelsMap[i, j].Text == string.Empty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Label GetNewLabel(int rowIndex, int columnIndex)
        {
            var newLabel = new Label();
            newLabel.BackColor = SystemColors.ButtonShadow;
            newLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            newLabel.Size = new Size(70, 70);
            newLabel.TextAlign = ContentAlignment.MiddleCenter;
            int x = 10 + columnIndex * 76;
            int y = 70 + rowIndex * 76;
            newLabel.Location = new Point(x, y);
            return newLabel;
        }

        private void ShowScore()
        {
            scoreLabel.Text = score.ToString();
        }

        private void CalculateBestScore()
        {
            var users = UserManager.GetAll();
            if (users.Count == 0)
            {
                return;
            }

            bestScore = users[0].Score;
            foreach (var user in users)
            {
                if (user.Score > bestScore)
                {
                    bestScore = user.Score;
                }
            }

            ShowBestScore();
        }

        private void ShowBestScore()
        {
            if (score > bestScore)
            {
                bestScore = score;
            }
            bestScoreLabel.Text = bestScore.ToString();
        }

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void �����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("����������� ������� �� ���������, ����� ���������� ������. ����� ��� ������ � ����������� �������� �������������, ��� ��������� � ����. ����� ����� �� ��������, ���� ���������!");
        }

        private void ����������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var resultsForm = new ResultsForm();
            resultsForm.ShowDialog();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Right &&
                e.KeyCode != Keys.Left &&
                e.KeyCode != Keys.Up &&
                e.KeyCode != Keys.Down)
            {
                return;
            }

            switch (e.KeyCode)
            {
                case Keys.Right: HandleRight(); break;
                case Keys.Left: HandleLeft(); break;
                case Keys.Up: HandleUp(); break;
                case Keys.Down: HandleDown(); break;
            }

            GenerateNumber();
            ShowScore();
            ShowBestScore();

            if (UserWon())
            {
                UserManager.Add(new User() { Name = "���", Score = score });
                MessageBox.Show("���! �� ��������!");
                return;
            }

            if (GameOver())
            {
                UserManager.Add(new User() { Name = "���", Score = score });
                MessageBox.Show("� ��������� �� ���������!");
                return;
            }
        }

        private void HandleRight()
        {
            MergeRight();
            MoveRight();
        }

        private void MergeRight()
        {
            for (int i = 0; i < mapSize; i++) // � ������ �������...
            {
                for (int j = mapSize - 1; j >= 0; j--) // ���������� �� ������ ������ ������ ������.
                {
                    var rightCell = labelsMap[i, j];
                    if (rightCell.Text != string.Empty) // ���� ������� �������� ������,...
                    {
                        for (int k = j - 1; k >= 0; k--) // �� ���������� �� ������ ������ ����� �� ���,...
                        {
                            var leftCell = labelsMap[i, k];
                            if (leftCell.Text != string.Empty) // � ����� ������� ����� ��������� �������� ������,...
                            {
                                if (leftCell.Text == rightCell.Text) // ��, ���� ����� � ���� ������� �����,...
                                {
                                    var number = int.Parse(rightCell.Text);
                                    rightCell.Text = (number * 2).ToString(); // � ������ ���������� �� �����,...
                                    leftCell.Text = string.Empty; // � ����� �������.
                                    score += number * 2; // � ����������� ���� �� ��� �����.
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void MoveRight()
        {
            for (int i = 0; i < mapSize; i++) // � ������ �������...
            {
                for (int j = mapSize - 1; j >= 0; j--) // ���������� �� ������ ������ ������ ������...
                {
                    var rightCell = labelsMap[i, j];
                    if (rightCell.Text == string.Empty) // � ����� ������� ������ ������...
                    {
                        for (int k = j - 1; k >= 0; k--) // ���������� �� ������ ������ ����� �� ���...
                        {
                            var leftCell = labelsMap[i, k];
                            if (leftCell.Text != string.Empty) // � ����� ������� ����� ��������� �������� ������...
                            {
                                rightCell.Text = leftCell.Text; // � ������ ������ ���������� ����� �� ����� ������...
                                leftCell.Text = string.Empty; // � ����� �������.
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void HandleLeft()
        {
            MergeLeft();
            MoveLeft();
        }

        private void MergeLeft()
        {
            for (int i = 0; i < mapSize; i++) // � ������ �������...
            {
                for (int j = 0; j < mapSize; j++) // ���������� �� ������ ������ ����� �������...
                {
                    var leftCell = labelsMap[i, j];
                    if (leftCell.Text != string.Empty) // � ����� ������� �������� ������...
                    {
                        for (int k = j + 1; k < mapSize; k++) // �� ���������� �� ������ ������ ������ �� ���...
                        {
                            var rightCell = labelsMap[i, k];
                            if (rightCell.Text != string.Empty) // � ����� ������� ������ ��������� �������� ������...
                            {
                                if (rightCell.Text == leftCell.Text) // �� ���� ����� � ���� ������� �����...
                                {
                                    var number = int.Parse(leftCell.Text);
                                    leftCell.Text = (number * 2).ToString(); // � ����� ���������� �� �����...
                                    rightCell.Text = string.Empty; // � ������ �������...
                                    score += number * 2; // � ����������� ���� �� ��� �����.
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void MoveLeft()
        {
            for (int i = 0; i < mapSize; i++) // � ������ �������...
            {
                for (int j = 0; j < mapSize; j++) // ���������� �� ������ ������ ����� �������...
                {
                    var leftCell = labelsMap[i, j];
                    if (leftCell.Text == string.Empty) // � ����� ������� ������ ������...
                    {
                        for (int k = j + 1; k < mapSize; k++) // ���������� �� ������ ������ ������ �� ���...
                        {
                            var rightCell = labelsMap[i, k];
                            if (rightCell.Text != string.Empty) // � ����� ������� ������ ��������� �������� ������...
                            {
                                leftCell.Text = rightCell.Text; // � ����� ������ ���������� ����� �� ������ ������...
                                rightCell.Text = string.Empty; // � ������ �������.
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void HandleUp()
        {
            MergeUp();
            MoveUp();
        }

        private void MergeUp()
        {
            for (int j = 0; j < mapSize; j++) // � ������ �������...
            {
                for (int i = 0; i < mapSize; i++) // ���������� �� ������ ������ ������ ����...
                {
                    var upperCell = labelsMap[i, j];
                    if (upperCell.Text != string.Empty) // � ����� ������� �������� ������...
                    {
                        for (int k = i + 1; k < mapSize; k++) // �� ���������� �� ������ ������ ���� �� ���...
                        {
                            var lowerCell = labelsMap[k, j];
                            if (lowerCell.Text != string.Empty) // � ����� ������� ����� ��������� �������� ������...
                            {
                                if (lowerCell.Text == upperCell.Text) // �� ���� ����� � ���� ������� �����...
                                {
                                    var number = int.Parse(upperCell.Text);
                                    upperCell.Text = (number * 2).ToString(); // � ������� ���������� �� �����...
                                    lowerCell.Text = string.Empty; // � ������ �������...
                                    score += number * 2; // � ����������� ���� �� ��� �����.
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void MoveUp()
        {
            for (int j = 0; j < mapSize; j++) // � ������ �������...
            {
                for (int i = 0; i < mapSize; i++) // ���������� �� ������ ������ ������ ����...
                {
                    var upperCell = labelsMap[i, j];
                    if (upperCell.Text == string.Empty) // � ����� ������� ������ ������...
                    {
                        for (int k = i + 1; k < mapSize; k++) // ���������� �� ������ ������ ���� �� ���...
                        {
                            var lowerCell = labelsMap[k, j];
                            if (lowerCell.Text != string.Empty) // � ����� ������� ����� ��������� �������� ������...
                            {
                                upperCell.Text = lowerCell.Text; // � ������� ������ ���������� ����� �� ������ ������...
                                lowerCell.Text = string.Empty; // � ������ �������.
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void HandleDown()
        {
            MergeDown();
            MoveDown();
        }

        private void MergeDown()
        {
            for (int j = 0; j < mapSize; j++) // � ������ �������...
            {
                for (int i = mapSize - 1; i >= 0; i--) // ���������� �� ������ ������ ����� �����...
                {
                    var lowerCell = labelsMap[i, j];
                    if (lowerCell.Text != string.Empty) // � ����� ������� �������� ������...
                    {
                        for (int k = i - 1; k >= 0; k--) // �� ���������� �� ������ ������ ����� �� ���...
                        {
                            var upperCell = labelsMap[k, j];
                            if (upperCell.Text != string.Empty) // � ����� ������� ������ ��������� �������� ������...
                            {
                                if (upperCell.Text == lowerCell.Text) // �� ���� ����� � ���� ������� �����...
                                {
                                    var number = int.Parse(lowerCell.Text);
                                    lowerCell.Text = (number * 2).ToString(); // � ������ ���������� �� �����...
                                    upperCell.Text = string.Empty; // � ������� �������...
                                    score += number * 2; // � ����������� ���� �� ��� �����.
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void MoveDown()
        {
            for (int j = 0; j < mapSize; j++) // � ������ �������...
            {
                for (int i = mapSize - 1; i >= 0; i--) // ���������� �� ������ ������ ����� �����...
                {
                    var lowerCell = labelsMap[i, j];
                    if (lowerCell.Text == string.Empty) // � ����� ������� ������ ������...
                    {
                        for (int k = i - 1; k >= 0; k--) // �� ���������� �� ������ ������ ����� �� ���...
                        {
                            var upperCell = labelsMap[k, j];
                            if (upperCell.Text != string.Empty) // � ����� ������� ������ ��������� �������� ������...
                            {
                                lowerCell.Text = upperCell.Text; // � ������ ������ ���������� ����� �� ������� ������...
                                upperCell.Text = string.Empty; // � ������� �������.
                                break;
                            }
                        }
                    }
                }
            }
        }

        private bool UserWon()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (labelsMap[i, j].Text == "2048")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool GameOver()
        {
            if (MapHasEmptyLabels())
            {
                return false;
            }

            //if(CellsCanBeMerged())
            //{
            //    return false;
            //}

            for (int i = 0; i < mapSize - 1; i++) // TODO: ���� i < mapSize, ����� ������ ������ ������ �� ����������� �� ��������� � ������ �������.
            {
                for (int j = 0; j < mapSize - 1; j++) // TODO: ���� j < mapSize, ����� ������ ����� ������ ������� �� ����������� �� ��������� � ������ �������.
                {
                    if (labelsMap[i, j].Text == labelsMap[i, j + 1].Text ||
                        labelsMap[i, j].Text == labelsMap[i + 1, j].Text)
                    {
                        return false;
                    }
                }
            }

            return true;
        }


    }
}
