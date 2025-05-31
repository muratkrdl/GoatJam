using System;
using _Scripts.Controllers;
using _Scripts.Events;
using UnityEngine;

namespace _Scripts.Managers
{
    public class PlayerInputManager : MonoBehaviour
    {
        private PlayerAnimationController _animationController;
        private bool _isHolding = true;

        private void Awake()
        {
            _animationController = GetComponent<PlayerAnimationController>();
        }

        private void OnEnable()
        {
            PlayerInputEvents.Instance.onHolding += OnReleasePlayer;
            PlayerInputEvents.Instance.onHoldingCanceled += OnReleasePlayerCanceled;
        }

        private void OnReleasePlayer()
        {
            _isHolding = true;
            _animationController.ChangePlayerSprites(_isHolding);
        }

        private void OnReleasePlayerCanceled()
        {
            _isHolding = false;
            _animationController.ChangePlayerSprites(_isHolding);
        }

        public bool GetIsHolding() => _isHolding;

    }
}