using NgocDev.Gameplay.VFX;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace NgocDev.Gameplay.UI
{
    internal class UITest : MonoBehaviour
    {
        public UIDocument document;
        public float spawnRate = 1f;
        public float lastSpawnTime = 0f;
        public Font font;

        private void Start()
        {
            lastSpawnTime = Time.time;
        }

        private void Update()
        {
            if (Time.time - lastSpawnTime >= spawnRate)
            {
                var damage = new Label();
                damage.style.unityFontDefinition = new StyleFontDefinition(font);
                damage.text = UnityEngine.Random.Range(10, 100).ToString();
                damage.style.fontSize = 100;
                damage.style.position = Position.Absolute;

                List<StylePropertyName> properties = new List<StylePropertyName>();
                properties.Add(new StylePropertyName("rotate"));
                //Given a VisualElement named "element"...
                damage.style.transitionProperty = new StyleList<StylePropertyName>(properties);
                List<TimeValue> durations = new List<TimeValue>();
              
                durations.Add(new TimeValue(1F, TimeUnit.Second));
                //Given a VisualElement named "element"...
                damage.style.transitionDuration = new StyleList<TimeValue>(durations);
                List<EasingFunction> easingFunctions = new List<EasingFunction>();
                easingFunctions.Add(new EasingFunction(EasingMode.Linear));
                //Given a VisualElement named "element"...
                damage.style.transitionTimingFunction = new StyleList<EasingFunction>(easingFunctions);
                document.rootVisualElement.Add(damage);
                damage.style.translate = new Vector2(Random.Range(0, 100), Random.Range(0, 100));
                lastSpawnTime = Time.time;
            }
        }
    }
}
