using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using UnityEngine;

public class StringTable : DataTable
{
    private class Data
    {
        public string Id { get; set; }
        public string String { get; set; }
    }

    private Dictionary<string, string> table = new Dictionary<string, string>();

    public string Get(string id)
    {
        if (!table.ContainsKey(id))
            return string.Empty;
        return table[id];
    }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();

        var textAsset = Resources.Load<TextAsset>(path);

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<Data>();
            foreach (var record in records)
            {
                table.Add(record.Id, record.String);
            }
        }
    }
}
