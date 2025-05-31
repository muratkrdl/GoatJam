using System;
using _Scripts.Controllers;
using _Scripts.Events;
using UnityEngine;

namespace _Scripts.Managers
{
    public class PlayerInputManager : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerInputEvents.Instance.onRelease += OnReleasePlayer;
        }

        private void OnReleasePlayer()
        {
            
        }

    }
}