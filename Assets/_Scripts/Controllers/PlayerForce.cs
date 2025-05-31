using System;
using _Scripts.Abstracts.Classes;
using _Scripts.Events;
using _Scripts.Keys;
using UnityEngine;

namespace _Scripts.Controllers
{
    public class PlayerForce : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D bodyRigidbody;

        private BasePlatform _currentHoldingPlatform;
        
        public void OnEnable()
        {
            PlayerInputEvents.Instance.onHolding += OnReleasePlayer;
            PhysicEvents.Instance.onHandCollisionEnter += OnHandCollisionEnter;
        }
        
        private void OnReleasePlayer()
        {
            
        }

        private void OnHandCollisionEnter(OnHandCollisionEnterParams arg0)
        {
            _currentHoldingPlatform = arg0.Other.transform.GetComponent<BasePlatform>();
        }

    }
}