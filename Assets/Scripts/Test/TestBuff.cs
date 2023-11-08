
    public class TestBuff : Effect, IEffect
    {
        public TestBuff(EffectType type, IEffectable effectable, float percentValue, float duration, bool periodical = false) : base(type, effectable, percentValue, duration, periodical = false) 
        {
            _type = type;
            _effectable = effectable;
            _percentage = percentValue;
            _duration = duration;
            _periodical = periodical;
        }
    }
