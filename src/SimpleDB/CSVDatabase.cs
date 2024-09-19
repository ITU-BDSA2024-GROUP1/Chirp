using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string _csvDBPath;

    public CSVDatabase(string path) => _csvDBPath = path;

    public IEnumerable<T> Read(int? limit = null)
    {
        using StreamReader reader = new(_csvDBPath);
        using CsvReader csv = new(reader, CultureInfo.InvariantCulture);
            
        IEnumerable<T> records = csv.GetRecords<T>().ToList();
        return limit == null ? records : records.TakeLast((int)limit).ToList();
    }

    public void Store(T record)
    {
        CsvConfiguration config = new(CultureInfo.InvariantCulture)
        {
            // Don't write the header again.
            HasHeaderRecord = false
        };

        using FileStream stream = File.Open(_csvDBPath, FileMode.Append);
        using StreamWriter writer = new(stream);
        using CsvWriter csv = new(writer, config);
            
        csv.WriteRecord(record);
        csv.NextRecord();
    }
}