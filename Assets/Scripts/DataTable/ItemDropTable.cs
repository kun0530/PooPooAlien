using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using UnityEngine;

public class ItemDropData
{
    public int Id { get; set; }
    public float DropChance { get; set; }
    public List<(int itemId, float itemChance)> itemDropChances = new List<(int itemId, float itemChance)>();
}

public class ItemDropTable : DataTable
{
    public Dictionary<int, ItemDropData> table = new Dictionary<int, ItemDropData>();

    public ItemDropData Get(int id)
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
            csvReader.Read();
            var columnCount = csvReader.ColumnCount;
            int patternStartIndex = 2;
            int patternCount = (columnCount - patternStartIndex) / 3;

            while (csvReader.Read())
            {
                var itemDropData = new ItemDropData();
                itemDropData.Id = csvReader.GetField<int>(0);
                itemDropData.DropChance = csvReader.GetField<float>(1);

                for (int i = patternStartIndex; i < columnCount; i += 2)
                {
                    (int itemId, float itemChance) itemDropChance = (csvReader.GetField<int>(i), csvReader.GetField<float>(i + 1));
                    itemDropData.itemDropChances.Add(itemDropChance);
                }

                table.Add(itemDropData.Id, itemDropData);
            }
        }
    }
}
