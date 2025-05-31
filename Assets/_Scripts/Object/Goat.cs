using System;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Object
{
    public class Goat : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private Ease easeMode;
        
        private void Start()
        {
            AnimateGoat();
        }

        public void AnimateGoat()
        {
            Sequence sequence = DOTween.Sequence();
            
            sequence
                .Append(ScaleVisualCard(1.1f))
                .Append(ScaleVisualCard(1.05f))
                .Append(ScaleVisualCard(1.15f))
                .Append(ScaleVisualCard(1.1f))
                .Append(ScaleVisualCard(1.20f));
        }
        
        private Tween ScaleVisualCard(float target)
        {
            return transform.DOScale(target, duration).SetEase(easeMode);
        }
        
    }
}