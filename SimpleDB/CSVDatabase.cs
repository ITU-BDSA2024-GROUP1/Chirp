using System.Collections;
using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB
{
    internal class CSVDatabase<T> : IDatabaseRepository<T>
    {
        private readonly string _path;
        internal CSVDatabase(string path) { _path = path; }

        public IEnumerable<T> Read(int? limit = null)
        {
            using var reader = new StreamReader(_path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = new List<T>(csv.GetRecords<T>());
            return records;
        }

        public void Store(T record)
        {
            List<T> records = new() { record };
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
            };
            using var stream = File.Open(_path, FileMode.Append);
            using var writer = new StreamWriter(stream);
            using var csv = new CsvWriter(writer, config);
            csv.WriteRecords(records as IEnumerable);
        }
    }
}
