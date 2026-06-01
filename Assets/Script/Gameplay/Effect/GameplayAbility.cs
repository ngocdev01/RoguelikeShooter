using NgocDev.Gameplay.Effect;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Script.Gameplay.Effect
{
    [CreateAssetMenu(fileName = "New Gameplay Ability", menuName = "Gameplay/Ability")]
    public class GameplayAbility : ScriptableObject
    {
        public GameAbility ability;
        public GameplayEffect effect;
    }
}
