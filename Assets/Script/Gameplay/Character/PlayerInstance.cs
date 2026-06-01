using UnityEngine;

namespace NgocDev.Gameplay.Character
{
    public class  PlayerInstance : MonoBehaviour
    {
        private CharacterPrefab _characterPrefab;

        public void Initialize()
        {
            
        }
        public void LoadCharacterPrefab(CharacterPrefab prefab)
        {
            _characterPrefab = prefab;
            Instantiate(_characterPrefab, transform);
        }
    }
}