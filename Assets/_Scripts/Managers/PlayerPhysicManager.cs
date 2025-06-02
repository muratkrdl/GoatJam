using _Scripts.Events;
using _Scripts.Keys;
using _Scripts.Object;
using UnityEngine;

namespace _Scripts.Managers
{
    public class PlayerPhysicManager : MonoBehaviour
    {
        [SerializeField] private Transform center;

        [SerializeField] private Rigidbody2D body;
        [SerializeField] private float slimeJumpForce;

        [SerializeField] private float firstJumpForce;

        [SerializeField] private float releaseJumpForce;

        [Header("UI References")]
        [SerializeField] private GameObject instructionText; // Inspector'dan UI text objesini baðlayýn

        private Transform _currentHoldingPlatform;
        private bool _hasPerformedFirstJump = false;
        private bool _isHoldingPlatform = false; // Platform tutma durumu

        private void OnEnable()
        {
            PhysicEvents.Instance.onHandCollisionEnter += OnHandCollisionEnter;
            PhysicEvents.Instance.onCollisionSlime += OnCollisionSlime;
            PlayerInputEvents.Instance.onReleaseFinished += OnReleaseFinished;
            PlayerInputEvents.Instance.onRelease += OnRelease;
        }

        private void OnRelease()
        {
            // Ýlk zýplama henüz yapýlmadýysa ve platform tutulmuyorsa
            if (!_hasPerformedFirstJump && !_isHoldingPlatform)
            {
                PerformFirstJump();
            }
        }

        private void OnReleaseFinished()
        {
            // Platform tutuluyorsa ve HandController'dan geliyorsa
            if (_isHoldingPlatform && _currentHoldingPlatform != null)
            {
                float complier = -1f;
#if UNITY_EDITOR
                complier = 1;
#endif
                Vector2 direction = complier * (body.transform.position - center.position).normalized;
                Vector2 realDirection = direction * releaseJumpForce;
                ApplyForceBody(realDirection);

                _currentHoldingPlatform = null;
                _isHoldingPlatform = false;
            }
        }

        private void OnHandCollisionEnter(OnHandCollisionEnterParams arg0)
        {
            _currentHoldingPlatform = arg0.Other.transform;
            _isHoldingPlatform = true;
        }

        private void OnCollisionSlime(SlimeObject slimeObj)
        {
            SoundManager.Instance.PlayBounce();
            ApplyForceBody(slimeObj.GetReflectDirection().normalized * slimeJumpForce);
            
        }

        private void OnDisable()
        {
            PhysicEvents.Instance.onCollisionSlime -= OnCollisionSlime;
            PhysicEvents.Instance.onHandCollisionEnter -= OnHandCollisionEnter;
            PlayerInputEvents.Instance.onReleaseFinished -= OnReleaseFinished;
            PlayerInputEvents.Instance.onRelease -= OnRelease;
        }

        private void ApplyForceBody(Vector2 direction)
        {
            body.linearVelocity = Vector2.zero;
            body.AddForce(direction, ForceMode2D.Impulse);
        }

        private void Start()
        {
            // Oyun baþladýðýnda instruction text'i göster
            if (instructionText != null)
            {
                instructionText.SetActive(true);
            }
            // Space tuþunu bekle
        }

        private void PerformFirstJump()
        {
            _hasPerformedFirstJump = true;

            // Instruction text'i kapat
            if (instructionText != null)
            {
                instructionText.SetActive(false);
            }

            SoundManager.Instance.PlayJump();
            ApplyForceBody(Vector2.up * firstJumpForce);
        }
    }
}