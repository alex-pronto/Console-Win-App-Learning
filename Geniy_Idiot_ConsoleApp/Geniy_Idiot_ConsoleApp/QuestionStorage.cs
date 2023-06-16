class QuestionStorage
{
    public List<Question> Questions;
    
   


    public List<Question> GetQuestions()

    {
        var questions = new List<Question>();

        questions.Add(new Question("Сколько будет два плюс два умноженное на два?", 6));
        questions.Add(new Question("Бревно нужно распилить на 10 частей, Сколько нужно сделать распилов?", 9));
        questions.Add(new Question("На двух руках 10 пальцев  (Сколько пальцев на 5 руках?)", 25));
        questions.Add(new Question("Укол делают каждые пол часа  Сколько нужно минут для 3 уколов?", 60));
        questions.Add(new Question("Пять свечей сгорело  Две потухли  Сколько свечей осталось?", 2));

        return questions;
    }


}






