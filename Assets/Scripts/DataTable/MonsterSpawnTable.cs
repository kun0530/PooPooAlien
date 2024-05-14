using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.IO;
using System.Globalization;

public class MonsterSpawnData
{
    public int StageId { get; set; }
    public int SectionId { get; set; }

    public int Lane1 { get; set; }
    public int Lane2 { get; set; }
    public int Lane3 { get; set; }
    public int Lane4 { get; set; }
    public int Lane5 { get; set; }

    public List<int> GetLaneData()
    {
        return new List<int>() { Lane1, Lane2, Lane3, Lane4, Lane5 };
    }

    public override string ToString()
    {
        return $"({StageId}, {SectionId}): {Lane1} / {Lane2} / {Lane3} / {Lane4} / {Lane5}";
    }
}

public class MonsterSpawnTable : DataTable
{
    private Dictionary<(int, int), List<List<int>>> table = new Dictionary<(int, int), List<List<int>>>();

    public List<List<int>> Get((int, int) key)
    {
        if (!table.ContainsKey(key))
            return null;

        return table[key];
    }

    public List<List<int>> Get(int stageId, int sectionId)
    {
        return Get((stageId, sectionId));
    }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        var textAsset = Resources.Load<TextAsset>(path);

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<MonsterSpawnData>();
            foreach (var record in records)
            {
                var key = (record.StageId, record.SectionId);
                var value = record.GetLaneData();

                if (table.ContainsKey(key))
                {
                    table[key].Add(value);
                }
                else
                {
                    var monsterList = new List<List<int>>();
                    monsterList.Add(value);
                    table.Add(key, monsterList);
                }
            }
        }
    }
}
