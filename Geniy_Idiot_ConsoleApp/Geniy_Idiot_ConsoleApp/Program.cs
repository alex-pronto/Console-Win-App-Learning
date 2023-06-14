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

            FileSystem fileSystemUser = new FileSystem();

            fileSystemUser.FileDBName = "users_geniyidiot_game";
            fileSystemUser.FileFolderPath = Path.GetTempPath();
            fileSystemUser.DBFilePath = fileSystemUser.FileFolderPath + fileSystemUser.FileDBName;

            FileSystem fileSystemQuestion = new FileSystem();

            fileSystemQuestion.FileDBName = "questions_geniyidiot_game";
            fileSystemQuestion.FileFolderPath = Path.GetTempPath();
            fileSystemQuestion.DBFilePath = fileSystemQuestion.FileFolderPath + fileSystemQuestion.FileDBName;



            fileSystemUser.CheckFileIsCreate();
            fileSystemQuestion.CheckFileIsCreate();



            UserStorage userStorage = new UserStorage();

            QuestionStorage questionStorage = new QuestionStorage();


            var allquestions = questionStorage.GetQuestions();
            fileSystemQuestion.SaveQuestionsToDB(allquestions);


            bool isWork = true;

            while (isWork)
            {

                var inputCommand = GetMenu();


                switch (inputCommand)
                {
                    case 0:
                        {
                            var allUsers = fileSystemUser.ReadAllUsersFromDB();
                            if (allUsers.Count == 0) Console.WriteLine("пока никого нет");
                            else
                            {
                                Console.OutputEncoding = Encoding.UTF8;
                                var data = InitUser(fileSystemUser);
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

                            var questions = questionStorage.GetQuestions();



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



                                userAnswer = userStorage.GetUserAnswer(i, randomQuestionIndex, questions);


                                var rightAnswer = questions[randomQuestionIndex].Answer;

                                if (userAnswer == rightAnswer)
                                {
                                    countRightAnswers++;
                                }

                                questions.RemoveAt(randomQuestionIndex);

                            }




                            string finalDiagnose = userStorage.CalculateDiagnose(countQuestions, countRightAnswers);


                            User newUser = new User(0, name, surname, countRightAnswers, finalDiagnose);

                            fileSystemUser.SaveUsersToDB(newUser);


                            Console.WriteLine("-----------------------------------");
                            Console.WriteLine(name + ", Ваш диагноз - " + finalDiagnose);


                            break;
                        }



                    case 2:
                        {

                            fileSystemUser.ClearDB();
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



        public static DataTable InitUser(FileSystem fileSystemUser)
        {

            var table = new DataTable();


            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Surname", typeof(string));
            table.Columns.Add("Right Answers", typeof(int));
            table.Columns.Add("Diagnose", typeof(string));

            var allUsers = fileSystemUser.ReadAllUsersFromDB();


            foreach (var user in allUsers)

                table.Rows.Add(user.Id, user.Name, user.Surname, user.CountRightAnswers, user.Diagnose);

            return table;
        }








    

}

