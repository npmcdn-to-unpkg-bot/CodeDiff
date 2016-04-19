using System;
using System.Diagnostics.Contracts;
using MsdrRu.CodeDiff.DiffAlgorithm;

namespace MsdrRu.CodeDiff
{
    public class LineParser
    {
        public static Tuple<Line[], Line[]> Parse(Diff.Item[] items, string version1, string version2)
        {
            Contract.Requires<ArgumentNullException>(items != null);
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(version1));
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(version1));

            // ReSharper disable once SuggestVarOrType_Elsewhere
            string[] version1Lines = version1.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            // ReSharper disable once SuggestVarOrType_Elsewhere
            string[] version2Lines = version2.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var version1LinesDescriptions = new Line[version1Lines.Length];
            var version2LinesDescriptions = new Line[version2Lines.Length];

            for (int i = 0; i < version1Lines.Length; i++)
            {
                version1LinesDescriptions[i] = new Line((ulong) (i + 1))
                {
                    Status = LineStatus.Original,
                    Content = version1Lines[i]
                };
            }

            for (int i = 0; i < version2Lines.Length; i++)
            {
                version2LinesDescriptions[i] = new Line((ulong)(i + 1))
                {
                    Status = LineStatus.Original,
                    Content = version2Lines[i]
                };
            }

            // ReSharper disable once SuggestVarOrType_SimpleTypes
            foreach (Diff.Item item in items)
            {
                //Make HTML for the modified rows.
                if (item.deletedA > 0 && item.insertedB > 0)
                {
                    for (var i = 0; i < item.deletedA; i++)
                    {
                        version1LinesDescriptions[item.StartA + i - 1].Status = LineStatus.Modified;
                    }

                    for (var i = 0; i < item.insertedB; i++)
                    {
                        version2LinesDescriptions[item.StartB + i - 1].Status = LineStatus.Modified;
                    }
                }

                //Make HTML for the removed rows.
                if (item.deletedA > 0 && item.insertedB == 0)
                {
                    for (var i = 0; i < item.deletedA; i++)
                    {
                        version1LinesDescriptions[item.StartA + i - 1].Status = LineStatus.Removed;
                    }
                }

                //Make HTML for the inserted rows.
                // ReSharper disable once InvertIf
                if (item.deletedA == 0 && item.insertedB > 0)
                {
                    for (var i = 0; i < item.insertedB; i++)
                    {
                        version2LinesDescriptions[item.StartB + i - 1].Status = LineStatus.Inserted;
                    }
                }
            }

            return new Tuple<Line[], Line[]>(version1LinesDescriptions, version2LinesDescriptions);
        }
    }
}
