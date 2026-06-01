
using System.Collections.Generic;
using UnityEngine;

namespace NgocDev.Gameplay.Combat
{
    public class DamagePackage
    {
        public float damageAmount;
        public bool isCritical;
        public bool isTrueDamage;
        public bool isCanceled;
        public DamageType damageType;
        public GameObject source;
    }

    public interface IDamageModifier
    {
        public void ModifyDamage(DamagePackage damagePackage, Damageable target);
    }


    public class DefenseModifier : IDamageModifier
    {
        public float defenseValue;
        public void ModifyDamage(DamagePackage damagePackage, Damageable target)
        {
            if (!damagePackage.isTrueDamage)
            {
                damagePackage.damageAmount -= defenseValue;
                if (damagePackage.damageAmount < 0)
                {
                    damagePackage.damageAmount = 0;
                }
            }

            // True damage ignores defense, do nothing

        }

    }
    public class Damageable : MonoBehaviour
    {
        public List<IDamageModifier> damageModifiers = new List<IDamageModifier>();

        public void ApplyDamage(DamagePackage damagePackage)
        {
            foreach (var modifier in damageModifiers)
            {
                modifier.ModifyDamage(damagePackage, this);
                if (damagePackage.isCanceled)
                {
                    return;
                }
            }

        }
    }

    public class DamageImmune : IDamageModifier
    {
        public void ModifyDamage(DamagePackage damagePackage, Damageable target)
        {
            if (damagePackage.damageType == target.isActiveAndEnabled)
            {
                damagePackage.isCanceled = true;
            }
        }
    }


    public class CombatController
    {
    }
}
