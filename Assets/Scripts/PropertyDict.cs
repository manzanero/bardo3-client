using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;


public class PropertyDict
{
    private readonly Dictionary<string, Property> properties = new();

    public List<string> Keys => properties.Keys.ToList();

    public bool Has(string name) => properties.ContainsKey(name);

    public class Bar
    {
        public Bar(float value = 100, float max = 100)
        {
            this.value = value;
            this.max = max;
        }

        public float value;
        public float max;
    }

    private Property Get(string name) => properties.TryGetValue(name, out var value) ? value : null;

    public string GetText(string name, string defaultText = "") => Has(name)
        ? Get(name).values[0]
        : defaultText;

    public float GetNum(string name, float defaultNum = 0) => Has(name)
        ? float.Parse(Get(name).values[0])
        : defaultNum;

    public Bar GetBar(string name, Bar defaultBar = null) => Has(name)
        ? new Bar(float.Parse(Get(name).values[0]), float.Parse(Get(name).values[1]))
        : defaultBar ?? new Bar();

    public bool GetBool(string name, bool defaultBool = false) => Has(name) && Get(name).values[0] == "true";

    public Color GetColor(string name, Color defaultColor = new()) => Has(name)
        ? new Color(
            Mathf.Clamp(float.Parse(Get(name).values[0]) / 255, 0, 1),
            Mathf.Clamp(float.Parse(Get(name).values[1]) / 255, 0, 1),
            Mathf.Clamp(float.Parse(Get(name).values[2]) / 255, 0, 1))
        : defaultColor;

    public float GetPercent(string name, float defaultPercent = 0) => Has(name)
        ? float.Parse(Get(name).values[0]) / 100
        : defaultPercent;


    private void Set(string name, string type, IEnumerable<string> values)
    {
        if (Has(name))
            Get(name).values = values.ToList();
        else
            properties.Add(name, new Property { name = name, type = type, values = values.ToList() });
    }

    public void SetText(string name, string text) => Set(name, "text", new[]
    {
        text
    });

    public void SetNum(string name, float numeric) => Set(name, "num", new[]
    {
        numeric.ToString(CultureInfo.InvariantCulture)
    });

    public void SetBar(string name, Bar bar) => Set(name, "bar", new[]
    {
        bar.value.ToString(CultureInfo.InvariantCulture), bar.max.ToString(CultureInfo.InvariantCulture)
    });

    public void SetBool(string name, bool boolean) => Set(name, "bool", new[]
    {
        boolean ? "true" : "false"
    });

    public void SetColor(string name, Color color) => Set(name, "color", new[]
    {
        Mathf.Round(color.r * 255).ToString(CultureInfo.InvariantCulture),
        Mathf.Round(color.g * 255).ToString(CultureInfo.InvariantCulture),
        Mathf.Round(color.b * 255).ToString(CultureInfo.InvariantCulture)
    });

    public void SetPercent(string name, float percent) => Set(name, "percent", new[]
    {
        (percent * 100).ToString(CultureInfo.InvariantCulture)
    });

    public void Remove(string name) => properties.Remove(name);


    [Serializable]
    public class Model
    {
        public List<Property> properties;
    }

    public Model Serializable()
    {
        return new Model
        {
            properties = properties.Keys.Select(x => properties[x]).ToList()
        };
    }

    public void Deserialize(Model model)
    {
        for (var i = 0; i < properties.Count; i++)
            Set(
                model.properties[i].name,
                model.properties[i].type,
                model.properties[i].values
            );
    }
}