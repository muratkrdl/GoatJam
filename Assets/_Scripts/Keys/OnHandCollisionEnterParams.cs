using _Scripts.Controllers;
using UnityEngine;

namespace _Scripts.Keys
{
    public struct OnHandCollisionEnterParams
    {
        public HandController Hand;
        public Collision2D Other;
    }
}