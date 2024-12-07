using GeniusIdiotCommon;

namespace GeniusIdiotWinFormsApp
{
    public partial class MainForm : Form
    {
        private Quiz quiz;
        private User user;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UserAnswerTextBox.GotFocus += UserAnswerTextBox_GotFocus;
            UserAnswerTextBox.KeyDown += UserAnswerTextBox_KeyDown;

            MeetNewUser();
            StartNewQuiz();
        }

        private void UserAnswerTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                NextButton_Click(sender, e);
            }
        }

        private void MeetNewUser()
        {
            var welcomeForm = new WelcomeForm();
            welcomeForm.ShowDialog();

            user = new User();
            user.Name = welcomeForm.UserNameTextBox.Text;
        }

        private void UserAnswerTextBox_GotFocus(object? sender, EventArgs e)
        {
            CommentLabel.Text = string.Empty;
        }

        private void StartNewQuiz()
        {
            ClearForms();

            quiz = new Quiz(user);
            quiz.ResetUserResult();

            ShowRandomQuestion();
        }

        private void ClearForms()
        {
            QuestionNumberLabel.Text = string.Empty;
            QuestionTextLabel.Text = string.Empty;
            UserAnswerTextBox.Clear();
            CommentLabel.Text = string.Empty;

            QuestionTextLabel.ForeColor = Color.Black;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (QuestionNumberLabel.Text.StartsWith("������ � "))
            {
                var userAnswer = GetNumericAnswer();

                if (userAnswer != null)
                {
                    quiz.AcceptUserAnswer(userAnswer);

                    if (quiz.IsEnded)
                    {
                        FinishTheQuiz();
                    }
                    else
                    {
                        ShowRandomQuestion();
                    }
                }
            }
            else
            {
                UserAnswerTextBox.Clear();
                CommentLabel.Text = "��������� � ���� � ����� ������� ����!";
            }
        }

        private int? GetNumericAnswer()
        {
            string userInput = UserAnswerTextBox.Text;
            int userAnswer;
            if (int.TryParse(userInput, out userAnswer))
            {
                return userAnswer;
            }
            else
            {
                UserAnswerTextBox.Clear();
                CommentLabel.Text = "������� ����� �� -2*10^9 �� 2*10^9!";
                return null;
            }
        }

        private void ShowRandomQuestion()
        {
            ClearForms();

            quiz.RandomizeCurrentQuestion();

            QuestionNumberLabel.Text = "������ � " + quiz.CurrentQuestionNumber;
            QuestionTextLabel.Text = quiz.CurrentQuestion.Text;

            UserAnswerTextBox.Focus();
        }

        private void FinishTheQuiz()
        {
            quiz.SetUserScore();
            quiz.SetUserDiagnosis();

            ShowDiagnosis();

            UsersStorage.Append(user);
        }

        private void ShowDiagnosis()
        {
            ClearForms();

            var userIsPathetic = user.Diagnosis == "�����" || user.Diagnosis == "������" || user.Diagnosis == "�����";
            QuestionTextLabel.ForeColor = userIsPathetic ? Color.Red : Color.Green;
            QuestionTextLabel.Text = $"���������� ���������� �������: {user.Score}\n{user.Name}, ��� �������: {user.Diagnosis.ToUpper()}";
        }

        private void �����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ����������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void ���������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var historyForm = new HistoryForm();
            historyForm.ShowDialog();
        }

        private void ��������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addQuestionForm = new AddQuestionForm();
            addQuestionForm.ShowDialog();

            var questions = QuestionsStorage.GetAll();
            if (questions.Count != quiz.Length)
            {
                StartNewQuiz();
            }
        }

        private void �������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var questionRemovalForm = new questionRemovalForm();
            questionRemovalForm.ShowDialog();

            var questions = QuestionsStorage.GetAll();
            if (questions.Count != quiz.Length)
            {
                StartNewQuiz();
            }
        }

        private void ���������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartNewQuiz();
        }
    }
}
