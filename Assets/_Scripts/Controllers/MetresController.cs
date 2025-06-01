using System;
using TMPro;
using UnityEngine;

namespace _Scripts.Controllers
{
    public class MetresController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI metreText;
        [SerializeField] private Transform appleTransform;
        [SerializeField] private Transform playerHeadTransform;

        private int _maxMetres;
        private int _currentMetres;

        private int _currentDistance;

        private Vector2 _baseScale;

        private Vector2 _maxReachPos;
        
        private void Start()
        {
            _baseScale = metreText.transform.localScale;
            _maxReachPos = playerHeadTransform.position;
            
            _currentDistance = Mathf.Abs(Mathf.RoundToInt(playerHeadTransform.position.y - appleTransform.position.y));
            _maxMetres = _currentDistance * 5;

            _currentMetres = 0;
        }

        private void Update()
        {
            CalculateMetres();
            if (playerHeadTransform.position.y >= _maxReachPos.y)
            {
                _maxReachPos = playerHeadTransform.position;
            }
        }

        private void CalculateMetres()
        {
            _currentDistance = Mathf.Abs(Mathf.RoundToInt(_maxReachPos.y - appleTransform.position.y));
            _currentMetres = _maxMetres - _currentDistance * 5;
            metreText.text = _currentMetres.ToString();
            
            metreText.transform.localScale = _baseScale + new Vector2((float)_currentMetres/_maxMetres, (float)_currentMetres/_maxMetres);
        }
        
    }
}