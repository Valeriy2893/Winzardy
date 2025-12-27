using _Project.Scripts.Core.ECS.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Infrastructure.Input.KeyboardInput
{
    public sealed class KeyboardInputAdapter : MonoBehaviour
    {
        public InputData Current { get; private set; }

        private InputAction _move;

        private void Awake()
        {
            _move = new InputAction(name: "Move", type: InputActionType.Value, expectedControlType: "Vector2");

            _move.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            
            _move.performed += OnMove;
            _move.canceled += OnMove;
        }

        private void OnEnable()
        {
            _move.Enable();
        }

        private void OnDisable()
        {
            _move.Disable();
        }

        private void OnDestroy()
        {
            _move.performed -= OnMove;
            _move.canceled -= OnMove;
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValue<Vector2>();
            Current = new InputData { X = value.x, Z = value.y };
        }
    }
}