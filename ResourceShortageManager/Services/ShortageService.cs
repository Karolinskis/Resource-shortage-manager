using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ResourceShortageManager.Models;

namespace ResourceShortageManager.Services;

public class ShortageService
{
    // TODO: This should probably be in a configuration file
    private readonly string _filePath = "shortages.json";

    public List<Shortage> LoadShortages()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Shortage>();
        }

        var jsonData = File.ReadAllText(_filePath);
        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        };
        List<Shortage> shortages = JsonConvert.DeserializeObject<List<Shortage>>(jsonData, settings)!;

        if (shortages == null)
        {
            return new List<Shortage>();
        }

        return shortages;
    }

    public void SaveShortages(List<Shortage> shortages)
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter> { new StringEnumConverter() }
        };
        var jsonData = JsonConvert.SerializeObject(shortages, settings);
        File.WriteAllText(_filePath, jsonData);
    }
}
