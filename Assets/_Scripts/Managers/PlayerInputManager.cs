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

        // private Vector2 _baseScale = Vector2.one;

        private void OnEnable()
        {
            PhysicEvents.Instance.onHandCollisionEnter += OnHandCollisionEnter;
            PlayerInputEvents.Instance.onRelease += OnRelease;
        }

        private void OnRelease()
        {
            if (!_canChangeScale) return;

            Vector3 bodyScales = body.transform.localScale;
            bodyScales *= -1;
            bodyScales.z = 1;
            body.transform.localScale = bodyScales;
        }

        private void OnHandCollisionEnter(OnHandCollisionEnterParams arg0)
        {
            _canChangeScale = false;

            Vector3 bodyScales = body.transform.localScale;

            if (bodyScales.x < 0)
            {
                bodyScales *= -1;
                bodyScales.z = 1;
            }

            body.transform.localScale = bodyScales;
        }

        public void ExitFromObstacle()
        {
            _canChangeScale = true;
            body.rotation = -180f;
        }

        private void OnDisable()
        {
            PhysicEvents.Instance.onHandCollisionEnter -= OnHandCollisionEnter;
            PlayerInputEvents.Instance.onRelease -= OnRelease;
        }

    }
}