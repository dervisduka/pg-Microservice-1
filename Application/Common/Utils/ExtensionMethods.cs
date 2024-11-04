using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Application.Common.Utils
{
    public static class ExtensionMethods
    {
        public static string ValidationStripText(this string inputText)
        {
            if (string.IsNullOrEmpty(inputText)) return inputText;

            string[] texts = new[] { "Command", "Query", "Add", "Update", "Delete", "Manage", "Approve", "Create", "Submit", "Reject", "MannualStart", "MannualStop" };
            foreach (var text in texts)
            {
                inputText = inputText.Replace(text, string.Empty);
            }
            return inputText;
        }

        public static string BeautifulVariable(this string variableName)
        {
            return Regex.Replace(variableName, @".[[\d]]", string.Empty);
        }

        public static bool IsStopWords(this string checkVal)
        {
            var stopWords = new[] { "Approve" };
            return stopWords.Any(x => checkVal.Contains(x));
        }

        /// <summary>
        /// Converts List of any class object into a DataTable
        /// </summary>
        /// <typeparam name="T">Generic type of input enumerable</typeparam>
        /// <param name="data">Input enumerables</param>
        /// <returns>DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> data)
            where T : class
        {
            var table = new DataTable();
            var properties = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                var row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    try
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                    catch (Exception)
                    {
                        row[prop.Name] = DBNull.Value;
                    }
                }

                table.Rows.Add(row);
            }

            return table;
        }
    }
}
