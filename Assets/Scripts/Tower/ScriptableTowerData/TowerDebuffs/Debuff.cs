using System;
using System.Text;

namespace Assets.Scripts.Tower.TowerAbilities
{
    [Serializable]
    public class Debuff : Effect
    {
        private StringBuilder _stringBuilder;
     
        public Debuff(IEffectable effectable, EffectData data)
        {
            _effectName = data.EffectName;
            _effectable = effectable;
            _type = data.EffectType;
            _image = data.EffectImage;
            CurrentDuration.Value = data.EffectDuration;
            _percentage = data.PercentageValue;
            _damagePerTick = data.Damage;
            _periodical = data.IsPeriodical;
            _tick = data.TickTime;
            _currentTick = data.TickTime;
            _damageSource = data.DamageSource;
            _description = BuildDescription(data.Description);
        }

        private string BuildDescription(string value)
        {
            _stringBuilder = new StringBuilder();
            _stringBuilder.Append(value);
            _stringBuilder.Replace("{Effect Duration}", $"{CurrentDuration.Value}");
            _stringBuilder.Replace("{Damage}", $"{_damagePerTick}");
            _stringBuilder.Replace("{Percentage Value}", $"{_percentage * 100}");
            _stringBuilder.Replace("{Tick Time}", $"{_tick}");

            return _stringBuilder.ToString();
        }
    }
}