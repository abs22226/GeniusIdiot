namespace _2048WinFormsApp
{
    public partial class MainForm : Form
    {
        private Label[,] labelsMap;
        private const int mapSize = 4;
        private static Random random = new Random();
        private int score = 0;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            KeyDown += MainForm_KeyDown;

            InitMap();
            GenerateNumber();
            ShowScore();
        }

        private void ShowScore()
        {
            scoreLabel.Text = score.ToString();
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right)
            {
                for (int i = 0; i < mapSize; i++) // � ������ �������...
                {
                    for (int j = mapSize - 1; j >= 0; j--) // ���������� �� ������ ������ ������ ������...
                    {
                        if (labelsMap[i, j].Text != string.Empty) // � ����� ������� �������� ������...
                        {
                            for (int k = j - 1; k >= 0; k--) // ���������� �� ������ ������ ����� �� ���...
                            {
                                if (labelsMap[i, k].Text != string.Empty) // � ����� ������� ��������� �������� ������...
                                {
                                    if (labelsMap[i, k].Text == labelsMap[i, j].Text) // �� ���� ����� � ���� �������� ������� �����...
                                    {
                                        var number = int.Parse(labelsMap[i, j].Text);
                                        labelsMap[i, j].Text = (number * 2).ToString(); // � ������ ���������� �� �����...
                                        labelsMap[i, k].Text = string.Empty; // � ����� �������.
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < mapSize; i++) // � ������ �������...
                {
                    for (int j = mapSize - 1; j >= 0; j--) // ���������� �� ������ ������ ������ ������...
                    {
                        if (labelsMap[i, j].Text == string.Empty) // � ����� ������� ������ ������...
                        {
                            for (int k = j - 1; k >= 0; k--) // ���������� �� ������ ������ ����� �� ���...
                            {
                                if (labelsMap[i, k].Text != string.Empty) // � ����� ������� ��������� �������� ������...
                                {
                                    labelsMap[i, j].Text = labelsMap[i, k].Text; // � ������ ������ ���������� ����� �� ����� ������...
                                    labelsMap[i, k].Text = string.Empty; // � ����� �������.
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                for (int i = 0; i < mapSize; i++) // � ������ �������...
                {
                    for (int j = 0; j < mapSize; j++) // ���������� �� ������ ������ ����� �������...
                    {
                        if (labelsMap[i, j].Text != string.Empty) // � ����� ������� �������� ������...
                        {
                            for (int k = j + 1; k < mapSize; k++) // ���������� �� ������ ������ ������ �� ���...
                            {
                                if (labelsMap[i, k].Text != string.Empty) // � ����� ������� ��������� �������� ������...
                                {
                                    if (labelsMap[i, k].Text == labelsMap[i, j].Text) // �� ���� ����� � ���� �������� ������� �����...
                                    {
                                        var number = int.Parse(labelsMap[i, j].Text);
                                        labelsMap[i, j].Text = (number * 2).ToString(); // � ����� ���������� �� �����...
                                        labelsMap[i, k].Text = string.Empty; // � ������ �������.
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < mapSize; i++) // � ������ �������...
                {
                    for (int j = 0; j < mapSize; j++) // ���������� �� ������ ������ ����� �������...
                    {
                        if (labelsMap[i, j].Text == string.Empty) // � ����� ������� ������ ������...
                        {
                            for (int k = j + 1; k < mapSize; k++) // ���������� �� ������ ������ ������ �� ���...
                            {
                                if (labelsMap[i, k].Text != string.Empty) // � ����� ������� ��������� �������� ������...
                                {
                                    labelsMap[i, j].Text = labelsMap[i, k].Text; // � ����� ������ ���������� ����� �� ������ ������...
                                    labelsMap[i, k].Text = string.Empty; // � ������ �������.
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                for (int j = 0; j < mapSize; j++) // � ������ �������...
                {
                    for (int i = 0; i < mapSize; i++) // ���������� �� ������ ������ ������ ����...
                    {
                        if (labelsMap[i, j].Text != string.Empty) // � ����� ������� �������� ������...
                        {
                            for (int k = i + 1; k < mapSize; k++) // ���������� �� ������ ������ ���� �� ���...
                            {
                                if (labelsMap[k, j].Text != string.Empty) // � ����� ������� ��������� �������� ������...
                                {
                                    if (labelsMap[k, j].Text == labelsMap[i, j].Text) // �� ���� ����� � ���� �������� ������� �����...
                                    {
                                        var number = int.Parse(labelsMap[i, j].Text);
                                        labelsMap[i, j].Text = (number * 2).ToString(); // � ������� ���������� �� �����...
                                        labelsMap[k, j].Text = string.Empty; // � ������ �������.
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int j = 0; j < mapSize; j++) // � ������ �������...
                {
                    for (int i = 0; i < mapSize; i++) // ���������� �� ������ ������ ������ ����...
                    {
                        if (labelsMap[i, j].Text == string.Empty) // � ����� ������� ������ ������...
                        {
                            for (int k = i + 1; k < mapSize; k++) // ���������� �� ������ ������ ���� �� ���...
                            {
                                if (labelsMap[k, j].Text != string.Empty) // � ����� ������� ��������� �������� ������...
                                {
                                    labelsMap[i, j].Text = labelsMap[k, j].Text; // � ������� ������ ���������� ����� �� ������ ������...
                                    labelsMap[k, j].Text = string.Empty; // � ������ �������.
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                for (int j = 0; j < mapSize; j++) // � ������ �������...
                {
                    for (int i = mapSize - 1; i >= 0; i--) // ���������� �� ������ ������ ����� �����...
                    {
                        if (labelsMap[i, j].Text != string.Empty) // � ����� ������� �������� ������...
                        {
                            for (int k = i - 1; k >= 0; k--) // ���������� �� ������ ������ ����� �� ���...
                            {
                                if (labelsMap[k, j].Text != string.Empty) // � ����� ������� ��������� �������� ������...
                                {
                                    if (labelsMap[k, j].Text == labelsMap[i, j].Text) // �� ���� ����� � ���� �������� ������� �����...
                                    {
                                        var number = int.Parse(labelsMap[i, j].Text);
                                        labelsMap[i, j].Text = (number * 2).ToString(); // � ������ ���������� �� �����...
                                        labelsMap[k, j].Text = string.Empty; // � ������� �������.
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int j = 0; j < mapSize; j++) // � ������ �������...
                {
                    for (int i = mapSize - 1; i >= 0; i--) // ���������� �� ������ ������ ����� �����...
                    {
                        if (labelsMap[i, j].Text == string.Empty) // � ����� ������� ������ ������...
                        {
                            for (int k = i - 1; k >= 0; k--) // ���������� �� ������ ������ ����� �� ���...
                            {
                                if (labelsMap[k, j].Text != string.Empty) // � ����� ������� ��������� �������� ������...
                                {
                                    labelsMap[i, j].Text = labelsMap[k, j].Text; // � ������ ������ ���������� ����� �� ������� ������...
                                    labelsMap[k, j].Text = string.Empty; // � ������� �������.
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            GenerateNumber();
            ShowScore();
        }

        private void InitMap()
        {
            labelsMap = new Label[mapSize, mapSize];

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    var newLabel = GetNewLabel(i, j);
                    Controls.Add(newLabel);
                    labelsMap[i, j] = newLabel;
                }
            }
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

        private void GenerateNumber()
        {
            while (true) // TODO: get rid of while(true)
            {
                var labelNumber = random.Next(mapSize * mapSize);
                var rowIndex = labelNumber / mapSize;
                var columnIndex = labelNumber % mapSize;
                if (labelsMap[rowIndex, columnIndex].Text == string.Empty)
                {
                    //TODO: randomly generate either 2 or 4
                    labelsMap[rowIndex, columnIndex].Text = "2";
                    break;
                }
            }
        }


    }
}
