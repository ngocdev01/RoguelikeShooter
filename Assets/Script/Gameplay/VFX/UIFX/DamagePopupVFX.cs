using UnityEngine;
using UnityEngine.UIElements;

namespace NgocDev.Gameplay.VFX
{
    public class DamagePopupVFX : VisualElement
    {
        public float DamageAmount { get; set; }
        public DamagePopupVFX()
        {
            var text = new Label();
            text.text = DamageAmount.ToString();
            Add(text);
        }

    }
}