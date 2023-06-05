using System;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Readwriteusertofile
{
    class Program
    {
        static string DBFilePAth { get; set; }

        static void Main(string[] args)
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

            string allCommands = "--------------\n0 - вывести всех\n1 - Добавить нового\n2 - удалить\n3 - выход\n4 --------------";
            Console.WriteLine(allCommands);
            string inputCommandStr = Console.ReadLine();

            int inputCommand = GetIntFromString(inputCommandStr);

            switch(inputCommand)
            {
                case 0:
                    {
                        var allUsers = ReadAllFromDB();
                        if (allUsers.Count == 0) Console.WriteLine("пока никого нет");
                        foreach (var user in allUsers) Console.WriteLine(user);
                        break;

                    }
                case 1:
                    {
                        Console.WriteLine("Введите имя");
                        string name = Console.ReadLine();

                        Console.WriteLine("Введите фамилию");
                        string surname = Console.ReadLine();

                        User newUser = new User(0, name, surname);
                        SaveToDB(newUser);
                        Console.WriteLine("успешно");
                        break;
                    }

                case 2:
                    {
                        Console.WriteLine("введите ID");
                        string idStr = Console.ReadLine();
                        int id = GetIntFromString(idStr);
                        if (id == 0) Console.WriteLine("нет такого id");
                        else
                        {
                            bool result = DeleteFromDB(id);
                            if (result) Console.WriteLine("успешно");
                            else Console.WriteLine("Ошибка");
                        }


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



        static int GetIntFromString(string inputStr)
        {
            int input = 0;

            try
            {
                input = int.Parse(inputStr);
            }

            catch (FormatException)
            {
                Console.WriteLine("Нет Такой команды");
            }
            return input;
        }

        static void SaveToDB(User user)
        {
            List<User> AllCurrentUsers = ReadAllFromDB();
            int lastId = AllCurrentUsers.Count == 0 ? 0 : AllCurrentUsers.Last().Id;
            //user.SetId(lastId + 1);
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


        static bool DeleteFromDB(int id)
        {
            List<User> allCurrentUsers = ReadAllFromDB();
            User userForDeletion = allCurrentUsers.FirstOrDefault(u => u.Id == id);
            bool result = false;

            if (userForDeletion != null)
            {
                allCurrentUsers.Remove(userForDeletion);
                result = true;
            }
            SaveToDB(allCurrentUsers);

            return result;
        }

        static List<User> ReadAllFromDB()
        {

            string json = File.ReadAllText(DBFilePAth);
            List<User> currentUsers = JsonConvert.DeserializeObject<List<User>>(json);

            return currentUsers ?? new List<User>(); 

        }

        



    }

    class User // класс Юзер
    {
        public string Name {get; private set;} // свойства класса 
        public string Surname {get; private set;}
        public int Id { get; private set; }

        public User(int id, string name, string surname) // конструктор который принимает свойства
        {
            Id = id;
            Name = name;
            Surname = surname;
            
        }

       

        public void SetNewId(int id)
        {
            Id = id;
        }


        public override string ToString()
        {
             return $"{Id} {Name} {Surname}";
        }

       


    }

    }
