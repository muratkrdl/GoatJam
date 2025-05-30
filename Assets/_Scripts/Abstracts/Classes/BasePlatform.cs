using System;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Abstracts.Classes
{
    public abstract class BasePlatform : MonoBehaviour
    {
        [SerializeField] private float minRpm;
        [SerializeField] private float maxRpm;
        
        [SerializeField] private float minAcceleration;
        [SerializeField] private float maxAcceleration;
        [SerializeField] private float accelerationTime;

        [SerializeField] private float randomizeTime;

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
            transform.Rotate(_currentRpmVec * Time.fixedDeltaTime);
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
        
    }
}