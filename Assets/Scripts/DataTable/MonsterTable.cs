using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.IO;
using System.Globalization;

public class MonsterData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Type { get; set; }
    public int Hp { get; set; }
    public int Atk { get; set; }
    public int Def { get; set; }
    public int VerticalSpd  { get; set; }
    public int HorizontalSpd  { get; set; }
    public int Score  { get; set; }
    public int KillPoint  { get; set; }
    public int ItemDropId { get; set; }

    public override string ToString()
    {
        return $"{Id}: {Name} / {Hp} / {Atk}";
    }
}

public class MonsterTable : DataTable
{

    private Dictionary<int, MonsterData> table = new Dictionary<int, MonsterData>();

    public MonsterData Get(int id)
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
