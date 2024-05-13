using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using UnityEngine;

public class ItemData
{
    public int Id { get; set; }
    public int ItemType { get; set; }
    public int Value { get; set; }

    public override string ToString()
    {
        return $"{Id}: {(ItemType)ItemType} / {Value}";
    }
}

public class ItemTable : DataTable
{
    private Dictionary<int, ItemData> table = new Dictionary<int, ItemData>();

    public ItemData Get(int id)
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
            var records = csvReader.GetRecords<ItemData>();
            foreach (var record in records)
            {
                table.Add(record.Id, record);
            }
        }
    }
}
