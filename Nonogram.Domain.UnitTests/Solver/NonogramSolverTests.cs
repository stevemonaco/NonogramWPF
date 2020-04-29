using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Nonogram.Domain.UnitTests
{
    //public class NonogramSolverTests
    //{
    //    [Theory]
    //    [MemberData(nameof(ChunkIntervalCases))]
    //    public void ChunkIntervals_ReturnsExpected(IList<NonogramCell> cells, IList<(int, int)> expected)
    //    {
    //        var actual = NonogramSolver.ChunkIntervals(cells);
    //        Assert.Equal(expected, actual);
    //    }

    //    public static IEnumerable<object[]> ChunkIntervalCases =>
    //        new List<object[]>
    //        {
    //            new object[]
    //            {
    //                NonogramUtility.CellsFromString("-"),
    //                new[] { (0, 1) }
    //            },
    //            new object[]
    //            {
    //                NonogramUtility.CellsFromString("x"),
    //                new (int, int)[] { }
    //            },
    //            new object[]
    //            {
    //                NonogramUtility.CellsFromString("----x-----"),
    //                new[] { (0, 4), (5, 5) }
    //            },
    //            new object[]
    //            {
    //                NonogramUtility.CellsFromString("----x--x-----xxx--xxxx---"),
    //                new[] { (0, 4), (5, 2), (8,5), (16,2), (22,3) }
    //            },
    //            new object[]
    //            {
    //                NonogramUtility.CellsFromString("-oo-x--xoooooxxxo-xxxx--o"),
    //                new[] { (0, 4), (5, 2), (8,5), (16,2), (22,3) }
    //            }
    //        };

    //    [Theory]
    //    [MemberData(nameof(LabelCellsCases))]
    //    public void LabelCells_ReturnsExpected(IList<NonogramCell> cells, IList<SortedSet<int>> expected)
    //    {
    //        var actual = NonogramSolver.LabelCells(cells);
    //        Assert.Equal(expected, actual);
    //    }

    //    public static IEnumerable<object[]> LabelCellsCases =>
    //        new List<object[]>
    //        {
    //            new object[]
    //            {
    //                NonogramUtility.CellsFromString("x-o------o"),
    //                new[]
    //                {
    //                    new SortedSet<int> { -1 },
    //                    new SortedSet<int> { -1, 2 },
    //                    new SortedSet<int> { 2, 3 },
    //                    new SortedSet<int> { 3, 4, 5, -6 },
    //                    new SortedSet<int> { 4, 5, -6 },
    //                    new SortedSet<int> { 5, -6, 7 },
    //                    new SortedSet<int> { -1, 2, 3, 4, 5, -6, 7, 8, 9, -10 },
    //                    new SortedSet<int> { 3, 5, -6, 7, 8, 9, -10 },
    //                    new SortedSet<int> { 8, 9, -10 },
    //                    new SortedSet<int> { 9 },
    //                }
    //            }
    //        };
    //}
}
