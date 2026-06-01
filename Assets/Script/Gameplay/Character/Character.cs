
using NgocDev.Gameplay.Combat;
using NgocDev.Gameplay.Effect;
using NgocDev.Gameplay.Stat;
using UnityEngine;

namespace NgocDev.Gameplay.Character
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(StatController))]
    [RequireComponent(typeof(EffectController))]
    [RequireComponent(typeof(Movement))]
    public class Character : MonoBehaviour
    {
        public StatController stats;
        public EffectController effects;
        public Movement movement;
        public Damageable damageable;
    }
}
