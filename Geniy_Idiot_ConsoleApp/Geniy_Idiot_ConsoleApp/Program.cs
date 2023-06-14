using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;
using System.Reflection.Emit;


partial class Program
{


        private static void Main(string[] args)
        {


            UserStorage userStorage = new UserStorage();

            userStorage.FileDBName = "users_geniyidiot_game";
            userStorage.FileFolderPath = Path.GetTempPath();
            userStorage.DBFilePath = userStorage.FileDBName + userStorage.DBFilePath;


            userStorage.CheckFileIsCreate();



            bool isWork = true;

            while (isWork)
            {

                var inputCommand = GetMenu();


                switch (inputCommand)
                {
                    case 0:
                        {
                            var allUsers = userStorage.ReadAllFromDB();
                            if (allUsers.Count == 0) Console.WriteLine("пока никого нет");
                            else
                            {
                                Console.OutputEncoding = Encoding.UTF8;
                                var data = InitUser(userStorage);
                                var columnNames = data.Columns.Cast<DataColumn>()
                                                        .Select(x => x.ColumnName)
                                                        .ToArray();

                                DataRow[] rows = data.Select();


                                var table = new ConsoleTable(columnNames);



                                foreach (DataRow row in rows)
                                {
                                    table.AddRow(row.ItemArray);
                                }

                                table.Write(Format.Alternative);
                            }



                            break;
                        }
                    case 1:
                        {

                            var questions = GetQuestions();



                            var countQuestions = questions.Count;

                            var countRightAnswers = 0;




                            Console.WriteLine("Введите Имя");
                            string name = Console.ReadLine();


                            Console.WriteLine("Введите фамилию");
                            string surname = Console.ReadLine();

                            var random = new Random();

                            for (int i = 0; i < countQuestions; i++)
                            {

                                var userAnswer = 0;

                                var randomQuestionIndex = random.Next(0, questions.Count);



                                userAnswer = GetUserAnswer(i, randomQuestionIndex, questions);


                                var rightAnswer = questions[randomQuestionIndex].Answer;

                                if (userAnswer == rightAnswer)
                                {
                                    countRightAnswers++;
                                }

                                questions.RemoveAt(randomQuestionIndex);

                            }




                            string finalDiagnose = CalculateDiagnose(countQuestions, countRightAnswers);


                            User newUser = new User(0, name, surname, countRightAnswers, finalDiagnose);

                            userStorage.SaveToDB(newUser);


                            Console.WriteLine("-----------------------------------");
                            Console.WriteLine(name + ", Ваш диагноз - " + finalDiagnose);


                            break;
                        }



                    case 2:
                        {

                            userStorage.ClearDB();
                            break;

                        }
                    case 3:
                        {
                            isWork = false;
                            Console.WriteLine("пока");
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("НЕТ такой команды");
                            break;
                        }
                }



            }
        }


        static int GetMenu()
        {

            while (true)
            {
                string allCommands = "-----------------------------------\n0 - вывести результаты всех\n1 - новая игра \n2 - очистить предыдущие результаты\n3 - выход  \n -----------------------------------";
                Console.WriteLine(allCommands);

                try
                {

                    return int.Parse(Console.ReadLine());

                }

                catch (FormatException)
                {
                    Console.WriteLine("Нет Такой команды");


                }

                catch (OverflowException)
                {
                    Console.WriteLine("Нет Такой команды");

                }
            }


        }



        public static DataTable InitUser(UserStorage fileDBName)
        {

            var table = new DataTable();


            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Surname", typeof(string));
            table.Columns.Add("Right Answers", typeof(int));
            table.Columns.Add("Diagnose", typeof(string));

            var allUsers = fileDBName.ReadAllFromDB();


            foreach (var user in allUsers)

                table.Rows.Add(user.Id, user.Name, user.Surname, user.CountRightAnswers, user.Diagnose);

            return table;
        }


        



   


    static int GetUserAnswer(int i, int randomQuestionIndex, List<Question> questions)
    {


        while (true)
        {

            try
            {

                Console.WriteLine();
                Console.WriteLine("Вопрос # " + (i + 1));
                Console.WriteLine(questions[randomQuestionIndex].Text);
                return Convert.ToInt32(Console.ReadLine());

            }

            catch (FormatException)
            {

                Console.WriteLine("ВВЕДИТЕ ЧИСЛО!");
                Console.WriteLine();

            }

            catch (OverflowException)
            {
                Console.WriteLine();
                Console.WriteLine("ВВЕДИТЕ ЧИСЛО от -2*10^-9 до 2*10^9");
            }
        }

    }


    static List<Question> GetQuestions()

    {
        var questions = new List<Question>();

        questions.Add(new Question("Сколько будет два плюс два умноженное на два?", 6));
        questions.Add(new Question("Бревно нужно распилить на 10 частей, Сколько нужно сделать распилов?", 9));
        questions.Add(new Question("На двух руках 10 пальцев  (Сколько пальцев на 5 руках?)", 25));
        questions.Add(new Question("Укол делают каждые пол часа  Сколько нужно минут для 3 уколов?", 60));
        questions.Add(new Question("Пять свечей сгорело  Две потухли  Сколько свечей осталось?", 2));

        return questions;
    }




    static string CalculateDiagnose(double countQuestions, int countRightAnswers)
    {

        var numbersOfDiagnoses = 6;

        var diagnoses = new string[numbersOfDiagnoses];
        diagnoses[0] = "Идиот";
        diagnoses[1] = "Кретин";
        diagnoses[2] = "Дурак";
        diagnoses[3] = "Нормальный";
        diagnoses[4] = "Талант";
        diagnoses[5] = "Гений";

        string finalDiagnose = "";

        var scaleOfDiagnose = countQuestions / (numbersOfDiagnoses - 1);

        if (countRightAnswers >= 0 && countRightAnswers < scaleOfDiagnose)
        {
            finalDiagnose = diagnoses[0];
        }
        else if (countRightAnswers >= scaleOfDiagnose && countRightAnswers < scaleOfDiagnose * 2)

        {
            finalDiagnose = diagnoses[1];

        }
        else if (countRightAnswers >= 2 * scaleOfDiagnose && countRightAnswers < scaleOfDiagnose * 3)

        {
            finalDiagnose = diagnoses[2];

        }
        else if (countRightAnswers >= 3 * scaleOfDiagnose && countRightAnswers < scaleOfDiagnose * 4)

        {
            finalDiagnose = diagnoses[3];

        }
        else if (countRightAnswers >= 4 * scaleOfDiagnose && countRightAnswers < scaleOfDiagnose * 5)

        {
            finalDiagnose = diagnoses[4];

        }
        else if (countRightAnswers >= 5 * scaleOfDiagnose && countRightAnswers <= scaleOfDiagnose * 6)

        {
            finalDiagnose = diagnoses[5];

        }



        return (finalDiagnose);



    }

}






