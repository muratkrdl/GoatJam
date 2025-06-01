using _Scripts.Events;
using _Scripts.Keys;
using UnityEngine;

namespace _Scripts.Managers
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private float rotateSpeed;

        private bool _canChangeScale = true;
        
        private Vector2 _baseScale = Vector2.one;
        
        private void OnEnable()
        {
            PhysicEvents.Instance.onHandCollisionEnter += OnHandCollisionEnter;
            PlayerInputEvents.Instance.onRelease += OnRelease;
        }

        private void OnRelease()
        {
            if (!_canChangeScale) return;
            
            Vector2 bodyScales = body.transform.localScale;
            _baseScale = bodyScales;
            bodyScales *= -1;
            body.transform.localScale = bodyScales;
        }

        private void OnHandCollisionEnter(OnHandCollisionEnterParams arg0)
        {
            _canChangeScale = false;
            body.transform.localScale = _baseScale;
        }
        
        public void ExitFromObstacle()
        {
            _canChangeScale = true;
        }

    }
}