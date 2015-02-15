using System;
using System.Collections.Generic;
using System.Drawing;
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
        private float WaterMaximum = 0.40f;
        private int _finalIterations = 1;
        private int _finalNeighborhoodSize = 2;
        private int _finalNeighborhoodThreshold = 8;

        private float _water;
        private static Color[,] _grid;
        private static int _gridSize;
        private Random _rng;
        private List<CaveRules> _rules;
        private QuickUnion _qu;
        private List<ContiguousRegion> _regions;
        private ContiguousRegion _mainRegion;
        private ContiguousRegion _mergableRegion;

        private static Color _wallColor = Color.SaddleBrown;
        private static Color _airColor = Color.Aquamarine;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvRules.Rows.Add(new object[] { "32", "3", "1", "4", "60" });
            dgvRules.Rows.Add(new object[] { "256", "4", "2", "10", "50" });
        }

        private void StartAutomata(object sender, EventArgs e)
        {
            GenerateCave();
        }
        
        private void GenerateCave()
        {
            button1.Enabled = false;
            txtAccept.Text = "...";

            try
            {
                // initialize all rules
                _rng = new Random((int) DateTime.Now.Ticks);

                _rules = new List<CaveRules>(2);

                for (var i = 0; i < dgvRules.Rows.Count - 1; i++)
                {
                    _rules.Add(new CaveRules
                                   {
                                       GridSize = int.Parse(dgvRules[0, i].Value.ToString()),
                                       Iterations = int.Parse(dgvRules[1, i].Value.ToString()),
                                       NeighborhoodSize = int.Parse(dgvRules[2, i].Value.ToString()),
                                       NeighborhoodThreshold = int.Parse(dgvRules[3, i].Value.ToString()),
                                       Initialization = float.Parse(dgvRules[4, i].Value.ToString())/100
                                   });
                }

                ConnectingThreshold = (int) numTunnelDistance.Value;
                TunnelSize = (int) numTunnelWidth.Value;
                TunnelPersistance = (int) numTunnelClearance.Value/100.0f;
                WaterMinimum = (int) minimumOpen.Value/100.0f;
                WaterMaximum = (int) maximumOpen.Value/100.0f;
                _finalIterations = (int) numFinalIterations.Value;
                _finalNeighborhoodSize = (int) numFinalSize.Value;
                _finalNeighborhoodThreshold = (int) numLocalThreshold.Value;

                // initialize the grid
                _gridSize = _rules[0].GridSize;
                _grid = new Color[_gridSize,_gridSize];
                for (var i = 0; i < _rules[0].GridSize; i++)
                {
                    for (var j = 0; j < _rules[0].GridSize; j++)
                    {
                        _grid[i, j] = _airColor;
                    }
                }

                // loop through the rules and passes
                foreach (var rule in _rules)
                {
                    var mul = (float) (_gridSize)/(rule.GridSize);
                    _gridSize = rule.GridSize;
                    var newGrid = new Color[_gridSize,_gridSize];
                    for (var i = 0; i < rule.GridSize; i++)
                    {
                        for (var j = 0; j < rule.GridSize; j++)
                        {
                            newGrid[i, j] = _grid[(int) (i*mul), (int) (j*mul)];
                        }
                    }

                    _grid = newGrid;

                    InitializeGrid(rule.GridSize, rule.Initialization);

                    picOutput.Refresh();

                    // run the CA rules for the set number of times
                    for (var i = 0; i < rule.Iterations; i++)
                    {
                        _grid = RunCellularAutomata(_grid, rule.GridSize, rule.NeighborhoodSize,
                                                    rule.NeighborhoodThreshold);

                        picOutput.Refresh();
                    }
                }

                DefineConnectedComponents();
                SortContiguousRegions();
                MergeNearbyRegions();
                for (var i = 0; i < _finalIterations; i++)
                {
                    _grid = RunCellularAutomata(_grid, _gridSize, _finalNeighborhoodSize, _finalNeighborhoodThreshold);
                    picOutput.Refresh();
                }
                DefineConnectedComponents();
                SortContiguousRegions();
                RejectIsolatedRegions();
                CalculateWater();
                MarkAcceptance();

                picOutput.Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("An error occured while generating this cave. Try adjusting the rules.");
            }

            button1.Enabled = true;
        }

        private void InitializeGrid(int gridSize, float threshold)
        {
            for (var i = 0; i < gridSize; i++)
            {
                for (var j = 0; j < gridSize; j++)
                {
                    if (_grid[i, j] == _wallColor) continue;

                    _grid[i, j] = _rng.NextDouble() < threshold ? _airColor : _wallColor;
                }
            }
        }

        private Color[,] RunCellularAutomata(Color[,] oldGrid, int gridSize, int size, float threshold)
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
                            if (_grid[i + k, j + l] == _airColor) neighbors++;
                        }
                    }

                    newGrid[i, j] = neighbors > threshold ? _airColor : _wallColor;
                }
            }
            return newGrid;
        }

        private void DefineConnectedComponents()
        {

            _qu = new QuickUnion(_gridSize*_gridSize);
            for (var i = 0; i < _gridSize; i++)
            {
                for (var j = 0; j < _gridSize; j++)
                {
                    if (_grid[i, j] == _wallColor) continue;

                    for (var k = -1; k <= 1; k++)
                    {
                        for (var l = -1; l <= 1; l++)
                        {
                            if (i + k < 0 || i + k >= _gridSize || j + l < 0 || j + l >= _gridSize || (k == 0 && l == 0)) continue;
                            if (_grid[i + k, j + l] == _airColor) _qu.Union(j * _gridSize + i, (j + l) * _gridSize + i + k);
                        }
                    }
                }
            }
        }

        private void SortContiguousRegions()
        {
            var r = new Dictionary<int, ContiguousRegion>();

            for (var i = 0; i < _gridSize * _gridSize; i++)
            {
                if (r.ContainsKey(_qu.Find(i)) || _grid[i % 256, i / _gridSize] == _wallColor) continue;
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
                    if (_grid[node.X, node.Y] == _wallColor)
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
                if (_grid[node.X, node.Y] == _airColor) continue;
                _grid[node.X, node.Y] = _airColor;

                var i = node.X;
                var j = node.Y;

                

                for (var k = -TunnelSize; k <= TunnelSize; k++)
                {
                    for (var l = -TunnelSize; l <= TunnelSize; l++)
                    {
                        if (i + k < 0 || i + k >= _gridSize || j + l < 0 || j + l >= _gridSize || (k == 0 && l == 0)) continue;

                        _grid[i + k, j + l] = _rng.NextDouble() < TunnelPersistance ? _airColor : _wallColor;
                    }
                }
            }

            _qu.Union(r1.Index, r2.Index);
            _regions.Remove(r2);

            picOutput.Refresh();
        }

        private void DrawBox(object sender, PaintEventArgs e)
        {
            var cellSize = (float)(picOutput.Width)/_gridSize;
            for (var i = 0; i < _gridSize; i++)
            {
                for (var j = 0; j < _gridSize; j++)
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
                    if (_grid[i, j] == _wallColor) continue;

                    if (_qu.Find(i + j * _gridSize) != _mainRegion.Index) _grid[i, j] = _wallColor;
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
                    if (_grid[i, j] == _airColor) water++;
                }
            }

            _water = water/(float) (_gridSize*_gridSize);
        }

        private void MarkAcceptance()
        {
            txtAccept.Text = (_water >= WaterMinimum && _water <= WaterMaximum ? "Accepted!" : "Rejected!") + string.Format(" ({0}%)", _water * 100);
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

                return _grid[pf.X, pf.Y] == _wallColor ? 10 : 1;
            }

            public override string ToString()
            {
                return String.Format("{0}, {1}", X, Y);
            }
        }
    }
}
