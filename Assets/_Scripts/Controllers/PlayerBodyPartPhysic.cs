using System;
using _Scripts.Events;
using _Scripts.Object;
using Runtime.Utilities;
using UnityEngine;

namespace _Scripts.Controllers
{
    public class PlayerBodyPartPhysic : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(ConstantsUtilities.Spike))
            {
                // TODO : Player Dead
                Debug.Log("Spike");
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<SlimeObject>(out var slime))
            {
                PhysicEvents.Instance.onCollisionSlime?.Invoke(slime);
            }
        }
        
    }
}