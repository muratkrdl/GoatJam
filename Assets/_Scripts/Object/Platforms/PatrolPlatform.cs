using System;
using _Scripts.Abstracts.Classes;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Object.Platforms
{
    public class PatrolPlatform : BasePlatform
    {
        [SerializeField] private Transform movePosTransform;
        [SerializeField] private Transform returnPosTransform;

        [SerializeField] private Ease easeMode = Ease.Linear;
        [SerializeField] private float duration = 4f;

        protected override void Start()
        {
            base.Start();
            StartMove();
        }

        private void StartMove()
        {
            visualObjectTransform.DOMove(movePosTransform.position, duration).SetEase(easeMode).OnComplete(() =>
            {
                visualObjectTransform.position = returnPosTransform.position;
                StartMove();
            });
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawLine(returnPosTransform.position, movePosTransform.position);
        }
    }
}