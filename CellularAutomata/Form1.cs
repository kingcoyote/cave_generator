using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/**
 * 1 - Generate all water grid
 * 2 - Execute the rule sets in sequence
 * 3 - Define connected components
 * 4 - Identify largest contiguous component and mark it as the main region
 * 5 - Attach any other components that are close enough
 * 6 - Delete all remaining components
 * 7 - Calculate water percentage
 * 8 - Accept or reject final map
 */

namespace CellularAutomata
{
    public partial class Form1 : Form
    {
        private Color[,] _grid = new Color[128,128];
        private Random _rng;
        private Timer _timer;
        private List<CaveRules> _rules;
        private int _currentRule;
        private int _currentPass;
        
        public Form1()
        {
            InitializeComponent();

            _rng = new Random((int)DateTime.Now.Ticks);
            _timer = new Timer();
            _timer.Enabled = false;
            _timer.Tick += ContinueAutomata;
            
            _rules = new List<CaveRules>(2);
            _rules.Add(new CaveRules { GridSize = 32, Iterations = 3, NeighborhoodSize = 1, NeighborhoodThreshold = 3, Initialization = 0.6f });
            _rules.Add(new CaveRules { GridSize = 256, Iterations = 4, NeighborhoodSize = 2, NeighborhoodThreshold = 10, Initialization = 0.5f });
        }

        private void StartAutomata(object sender, EventArgs e)
        {
            StartAutomata();
            _timer.Interval = 1000 / (int) numSpeed.Value;
            _timer.Enabled = true;
        }
        
        private void StartAutomata()
        {
            _grid = new Color[_rules[0].GridSize, _rules[0].GridSize];
            _currentRule = 0;
            _currentPass = 0;

            for (var i = 0; i < _rules[0].GridSize; i++)
            {
                for (var j = 0; j < _rules[0].GridSize; j++)
                {
                    _grid[i, j] = Color.Aquamarine;
                }
            }

            InitializeGrid(_rules[0].GridSize, _rules[0].Initialization);

            picOutput.Refresh();
        }

        private void InitializeGrid(int gridSize, float threshold)
        {
            for (var i = 0; i < gridSize; i++)
            {
                for (var j = 0; j < gridSize; j++)
                {
                    if (_grid[i, j] == Color.SaddleBrown) continue;

                    _grid[i, j] = _rng.NextDouble() < threshold ? Color.SaddleBrown : Color.Aquamarine;
                }
            }
        }

        private void ContinueAutomata(object sender, EventArgs e)
        {
            ContinueAutomata();
        }

        private void ContinueAutomata()
        {
            if (_currentRule == _rules.Count - 1 && _currentPass >= _rules[_currentRule].Iterations) return;
            if (_currentPass == _rules[_currentRule].Iterations)
            {
                _currentRule++;
                _currentPass = 0;
                var newGrid = new Color[_rules[_currentRule].GridSize, _rules[_currentRule].GridSize];
                var mul = (float)(_rules[_currentRule - 1].GridSize)/(float)(_rules[_currentRule].GridSize);
                for(var i = 0; i < _rules[_currentRule].GridSize; i++)
                {
                    for (var j = 0; j < _rules[_currentRule].GridSize; j++)
                    {
                        newGrid[i, j] = _grid[(int) (i*mul), (int) (j*mul)];
                    }
                }
                
                _grid = newGrid;

                InitializeGrid(_rules[_currentRule].GridSize, _rules[_currentRule].Initialization);
            }

            _grid = GenerateGrid(_grid, _rules[_currentRule].GridSize, _rules[_currentRule].NeighborhoodSize,
                                 _rules[_currentRule].NeighborhoodThreshold);

            _currentPass++;

            if (_currentRule == _rules.Count - 1 && _currentPass >= _rules[_currentRule].Iterations)
            {
                DefineConnectedComponents();
            };
            
            picOutput.Refresh();
        }

        private Color[,] GenerateGrid(Color[,] oldGrid, int gridSize, int size, float threshold)
        {
            var newGrid = new Color[gridSize, gridSize];
            for (var i = 0; i < gridSize; i++)
            {
                for (var j = 0; j < gridSize; j++)
                {
                    var neighbors = 0;

                    for (var k = -size; k <= size; k++)
                    {
                        for (var l = -size; l <= size; l++)
                        {
                            if (i + k < 0 || i + k >= gridSize || j + l < 0 || j + l >= gridSize || (k == 0 && l == 0)) continue;
                            if (_grid[i + k, j + l] == Color.Aquamarine) neighbors++;
                        }
                    }

                    newGrid[i, j] = neighbors > threshold ? Color.Aquamarine : Color.SaddleBrown;
                }
            }
            return newGrid;
        }

        private void DefineConnectedComponents()
        {
            Console.WriteLine("defining connected components");
        }

        private void DrawBox(object sender, PaintEventArgs e)
        {
            var cellSize = (float)(picOutput.Width)/_rules[_currentRule].GridSize;
            for (var i = 0; i < _rules[_currentRule].GridSize; i++)
            {
                for (var j = 0; j < _rules[_currentRule].GridSize; j++)
                {
                    var p = new SolidBrush(_grid[i, j]);
                    e.Graphics.FillRectangle(p, i*cellSize, j*cellSize, cellSize, cellSize);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _timer.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartAutomata();
        }

        private struct CaveRules
        {
            public int GridSize;
            public int NeighborhoodSize;
            public int NeighborhoodThreshold;
            public int Iterations;
            public float Initialization;
        }
    }
}
