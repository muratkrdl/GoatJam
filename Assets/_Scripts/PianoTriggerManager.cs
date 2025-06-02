using UnityEngine;
using _Scripts.Managers;
using System.Collections.Generic;

namespace _Scripts.Controllers
{
    // Bu script her piano objesine eklenir
    public class PianoTriggerManager : MonoBehaviour
    {
        // T�m pianolar i�in ortak cooldown
        private static float _globalCooldownEndTime = 0f;
        private const float COOLDOWN_DURATION = 3f;

        // Bu piano i�in i�eride olan player par�alar�
        private HashSet<Collider2D> _playerPartsInside = new HashSet<Collider2D>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Player tag'ini kontrol et
            if (other.CompareTag("Player"))
            {
                // Bu player par�as�n� listeye ekle
                _playerPartsInside.Add(other);

                // E�er bu piano'ya ilk giren par�a ise ve global cooldown dolmu�sa
                if (_playerPartsInside.Count == 1 && Time.time >= _globalCooldownEndTime)
                {
                    // Ses �al ve cooldown'� ba�lat
                    _globalCooldownEndTime = Time.time + COOLDOWN_DURATION;
                    SoundManager.Instance?.PlayPiano();
                    Debug.Log($"Piano sound played! Next available at: {_globalCooldownEndTime}");
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerPartsInside.Remove(other);
            }
        }

        // Sahne yeniden y�klendi�inde veya obje yok edildi�inde temizlik
        private void OnDestroy()
        {
            _playerPartsInside.Clear();
        }

        // Debug i�in
        private void OnDrawGizmos()
        {
            if (_playerPartsInside != null && _playerPartsInside.Count > 0)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(transform.position, transform.localScale * 1.1f);
            }
        }
    }
}