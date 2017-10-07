using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stride.Persistence
{
    public class Database
    {
        readonly string StorageFullPath = @"C:\Dropbox\Temp\Stride\c4-c6_diatonic.db";

        public List<SessionRecord> Load() =>
            File.ReadAllLines(StorageFullPath).Select(SessionRecord.Parse).ToList();

        public void Save(IReadOnlyList<SessionRecord> records) =>
            File.WriteAllLines(StorageFullPath, records.Select(SessionRecord.Serialize));
    }
}