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
        
        private void Start()
        {
            _currentDistance = Mathf.Abs(Mathf.RoundToInt(playerHeadTransform.position.y - appleTransform.position.y));
            _maxMetres = _currentDistance * 5;

            _currentMetres = 0;
        }

        private void Update()
        {
            CalculateMetres();
        }

        private void CalculateMetres()
        {
            _currentDistance = Mathf.Abs(Mathf.RoundToInt(playerHeadTransform.position.y - appleTransform.position.y));
            _currentMetres = _maxMetres - _currentDistance * 5;
            metreText.text = _currentMetres.ToString();
        }
        
    }
}