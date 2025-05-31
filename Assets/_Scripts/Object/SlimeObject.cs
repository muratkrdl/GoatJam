using UnityEngine;

namespace _Scripts.Object
{
    public class SlimeObject : MonoBehaviour
    {
        [SerializeField] private Vector2 reflectDirection;
        
        public Vector2 GetReflectDirection() => reflectDirection;
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawLine(transform.position, reflectDirection);
        }
        
    }
}