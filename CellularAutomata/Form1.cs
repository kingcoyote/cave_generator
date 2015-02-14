using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Algorithms;

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
        public int ConnectingThreshold = 25;
        public int TunnelSize = 6;
        public float TunnelPersistance = 0.8f;
        public float WaterMinimum = 0.33f;

        private float _water;
        private static Color[,] _grid = new Color[128,128];
        private static int _gridSize;
        private Random _rng;
        private Timer _timer;
        private List<CaveRules> _rules;
        private int _currentRule;
        private int _currentPass;
        private QuickUnion _qu;
        private List<ContiguousRegion> _regions;
        private ContiguousRegion _mainRegion;
        private ContiguousRegion _mergableRegion;
        
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
                _gridSize = _rules[_currentRule].GridSize;
                DefineConnectedComponents();
                SortContiguousRegions();
                MergeNearbyRegions();
                _grid = GenerateGrid(_grid, _rules[_currentRule].GridSize, _rules[_currentRule].NeighborhoodSize,
                                 _rules[_currentRule].NeighborhoodThreshold);
                DefineConnectedComponents();
                SortContiguousRegions();
                RejectIsolatedRegions();
                CalculateWater();
                MarkAcceptance();
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
            var gridSize = _rules[_currentRule].GridSize;
            _qu = new QuickUnion(gridSize*gridSize);
            for (var i = 0; i < gridSize; i++)
            {
                for (var j = 0; j < gridSize; j++)
                {
                    if (_grid[i, j] == Color.SaddleBrown) continue;

                    for (var k = -1; k <= 1; k++)
                    {
                        for (var l = -1; l <= 1; l++)
                        {
                            if (i + k < 0 || i + k >= gridSize || j + l < 0 || j + l >= gridSize || (k == 0 && l == 0)) continue;
                            if (_grid[i + k, j + l] == Color.Aquamarine) _qu.Union(j*gridSize + i, (j+l) * gridSize + i+k);
                        }
                    }
                }
            }
        }

        private void SortContiguousRegions()
        {
            var gridSize = _rules[_currentRule].GridSize;
            var r = new Dictionary<int, ContiguousRegion>();

            for (var i = 0; i < gridSize * gridSize; i++)
            {
                if (r.ContainsKey(_qu.Find(i)) || _grid[i % 256, i / gridSize] == Color.SaddleBrown) continue;
                r.Add(_qu.Find(i), new ContiguousRegion() { Index = _qu.Find(i), Size = _qu.Size(i) });
            }

            var pq = new PriorityQueue<ContiguousRegion>(r.Count);

            foreach (var cr in r.Values)
            {
                pq.Insert(cr);
            }

            _mainRegion = pq.DelMin();

            _regions = new List<ContiguousRegion>();

            while (!pq.IsEmpty())
            {
                _regions.Add(pq.DelMin());
            }
        }

        private void MergeNearbyRegions()
        {
            while (MergeableRegionExists())
            {
                CombineRegions(_mainRegion, _mergableRegion);
            }
        }

        private bool MergeableRegionExists()
        {
            foreach (var region in _regions)
            {
                int pathLength = 0;
                var path = AStar.GeneratePath(
                    new PathFinder(_mainRegion.Index % _gridSize, _mainRegion.Index / _gridSize), 
                    new PathFinder(region.Index % _gridSize, region.Index / _gridSize)
                );

                foreach (PathFinder node in path)
                {
                    if (_grid[node.X, node.Y] == Color.SaddleBrown)
                    {
                        pathLength += 1;
                    }
                }

                if (pathLength < ConnectingThreshold)
                {
                    _mergableRegion = region;
                    return true;
                }
            }

            return false;
        }

        private void CombineRegions(ContiguousRegion r1, ContiguousRegion r2)
        {
            var path = AStar.GeneratePath(
                    new PathFinder(r1.Index % _gridSize, r1.Index / _gridSize),
                    new PathFinder(r2.Index % _gridSize, r2.Index / _gridSize)
                );

            foreach (PathFinder node in path)
            {
                if (_grid[node.X, node.Y] == Color.Aquamarine) continue;
                _grid[node.X, node.Y] = Color.Aquamarine;

                var i = node.X;
                var j = node.Y;

                

                for (var k = -TunnelSize; k <= TunnelSize; k++)
                {
                    for (var l = -TunnelSize; l <= TunnelSize; l++)
                    {
                        if (i + k < 0 || i + k >= _gridSize || j + l < 0 || j + l >= _gridSize || (k == 0 && l == 0)) continue;

                         _grid[i + k, j + l] = _rng.NextDouble() < TunnelPersistance ? Color.Aquamarine : Color.Brown;
                    }
                }
            }

            _qu.Union(r1.Index, r2.Index);
            _regions.Remove(r2);

            picOutput.Refresh();
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

        private void RejectIsolatedRegions()
        {
            for (var i = 0; i < _gridSize; i++)
            {
                for (var j = 0; j < _gridSize; j++)
                {
                    if (_grid[i, j] == Color.SaddleBrown) continue;

                    if (_qu.Find(i + j * _gridSize) != _mainRegion.Index) _grid[i, j] = Color.SaddleBrown;
                }
            }

        }

        private void CalculateWater()
        {
            var water = 0;
            for(var i = 0; i < _gridSize; i++)
            {
                for (var j = 0; j < _gridSize; j++)
                {
                    if (_grid[i, j] == Color.Aquamarine) water++;
                }
            }

            _water = water/(float) (_gridSize*_gridSize);
        }

        private void MarkAcceptance()
        {
            txtAccept.Text = (_water > WaterMinimum ? "Accepted!" : "Rejected!") + string.Format(" ({0}%)", _water * 100);
        }

        private class ContiguousRegion : IComparable<ContiguousRegion>
        {
            public int Index;
            public int Size;

            public int CompareTo(ContiguousRegion other)
            {
                return other.Size - Size;
            }
        }

        private class CaveRules
        {
            public int GridSize;
            public int NeighborhoodSize;
            public int NeighborhoodThreshold;
            public int Iterations;
            public float Initialization;
        }

        public class PathFinder : IAStarNode
        {
            public readonly int X;
            public readonly int Y;

            public PathFinder(int i, int j)
            {
                X = i;
                Y = j;
            }

            public override int GetHashCode()
            {
                return Y*_gridSize + X;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is PathFinder)) return false;
                return ((PathFinder) obj).X == X && ((PathFinder) obj).Y == Y;
            }

            public List<IAStarNode> GetNeighbors(){
                var neighbors = new List<IAStarNode>();


                for (var k = -1; k <= 1; k++)
                {
                    for (var l = -1; l <= 1; l++)
                    {
                        if (X + k < 0 || X + k >= _gridSize || Y + l < 0 || Y + l >= _gridSize || (k == 0 && l == 0)) continue;
                        neighbors.Add(new PathFinder(X + k, Y + l));
                    }
                }

                return neighbors;
            }

            public double EstimateDistance(IAStarNode end)
            {
                var pf = (PathFinder) end;

                return Math.Abs(pf.X - X) + Math.Abs(pf.Y - Y);
            }

            public double Distance(IAStarNode destination)
            {
                var pf = (PathFinder) destination;

                return _grid[pf.X, pf.Y] == Color.SaddleBrown ? 10 : 1;
            }

            public override string ToString()
            {
                return String.Format("{0}, {1}", X, Y);
            }
        }
    }
}
