using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Stride.Model;

namespace Stride.Persistence
{
    public class Database
    {
        readonly string StorageFolder = @"C:\Dropbox\Temp\Stride";
        readonly IReadOnlyDictionary<DrillId, string> StorageNames =
            new Dictionary<DrillId, string>
            {
                {DrillId.DiatonicOneOctaveC3, "c3-c4_diatonic"},
                {DrillId.DiatonicOneOctaveC4, "c4-c5_diatonic"},
                {DrillId.DiatonicTwoOctavesC3, "c3-c5_diatonic"},
                {DrillId.DiatonicTwoOctavesC4, "c4-c6_diatonic"}
            };

        string GetStorageFullPath(DrillId drill)
        {
            var drillStorageName = StorageNames[drill] + ".db";
            return Path.Combine(StorageFolder, drillStorageName);
        }

        public List<SessionRecord> Load(DrillId drill)
        {
            var path = GetStorageFullPath(drill);
            return File.ReadAllLines(path).Select(SessionRecord.Parse).ToList();
        }

        public void Save(DrillId drill, IReadOnlyList<SessionRecord> records)
        {
            var path = GetStorageFullPath(drill);
            var lines = records.Select(SessionRecord.Serialize);
            File.WriteAllLines(path, lines);
        }
    }
}