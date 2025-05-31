using System;
using _Scripts.Abstracts.Classes;
using _Scripts.Events;
using _Scripts.Keys;
using _Scripts.Object;
using _Scripts.Object.Platforms;
using UnityEngine;

namespace _Scripts.Managers
{
    public class PlayerPhysicManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private float slimeJumpForce;
        
        [SerializeField] private float firstJumpForce;
        
        [SerializeField] private float releaseJumpForce;
        [SerializeField] private float divideReleaseForce;
        
        private Transform _currentHoldingPlatform;

        private void OnEnable()
        {
            PhysicEvents.Instance.onCollisionSlime += OnCollisionSlime;
            PlayerInputEvents.Instance.onHoldingCanceled += OnReleasePlayer;
            PhysicEvents.Instance.onHandCollisionEnter += OnHandCollisionEnter;
        }
        
        private void OnReleasePlayer()
        {
            // TODO : Calculate Force Direction
            if (!_currentHoldingPlatform) return;
            
            BasePlatform platform = _currentHoldingPlatform.GetComponentInParent<BasePlatform>();

            Vector2 direction = (_currentHoldingPlatform.transform.position - body.transform.position).normalized;

            Vector2 realDirection = direction * (platform.GetCurrentRpm() / divideReleaseForce);
            
            ApplyForceBody(realDirection);
            
            Debug.Log(realDirection);

            _currentHoldingPlatform = null;
        }

        private void OnHandCollisionEnter(OnHandCollisionEnterParams arg0)
        {
            _currentHoldingPlatform = arg0.Other.transform;
        }

        private void OnCollisionSlime(SlimeObject slimeObj)
        {
            ApplyForceBody(slimeObj.GetReflectDirection() * slimeJumpForce);
        }

        private void OnDisable()
        {
            PhysicEvents.Instance.onCollisionSlime -= OnCollisionSlime;
            PlayerInputEvents.Instance.onHolding -= OnReleasePlayer;
            PhysicEvents.Instance.onHandCollisionEnter -= OnHandCollisionEnter;
        }

        private void ApplyForceBody(Vector2 direction)
        {
            body.linearVelocity = Vector2.zero;
            body.AddForce(direction, ForceMode2D.Impulse);
        }

        private void Start()
        {
            body.AddForce(Vector2.up * firstJumpForce, ForceMode2D.Impulse);
        }
        
    }
}