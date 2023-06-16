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

            var fileSystemUser = new FileSystem();

            fileSystemUser.FileDBName = "users_geniyidiot_game";
            fileSystemUser.FileFolderPath = Path.GetTempPath();
            fileSystemUser.DBFilePath = fileSystemUser.FileFolderPath + fileSystemUser.FileDBName;

            var fileSystemQuestion = new FileSystem();

            fileSystemQuestion.FileDBName = "questions_geniyidiot_game";
            fileSystemQuestion.FileFolderPath = Path.GetTempPath();
            fileSystemQuestion.DBFilePath = fileSystemQuestion.FileFolderPath + fileSystemQuestion.FileDBName;



            fileSystemUser.CheckFileIsCreate();
            fileSystemQuestion.CheckFileIsCreate();

        

            var userStorage = new UserStorage();

            //var questionStorage = new QuestionStorage();


            var allquestions = fileSystemQuestion.ReadQuestionsFromDB();
        
            
            

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

                            var questions = fileSystemQuestion.ReadQuestionsFromDB();

                            var countQuestions = questions.Count;

                            var countRightAnswers = 0;

                            if (countQuestions == 0)
                        {
                            Console.WriteLine("В базе нет вопросов");
                            break;
                        }


                            Console.WriteLine("Введите Имя");
                            string name = Console.ReadLine();


                            Console.WriteLine("Введите фамилию");
                            string surname = Console.ReadLine();


                            var random = new Random();

                            for (int i = 0; i < countQuestions; i++)
                            {

                                var userAnswer = 0;

                                var randomQuestionIndex = random.Next(0, questions.Count);


                                Console.WriteLine();
                                Console.WriteLine("Вопрос # " + (i + 1));
                                Console.WriteLine(questions[randomQuestionIndex].Text);
                                userAnswer = UserStorage.GetNumber();


                                var rightAnswer = questions[randomQuestionIndex].Answer;

                                if (userAnswer == rightAnswer)
                                {
                                    countRightAnswers++;
                                }

                                questions.RemoveAt(randomQuestionIndex);


                            }

                            

                            string finalDiagnose = userStorage.CalculateDiagnose(countQuestions, countRightAnswers);
                            
                            
                            var newUser = new User(0, name, surname, countRightAnswers, finalDiagnose);

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
                        
                       
                            var questions = fileSystemQuestion.ReadQuestionsFromDB();

                            if (questions.Count == 0)
                            {
                                Console.WriteLine("Похоже в базе больше нет вопросов"); break;
                            }

                        for (int i = 0; i < questions.Count; i++)
                            {
                                Console.WriteLine($"{i+1} {questions[i].Text}");
                            }
                            Console.WriteLine();
                            Console.WriteLine("-----------------------------------\n введите номер вопроса который хотите удалить \n или ввведите 0 для выхода в меню");


                            

                        var questionToDelete = UserStorage.GetQuestionNumber(questions.Count);

                        if (questionToDelete == 0)
                        {
                            break; 
                        }
                        else
                        {
                            questions.RemoveAt(questionToDelete - 1);


                            fileSystemQuestion.ClearDB();
                            fileSystemQuestion.SaveQuestionsToDB(questions);

                            
                            Console.WriteLine("Готово.  Вопрос удален");
                            Console.WriteLine("Текущий список вопросов");
                            
                            for (int i = 0; i < questions.Count; i++)
                            {
                                Console.WriteLine($"{i + 1} {questions[i].Text}");
                            }
                            Console.WriteLine();
                        }
                            break;

                        }

                    case 4:
                        {
                            var newQuestion = UserStorage.AddNewQuestion();
                            var currentQuestions = fileSystemQuestion.ReadQuestionsFromDB();
                            currentQuestions.Add(newQuestion);
                            fileSystemQuestion.SaveQuestionsToDB(currentQuestions);


                        break;
                        }

                    case 5:
                       {
                            allquestions = QuestionStorage.GetQuestions();
                            fileSystemQuestion.SaveQuestionsToDB(allquestions);
                            break;
                       }



                    case 6:
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
                string allCommands = "-----------------------------------\n0 - вывести результаты всех пользователей \n1 - новая игра \n2 - очистить предыдущие результаты\n3 - выбрать и удалить вопросы \n4 - добавить вопросы \n5 - сбросить список вопросов в исходное состояние \n6 - выход    \n -----------------------------------";
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

