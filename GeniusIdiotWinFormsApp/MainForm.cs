using GeniusIdiotConsApp;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

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
            if (QuestionNumberLabel.Text == "������� ���� ���:")
            {
                HandleGettingName();
            }
            else if (QuestionNumberLabel.Text.StartsWith("������ � "))
            {
                HandleGettingNumericAnswer();
            }
            else if (QuestionTextLabel.Text.StartsWith("�������"))
            {
                HandleAddingNewQuestion();
            }
            else
            {
                HandleGettingDecision();
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
            ClearForms();

            questionNumber++;
            QuestionNumberLabel.Text = "������ � " + questionNumber;

            var random = new Random();
            var randomIndex = random.Next(0, questions.Count);
            currentQuestion = questions[randomIndex];
            QuestionTextLabel.Text = currentQuestion.Text;
        }

        private void ClearForms()
        {
            QuestionNumberLabel.Text = string.Empty;
            QuestionTextLabel.Text = string.Empty;
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
                UserAnswerTextBox.Clear();
                CommentTextLabel.Text = "������� ����� �� -2*10^9 �� 2*10^9!";
                return null;
            }
        }

        private void FinishTheQuiz()
        {
            ClearForms();

            user.SetScore(rightAnswersCount, startingQuestionsCount);
            user.SetDiagnosis(rightAnswersCount, startingQuestionsCount);

            var userIsPathetic = user.Diagnosis == "�����" || user.Diagnosis == "������" || user.Diagnosis == "�����";

            QuestionNumberLabel.ForeColor = userIsPathetic ? Color.Red : Color.Green;
            QuestionNumberLabel.Text = $"���������� ���������� �������: {user.Score}\n{user.Name}, ��� �������: {user.Diagnosis.ToUpper()}";

            UsersStorage.Save(user);

            QuestionTextLabel.Text = "������ ���������� ������� �����������? (��/���)";
        }

        private void HandleAddingNewQuestion()
        {
            if (QuestionTextLabel.Text == "������� ����� �������:\n- ������ # ����������")
            {
                var questionText = GetNewQuestionText();
                if (!string.IsNullOrEmpty(questionText))
                {
                    newQuestionText = questionText;
                    DisplayText("������� �������� �����:");
                }
            }
            else
            {
                var numericAnswer = GetNumericAnswer();
                if (numericAnswer != null)
                {
                    QuestionsStorage.Add(new Question(newQuestionText, (int)numericAnswer));
                }
            }
        }

        private string? GetNewQuestionText()
        {
            var userInput = UserAnswerTextBox.Text;
            if (string.IsNullOrEmpty(userInput) || userInput.Contains('#'))
            {
                CommentTextLabel.Text = "���������� ������ ���������� �����!";
                UserAnswerTextBox.Clear();
                return null;
            }
            else
            {
                var newQuestionText = userInput.Trim();
                return newQuestionText;
            }
        }

        private void HandleGettingDecision()
        {
            var userIsReady = GetUserDecision();
            if (userIsReady.HasValue)
            {
                if (QuestionTextLabel.Text == "������ ���������� ������� �����������? (��/���)")
                {
                    if ((bool)userIsReady)
                    {
                        ShowHistory();
                    }
                    else
                    {
                        DisplayText("������ �������� ����� ������? (��/���)");
                    }
                }
                else if (QuestionTextLabel.Text == "������ �������� ����� ������? (��/���)")
                {
                    if ((bool)userIsReady)
                    {
                        DisplayText("������� ����� �������:\n- ������ # ����������");
                    }
                    else
                    {
                        // TODO: Display next question
                    }
                }
            }
        }

        private bool? GetUserDecision()
        {
            var userInput = UserAnswerTextBox.Text;
            if (string.IsNullOrEmpty(userInput) || userInput.Trim().ToLower() != "��" && userInput.Trim().ToLower() != "���")
            {
                CommentTextLabel.Text = "���������� ������: �� ��� ���!";
                UserAnswerTextBox.Clear();
                return null;
            }
            else
            {
                var userDecision = userInput.Trim().ToLower();
                if (userDecision.Length < userInput.Length)
                {
                    UserAnswerTextBox.Text = userDecision;
                }
                return userDecision == "��" ? true : false;
            }
        }

        private void ShowHistory()
        {
            var table = string.Empty;

            var allUsers = UsersStorage.GetAll();
            foreach (var user in allUsers)
            {
                table += $"{user.Name} - {user.Score} - {user.Diagnosis}\n";
            }

            var result = MessageBox.Show(table);
            if (result == DialogResult.OK || result == DialogResult.Cancel)
            {
                DisplayText("������ �������� ����� ������? (��/���)");
            }
        }

        private void DisplayText(string text)
        {
            QuestionTextLabel.Text = text;
            UserAnswerTextBox.Clear();
            CommentTextLabel.Text = string.Empty;
        }




    }
}
