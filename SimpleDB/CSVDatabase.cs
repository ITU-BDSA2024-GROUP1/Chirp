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
            StreamReader reader = new(_path);
            CsvReader csv = new(reader, CultureInfo.InvariantCulture);
            
            IEnumerable<T> records = csv.GetRecords<T>();
            return limit == null ? records : records.Take((int)limit);
        }

        public void Store(T record)
        {
            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false
            };

            FileStream stream = File.Open(_path, FileMode.Append);
            StreamWriter writer = new(stream);
            CsvWriter csv = new(writer, config);
            csv.WriteRecord(record);
        }
    }
}