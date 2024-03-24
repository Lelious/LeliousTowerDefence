using System;
using System.Text;

[Serializable]
public class Buff : Effect
{
    private StringBuilder _stringBuilder;

    public Buff(IEffectable effectable, EffectData data)
    {
        _effectName = data.EffectName;
        _effectable = effectable;
        _type = data.EffectType;
        _image = data.EffectImage;
        CurrentDuration.Value = data.EffectDuration;
        _percentage = data.PercentageValue;
        CurrentDuration.Value = CurrentDuration.Value;

        _description = BuildDescription(data.Description);
    }

    private string BuildDescription(string value)
    {
        _stringBuilder = new StringBuilder();
        _stringBuilder.Append(value);
        _stringBuilder.Replace("{Effect Duration}", $"{CurrentDuration.Value}");
        _stringBuilder.Replace("{Percentage Value}", $"{_percentage * 100}");

        return _stringBuilder.ToString();
    }
}
