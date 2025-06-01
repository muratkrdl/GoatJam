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

        private void OnEnable()
        {
            PhysicEvents.Instance.onHandCollisionEnter += OnHandCollisionEnter;
            PlayerInputEvents.Instance.onRelease += OnRelease;
            PlayerInputEvents.Instance.onReleaseFinished += OnReleaseFinished;
        }

        private void OnReleaseFinished()
        {
            ExitFromObstacle();
        }

        private void OnRelease()
        {
            Vector3 bodyScales = body.transform.localScale;
            bodyScales *= -1;
            bodyScales.z = 1;
            body.transform.localScale = bodyScales;
        }

        private void OnHandCollisionEnter(OnHandCollisionEnterParams arg0)
        {
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