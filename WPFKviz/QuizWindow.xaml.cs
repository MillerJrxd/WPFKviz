using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFKviz
{
    /// <summary>
    /// Interaction logic for QuizWindow.xaml
    /// </summary>
    public partial class QuizWindow : Window
    {
        private readonly string _connectionString = "Server=localhost;Database=szokviz;User=root;Password=;SslMode=None;";
        private List<Szavak> _questions;
        private int _currentIndex = 0;

        public QuizWindow()
        {
            InitializeComponent();
            LoadQuestions();
            DisplayQuestion();
        }

        private void LoadQuestions()
        {
            _questions = new List<Szavak>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM szavak WHERE tanult = 0";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _questions.Add(new Szavak
                        {
                            Id = reader.GetInt32("id"),
                            Szo = reader.GetString("szo"),
                            Jelentese = reader.GetString("jelentese"),
                            Tanult = reader.GetBoolean("tanult"),
                            HelyesValaszokSzama = reader.GetInt32("helyes_valaszok_szama")
                        });
                    }
                }
            }

            if (_questions.Count == 0)
            {
                MessageBox.Show("Nincs több kérdés. Gratulálok, minden szót megtanultál!");
                Close();
            }
        }

        private void DisplayQuestion()
        {
            if (_currentIndex >= _questions.Count)
            {
                MessageBox.Show("Ciklus vége! Próbáld újra később.");
                Close();
                return;
            }

            var question = _questions[_currentIndex];
            QuestionText.Text = $"Mit jelent az alábbi szó: {question.Szo}?";

            var answers = _questions.Select(q => q.Jelentese).OrderBy(_ => Guid.NewGuid()).Take(3).ToList();
            if (!answers.Contains(question.Jelentese))
                answers[0] = question.Jelentese;
            answers = answers.OrderBy(_ => Guid.NewGuid()).ToList();

            Answer1.Content = answers[0];
            Answer2.Content = answers[1];
            Answer3.Content = answers[2];
            Answer4.Content = answers.Count > 3 ? answers[3] : question.Jelentese;
        }

        private void Answer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content.ToString() == _questions[_currentIndex].Jelentese)
            {
                IncrementCorrectAnswerCount(_questions[_currentIndex].Id);
                MessageBox.Show("Helyes válasz!");
            }
            else
            {
                MessageBox.Show("Hibás válasz.");
            }

            _currentIndex++;
            DisplayQuestion();
        }

        private void IncrementCorrectAnswerCount(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE szavak SET helyes_valaszok_szama = helyes_valaszok_szama + 1 WHERE id = @Id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
