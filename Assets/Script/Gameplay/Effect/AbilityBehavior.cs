namespace NgocDev.Gameplay.Effect
{
    public abstract class AbilityBehavior
    {      
        public abstract void Apply(EffectController target);
        public virtual void OnTrigger(EffectController effectTarget) { }
        public virtual void Remove(EffectController target) { }
        public virtual void Update(EffectController target) { }
    }

}
