using GeniusIdiotCommon;
using System.Xml.Linq;

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
            CommentTextLabel.Text = string.Empty;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (QuestionNumberLabel.Text.StartsWith("������ � "))
            {
                HandleGettingNumericAnswer();
            }
            else if (QuestionTextLabel.Text.StartsWith("�������"))
            {
                HandleQuestionsStorageModification();
            }
            else
            {
                HandleGettingDecision();
            }
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
            CommentTextLabel.Text = string.Empty;

            QuestionNumberLabel.ForeColor = Color.Black;
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

            UserAnswerTextBox.Focus();
        }

        private void HandleQuestionsStorageModification()
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
            else if (QuestionTextLabel.Text == "������� �������� �����:")
            {
                var numericAnswer = GetNumericAnswer();
                if (numericAnswer != null)
                {
                    QuestionsStorage.Add(new Question(newQuestionText, (int)numericAnswer));

                    DisplayText("������ ������� �����-�� ������? (��/���)");
                }
            }
            else if (QuestionTextLabel.Text == "������� ����� ������� ��� ��������:")
            {
                var number = GetQuestionNumber(questions.Count);
                if (number != null)
                {
                    var index = (int)number - 1;
                    var question = questions[index];
                    QuestionsStorage.Remove(question);

                    DisplayText("������ ������ ���� �����? (��/���)");
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

        private int? GetQuestionNumber(int questionsCount)
        {
            var userInput = UserAnswerTextBox.Text;
            int number;
            if (int.TryParse(userInput, out number) && number >= 1 && number <= questionsCount)
            {
                return number;
            }
            else
            {
                UserAnswerTextBox.Clear();
                CommentTextLabel.Text = $"������� ����� �� 1 �� {questionsCount}!";
                return null;
            }
        }

        private void HandleGettingDecision()
        {
            var userIsReady = GetUserDecision();
            if (userIsReady.HasValue)
            {
                if (QuestionTextLabel.Text == "������ �������� ����� ������? (��/���)")
                {
                    if ((bool)userIsReady) DisplayText("������� ����� �������:\n- ������ # ����������");
                    else DisplayText("������ ������� �����-�� ������? (��/���)");
                }
                else if (QuestionTextLabel.Text == "������ ������� �����-�� ������? (��/���)")
                {
                    if ((bool)userIsReady) InitiateQuestionRemoval();
                    else DisplayText("������ ������ ���� �����? (��/���)");
                }
                else if (QuestionTextLabel.Text == "������ ������ ���� �����? (��/���)")
                {
                    if ((bool)userIsReady) StartNewQuiz();
                    else CloseTheApp();
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

        private void DisplayText(string text)
        {
            QuestionTextLabel.Text = text;
            UserAnswerTextBox.Clear();
            CommentTextLabel.Text = string.Empty;

            UserAnswerTextBox.Focus();
        }

        private void InitiateQuestionRemoval()
        {
            questions = QuestionsStorage.GetAll();

            var text = string.Empty;
            for (int i = 0; i < questions.Count; i++)
            {
                text += $"\n{i + 1}. {questions[i].Text}";
            }

            DisplayText("��������� ����� ������� ��� ��������:");

            var pressedMessageBoxButton = MessageBox.Show(text);
            if (pressedMessageBoxButton == DialogResult.OK || pressedMessageBoxButton == DialogResult.Cancel)
            {
                DisplayText("������� ����� ������� ��� ��������:");
            }
        }

        private void CloseTheApp()
        {
            this.Close();
        }

        private void �����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void ���������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var historyForm = new HistoryForm();
            historyForm.ShowDialog();
        }
    }
}
