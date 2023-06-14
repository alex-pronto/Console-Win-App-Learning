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


    public int GetUserAnswer(int i, int randomQuestionIndex, List<Question> questions)
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




}








