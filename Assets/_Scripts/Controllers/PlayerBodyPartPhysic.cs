using System;
using _Scripts.Events;
using _Scripts.Object;
using Runtime.Utilities;
using UnityEngine;

namespace _Scripts.Controllers
{
    public class PlayerBodyPartPhysic : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(ConstantsUtilities.Spike))
            {
                // TODO : Player Dead
                Debug.Log("Spike");
                SoundManager.Instance?.PlayDeathbySpike();
                // Death panel'i göster ve oyunu durdur
                UIManager uiManager = FindFirstObjectByType<UIManager>();
                if (uiManager != null)
                {
                    uiManager.ShowDeathPanel();
                }
            }
            if (other.CompareTag("LostInSpace"))
            {
                // TODO : Player Dead
                Debug.Log("Lost in Space");
                SoundManager.Instance?.PlayDeathbylostinspace();
                // Death panel'i göster ve oyunu durdur
                UIManager uiManager = FindFirstObjectByType<UIManager>();
                if (uiManager != null)
                {
                    uiManager.ShowDeathPanel();
                }
            }
            if (other.CompareTag("Elma")) // Elma tag'i eklendi
            {
                // TODO : Player Win
                Debug.Log("Apple collected - Victory!");
                SoundManager.Instance?.PlayVictory();
                // Win panel'i göster ve oyunu durdur
                UIManager uiManager = FindFirstObjectByType<UIManager>();
                if (uiManager != null)
                {
                    uiManager.ShowWinPanel();
                }

                // Elmayý yok et (opsiyonel)
                Destroy(other.gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<SlimeObject>(out var slime))
            {
                PhysicEvents.Instance.onCollisionSlime?.Invoke(slime);
            }
        }
    }
}