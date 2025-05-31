using _Scripts.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Events
{
    public class PlayerInputEvents : MonoSingleton<PlayerInputEvents>
    {
        public UnityAction onRelease;
        public UnityAction<Vector2> onRotate;
    }
}