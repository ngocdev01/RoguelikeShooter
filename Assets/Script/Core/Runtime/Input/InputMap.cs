using UnityEngine;
using UnityEngine.InputSystem;

namespace NgocDev.Core.Input
{
    [CreateAssetMenu(fileName = "InputMap", menuName = "NgocDev/Input Map")]
    public class InputMap : ScriptableObject
    {
        public InputActionReference moveAction;
        public InputActionReference jumpAction;
        
    }
}