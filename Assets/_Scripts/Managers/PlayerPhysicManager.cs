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
        [SerializeField] private Transform center;
        
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private float slimeJumpForce;
        
        [SerializeField] private float firstJumpForce;
        
        [SerializeField] private float releaseJumpForce;
        
        private Transform _currentHoldingPlatform;

        private void OnEnable()
        {
            PhysicEvents.Instance.onHandCollisionEnter += OnHandCollisionEnter;
            PhysicEvents.Instance.onCollisionSlime += OnCollisionSlime;
            PlayerInputEvents.Instance.onRelease += OnReleasePlayer;
        }

        
            
        
        private void OnReleasePlayer()
        {
            // TODO : Calculate Force Direction
            if (!_currentHoldingPlatform) return;
            
            Vector2 direction = (body.transform.position - center.position).normalized;

            Vector2 realDirection = direction * releaseJumpForce;
            
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
            ApplyForceBody(slimeObj.GetReflectDirection().normalized * slimeJumpForce);
        }

        private void OnDisable()
        {
            PhysicEvents.Instance.onCollisionSlime -= OnCollisionSlime;
            PlayerInputEvents.Instance.onRelease -= OnReleasePlayer;
            PhysicEvents.Instance.onHandCollisionEnter -= OnHandCollisionEnter;
        }

        private void ApplyForceBody(Vector2 direction)
        {
            body.linearVelocity = Vector2.zero;
            body.AddForce(direction, ForceMode2D.Impulse);
        }

        private void Start()
        {
            Invoke(nameof(OnReleasePlayer), 0.1f); // 0.1 saniye gecikme ile
            SoundManager.Instance.PlayJump();
            body.AddForce(Vector2.up * firstJumpForce, ForceMode2D.Impulse);
        }
        
    }
}