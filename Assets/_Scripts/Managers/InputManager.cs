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
        private bool _isHolding = false; // Space tuþunun durumunu takip etmek için

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                // OnEnable henüz çalýþmadýysa _input null olacaktýr
                // Bu yüzden doðrudan Destroy kullanýyoruz
                Destroy(gameObject);
                return; // Destroy'dan sonra kod çalýþmasýný durdur
            }
        }

        private PlayerInputActions _input;

        private void OnEnable()
        {
            _input = new PlayerInputActions();
            _input.Enable();
            _input.Player.Holding.started += OnHoldingStarted;
            _input.Player.Holding.canceled += OnHoldingCanceled;
        }

        private void OnHoldingStarted(InputAction.CallbackContext obj)
        {
            _isHolding = true;
            PlayerInputEvents.Instance.onRelease.Invoke();
        }

        private void OnHoldingCanceled(InputAction.CallbackContext obj)
        {
            if (_isHolding)
            {
                _isHolding = false;
                PlayerInputEvents.Instance.onReleaseFinished.Invoke();
            }
        }

        private void OnDisable()
        {
            if (_input != null)
            {
                _input.Player.Holding.started -= OnHoldingStarted;
                _input.Player.Holding.canceled -= OnHoldingCanceled;
                _input.Disable();
            }
        }
    }
}