using UnityEngine;

namespace NgocDev.Gameplay.Player
{

    public class PlayerController : MonoBehaviour
    {
        public GameObject playerObject { get; private set; }
        private void Awake()
        {
            playerObject = this.gameObject;
        }



        public void InitializeCharacter()
        {
            //TODO: Load character model;
        }
    }
}