using System.Data;
using Newtonsoft.Json;

class UserStorage
{

    
    public string CalculateDiagnose(double countQuestions, int countRightAnswers)
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


    public static int GetNumber()
    {


        while (true)
        {

            try
            {

                
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


    public static string EnterNewQuestion()
    {

        while (true)
        {

            try
            {

                return Console.ReadLine(); ;



            }

            catch (FormatException)
            {

                Console.WriteLine($"Введите текст вопроса");
                Console.WriteLine();

            }

            catch (OverflowException)
            {
                Console.WriteLine();
                Console.WriteLine($"Введите текст вопроса");
            }
        }

    }


    public static int GetQuestionNumber(int questionsCount)
    {

        while (true)
        {

            try
            {
                int questionNumber = Convert.ToInt32(Console.ReadLine());
                if (questionNumber <= questionsCount && questionNumber > 0)
                {
                    return questionNumber;
                }
                else if (questionNumber == 0)
                {
                    return questionNumber;
                }
                else Console.WriteLine($"Введите номер вопроса от 1 до {questionsCount} или 0 для выхода в меню");

            }

            catch (FormatException)
            {

                Console.WriteLine($"Введите номер вопроса от 1 до {questionsCount} или 0 для выхода в меню");
                Console.WriteLine();

            }

            catch (OverflowException)
            {
                Console.WriteLine();
                Console.WriteLine($"Введите номер вопроса от 1 до {questionsCount} или 0 для выхода в меню");
            }
        }

    }

    public static Question AddNewQuestion()
    {
        

        Console.WriteLine("-----------------------------------\n введите текст вопроса который хотите добавить");

        var userQuestion = UserStorage.EnterNewQuestion();


        Console.WriteLine("-----------------------------------\n введите число - ответ на вопрос");

        var userAnswer = UserStorage.GetNumber();

        Question newQuestion = new Question(userQuestion, userAnswer);

        Console.WriteLine("-----------------------------------\n Вопрос и ответ добавлен");

        return newQuestion;

        

        

        
    }



}








