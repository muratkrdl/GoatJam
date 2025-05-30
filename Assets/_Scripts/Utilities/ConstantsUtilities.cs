using UnityEngine;

namespace Runtime.Utilities
{
    public static class ConstantsUtilities
    {
        
#region Vectors
        
        public static readonly Vector2 Zero2 = Vector2.zero;
        public static readonly Vector2 One2 = Vector2.one;
        
        public static readonly Vector3 Zero3 = Vector3.zero;
        public static readonly Vector3 One3 = Vector3.one;
        
#endregion

#region Layer

        public static readonly int AllLayerMask = ~0;
        public static readonly int PlayerLayer = LayerMask.NameToLayer("Player");
        public static readonly int ObstacleLayer = LayerMask.NameToLayer("Obstacle");

#endregion

#region LayerMask
        
        public static readonly LayerMask PlayerLayerMask = LayerMask.GetMask("Player");
        public static readonly LayerMask EnemyAndObstacleLayerMask = LayerMask.GetMask("Enemy", "Obstacle");

#endregion
        
#region Tags

        public const string Player = "Player";
        public const string Obstacles = "Obstacles";
        public const string Ground = "Ground";
        public const string Hand = "Hand";

#endregion
            
#region Animation Hash

        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int Walk = Animator.StringToHash("Walk");
        public static readonly int Run = Animator.StringToHash("Run");
        
        public static readonly int IsWalking = Animator.StringToHash("IsWalking");

        public static readonly int Speed = Animator.StringToHash("Speed");

        public static readonly int AttackIndex = Animator.StringToHash("AttackIndex");

#endregion

    }
}