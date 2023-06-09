class Question
{

    public string Text; // это поля класса  его описание
    public int Answer;

    public Question(string text, int answer)
    {
        Text = text;
        Answer = answer;
    }

    public string Print()  // это метод  функция которая делает какие то вычисления операции с полями класса
    {
        return "Вопрос " + Text;
    }

    
    
}



