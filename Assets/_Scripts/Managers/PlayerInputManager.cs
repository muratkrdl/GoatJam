using System;
using _Scripts.Events;
using _Scripts.Keys;
using UnityEngine;

namespace _Scripts.Managers
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private float rotateSpeed;

        private bool _isHoldingPlatform = false;
        private bool _hasPerformedFirstJump = false;

        private void OnEnable()
        {
            PhysicEvents.Instance.onHandCollisionEnter += OnHandCollisionEnter;
            PlayerInputEvents.Instance.onRelease += OnRelease;
            PlayerInputEvents.Instance.onReleaseFinished += OnReleaseFinished;
        }

        private void OnReleaseFinished()
        {
            // Sadece platform tutuluyorsa ve ilk z�plama yap�ld�ysa �al��s�n
            if (_isHoldingPlatform && _hasPerformedFirstJump)
            {
                ExitFromObstacle();
                _isHoldingPlatform = false;
            }
        }

        private void OnRelease()
        {
            // �lk z�plama yap�ld�ysa scale de�i�ikli�i yap
            if (_hasPerformedFirstJump)
            {
                Vector3 bodyScales = body.transform.localScale;
                bodyScales *= -1;
                bodyScales.z = 1;
                body.transform.localScale = bodyScales;
            }
            else
            {
                // �lk z�plama hen�z yap�lmad�ysa, sadece i�aretle
                _hasPerformedFirstJump = true;
            }
        }

        private void OnHandCollisionEnter(OnHandCollisionEnterParams arg0)
        {
            _isHoldingPlatform = true;
            Vector3 bodyScales = body.transform.localScale;
            if (bodyScales.x < 0)
            {
                bodyScales *= -1;
                bodyScales.z = 1;
            }
            body.transform.localScale = bodyScales;
        }

        private void ExitFromObstacle()
        {
            body.rotation = -180f;
        }

        private void OnDisable()
        {
            PhysicEvents.Instance.onHandCollisionEnter -= OnHandCollisionEnter;
            PlayerInputEvents.Instance.onRelease -= OnRelease;
            PlayerInputEvents.Instance.onReleaseFinished -= OnReleaseFinished;
        }
    }
}