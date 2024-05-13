using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataTableManager
{
    private static Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableManager()
    {
        MonsterTable monsterTable = new MonsterTable();
        monsterTable.Load(DataTableIds.Monster);
        tables.Add(DataTableIds.Monster, monsterTable);

        MonsterSpawnTable monsterGroupTable = new MonsterSpawnTable();
        monsterGroupTable.Load(DataTableIds.MonsterSpawn);
        tables.Add(DataTableIds.MonsterSpawn, monsterGroupTable);

        ItemTable itemTable = new ItemTable();
        itemTable.Load(DataTableIds.Item);
        tables.Add(DataTableIds.Item, itemTable);

        ItemDropTable itemDropTable = new ItemDropTable();
        itemDropTable.Load(DataTableIds.ItemDrop);
        tables.Add(DataTableIds.ItemDrop, itemDropTable);
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
            return null;
        return tables[id] as T;
    }
}