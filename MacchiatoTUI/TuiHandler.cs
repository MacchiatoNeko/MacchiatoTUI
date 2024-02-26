namespace MacciatoTUI
{
    /// <summary>
    /// Handles text-based user interface (TUI) rendering and management.
    /// </summary>
    public class TuiHandler
    {
        private int height;
        private int width;
        public int FrameRate { get; set; } = 30; // Frame rate for TUI rendering.
        public bool ShouldTick { get; set; } = true; // Indicates whether TUI rendering should continue.

        private string[] offScreenBuffer; // Buffer to store TUI content.
        private string[] previousFrame; // Buffer to store the previous frame for comparison.

        /// <summary>
        /// Starts the TUI rendering process in a separate task.
        /// </summary>
        public void Start()
        {
            height = Console.WindowHeight;
            width = Console.WindowWidth;

            offScreenBuffer = new string[height]; // Initialize off-screen buffer with current window height.
            previousFrame = new string[height]; // Initialize previous frame buffer with current window height.

            Task.Run(Tick); // Start rendering in a separate task.
        }

        /// <summary>
        /// Edits a specific line in the off-screen buffer.
        /// </summary>
        /// <param name="lineIndex">Index of the line to be edited.</param>
        /// <param name="newContent">New content for the line.</param>
        public void EditBufferLine(int lineIndex, string newContent)
        {
            if (lineIndex >= 0 && lineIndex < offScreenBuffer.Length)
            {
                offScreenBuffer[lineIndex] = newContent; // Update the content of the specified line.
            }
            else if (lineIndex >= offScreenBuffer.Length)
            {
                Array.Resize(ref offScreenBuffer, lineIndex + 1); // Resize the buffer if the line index is out of bounds.
                offScreenBuffer[lineIndex] = newContent; // Add the new content to the specified line.
            }
        }

        private void Tick() // Main rendering loop.
        {
            while (ShouldTick)
            {
                // Check if the window size has changed.
                if (height != Console.WindowHeight || width != Console.WindowWidth)
                {
                    height = Console.WindowHeight;
                    width = Console.WindowWidth;
                    string[] newBuffer = new string[height]; // Resize the buffer to match the new window height.
                    Array.Copy(offScreenBuffer, newBuffer, Math.Min(offScreenBuffer.Length, height));
                    offScreenBuffer = newBuffer;
                    previousFrame = new string[height]; // Reset the previous frame buffer.
                }

                // Check if the current frame is different from the previous frame.
                if (!FramesEqual(offScreenBuffer, previousFrame))
                {
                    DrawBufferToScreen(offScreenBuffer); // Draw the buffer content to the console screen.
                    Array.Copy(offScreenBuffer, previousFrame, offScreenBuffer.Length); // Update the previous frame buffer with the current frame.
                }
                Thread.Sleep(1000 / FrameRate); // Pause briefly to control frame rate.
            }
        }

        private bool FramesEqual(string[] frame1, string[] frame2) // Checks if two frames are equal.
        {
            if (frame1.Length != frame2.Length)
                return false;

            for (int i = 0; i < frame1.Length; i++)
            {
                if (frame1[i] != frame2[i])
                    return false;
            }

            return true;
        }

        private void DrawBufferToScreen(string[] buffer) // Draws the buffer content to the console screen.
        {
            Console.SetCursorPosition(0, 0);

            foreach (string line in buffer)
            {
                Console.WriteLine(line);
            }
        }
    }
}
