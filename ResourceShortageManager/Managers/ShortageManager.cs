using System;
using System.Collections.Generic;
using System.Linq;

using ResourceShortageManager.Models;
using ResourceShortageManager.Services;

namespace ResourceShortageManager.Managers;

public class ShortageManager
{
    private ShortageService shortageService = new ShortageService();
    private List<Shortage> shortages;
    private string currentUser;

    public ShortageManager(string username)
    {
        currentUser = username;
        shortages = shortageService.LoadShortages();
    }

    public void RegisterShortage()
    {
        var title = Utils.PromptUser("Enter title");
        if (title == null) return;

        bool roomInput = Utils.PromptUserEnum(
            "Enter room (Meeting room / Kitchen / Bathroom)",
            out Room room
        );
        if (roomInput == false) return;

        bool categoryInput = Utils.PromptUserEnum(
            "Enter category (Electronics / Food / Other)",
            out Category category
        );
        if (categoryInput == false) return;

        var priority = Utils.PromptUserInt("Enter priority");
        if (priority == null) return;

        var existingShortage = shortages.FirstOrDefault(s => s.Title == title && s.Name == currentUser);
        if (existingShortage != null)
        {
            if (priority > existingShortage.Priority)
            {
                Console.WriteLine("Priority has been increased. Do you want to update the priority? (Y/N)");
                var response = Console.ReadLine();
                if (response == "Y" || response == "y")
                {
                    Console.WriteLine("Overwriting existing shortage...");
                    existingShortage.Priority = (int)priority;
                }
                else
                {
                    Console.WriteLine("Shortage already exists with equal or higher priority.");
                    return;
                }
            }
        }
        else
        {
            var newShortage = new Shortage()
            {
                Title = title,
                Name = currentUser,
                Room = room,
                Category = category,
                Priority = (int)priority,
                CreatedOn = DateTime.Now
            };
            shortages.Add(newShortage);
        }

        shortageService.SaveShortages(shortages);
        Console.WriteLine("Shortage registered successfully.");
    }

    public void DeleteShortage()
    {
        var title = Utils.PromptUser("Enter title");
        if (title == null) return;

        bool roomInput = Utils.PromptUserEnum(
            "Enter room (Meeting room / Kitchen / Bathroom)",
            out Room room
        );
        if (roomInput == false) return;

        bool categoryInput = Utils.PromptUserEnum(
            "Enter category (Electronics / Food / Other)",
            out Category category
        );
        if (categoryInput == false) return;

        var shortage = shortages.FirstOrDefault
        (
            s => s.Title == title &&
            s.Name == currentUser &&
            Enum.Equals(s.Room, room) &&
            Enum.Equals(s.Category, category)
        );

        if (shortage == null)
        {
            Console.WriteLine("Shortage not found.");
            return;
        }

        Console.WriteLine(shortage.ToString());
        Console.WriteLine("Are you sure you want to delete this shortage? (Y/N)");
        var response = Console.ReadLine();
        if (response != "Y" && response != "y")
        {
            Console.WriteLine("Shortage deletion cancelled.");
            return;
        }

        if (shortage.Name == currentUser || currentUser == "admin")
        {
            shortages.Remove(shortage);
            shortageService.SaveShortages(shortages);
            Console.WriteLine("Shortage deleted successfully.");
        }
        else
        {
            Console.WriteLine("You do not have permission to delete this shortage.");
        }
    }

    public void ListShortages()
    {
        string? titleFilter = null;
        DateTime? startDate = null;
        DateTime? endDate = null;
        bool categoryInput = false;
        Category categoryFilter = default;
        bool roomInput = false;
        Room roomFilter = default;

        while (true)
        {
            Console.WriteLine("Choose a filter to apply:");
            Console.WriteLine("1. Title");
            Console.WriteLine("2. Start date");
            Console.WriteLine("3. End date");
            Console.WriteLine("4. Category");
            Console.WriteLine("5. Room");
            Console.WriteLine("6. Apply filters");
            Console.WriteLine("7. Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    titleFilter = Utils.PromptUser("Enter title");
                    break;
                case "2":
                    Console.WriteLine("Enter start date (yyyy-MM-dd)");
                    Console.WriteLine("or press Enter to skip:");
                    var startDateInput = Console.ReadLine();
                    startDate = string.IsNullOrEmpty(startDateInput) ? (DateTime?)null : DateTime.Parse(startDateInput);
                    Console.WriteLine(startDate);
                    break;
                case "3":
                    Console.WriteLine("Enter end date (yyyy-MM-dd)");
                    Console.WriteLine("or press Enter to skip:");
                    var endDateInput = Console.ReadLine();
                    endDate = string.IsNullOrEmpty(endDateInput) ? (DateTime?)null : DateTime.Parse(endDateInput);
                    break;
                case "4":
                    categoryInput = Utils.PromptUserEnum(
                        "Filter by category (Electronics / Food / Other)",
                        out categoryFilter
                    );
                    break;
                case "5":
                    roomInput = Utils.PromptUserEnum(
                        "Filter by room (Meeting room / Kitchen / Bathroom)",
                        out roomFilter
                    );
                    break;
                case "6":
                    Console.Clear();
                    var filteredShortages = shortages.Where(s =>
                        (string.IsNullOrEmpty(titleFilter) || s.Title.Contains(titleFilter, StringComparison.OrdinalIgnoreCase)) &&
                        (categoryInput == false || Enum.Equals(s.Category, categoryFilter)) &&
                        (roomInput == false || Enum.Equals(s.Room, roomFilter)) &&
                        (!startDate.HasValue || s.CreatedOn >= startDate.Value) &&
                        (!endDate.HasValue || s.CreatedOn <= endDate.Value) &&
                        (s.Name == currentUser || currentUser == "admin")
                    ).OrderByDescending(s => s.Priority).ToList();

                    Console.WriteLine("| {0, -20} | {1, -10} | {2, -15} | {3, -12} | {4, -8} | {5, -20} |",
                        "Title", "Name", "Room", "Category", "Priority", "Created On");

                    foreach (var shortage in filteredShortages)
                    {
                        Console.WriteLine(shortage.ToString());
                    }
                    return;
                case "7":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
