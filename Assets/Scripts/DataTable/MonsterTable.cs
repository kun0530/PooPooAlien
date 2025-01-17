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
    public float VerticalSpd  { get; set; }
    public float HorizontalSpd  { get; set; }
    public bool IsBounce { get; set; }
    public int Score  { get; set; }
    public int KillPoint  { get; set; }
    public float GetGold { get; set; }
    public int ItemDropId { get; set; }
    public string MaterialName { get; set; }

    private string materialFilePath = "Materials/{0}";
    public Material GetMaterial()
    {
        if (MaterialName == null || string.Equals(FrequentlyUsed.Default, MaterialName))
            return null;
        return Resources.Load<Material>(string.Format(materialFilePath, MaterialName));
    }

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
