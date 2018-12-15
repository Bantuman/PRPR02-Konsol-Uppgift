using Battleships.Objects;
using System;
using System.Linq;
using System.Reflection;

namespace Battleships
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        private static Ship winner;
        private static Ship loser;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            while(true)
            {
                Objects.Object.Collider.Reset();
                Console.WriteLine("Battleships\n");
                Type[] shipTypes = Assembly.GetAssembly(typeof(AIPlayer)).GetTypes().Where(theType => theType.IsSubclassOf(typeof(Ship))).ToArray();
                for (int i = 0; i < shipTypes.Length; ++i)
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

                Console.WriteLine();

                float number = -1;
                int startTop = Console.CursorTop;
                while (true)
                {
                    Console.CursorTop = startTop + 1;
                    for (int i = 0; i < Console.BufferWidth; ++i)
                    {
                        Console.Write(' ');
                    }
                    Console.CursorTop = startTop;
                    Console.WriteLine();

                    Console.Write("Enter length of round (seconds): ");

                    ConsoleColor startColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;
                    string input = Console.ReadLine();
                    Console.ForegroundColor = startColor;

                    if (float.TryParse(input, out float value))
                    {
                        if (value > 0)
                        {
                            number = value;
                            break;
                        }
                    }
                }

                using (Game1 game = new Game1(typeOne, typeTwo, (Ship winner, Ship loser) => { Program.winner = winner; Program.loser = loser; }, number))
                {
                    game.Run();
                }
                if (winner == null)
                {
                    Console.WriteLine($"\nTie!\n");
                }
                else
                {
                    Console.WriteLine($"\n{winner.GetType().Name} wins!\n");

                    for (int i = 0; i < 2; ++i)
                    {
                        Ship ship = (i == 0) ? winner : loser;

                        Console.WriteLine($"{((i == 0) ? "Winner" : "Loser")} statistics:");
                        Console.Write("Type: ");
                        WriteInColor($"{ship.GetType().Name}\n", ConsoleColor.Magenta);
                        Console.Write("HP left: ");
                        WriteInColor($"{ (ship.Health / ship.MaxHealth) * 100 }%\n", ConsoleColor.Yellow);

                        Console.Write("Shots fired: ");
                        WriteInColor($"{ ship.ShotsFired }\n", ConsoleColor.Yellow);
                        Console.Write("Shots hit: ");
                        WriteInColor($"{ (ship.ShotsFired == 0 ? 100 : ((float)ship.ShotsHit / ship.ShotsFired) * 100) }%\n", ConsoleColor.Yellow);

                        Console.Write("Missiles fired: ");
                        WriteInColor($"{ ship.MissilesFired }\n", ConsoleColor.Yellow);
                        Console.Write("Missiles hit: ");
                        WriteInColor($"{ (ship.MissilesFired == 0 ? 100 : ((float)ship.MissilesHit / ship.MissilesFired) * 100) }%\n", ConsoleColor.Yellow);

                        Console.Write("Distance traveled: ");
                        WriteInColor($"{ Math.Round(ship.DistanceTraveled) } units\n", ConsoleColor.Yellow);
                        Console.Write("Highest velocity: ");
                        WriteInColor($"{ Math.Round(ship.HighestVelocity) } units/second\n", ConsoleColor.Yellow);

                        Console.WriteLine();
                    }
                }

                Console.WriteLine("Do you wish to restart? (Y/N)");
                char character;
                do
                {
                    character = char.ToUpper(Console.ReadKey(true).KeyChar);
                } while (character != 'Y' && character != 'N');
                Console.WriteLine(character);
                if (character == 'N')
                {
                    return;
                }
                Console.Clear();
            }
        }

        /// <summary>
        /// Gets the index for the input type.
        /// </summary>
        /// <param name="number">Ship number.</param>
        /// <param name="indexCount">Number of valid indecies.</param>
        /// <returns>Index for ship type.</returns>
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

        /// <summary>
        /// Writes string in color.
        /// </summary>
        /// <param name="message">Tring to write.</param>
        /// <param name="color">Color to write in.</param>
        private static void WriteInColor(string message, ConsoleColor color)
        {
            ConsoleColor startColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = startColor;
        }
    }
}
