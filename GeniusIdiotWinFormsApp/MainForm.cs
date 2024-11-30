using GeniusIdiotConsApp;

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

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetSizes();

            GreetNewUser();

            UserAnswerTextBox.GotFocus += UserAnswerTextBox_GotFocus;
        }

        private void SetSizes()
        {
            this.MaximumSize = new Size(430, 489);
            this.MinimumSize = new Size(430, 489);

            QuestionTextLabel.MaximumSize = new Size(296, 90);

            CommentTextLabel.MaximumSize = new Size(296, 90);
        }

        private void GreetNewUser()
        {
            QuestionNumberLabel.Text = "������� ���� ���:";
            QuestionTextLabel.Text = "- �� 20 ��������,\n- ������ # ����������";

            UserAnswerTextBox.Clear();

            CommentTextLabel.Text = string.Empty;
        }

        private void UserAnswerTextBox_GotFocus(object? sender, EventArgs e)
        {
            CommentTextLabel.Text = string.Empty;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            switch (QuestionNumberLabel.Text)
            {
                case "������� ���� ���:": HandleGettingName(); break;
                default: HandleGettingNumericAnswer(); break;
            }
        }

        private void HandleGettingName()
        {
            var name = GetUserName();

            if (!string.IsNullOrEmpty(name))
            {
                user = new User(name);

                StartNewQuiz();
            }
        }

        private string? GetUserName()
        {
            var userInput = UserAnswerTextBox.Text;
            if (string.IsNullOrEmpty(userInput) || userInput.Contains('#') || userInput.Trim().Length > 20)
            {
                CommentTextLabel.Text = "���������� ������ ���������� ���!";
                UserAnswerTextBox.Clear();
                return null;
            }
            else
            {
                var userName = userInput.Trim();
                return userName;
            }
        }

        private void StartNewQuiz()
        {
            questions = QuestionsStorage.GetAll();
            startingQuestionsCount = questions.Count;

            ShowRandomQuestion();
        }

        private void ShowRandomQuestion()
        {
            var random = new Random();
            var randomIndex = random.Next(0, questions.Count);

            currentQuestion = questions[randomIndex];

            questionNumber++;
            QuestionNumberLabel.Text = "������ � " + questionNumber;

            QuestionTextLabel.Text = currentQuestion.Text;

            UserAnswerTextBox.Clear();

            CommentTextLabel.Text = string.Empty;
        }

        private void HandleGettingNumericAnswer()
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
                CommentTextLabel.Text = "������� ����� �� -2*10^9 �� 2*10^9!";
                UserAnswerTextBox.Clear();
                return null;
            }
        }

        private void FinishTheQuiz()
        {
            ShowUserDiagnosis();

            QuestionTextLabel.Text = string.Empty;
            UserAnswerTextBox.Clear();
            CommentTextLabel.Text = string.Empty;



        }

        private void ShowUserDiagnosis()
        {
            user.SetScore(rightAnswersCount, startingQuestionsCount);
            user.SetDiagnosis(rightAnswersCount, startingQuestionsCount);

            QuestionNumberLabel.Text = $"���������� ���������� �������: {user.Score}\n{user.Name}, ��� �������: {user.Diagnosis}";
        }
    }
}
