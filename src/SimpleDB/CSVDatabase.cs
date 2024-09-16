﻿using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

namespace SimpleDB
{
    public class CSVDatabase<T> : IDatabaseRepository<T>
    {
        private const string Path = "data/chirp_cli_db.csv";

        private static CSVDatabase<T>? s_instance;
        private CSVDatabase() {}
        public static CSVDatabase<T> Instance
        {
            get => s_instance ??= new();
        }

        public IEnumerable<T> Read(int? limit = null)
        {
            StreamReader reader = new(Path);
            CsvReader csv = new(reader, CultureInfo.InvariantCulture);
            
            IEnumerable<T> records = csv.GetRecords<T>();
            return limit == null ? records : records.TakeLast((int)limit);
        }

        public void Store(T record)
        {
            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                // Don't write the header again.
                HasHeaderRecord = false
            };

            using (FileStream stream = File.Open(Path, FileMode.Append))
            using (StreamWriter writer = new(stream))
            using (CsvWriter csv = new(writer, config))
            {
                csv.WriteRecord(record);
                csv.NextRecord();
            }
        }
    }
}