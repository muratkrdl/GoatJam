using System;
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
    }
}