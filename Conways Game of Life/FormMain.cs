using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Conways_Game_of_Life
{
    public partial class FormMain : Form
    {
        private const int CellSize = 10; // Size of each cell in pixels
        private const int GridWidth = 80; // Width of the game grid
        private const int GridHeight = 50; // Height of the game grid

        private bool[,] grid; // The game grid
        private bool isRunning; // Flag indicating whether the game is running

        public FormMain()
        {
            this.FormClosing += Form1_FormClosing;
            InitializeComponent();
            InitializeGrid();
        }


        private void InitializeGrid()
        {
            grid = new bool[GridWidth, GridHeight];
            Random random = new Random();

            for (int x = 0; x < GridWidth; x++)
            {
                for (int y = 0; y < GridHeight; y++)
                {
                    grid[x, y] = random.Next(2) == 0; // Randomly set the cell to alive or dead
                }
            }
        }
        private void Main1_Load(object sender, EventArgs e)
        {
            isRunning = true;
            ThreadPool.QueueUserWorkItem(GameLoop);
        }

        private void Main1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int x = 0; x < GridWidth; x++)
            {
                for (int y = 0; y < GridHeight; y++)
                {
                    Brush brush = grid[x, y] ? Brushes.Black : Brushes.White;
                    g.FillRectangle(brush, x * CellSize, y * CellSize, CellSize, CellSize);
                }
            }
        }

        private void GameLoop(object state)
        {
            while (isRunning)
            {
                CalculateNextGeneration();
                Invalidate(); // Redraw the form
                Thread.Sleep(200); // Pause for a short time between generations
            }
        }

        private void CalculateNextGeneration()
        {
            bool[,] nextGeneration = new bool[GridWidth, GridHeight];

            for (int x = 0; x < GridWidth; x++)
            {
                for (int y = 0; y < GridHeight; y++)
                {
                    int liveNeighbors = CountLiveNeighbors(x, y);

                    if (grid[x, y])
                    {
                        // Cell is alive
                        if (liveNeighbors == 2 || liveNeighbors == 3)
                        {
                            // Cell stays alive
                            nextGeneration[x, y] = true;
                        }
                        else
                        {
                            // Cell dies due to underpopulation or overpopulation
                            nextGeneration[x, y] = false;
                        }
                    }
                    else
                    {
                        // Cell is dead
                        if (liveNeighbors == 3)
                        {
                            // Cell becomes alive due to reproduction
                            nextGeneration[x, y] = true;
                        }
                        else
                        {
                            // Cell stays dead
                            nextGeneration[x, y] = false;
                        }
                    }
                }
            }

            grid = nextGeneration;
        }

        private int CountLiveNeighbors(int x, int y)
        {
            int liveNeighbors = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    int neighborX = x + i;
                    int neighborY = y + j;

                    if (neighborX >= 0 && neighborX < GridWidth && neighborY >= 0 && neighborY < GridHeight)
                    {
                        if (grid[neighborX, neighborY])
                        {
                            liveNeighbors++;
                        }
                    }
                }
            }

            return liveNeighbors;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRunning = false;
        }
    }
}
