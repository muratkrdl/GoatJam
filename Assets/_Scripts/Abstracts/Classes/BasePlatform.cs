using System;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Abstracts.Classes
{
    public abstract class BasePlatform : MonoBehaviour
    {
        [SerializeField] protected Transform visualObjectTransform;
        
        [SerializeField] private float minRpm = -50;
        [SerializeField] private float maxRpm = 50;
        
        [SerializeField] private float minAcceleration = -20;
        [SerializeField] private float maxAcceleration = 20;
        [SerializeField] private float accelerationTime = 1f;

        [SerializeField] private float randomizeTime = 7.5f;

        private float3 _currentRpmVec = float3.zero;
        private float _currentRpm;
        private float _currentAcceleration;
        
        protected virtual void Start()
        {
            Randomize();
            IncreaseRpmByAcceleration().Forget();
            RandomizeRpmAcceleration().Forget();
        }

        private void Update()
        {
            visualObjectTransform.Rotate(_currentRpmVec * Time.fixedDeltaTime);
        }
        
        private void Randomize()
        {
            _currentRpm = Random.Range(minRpm, maxRpm);
            UpdateRpmVec();
            _currentAcceleration = Random.Range(minAcceleration, maxAcceleration);
        }

        private async UniTaskVoid IncreaseRpmByAcceleration()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(accelerationTime));
                _currentRpm += _currentAcceleration;
                UpdateRpmVec();
                _currentRpm = Mathf.Min(_currentRpm, maxRpm);
            }
        }
        
        private async UniTaskVoid RandomizeRpmAcceleration()
        {
            while (true)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(randomizeTime));
                Randomize();
            }
        }

        private void UpdateRpmVec()
        {
            _currentRpmVec.z = _currentRpm;
        }
        
        public float GetCurrentRpm() => _currentRpm;
        
    }
}