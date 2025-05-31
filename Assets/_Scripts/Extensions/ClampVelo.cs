using System;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Extensions
{
    public class ClampVelo : MonoBehaviour
    {
        [SerializeField] private float minXVelocity;
        [SerializeField] private float maxXVelocity;
        
        [SerializeField] private float minYVelocity;
        [SerializeField] private float maxYVelocity;
        
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            float2 newVelocity = _rigidbody.linearVelocityX;
            newVelocity.x = Mathf.Clamp(newVelocity.x, minXVelocity, maxXVelocity);
            newVelocity.y = Mathf.Clamp(newVelocity.y, minYVelocity, maxYVelocity);
            
            _rigidbody.linearVelocity = newVelocity;
        }
        
    }
}
