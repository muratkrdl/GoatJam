using _Scripts.Extensions;
using _Scripts.Keys;
using _Scripts.Object;
using UnityEngine.Events;

namespace _Scripts.Events
{
    public class PhysicEvents : MonoSingleton<PhysicEvents>
    {
        public UnityAction<OnHandCollisionEnterParams> onHandCollisionEnter;
        public UnityAction<SlimeObject> onCollisionSlime;
    }
}