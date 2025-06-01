using _Scripts.Events;
using _Scripts.Keys;
using _Scripts.Object;
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
            PlayerInputEvents.Instance.onReleaseFinished += OnReleaseFinished;
        }

        private void OnReleaseFinished()
        {
            Vector2 direction = -(body.transform.position - center.position).normalized;

            Vector2 realDirection = direction * releaseJumpForce;

            ApplyForceBody(realDirection);
            
            Debug.LogError(realDirection);
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
            PhysicEvents.Instance.onHandCollisionEnter -= OnHandCollisionEnter;
            PlayerInputEvents.Instance.onReleaseFinished -= OnReleaseFinished;
        }

        private void ApplyForceBody(Vector2 direction)
        {
            body.linearVelocity = Vector2.zero;
            body.AddForce(direction, ForceMode2D.Impulse);
        }

        private void Start()
        {
            SoundManager.Instance.PlayJump();
            ApplyForceBody(Vector2.up * firstJumpForce);
        }
        
    }
}