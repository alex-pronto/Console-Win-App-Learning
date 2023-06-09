using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;


class User // класс Юзер
{
    public string Name { get; private set; } // свойства класса 
    public string Surname { get; private set; }
    public int Id { get; private set; }
    public int CountRightAnswers { get; private set; }
    public string Diagnose { get; private set; }

    public User(int id, string name, string surname, int countRightAnswers, string diagnose) // конструктор который принимает свойства
    {
        Id = id;
        Name = name;
        Surname = surname;
        CountRightAnswers = countRightAnswers;
        Diagnose = diagnose;

    }



    public void SetNewId(int id)
    {
        Id = id;
    }


    public override string ToString()
    {
        return $"| {Id} | {Name} {Surname} {CountRightAnswers} {Diagnose}";
    }




}

internal class Program
{

    static string DBFilePAth { get; set; }


    private static void Main(string[] args)
    {
        var fileDBName = "users_geniyidiot_game";
        var fileFolderPath = Path.GetTempPath();
        DBFilePAth = fileFolderPath + fileDBName;



        if (File.Exists(DBFilePAth) == false)
        {
            var file = File.Create(DBFilePAth);
            file.Close();
        }



        var isWork = true;

        while (isWork)
        {



            var inputCommand = GetMenu();


            switch (inputCommand)
            {
                case 0:
                    {
                        var allUsers = ReadAllFromDB();
                        if (allUsers.Count == 0) Console.WriteLine("пока никого нет");


                        Console.OutputEncoding = Encoding.UTF8;
                        var data = InitUser();
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


                        break;
                    }
                case 1:
                    {

                        var questions = GetQuestions();

                        var answers = GetAnswers();

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


                            int rightAnswer = answers[randomQuestionIndex];

                            if (userAnswer == rightAnswer)
                            {
                                countRightAnswers++;
                            }

                            questions.RemoveAt(randomQuestionIndex);
                            answers.RemoveAt(randomQuestionIndex);
                        }




                        string finalDiagnose = CalculateDiagnose(countQuestions, countRightAnswers);


                        User newUser = new User(0, name, surname, countRightAnswers, finalDiagnose);

                        SaveToDB(newUser);

                        Console.WriteLine("-----------------------------------");
                        Console.WriteLine(name + ", Ваш диагноз - " + finalDiagnose);


                        break;
                    }



                case 2:
                    {

                        ClearDB();
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



<<<<<<< HEAD
=======
            Console.WriteLine(name + ", Ваш диагноз - " + finalDiagnose);

            string messageToUser = "Если желаете повторить - нажмите - ДА или НЕТ, если хотите выйти";


            bool userChoise = GetUserChoise(messageToUser);

            if (userChoise == false)
            {
                break;
            }
>>>>>>> egorov_lesson_2_2
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



    public static DataTable InitUser()
    {

        var table = new DataTable();


        table.Columns.Add("Id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Surname", typeof(string));
        table.Columns.Add("Right Answers", typeof(int));
        table.Columns.Add("Diagnose", typeof(string));

        var allUsers = ReadAllFromDB();


        foreach (var user in allUsers)

            table.Rows.Add(user.Id, user.Name, user.Surname, user.CountRightAnswers, user.Diagnose);

        return table;
    }


    static void ClearDB()
    {

        File.WriteAllText(DBFilePAth, "");
        Console.WriteLine("Готово");

    }




    static void SaveToDB(User user)
    {
        List<User> AllCurrentUsers = ReadAllFromDB();
        int lastId = AllCurrentUsers.Count == 0 ? 0 : AllCurrentUsers.Last().Id;

        user.SetNewId(lastId + 1);

        AllCurrentUsers.Add(user);
        string serializedUsers = JsonConvert.SerializeObject(AllCurrentUsers);
        File.WriteAllText(DBFilePAth, serializedUsers);

    }

    static void SaveToDB(List<User> users)
    {

        string serializedUsers = JsonConvert.SerializeObject(users);
        File.WriteAllText(DBFilePAth, serializedUsers);

    }

    static List<User> ReadAllFromDB()
    {

        string json = File.ReadAllText(DBFilePAth);
        List<User> currentUsers = JsonConvert.DeserializeObject<List<User>>(json);

        return currentUsers ?? new List<User>();

    }



    static int GetUserAnswer(int i, int randomQuestionIndex, List<string> questions)
    {


        while (true)
        {

            try
            {

                Console.WriteLine();
                Console.WriteLine("Вопрос # " + (i + 1));
                Console.WriteLine(questions[randomQuestionIndex]);
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


<<<<<<< HEAD


    static List<string> GetQuestions()
=======
    static string[] GetQuestions(int countQuestions)
>>>>>>> egorov_lesson_2_2
    {
        var questions = new List<string>();
        questions.Add("Сколько будет два плюс два умноженное на два?");
        questions.Add("Бревно нужно распилить на 10 частей, Сколько нужно сделать распилов?");
        questions.Add("На двух руках 10 пальцев  (Сколько пальцев на 5 руках?)");
        questions.Add("Укол делают каждые пол часа  Сколько нужно минут для 3 уколов?");
        questions.Add("Пять свечей сгорело  Две потухли  Сколько свечей осталось?");
        questions.Add("Два умножить на два?");
        questions.Add("Три умножить на три?");

        return questions;
    }

    static List<int> GetAnswers()
    {
        var answers = new List<int>();
        answers.Add(6);
        answers.Add(9);
        answers.Add(25);
        answers.Add(60);
        answers.Add(2);
        answers.Add(4);
        answers.Add(9);

        return (answers);
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



