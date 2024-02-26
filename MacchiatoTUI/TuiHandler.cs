namespace MacciatoTUI
{
    /// <summary>
    /// Handles text-based user interface (TUI) rendering and management.
    /// </summary>
    public class TuiHandler
    {
        private int _height;
        private string[,] _displayMatrix;
        private string[,] _previousFrame;

        /// <summary>
        /// Gets or sets the frame rate at which the TUI updates (in frames per second). Default is 30 FPS.
        /// </summary>
        public int FrameRate { get; set; } = 30;

        /// <summary>
        /// Gets or sets a value indicating whether the TUI should continue updating.
        /// </summary>
        public bool ShouldTick { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the TuiHandler class with the specified width and height.
        /// </summary>
        /// <param name="width">The width of the TUI display.</param>
        /// <param name="height">The height of the TUI display.</param>
        public TuiHandler(int width, int height)
        {
            _height = height;
            _displayMatrix = new string[height, width];
            _previousFrame = new string[height, width];
        }

        /// <summary>
        /// Starts the TUI rendering and updating process in a separate thread.
        /// </summary>
        public void Start()
        {
            Task.Run(Tick);
        }

        private void Tick()
        {
            while (ShouldTick)
            {
                UpdateHeight();
                if (!MatricesEqual(_displayMatrix, _previousFrame))
                {
                    DrawMatrixToScreen(_displayMatrix);
                    Array.Copy(_displayMatrix, _previousFrame, _displayMatrix.Length);
                    Console.SetCursorPosition(0, _height);
                }
                Thread.Sleep(1000 / FrameRate);
            }
        }

        private void UpdateHeight()
        {
            if (_height != Console.WindowHeight)
            {
                int newHeight = Console.WindowHeight;
                int width = _displayMatrix.GetLength(1);
                string[,] newMatrix = new string[newHeight, width];

                for (int i = 0; i < Math.Min(newHeight, _height); i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (i < _height)
                        {
                            newMatrix[i, j] = _displayMatrix[i, j];
                        }
                        else
                        {
                            newMatrix[i, j] = string.Empty;
                        }
                    }
                }
                _height = newHeight;
                _displayMatrix = newMatrix;
                _previousFrame = new string[_height, width];
            }
        }

        private static bool MatricesEqual(string[,] matrix1, string[,] matrix2)
        {
            if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
                return false;

            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    if (matrix1[i, j] != matrix2[i, j])
                        return false;
                }
            }

            return true;
        }
        private static void DrawMatrixToScreen(string[,] matrix)
        {
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j]);
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Modifies the content of a cell in the display matrix.
        /// </summary>
        /// <param name="row">The row index of the cell to edit.</param>
        /// <param name="col">The column index of the cell to edit.</param>
        /// <param name="newValue">The new value to set for the cell.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the row or column index is out of range.</exception>
        public void EditMatrixCell(int row, int col, string newValue)
        {
            if (row >= 0 && row < _displayMatrix.GetLength(0) && col >= 0 && col < _displayMatrix.GetLength(1))
            {
                _displayMatrix[row, col] = newValue;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Index out of range.");
            }
        }
    }
}
