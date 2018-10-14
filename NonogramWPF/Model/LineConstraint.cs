using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramWPF.Model
{
    public class LineConstraint
    {
        public List<int> Items { get; private set; } = new List<int>();

        public LineConstraint() { }

        public LineConstraint(IEnumerable<int> constraintValues)
        {
            foreach(var value in constraintValues)
                Items.Add(value);
        }

        public void Add(int constraint)
        {
            Items.Add(constraint);
        }

    }
}
