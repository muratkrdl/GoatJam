using _Scripts.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Events
{
    public class PlayerInputEvents : MonoBehaviour
    {
        public static PlayerInputEvents Instance;

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
        
        public UnityAction onRelease;
    }
}