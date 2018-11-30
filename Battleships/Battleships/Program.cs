using Battleships.Objects;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Battleships
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Console.WriteLine("Battleships\n");
            Type[] shipTypes = Assembly.GetAssembly(typeof(AIPlayer)).GetTypes().Where(theType => theType.IsSubclassOf(typeof(Ship))).ToArray();
            for(int i = 0; i < shipTypes.Length; ++i)
            {
                Console.Write("[");
                WriteInColor(i.ToString(), ConsoleColor.Yellow);
                Console.Write("] : ");
                WriteInColor(shipTypes[i].Name + '\n', ConsoleColor.Magenta);
            }
            Console.WriteLine("\nEnter indecies of ship types to battle.");
            Type typeOne = shipTypes[GetInputTypeIndex(1, shipTypes.Length)];
            WriteInColor("1", ConsoleColor.Yellow);
            Console.Write(": ");
            WriteInColor(typeOne.Name, ConsoleColor.Magenta);
            Type typeTwo = shipTypes[GetInputTypeIndex(2, shipTypes.Length)];
            WriteInColor("2", ConsoleColor.Yellow);
            Console.Write(": ");
            WriteInColor(typeTwo.Name, ConsoleColor.Magenta);
            using (Game1 game = new Game1(typeOne, typeTwo))
            {
                game.Run();
            }
        }

        private static int GetInputTypeIndex(int number, int indexCount)
        {
            int startTop = Console.CursorTop;
            while (true)
            {
                Console.CursorTop = startTop + 1;
                for(int i = 0; i < Console.BufferWidth; ++i)
                {
                    Console.Write(' ');
                }
                Console.CursorTop = startTop;
                Console.WriteLine();
                WriteInColor(number.ToString(), ConsoleColor.Yellow);
                Console.Write(": ");

                ConsoleColor startColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                string input = Console.ReadLine();
                Console.ForegroundColor = startColor;

                if (int.TryParse(input, out int index))
                {
                    if (index < indexCount && index >= 0)
                    {
                        Console.CursorTop = startTop + 1;
                        return index;
                    }
                }
            }
        }

        private static void WriteInColor(string message, ConsoleColor color)
        {
            ConsoleColor startColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = startColor;
        }
    }
}
