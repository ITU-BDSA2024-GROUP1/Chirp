using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CsvHelper;
using CsvHelper.Configuration;


namespace SimpleDB
{
    public class CSVDatabase<T> : IDatabaseRepository<T>
    {
        string path;
        public CSVDatabase(string path) { this.path = path; }

        public IEnumerable<T> Read(int? limit = null)
        {
            IEnumerable<T> records;
            using (StreamReader reader = new StreamReader(path))
            using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = new List<T>(csv.GetRecords<T>());
            }
            return records;
        }

        public void Store(T record)
        {
            List<T> records = new List<T>();
            records.Add(record);
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false,
            };
            using (var stream = File.Open(path, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(records);
            }
        }
    }
}
