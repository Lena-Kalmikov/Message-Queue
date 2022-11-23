using Logic;
using System;
using System.Collections.Generic;

namespace MessageQueueConsole
{
    internal class Program
    {
        enum ActionMenu
        {
            ShowContactNames = 1,
            SendMessageToContact = 2,
            GetAndRemoveOldestMessageFromContact = 3,
            GetAndRemoveOldestMessageFromMessagesSystem = 4,
            GetNMessagesFromSystem = 5,
            GetMessagesByDateFromSystem = 6,
            SearchContactMessagesByWord = 7,
            Exit = 8
        }

        static void ShowMenu()
        {
            Console.WriteLine("=====================================================================");
            Console.WriteLine("1 - Show Contact Names");
            Console.WriteLine("2 - Send Message to Contact");
            Console.WriteLine("3 - Get And Remove Oldest Message From Contact");
            Console.WriteLine("4 - Get And Remove Oldest Message From Messages System");
            Console.WriteLine("5 - Get Desired Number Of Newest/Oldest Messages From System");
            Console.WriteLine("6 - Get Newest/Oldest Messages By Date From System");
            Console.WriteLine("7 - Search Contact's Messages By Word");
            Console.WriteLine("8 - Exit");
            Console.WriteLine("=====================================================================");
        }

        static void Main(string[] args)
        {
            ActionMenu choice = ActionMenu.ShowContactNames;
            Manager manager = new Manager();

            //<< ------------------Sets initial data------------------>>//
            manager.SendMessageToContact("Alon", "hello luno");
            manager.SendMessageToContact("Aviv", "hello luno the cat");
            manager.SendMessageToContact("Guy", "Good Morning");
            manager.SendMessageToContact("Ben", "whats up?");
            manager.SendMessageToContact("Alon", "hello tigo");
            manager.SendMessageToContact("Lena", "bye boo you");
            manager.SendMessageToContact("Tal", "hello boo");
            manager.SendMessageToContact("Lena", "dear foo");


            while (choice != ActionMenu.Exit)
            {
                ShowMenu();
                choice = (ActionMenu)Enum.Parse(typeof(ActionMenu), Console.ReadLine());

                switch (choice)
                {
                    case ActionMenu.ShowContactNames: //1

                        try
                        {
                            List<string> listOfContacts = manager.ShowContactNames();

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Contacts are:");

                            foreach (string contact in listOfContacts)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine(contact);
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        break;

                    case ActionMenu.SendMessageToContact: //2

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Enter a Contact name: ");
                        string contactName = Console.ReadLine();

                        Console.Write("Enter your Message: ");
                        string messageText = Console.ReadLine();

                        manager.SendMessageToContact(contactName, messageText);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Message Sent!");
                        Console.ForegroundColor = ConsoleColor.Gray;

                        break;

                    case ActionMenu.GetAndRemoveOldestMessageFromContact: //3

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Enter a Contact name: ");
                        string name = Console.ReadLine();

                        try
                        {
                            Console.Write(manager.GetAndRemoveOldestMessageFromContactQueue(name));

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n^This message has been deleted successfully^");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        break;

                    case ActionMenu.GetAndRemoveOldestMessageFromMessagesSystem: //4

                        try
                        {
                            Console.WriteLine(manager.GetAndRemoveOldestMessageFromMessagesSystem());

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\t^This message has been deleted successfully^");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        break;

                    case ActionMenu.GetNMessagesFromSystem: //5

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("How many messages do you want to see? ");

                        bool isuserinputOk;
                        int userinput;

                        do
                        {
                            isuserinputOk = int.TryParse(Console.ReadLine(), out int userNum);
                            userinput = userNum;

                            Console.ForegroundColor = ConsoleColor.Red;

                            if (!isuserinputOk)
                                Console.Write("Oops, this is not valid, enter a positive number: ");
                        }
                        while (!isuserinputOk);


                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("- If you want to see the newset messages, press any letter" +
                                        "\n- If you want to see the oldest messages, press 'o'");
                        Console.ForegroundColor = ConsoleColor.Gray;


                        bool isNew = true;

                        string userAnswer = Console.ReadLine();

                        if (userAnswer == "o")
                            isNew = false;

                        try
                        {
                            List<Message> kMessages = manager.GetNMessagesFromSystem(userinput, isNew);

                            foreach (Message message in kMessages)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("\t<----------------------->");
                                Console.ForegroundColor = ConsoleColor.Gray;

                                Console.WriteLine(message);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        break;

                    case ActionMenu.GetMessagesByDateFromSystem: //6

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Please enter a date in the following format - month/day/year h:mm:ss tt (time is not mandatory): ");

                        DateTime dateTime;
                        bool isDateTimeOk;

                        do
                        {
                            isDateTimeOk = DateTime.TryParse(Console.ReadLine(), out DateTime userDateTime);
                            dateTime = userDateTime;
                            Console.ForegroundColor = ConsoleColor.Red;

                            if (!isDateTimeOk)
                                Console.WriteLine("Oops, this is not valid, try again");
                        }
                        while (!isDateTimeOk);

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("- If you want to see messages newer than this date, press any letter" +
                                        "\n- If you want to see messages older than this date, press 'o'");

                        bool isMessageNew = true;

                        string answer = Console.ReadLine();

                        if (answer == "o")
                            isMessageNew = false;

                        try
                        {
                            List<Message> messagesByDate = manager.GetMessagesByDateFromSystem(dateTime, isMessageNew);

                            foreach (Message message in messagesByDate)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("\t<----------------------->");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine(message);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        break;

                    case ActionMenu.SearchContactMessagesByWord: //7

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Enter a Contact name: ");
                        string userInputContactName = Console.ReadLine();

                        Console.Write("Enter a Word For search: ");
                        string userInputSearchWord = Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.Gray;

                        try
                        {
                            List<Message> messagesFound = manager.SearchContactMessagesByWord(userInputContactName, userInputSearchWord);

                            foreach (Message message in messagesFound)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("\t<----------------------->");
                                Console.ForegroundColor = ConsoleColor.Gray;

                                Console.WriteLine(message);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                        break;

                    case ActionMenu.Exit:
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
