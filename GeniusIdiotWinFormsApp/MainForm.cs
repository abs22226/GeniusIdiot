using GeniusIdiotCommon;

namespace GeniusIdiotWinFormsApp
{
    public partial class MainForm : Form
    {
        private List<Question> questions;
        private int startingQuestionsCount;
        private Question currentQuestion;
        private int questionNumber;
        private User user;
        private int rightAnswersCount;
        private string newQuestionText;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            UserAnswerTextBox.GotFocus += UserAnswerTextBox_GotFocus;

            var welcomeForm = new WelcomeForm();
            welcomeForm.ShowDialog();

            user = new User(welcomeForm.UserNameTextBox.Text);

            StartNewQuiz();
        }

        private void UserAnswerTextBox_GotFocus(object? sender, EventArgs e)
        {
            CommentLabel.Text = string.Empty;
        }

        private void StartNewQuiz()
        {
            ResetScoring();
            ClearForms();

            questions = QuestionsStorage.GetAll();
            startingQuestionsCount = questions.Count;

            ShowRandomQuestion();
        }

        private void ResetScoring()
        {
            startingQuestionsCount = 0;
            questionNumber = 0;
            rightAnswersCount = 0;
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
                    if (userAnswer == currentQuestion.Answer)
                    {
                        rightAnswersCount++;
                    }

                    questions.Remove(currentQuestion);

                    if (questions.Count > 0)
                    {
                        ShowRandomQuestion();
                    }
                    else
                    {
                        FinishTheQuiz();
                    }
                }
            }
            else
            {
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

            questionNumber++;
            QuestionNumberLabel.Text = "������ � " + questionNumber;

            var random = new Random();
            var randomIndex = random.Next(0, questions.Count);
            currentQuestion = questions[randomIndex];
            QuestionTextLabel.Text = currentQuestion.Text;

            UserAnswerTextBox.Focus();
        }

        private void FinishTheQuiz()
        {
            ClearForms();

            user.SetScore(rightAnswersCount, startingQuestionsCount);
            user.SetDiagnosis(rightAnswersCount, startingQuestionsCount);

            QuestionNumberLabel.Text = string.Empty;

            var userIsPathetic = user.Diagnosis == "�����" || user.Diagnosis == "������" || user.Diagnosis == "�����";
            QuestionTextLabel.ForeColor = userIsPathetic ? Color.Red : Color.Green;
            QuestionTextLabel.Text = $"���������� ���������� �������: {user.Score}\n{user.Name}, ��� �������: {user.Diagnosis.ToUpper()}";

            UsersStorage.Save(user);
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
            if (questions.Count != startingQuestionsCount)
            {
                StartNewQuiz();
            }
        }

        private void �������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var questionRemovalForm = new questionRemovalForm();
            questionRemovalForm.ShowDialog();

            var questions = QuestionsStorage.GetAll();
            if (questions.Count != startingQuestionsCount)
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
