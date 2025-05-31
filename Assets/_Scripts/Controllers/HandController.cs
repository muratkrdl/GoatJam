using System;
using _Scripts.Events;
using _Scripts.Keys;
using _Scripts.Managers;
using Cysharp.Threading.Tasks;
using Runtime.Utilities;
using UnityEngine;

namespace _Scripts.Controllers
{
    public class HandController : MonoBehaviour
    {
        private PlayerInputManager _playerInputManager;
        private SpringJoint2D _handSpring;
        private PolygonCollider2D _handCollider;
        private Rigidbody2D _rigidbody;
        private Transform _initialParent;

        private Transform _currentHandedObstacle;
        private Rigidbody2D _connectedBody;

        private void Awake()
        {
            _playerInputManager = GetComponentInParent<PlayerInputManager>();
            _handSpring = GetComponent<SpringJoint2D>();
            _handCollider = GetComponent<PolygonCollider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _initialParent =  transform.parent;
        }

        private void OnEnable()
        {
            PhysicEvents.Instance.onHandCollisionEnter += OnHandCollisionEnter;
            PlayerInputEvents.Instance.onHoldingCanceled += OnHoldingCanceled;
        }

        private void OnHandCollisionEnter(OnHandCollisionEnterParams arg)
        {
            if (arg.Hand != this) return;
            OnTriggerEnterFunc(arg.Other);
        }

        private void OnHoldingCanceled()
        {
            OnTriggerExitFunc();
        }

        // TODO : Collision instead of trigger

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(ConstantsUtilities.Obstacles))
            {
                if (!_playerInputManager.GetIsHolding()) return;
                
                PhysicEvents.Instance.onHandCollisionEnter?.Invoke(new OnHandCollisionEnterParams()
                {
                    Hand = this,
                    Other = other
                });
            }
        }

        private void OnTriggerEnterFunc(Collision2D other)
        {
            _handSpring.enabled = true;
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
            _currentHandedObstacle = other.transform;
            _connectedBody = other.gameObject.GetComponent<Rigidbody2D>();
            transform.SetParent(_currentHandedObstacle);
            _handSpring.connectedBody = _connectedBody;
            _handSpring.connectedAnchor = _currentHandedObstacle.position;
        }
        
        private void OnTriggerExitFunc()
        {
            // Exit from obstacle
            if (!_currentHandedObstacle) return;
            Collider2D otherCollider = _currentHandedObstacle.GetComponent<Collider2D>();
            IgnoreCollider(otherCollider).Forget();
            
            _handSpring.enabled = false;
            transform.SetParent(_initialParent);
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _currentHandedObstacle = null;
            _connectedBody = null;
            _handSpring.connectedBody = null;
            _handSpring.connectedAnchor = Vector2.zero;
        }

        private async UniTaskVoid IgnoreCollider(Collider2D otherCollider)
        {
            Physics2D.IgnoreCollision(_handCollider, otherCollider, true);
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            Physics2D.IgnoreCollision(_handCollider, otherCollider, false);
        }
        
    }
}