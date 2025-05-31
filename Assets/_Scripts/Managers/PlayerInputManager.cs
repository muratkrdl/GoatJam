using _Scripts.Events;
using UnityEngine;

namespace _Scripts.Managers
{
    public class PlayerInputManager : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerInputEvents.Instance.onRelease += OnReleasePlayer;
            PlayerInputEvents.Instance. += OnReleasePlayer;
        }

        private void OnReleasePlayer()
        {
            
        }

    }
}