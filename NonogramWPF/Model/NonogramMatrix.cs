using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramWPF.Model
{
    public class NonogramMatrix
    {
        public int Columns { get => Cells.GetLength(0); }
        public int Rows { get => Cells.GetLength(1); }

        public List<LineConstraint> SolutionRowConstraints { get; private set; } = new List<LineConstraint>();
        public List<LineConstraint> SolutionColumnConstraints { get; private set; } = new List<LineConstraint>();

        public List<LineConstraint> RowConstraints { get; private set; } = new List<LineConstraint>();
        public List<LineConstraint> ColumnConstraints { get; private set; } = new List<LineConstraint>();

        public NonogramCell[,] Cells { get; set; } = new NonogramCell[0, 0];

        public bool IsGameActive { get; private set; } = false;

        public NonogramMatrix()
        {
        }

        public NonogramMatrix(int x, int y)
        {
            Cells = new NonogramCell[x, y];

            ResetMatrixStates();
        }

        public bool LoadFromXml(string fileName)
        {
            var root = XElement.Load(fileName);

            var rowConstraints = root.Element("RowConstraints");
            foreach(var el in rowConstraints.Descendants())
            {
                var vals = el.Value.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Where(x => int.TryParse(x, out _));
                var constraints = new LineConstraint(vals.Select(x => int.Parse(x)));
                SolutionRowConstraints.Add(constraints);
            }

            var columnConstraints = root.Element("ColumnConstraints");
            foreach (var el in columnConstraints.Descendants())
            {
                var vals = el.Value.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries).Where(x => int.TryParse(x, out _));
                var constraints = new LineConstraint(vals.Select(x => int.Parse(x)));
                SolutionColumnConstraints.Add(constraints);
            }

            int.TryParse(rowConstraints.Attribute("count").Value, out var rowConstraintsCount);
            int.TryParse(columnConstraints.Attribute("count").Value, out var columnConstraintsCount);

            InitializeBoard(columnConstraintsCount, rowConstraintsCount);
            IsGameActive = true;

            return true;
        }

        public void InitializeBoard(int x, int y)
        {
            Cells = new NonogramCell[x, y];
            ResetMatrixStates();
        }

        public bool CheckWinState()
        {
            BuildConstraints();

            for(int i = 0; i < RowConstraints.Count; i++)
            {
                if (!SolutionRowConstraints[i].Equals(RowConstraints[i]))
                    return false;
            }

            for (int i = 0; i < ColumnConstraints.Count; i++)
            {
                if (!SolutionColumnConstraints[i].Equals(ColumnConstraints[i]))
                    return false;
            }

            IsGameActive = false;
            return true;
        }

        public void BuildConstraints()
        {
            RowConstraints.Clear();
            ColumnConstraints.Clear();

            // Build Row Constraints
            for(int y = 0; y < Rows; y++)
            {
                LineConstraint constraint = new LineConstraint();
                int run = 0;
                for(int x = 0; x < Columns; x++)
                {
                    if (Cells[x, y].CellState == CellState.Filled)
                        run++;
                    else if(run > 0)
                    {
                        constraint.Add(run);
                        run = 0;
                    }
                }
                if(run > 0)
                    constraint.Add(run);

                if (constraint.Items.Count == 0)
                    constraint.Add(0);

                RowConstraints.Add(constraint);
            }

            // Build Column Constraints
            for (int x = 0; x < Columns; x++)
            {
                LineConstraint constraint = new LineConstraint();
                int run = 0;
                for (int y = 0; y < Rows; y++)
                {
                    if (Cells[x, y].CellState == CellState.Filled)
                        run++;
                    else if (run > 0)
                    {
                        constraint.Add(run);
                        run = 0;
                    }
                }
                if (run > 0)
                    constraint.Add(run);

                if (constraint.Items.Count == 0)
                    constraint.Add(0);

                ColumnConstraints.Add(constraint);
            }
        }

        public void ResetMatrixStates()
        {
            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Columns; x++)
                {
                    var cell = new NonogramCell();
                    cell.CellState = CellState.Undetermined;
                    Cells[x, y] = cell;
                }
            }
        }

        public CellState GetState(int x, int y)
        {
            if (x < Columns && y < Rows)
                return Cells[x, y].CellState;
            else
                throw new IndexOutOfRangeException();
        }

        public void SetState(int x, int y, CellState cs)
        {
            if (IsGameActive)
            {
                if (x < Columns && y < Rows)
                    Cells[x, y].CellState = cs;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public IEnumerable<NonogramCell> Board
        {
            get
            {
                for (int y = 0; y < Rows; y++)
                    for (int x = 0; x < Columns; x++)
                        yield return Cells[x, y];
            }
        }
    }
}
