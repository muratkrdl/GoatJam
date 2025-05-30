using _Scripts.Extensions;
using _Scripts.Keys;
using UnityEngine.Events;

namespace _Scripts.Events
{
    public class PhysicEvents : MonoSingleton<PhysicEvents>
    {
        public UnityAction<OnHandCollisionEnterParams> onHandCollisionEnter;
    }
}