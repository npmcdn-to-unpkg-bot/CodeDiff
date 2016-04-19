using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace MsdrRu.CodeDiff.DiffVisualization
{
    public class HtmlVisualizer
    {
        public static DiffHtmlVisualizationResult Visualize(Tuple<Line[], Line[]> parsingResults)
        {
            Contract.Requires<ArgumentNullException>(parsingResults != null);

            const string emptyStringPattern = "<span></span><pre class=\"diff-empty\"></pre>";
            const string originalStringPattern = "<span>{0}</span><pre class=\"diff-original\">{1}</pre>";
            const string insertedStringPattern = "<span>{0}</span><pre class=\"diff-inserted\">{1}</pre>";
            const string modifiedStringPattern = "<span>{0}</span><pre class=\"diff-modified\">{1}</pre>";
            const string removedStringPattern = "<span>{0}</span><pre class=\"diff-removed\">{1}</pre>";

            var maxLines = Math.Max(parsingResults.Item1.Length, parsingResults.Item2.Length);

            var version1LinesDescriptions = new Line[maxLines];
            var version2LinesDescriptions = new Line[maxLines];


            var version1DiffHtmlSb = new StringBuilder();
            var version2DiffHtmlSb = new StringBuilder();

            for (var a = 0; a < maxLines; a++)
            {
                if (parsingResults.Item1.Length < a)
                {
                    version1DiffHtmlSb.Append(emptyStringPattern);

                    var lineNumber = parsingResults.Item2[a].LineNumber;
                    var content = parsingResults.Item2[a].Content;
                    version2DiffHtmlSb.Append(string.Format(insertedStringPattern, lineNumber, content));
                    continue;
                }

                if (parsingResults.Item2.Length < a)
                {
                    var lineNumber = parsingResults.Item1[a].LineNumber;
                    var content = parsingResults.Item1[a].Content;
                    version1DiffHtmlSb.Append(string.Format(removedStringPattern, lineNumber, content));

                    version2DiffHtmlSb.Append(emptyStringPattern);
                    continue;
                }

                if (parsingResults.Item1[a].Status == LineStatus.Original)
                {
                    var lineNumber1 = parsingResults.Item1[a].LineNumber;
                    var content1 = parsingResults.Item1[a].Content;
                    version1DiffHtmlSb.Append(string.Format(originalStringPattern, lineNumber1, content1));

                    var lineNumber2 = parsingResults.Item2[a].LineNumber;
                    var content2 = parsingResults.Item2[a].Content;
                    version2DiffHtmlSb.Append(string.Format(originalStringPattern, lineNumber2, content2));
                    continue;
                }

                if (parsingResults.Item1[a].Status == LineStatus.Modified)
                {
                    var lineNumber1 = parsingResults.Item1[a].LineNumber;
                    var content1 = parsingResults.Item1[a].Content;
                    version1DiffHtmlSb.Append(string.Format(modifiedStringPattern, lineNumber1, content1));

                    var lineNumber2 = parsingResults.Item2[a].LineNumber;
                    var content2 = parsingResults.Item2[a].Content;
                    version2DiffHtmlSb.Append(string.Format(modifiedStringPattern, lineNumber2, content2));
                    continue;
                }

                if (parsingResults.Item1[a].Status == LineStatus.Removed)
                {
                    var lineNumber1 = parsingResults.Item1[a].LineNumber;
                    var content1 = parsingResults.Item1[a].Content;
                    version1DiffHtmlSb.Append(string.Format(removedStringPattern, lineNumber1, content1));

                    version2DiffHtmlSb.Append(emptyStringPattern);
                    continue;
                }

                if (parsingResults.Item2[a].Status == LineStatus.Inserted)
                {
                    version1DiffHtmlSb.Append(emptyStringPattern);

                    var lineNumber2 = parsingResults.Item2[a].LineNumber;
                    var content2 = parsingResults.Item2[a].Content;
                    version2DiffHtmlSb.Append(string.Format(originalStringPattern, lineNumber2, content2));
                }
            }

            return new DiffHtmlVisualizationResult()
            {
                Version1DiffHtml = version1DiffHtmlSb.ToString(),
                Version2DiffHtml = version2DiffHtmlSb.ToString()
            };
        }
    }
}
