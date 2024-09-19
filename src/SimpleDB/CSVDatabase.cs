using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    private const string ProdPath = "data/chirp_cli_db.csv";
    private const string TestPath = "data/test.csv";

    public bool InTestingDatabase = false;
    private string CurrentDBPath => InTestingDatabase ? TestPath : ProdPath;

    private static CSVDatabase<T>? s_instance;
    private CSVDatabase() {}
    public static CSVDatabase<T> Instance
    {
        get => s_instance ??= new CSVDatabase<T>();
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        using StreamReader reader = new(CurrentDBPath);
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

        using FileStream stream = File.Open(CurrentDBPath, FileMode.Append);
        using StreamWriter writer = new(stream);
        using CsvWriter csv = new(writer, config);
            
        csv.WriteRecord(record);
        csv.NextRecord();
    }
}