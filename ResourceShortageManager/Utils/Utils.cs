using System;

namespace ResourceShortageManager
{
    public static class Utils
    {
        public static string? PromptUser(string message)
        {
            string? input;
            do
            {
                Console.WriteLine(message + " (or type 'exit' to cancel):");
                input = Console.ReadLine();
                if (input?.ToLower() == "exit") return null;
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }

        public static int? PromptUserInt(string message)
        {
            int result;
            string? input;

            do
            {
                Console.WriteLine(message + " (or type 'exit' to cancel):");
                input = Console.ReadLine();
                if (input?.ToLower() == "exit") return null;
            } while (!int.TryParse(input, out result));

            return result;
        }

        // public static object PromptUserEnum<T>(string message, out T result) where T : struct
        // {
        //     string? input;

        //     do
        //     {
        //         Console.WriteLine(message + " (or type 'exit' to cancel):");
        //         input = Console.ReadLine().Replace(" ", "").ToLower();
        //         if (input?.ToLower() == "exit")
        //         {
        //             result = default;
        //             return null;
        //         }
        //     } while (!Enum.TryParse(input, ignoreCase: true, out result));

        //     return result;
        // }

        public static bool PromptUserEnum<T>(string message, out T result) where T : struct
        {
            string? input;

            do
            {
                Console.WriteLine(message + " (or type 'exit' to cancel):");
                input = Console.ReadLine()!.Replace(" ", "").ToLower();
                if (input?.ToLower() == "exit")
                {
                    result = default;
                    return false;
                }
            } while (!Enum.TryParse(input, ignoreCase: true, out result));

            return true;
        }

    }
}
