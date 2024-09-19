using System;
using System.Collections.Generic;
using System.Linq;

using ResourceShortageManager.Models;
using ResourceShortageManager.Services;
using ResourceShortageManager.Managers;

namespace ResourceShortageManager;

class Program
{
    static string? currentUser;

    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("Welcome to the Resource Shortage Manager");
        currentUser = Utils.PromptUser("Enter your username");

        if (currentUser == null)
        {
            Console.WriteLine("Exiting...");
            return;
        }

        ShortageManager shortageManager = new ShortageManager(currentUser);

        string command = string.Empty;
        while (command != "exit" || command != "q" || command != "quit")
        {
            Console.WriteLine("Enter command: register, delete, list or exit");
            command = Console.ReadLine()!;

            switch (command)
            {
                case "register":
                    Console.Clear();
                    shortageManager.RegisterShortage();
                    break;
                case "delete":
                    Console.Clear();
                    shortageManager.DeleteShortage();
                    break;
                case "list":
                    Console.Clear();
                    shortageManager.ListShortages();
                    break;
                case "exit":
                case "q":
                case "quit":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid command.");
                    break;
            }
        }
    }
}
