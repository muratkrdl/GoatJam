using System;
using _Scripts.Events;
using _Scripts.Extensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Managers
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private PlayerInputActions _input;

        private void OnEnable()
        {
            _input = new PlayerInputActions();
            _input.Enable();

            _input.Player.Holding.started += OnRelease;
        }

        private void OnRelease(InputAction.CallbackContext obj)
        {
            PlayerInputEvents.Instance.onRelease.Invoke();
        }
        
    }
}