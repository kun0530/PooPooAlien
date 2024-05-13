using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.IO;
using System.Globalization;

public class MonsterData
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Type { get; set; }
    public int Hp { get; set; }
    public int Atk { get; set; }

    public override string ToString()
    {
        return $"{Id}: {Name} / {Hp} / {Atk}";
    }
}

public class MonsterTable : DataTable
{

    private Dictionary<string, MonsterData> table = new Dictionary<string, MonsterData>();

    public MonsterData Get(string id)
    {
        if (!table.ContainsKey(id))
            return null;

        return table[id];
    }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        var textAsset = Resources.Load<TextAsset>(path);

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<MonsterData>();
            foreach (var record in records)
            {
                table.Add(record.Id, record);
            }
        }
    }
}
