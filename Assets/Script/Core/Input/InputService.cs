using NgocDev.Core.ServiceLocator;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace NgocDev.Core.Input
{

    public class InputService : MonoService
    {
        private InputMap _inputMap;
        public Vector2 MoveInput => _inputMap.moveAction.action.ReadValue<Vector2>();
        public bool JumpInput => _inputMap.jumpAction.action.WasPressedThisFrame();

       

        private void Awake()
        {
            _inputMap = Addressables.LoadAssetAsync<InputMap>("InputMap").WaitForCompletion();
        }
    }
}