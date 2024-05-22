using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using UnityEngine;

public class StageData
{
    public int StageId { get; set; }
    public string Name { get; set; }
    public int StageNum { get; set; }
    public float StageKillp { get; set; }
    public float StageTimerset { get; set;}
    public int StageScoreget { get; set; }
    public float StageTimerleft { get; set; }
    public int StageHitcount { get; set; }
    public float ClearGold { get; set; }
    public string Desc { get; set; }

    public override string ToString()
    {
        return $"{StageNum}스테이지: {Name} / {StageKillp} / {StageTimerset} / {StageScoreget} / {StageTimerleft} / {StageHitcount} / {ClearGold} / {Desc}";
    }
}

public class StageTable : DataTable
{
    private Dictionary<int, StageData> table = new Dictionary<int, StageData>();

    public StageData Get(int id)
    {
        if (!table.ContainsKey(id))
            return null;

        return table[id];
    }

    public int CountStage()
    {
        return table.Count;
    }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        var textAsset = Resources.Load<TextAsset>(path);

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<StageData>();
            foreach (var record in records)
            {
                table.Add(record.StageNum, record);
            }
        }
    }
}
