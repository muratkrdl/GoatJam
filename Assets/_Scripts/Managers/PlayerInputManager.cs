using _Scripts.Events;
using UnityEngine;

namespace _Scripts.Managers
{
    public class PlayerInputManager : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerInputEvents.Instance.onRotateStarted += OnRotateStarted;
            PlayerInputEvents.Instance.onRotateCanceled += OnRotateCanceled;
        }

        private void OnRotateStarted(Vector2 arg0)
        {
            
        }

        private void OnRotateCanceled()
        {
            
        }

    }
}