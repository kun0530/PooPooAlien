using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using UnityEngine;

public class ProjectileData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Type { get; set; }
    public int Phase { get; set; }
    public float Damage { get; set; }
    public float Scale { get; set; }
    public float Speed { get; set; }
    public float Interval { get; set; }

    public override string ToString()
    {
        return $"{Name} : {(WeaponType)Type} / {Phase}단계 / 데미지 {Damage} / 스케일 {Scale} / 스피드 {Speed} / 간격 {Interval}";
    }
}

public class ProjectileTable : DataTable
{
    private Dictionary<(WeaponType, int), ProjectileData> table = new Dictionary<(WeaponType, int), ProjectileData>();

    public ProjectileData Get(WeaponType type, int phase)
    {
        if (!table.ContainsKey((type, phase)))
            return null;
        
        return table[(type, phase)];
    }

    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        var textAsset = Resources.Load<TextAsset>(path);

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csvReader.GetRecords<ProjectileData>();
            foreach (var record in records)
            {
                table.Add(((WeaponType)record.Type, record.Phase), record);
            }
        }
    }
}
