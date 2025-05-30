using _Scripts.Abstracts.Classes;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Object.Platforms
{
    public class PatrolPlatform : BasePlatform
    {
        [SerializeField] private float3 movePos;
        [SerializeField] private float3 returnPos;

        [SerializeField] private Ease easeMode = Ease.Linear;
        [SerializeField] private float duration = 4f;

        protected override void Start()
        {
            base.Start();
            StartMove();
        }

        private void StartMove()
        {
            transform.DOMove(movePos, duration).SetEase(easeMode).OnComplete(() =>
            {
                transform.position = returnPos;
                StartMove();
            });
        }
        
    }
}