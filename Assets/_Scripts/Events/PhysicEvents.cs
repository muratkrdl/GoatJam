using _Scripts.Extensions;
using _Scripts.Keys;
using _Scripts.Object;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Events
{
    public class PhysicEvents : MonoBehaviour
    {
        public static PhysicEvents Instance;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        
        public UnityAction<OnHandCollisionEnterParams> onHandCollisionEnter;
        public UnityAction<SlimeObject> onCollisionSlime;
    }
}