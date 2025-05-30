using _Scripts.Abstracts.Classes;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Object.Platforms
{
    public class OneTimePatrolPlatform : BasePlatform
    {
        [SerializeField] private Transform movePosTransform;

        [SerializeField] private Ease easeMode = Ease.Linear;
        [SerializeField] private float duration = 4f;
        
        // TODO : OnCollisionMove
        public void StartMove()
        {
            visualObjectTransform.DOMove(movePosTransform.position, duration).SetEase(easeMode).OnComplete(() =>
            {
                visualObjectTransform.position = movePosTransform.position;
            });
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawLine(visualObjectTransform.position, movePosTransform.position);
        }
        
    }
}