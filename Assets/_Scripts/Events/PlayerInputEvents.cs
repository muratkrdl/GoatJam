using _Scripts.Extensions;
using UnityEngine.Events;

namespace _Scripts.Events
{
    public class PlayerInputEvents : MonoSingleton<PlayerInputEvents>
    {
        public UnityAction onHolding;
        public UnityAction onHoldingCanceled;
        
        // public UnityAction onHand;
        // public UnityAction onRotateStarted;
        // public UnityAction onRotateCanceled;
    }
}