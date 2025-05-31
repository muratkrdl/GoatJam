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
        [SerializeField] private SpringJoint2D handSpring;
        [SerializeField] private Rigidbody2D myRigidbody;
        [SerializeField] private Transform initialParent;
        
        private BoxCollider2D _handCollider;

        private Transform _currentHandedObstacle;
        private Rigidbody2D _connectedBody;

        private void Awake()
        {
            _handCollider = GetComponent<BoxCollider2D>();
        }

        private void OnEnable()
        {
            PhysicEvents.Instance.onHandCollisionEnter += OnHandCollisionEnter;
            PlayerInputEvents.Instance.onRelease += OnRelease;
        }

        private void OnHandCollisionEnter(OnHandCollisionEnterParams arg)
        {
            OnTriggerEnterFunc(arg.Other);
        }

        private void OnRelease()
        {
            OnTriggerExitFunc();
        }

        // TODO : Collision instead of trigger

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(ConstantsUtilities.Obstacles))
            {
                PhysicEvents.Instance.onHandCollisionEnter?.Invoke(new OnHandCollisionEnterParams()
                {
                    Hand = this,
                    Other = other
                });
            }
        }

        private void OnTriggerEnterFunc(Collision2D other)
        {
            handSpring.enabled = true;
            myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            _currentHandedObstacle = other.transform;
            _connectedBody = other.gameObject.GetComponent<Rigidbody2D>();
            myRigidbody.transform.SetParent(_currentHandedObstacle);
            handSpring.connectedBody = _connectedBody;
            handSpring.connectedAnchor = _currentHandedObstacle.position;
        }
        
        private void OnTriggerExitFunc()
        {
            // Exit from obstacle
            if (!_currentHandedObstacle) return;
            
            Collider2D otherCollider = _currentHandedObstacle.GetComponent<Collider2D>();
            IgnoreCollider(otherCollider).Forget();
            
            handSpring.enabled = false;
            myRigidbody.transform.SetParent(initialParent);
            myRigidbody.bodyType = RigidbodyType2D.Dynamic;
            _currentHandedObstacle = null;
            _connectedBody = null;
            handSpring.connectedBody = null;
            handSpring.connectedAnchor = Vector2.zero;
        }

        private async UniTaskVoid IgnoreCollider(Collider2D otherCollider)
        {
            Physics2D.IgnoreCollision(_handCollider, otherCollider, true);
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            Physics2D.IgnoreCollision(_handCollider, otherCollider, false);
        }
        
    }
}