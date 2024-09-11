using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB
{
    public class CSVDatabase<T> : IDatabaseRepository<T>
    {
        private readonly string _path;
        public CSVDatabase(string path) { _path = path; }

        public IEnumerable<T> Read(int? limit = null)
        {
            IEnumerable<T> records;
            using (StreamReader reader = new StreamReader(path))
            using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = new List<T>(csv.GetRecords<T>());
            }
            if (limit == null) return records;
            if (limit >= records.Count()) return records;
            List<T> values = new List<T>();
            for (int i = records.Count() - (int)limit; i < records.Count(); i++)
            {
                values.Add(((List<T>)records)[i]);
            }
            return values;
        }

        public void Store(T record)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false
            };
            
            using var stream = File.Open(_path, FileMode.Append);
            using var writer = new StreamWriter(stream);
            using var csv = new CsvWriter(writer, config);
            csv.WriteRecord(record);
        }
    }
}