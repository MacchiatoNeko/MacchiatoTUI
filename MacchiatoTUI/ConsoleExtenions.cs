namespace MacciatoTUI
{
    public class ConsoleExtenions
    {
        /// <summary>
        /// Retrieves the width and height of the current console window.
        /// </summary>
        /// <returns>
        /// An array containing the width and height of the console window.
        /// The first element represents the width, and the second element represents the height.
        /// </returns>
        public static int[] GetConsoleDimensions()
        {
            int[] dimensions = new int[2];
            dimensions[0] = Console.WindowWidth;
            dimensions[1] = Console.WindowHeight;
            return dimensions;
        }
        /// <summary>
        /// Returns the provided text formatted with specified foreground and background colors in ANSI escape code format.
        /// </summary>
        /// <param name="text">The text to be formatted.</param>
        /// <param name="foregroundColor">The foreground color of the text. Default is ConsoleColor.White.</param>
        /// <param name="backgroundColor">The background color behind the text. Default is ConsoleColor.Black.</param>
        /// <param name="reset">Determines if the ANSI reset code should be appended to reset the text format. Default is true.</param>
        /// <returns>The input text formatted with specified foreground and background colors.</returns>
        public static string GetColoredText(string text, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black, bool reset = true)
        {
            const string ANSI_RESET = "\u001b[0m";
            const string ANSI_FOREGROUND = "\u001b[38;5;";
            const string ANSI_BACKGROUND = "\u001b[48;5;";
            string resetCode = reset ? ANSI_RESET : "";
            string foregroundCode = $"{ANSI_FOREGROUND}{(int)foregroundColor % 256}m";
            string backgroundCode = $"{ANSI_BACKGROUND}{(int)backgroundColor % 256}m";
            string coloredText = $"{foregroundCode}{backgroundCode}{text}{resetCode}";
            return coloredText;
        }
    }
}
