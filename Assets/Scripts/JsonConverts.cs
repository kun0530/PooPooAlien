using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;

public class SaveDataConverter : JsonConverter<SaveData>
{
    // PlayerStat StringToEnum(string stat)
    // {
    //     if (Enum.TryParse(typeof(PlayerStat), stat, false, out var result))
    //     {
    //         return (PlayerStat)result;
    //     }
    //     return PlayerStat.None;
    // }

    public override SaveData ReadJson(JsonReader reader, Type objectType, SaveData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        SaveDataV1 data = new SaveDataV1();
        JObject jObj = JObject.Load(reader);

        data.Gold = (float)jObj["Gold"];

        var enhanceStatData = jObj["EnhanceStatData"];
        for (int i = 0; i < (int)PlayerStat.Count; i++)
        {
            int level = 1;
            try
            {
                level = (int)enhanceStatData[((PlayerStat)i).ToString()];
            }
            catch
            {
                continue;
            }
            data.EnhanceStatData[(PlayerStat)i] = level;
        }

        var stageClearData = jObj["StageClearData"]["$values"];
        for (int i = 0; i <= 10; i++)
        {
            data.StageClearData[i] = (int)stageClearData[i];
        }

        data.CurrentLang = (Languages)(int)jObj["CurrentLang"];

        return data;
    }

    public override void WriteJson(JsonWriter writer, SaveData value2, JsonSerializer serializer)
    {
        var value = value2 as SaveDataV1;
        Debug.Log("Write!");
        writer.WriteStartObject();
        writer.WritePropertyName("Gold");
        writer.WriteValue(value.Gold);
        writer.WritePropertyName("EnhanceStatData");
        writer.WriteValue(value.EnhanceStatData);
        writer.WritePropertyName("StageClearData");
        writer.WriteValue(value.StageClearData);
        writer.WritePropertyName("CurrentLang");
        writer.WriteValue(value.CurrentLang);
        writer.WritePropertyName("Version");
        writer.WriteValue(value.Version);
        writer.WriteEndObject();
    }
}