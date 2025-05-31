using System;
using _Scripts.Events;
using _Scripts.Object;
using UnityEngine;

namespace _Scripts
{
    public class FirstTimeJump : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private float slimeJumpForce;
        
        [SerializeField] private float firstJumpForce;

        private void OnEnable()
        {
            PhysicEvents.Instance.onCollisionSlime += OnCollisionSlime;
        }

        private void OnCollisionSlime(SlimeObject slimeObj)
        {
            body.linearVelocity = Vector2.zero;
            body.AddForce(slimeObj.GetReflectDirection() * slimeJumpForce, ForceMode2D.Impulse);
        }

        private void Start()
        {
            body.AddForce(Vector2.up * firstJumpForce, ForceMode2D.Impulse);
        }
        
    }
}