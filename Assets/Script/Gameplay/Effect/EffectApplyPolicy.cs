

namespace NgocDev.Gameplay.Effect
{
    public abstract class EffectApplyPolicy
    {
        public abstract void Apply(ActiveEffect effect, EffectController target);
    }

    public class ReplacePolicy : EffectApplyPolicy
    {
        public override void Apply(ActiveEffect effect, EffectController target)
        {
            effect.stackCount = 1;
            effect.stackDuration.Clear();
        }
    }
    public class StackPolicy : EffectApplyPolicy
    {
        public override void Apply(ActiveEffect effect, EffectController target)
        {
            if (effect.stackCount < effect.maxStack)
            {
                effect.stackCount++;
                effect.stackDuration.Add(0);
            }
        }
    }
}
