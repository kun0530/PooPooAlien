using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using UnityEngine;

public enum PlayerStat
{
    MaxHP,
    StartHP,
    BasicAttack,
    FocusAttack,
    SpreadAttack,
    LazorAttack,
    PenetAttack,
    PowerUpDamage,
    BoosterSpeed,
    BoosterSize,
    Count
}

public class EnhanceData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Stat { get; set; }
    public int MaxLevel { get; set; }
    public float BasicStat { get; set; }
    public float StatIncrease { get; set; }
    public float RequiredGold { get; set; }
    public float RequiredGoldIncrease { get; set; }
    public string Sprite { get; set; }
    public string Desc { get; set; }

    public Sprite GetIcon()
    {
        return Resources.Load<Sprite>(string.Format("EnhanceIcon/{0}", Sprite));
    }

    public string GetName()
    {
        return DataTableManager.GetStringTable().Get(Name);
    }

    public string GetDesc()
    {
        return DataTableManager.GetStringTable().Get(Desc);
    }

    public override string ToString()
    {
        return $"{Id}: {Name} / {Stat} / {MaxLevel} / {StatIncrease} / {RequiredGold} / {RequiredGoldIncrease}";
    }
}

public class EnhanceTable : DataTable
{
    private Dictionary<PlayerStat, EnhanceData> table = new Dictionary<PlayerStat, EnhanceData>();

    public EnhanceData Get(PlayerStat id)
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
            var records = csvReader.GetRecords<EnhanceData>();
            foreach (var record in records)
            {
                table.Add((PlayerStat)record.Stat, record);
            }
        }
    }
}
