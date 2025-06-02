using UnityEngine;
using _Scripts.Managers;
using System.Collections.Generic;

namespace _Scripts.Controllers
{
    // Bu script her piano objesine eklenir
    public class PianoTriggerManager : MonoBehaviour
    {
        // Tüm pianolar için ortak cooldown
        private static float _globalCooldownEndTime = 0f;
        private const float COOLDOWN_DURATION = 3f;

        // Bu piano için içeride olan player parçalarý
        private HashSet<Collider2D> _playerPartsInside = new HashSet<Collider2D>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Player tag'ini kontrol et
            if (other.CompareTag("Player"))
            {
                // Bu player parçasýný listeye ekle
                _playerPartsInside.Add(other);

                // Eðer bu piano'ya ilk giren parça ise ve global cooldown dolmuþsa
                if (_playerPartsInside.Count == 1 && Time.time >= _globalCooldownEndTime)
                {
                    // Ses çal ve cooldown'ý baþlat
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

        // Sahne yeniden yüklendiðinde veya obje yok edildiðinde temizlik
        private void OnDestroy()
        {
            _playerPartsInside.Clear();
        }

        // Debug için
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