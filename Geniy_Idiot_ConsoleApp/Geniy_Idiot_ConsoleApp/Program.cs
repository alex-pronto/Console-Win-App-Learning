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

    public User(int id, string name, string surname,int countRightAnswers, string diagnose) // конструктор который принимает свойства
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
        string fileDBName = "users_geniyidiot_game";
        string fileFolderPath = Path.GetTempPath();
        DBFilePAth = fileFolderPath + fileDBName;



        if (File.Exists(DBFilePAth) == false)
        {
            var file = File.Create(DBFilePAth);
            file.Close();
        }



        bool isWork = true;
           
        while (isWork)
        {
            


            int inputCommand = GetMenu();


            switch (inputCommand)
            {
                case 0:
                    {
                        var allUsers = ReadAllFromDB();
                        if (allUsers.Count == 0) Console.WriteLine("пока никого нет");


                        Console.OutputEncoding = Encoding.UTF8;
                        var data = InitUser();
                        string[] columnNames = data.Columns.Cast<DataColumn>()
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
                        var countQuestions = 7;

                        int[] arrayCountQuestions = GetArrayCountQuestions(countQuestions);

                        string[] questions = GetQuestions(countQuestions);

                        int[] answers = GetAnswers(countQuestions);

                        int countRightAnswers = 0;

                        arrayCountQuestions = RandomizeNumbers(countQuestions, arrayCountQuestions);

                        string[] randomQuestions = RandomizeQuestions(questions, arrayCountQuestions, countQuestions);

                        int[] randomAnswers = RandomizeAnswers(answers, arrayCountQuestions, countQuestions);

                        Console.WriteLine("Введите Имя");
                        string name = Console.ReadLine();


                        Console.WriteLine("Введите фамилию");
                        string surname = Console.ReadLine();
                        

                        for (int i = 0; i < countQuestions; i++)
                        {

                            int userAnswer = 0;


                            userAnswer = CheckUserAnswer(randomQuestions, i, userAnswer);


                            int rightAnswer = randomAnswers[i];

                            if (userAnswer == rightAnswer)
                            {
                                countRightAnswers++;
                            }
                        }


                        string finalDiagnose = CalculateDiagnose(countQuestions, countRightAnswers);

                       
                        User newUser = new User(0, name, surname, countRightAnswers, finalDiagnose);
                        
                        SaveToDB(newUser);

                        Console.WriteLine("----------------------------");
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


            
        }
    }


    static int GetMenu()
    {
        bool checkAnswer = false;
        int input = 0;
        while (checkAnswer == false)
        {
            string allCommands = "----------------------------\n0 - вывести результаты всех\n1 - новая игра \n2 - очистить предыдущие результаты\n3 - выход  \n ----------------------------";
            Console.WriteLine(allCommands);
            
            try
            {
                checkAnswer = true;
                 input = int.Parse(Console.ReadLine());

            }

            catch (FormatException)
            {
                Console.WriteLine("Нет Такой команды");
                checkAnswer = false;
                
            }
        }

        return (input);

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



    static int CheckUserAnswer(string[] randomQuestions, int i, int userAnswer)
    {
        bool checkAnswer = false;

        while (checkAnswer == false)
        {

            try
            {
                checkAnswer = true;
                Console.WriteLine();
                Console.WriteLine("Вопрос # " + (i + 1));
                Console.WriteLine(randomQuestions[i]);
                userAnswer = Convert.ToInt32(Console.ReadLine());

            }

            catch
            {
                Console.WriteLine("ВВЕДИТЕ ЧИСЛО!");
                Console.WriteLine();
                checkAnswer = false;

            }
        }
        return (userAnswer);
    }


    

    static int[] GetArrayCountQuestions(int countQuestions)
    {
        int[] arrayCountQuestions = new int[countQuestions];


        for (int i = 0; i < countQuestions; i++)
        {

            arrayCountQuestions[i] = i + 1;

        }


        return (arrayCountQuestions);
    }

    static int[] RandomizeNumbers(int countQuestions, int[] arrayCountQuestions)
    {
        Random randomIndex = new Random();


        for (int i = countQuestions - 1; i >= 0; i--)
        {


            int j = randomIndex.Next(i);

            int tmpNumbers = arrayCountQuestions[j];
            arrayCountQuestions[j] = arrayCountQuestions[i];
            arrayCountQuestions[i] = tmpNumbers;




        }
        return (arrayCountQuestions);
    }




    static string[] GetQuestions(int countQuestions)
    {
        string[] questions = new string[countQuestions];
        questions[0] = "Сколько будет два плюс два умноженное на два?";
        questions[1] = "Бревно нужно распилить на 10 частей, Сколько нужно сделать распилов?";
        questions[2] = "На двух руках 10 пальцев  (Сколько пальцев на 5 руках?)";
        questions[3] = "Укол делают каждые пол часа  Сколько нужно минут для 3 уколов?";
        questions[4] = "Пять свечей сгорело  Две потухли  Сколько свечей осталось?";
        questions[5] = "Два умножить на два?";
        questions[6] = "Три умножить на три?";

        return (questions);
    }

    static int[] GetAnswers(int countQuestions)
    {
        int[] answers = new int[countQuestions];
        answers[0] = 6;
        answers[1] = 9;
        answers[2] = 25;
        answers[3] = 60;
        answers[4] = 2;
        answers[5] = 4;
        answers[6] = 9;
        return (answers);
    }




    static string CalculateDiagnose(double countQuestions, int countRightAnswers)
    {

        var numbersOfDiagnoses = 6;

        string[] diagnoses = new string[numbersOfDiagnoses];
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



    static string[] RandomizeQuestions(string[] questions, int[] arrayCountQuestions, int countQuestions)
    {


        string[] randomQuestions = new string[countQuestions];

        for (int i = 0; i < countQuestions; i++)
        {


            randomQuestions[i] = questions[arrayCountQuestions[i] - 1];

        }


        return (randomQuestions);
    }


    static int[] RandomizeAnswers(int[] answers, int[] arrayCountQuestions, int countQuestions)
    {


        int[] randomAnswers = new int[countQuestions];

        for (int i = 0; i < countQuestions; i++)
        {


            randomAnswers[i] = answers[arrayCountQuestions[i] - 1];

        }


        return (randomAnswers);
    }


}



