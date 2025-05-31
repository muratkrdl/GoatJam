using System;
using _Scripts.Events;
using _Scripts.Keys;
using Cysharp.Threading.Tasks;
using Runtime.Utilities;
using UnityEngine;

namespace _Scripts.Controllers
{
    public class Try : MonoBehaviour
    {
        /*
        [SerializeField] private SpringJoint2D handSpring;
        [SerializeField] private Rigidbody2D myRigidbody;
        [SerializeField] private Transform initialParent;

        [Header("Rotation Settings")] [SerializeField]
        private float rotationSpeed = 10f;

        [SerializeField] private float alignmentTolerance = 0.1f;
        [SerializeField] private float rotationOffset = -90f; // Sprite'ın yönüne göre ayarlayın

        private PolygonCollider2D _handCollider;
        private Transform _currentHandedObstacle;
        private Rigidbody2D _connectedBody;

        // Rotation için yeni değişkenler
        private bool _isAligning = false;
        private float _targetRotation;
        private Vector2 _attachmentNormal;
        private ContactPoint2D _contactPoint;

        private void Awake()
        {
            _handCollider = GetComponent<PolygonCollider2D>();
        }

        private void OnEnable()
        {
            PhysicEvents.Instance.onHandCollisionEnter += OnHandCollisionEnter;
            PlayerInputEvents.Instance.onRelease += OnRelease;
        }

        private void Update()
        {
            // Obstacle'a bağlıyken ve hizalanıyorsa rotation uygula
            if (_isAligning && _currentHandedObstacle != null)
            {
                AlignRotation();
            }
        }

        private void OnHandCollisionEnter(OnHandCollisionEnterParams arg)
        {
            OnTriggerEnterFunc(arg.Other);
        }

        private void OnRelease()
        {
            OnTriggerExitFunc();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(ConstantsUtilities.Obstacles))
            {
                // Collision noktasını sakla
                if (other.contactCount > 0)
                {
                    _contactPoint = other.GetContact(0);
                    _attachmentNormal = _contactPoint.normal;
                }

                PhysicEvents.Instance.onHandCollisionEnter?.Invoke(new OnHandCollisionEnterParams()
                {
                    Hand = this,
                    Other = other
                });
            }
        }

        private void OnTriggerEnterFunc(Collision2D other)
        {
            handSpring.enabled = true;
            myRigidbody.bodyType = RigidbodyType2D.Kinematic;
            _currentHandedObstacle = other.transform;
            _connectedBody = other.gameObject.GetComponent<Rigidbody2D>();
            myRigidbody.transform.SetParent(_currentHandedObstacle);
            handSpring.connectedBody = _connectedBody;
            handSpring.connectedAnchor = _currentHandedObstacle.position;

            // Rotation alignment başlat
            StartRotationAlignment();
        }

        private void StartRotationAlignment()
        {
            _isAligning = true;

            // Normal vektörün tersini al (pompanın yönü için)
            Vector2 pompaDirection = -_attachmentNormal;

            // Pompanın rotasyonunu hesapla
            float angle = Mathf.Atan2(pompaDirection.y, pompaDirection.x) * Mathf.Rad2Deg;

            // Sprite'ın orijinal yönüne göre düzeltme
            angle += rotationOffset;

            _targetRotation = angle;
        }

        private void AlignRotation()
        {
            float currentRotation = transform.rotation.eulerAngles.z;
            float difference = Mathf.Abs(Mathf.DeltaAngle(currentRotation, _targetRotation));

            if (difference < alignmentTolerance)
            {
                _isAligning = false;
                transform.rotation = Quaternion.Euler(0, 0, _targetRotation);
            }
            else
            {
                float newRotation = Mathf.LerpAngle(currentRotation, _targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, 0, newRotation);
            }
        }

        private void OnTriggerExitFunc()
        {
            // Exit from obstacle
            if (!_currentHandedObstacle) return;

            Collider2D otherCollider = _currentHandedObstacle.GetComponent<Collider2D>();
            IgnoreCollider(otherCollider).Forget();

            handSpring.enabled = false;
            myRigidbody.transform.SetParent(initialParent);
            myRigidbody.bodyType = RigidbodyType2D.Dynamic;
            _currentHandedObstacle = null;
            _connectedBody = null;
            handSpring.connectedBody = null;
            handSpring.connectedAnchor = Vector2.zero;

            // Rotation state'ini sıfırla
            _isAligning = false;

            // Release anında force uygula - yüzey normaline göre
            ApplyReleaseForce();
        }

        private void ApplyReleaseForce()
        {
            // Eğer PhysicEvents üzerinden force uygulanıyorsa bu metodu kullanmayın
            // Aksi halde, yüzey normaline göre force uygulayabilirsiniz:

            // Vector2 forceDirection = _attachmentNormal;
            // float jumpForce = 10f; // Bu değeri serialize edebilirsiniz
            // myRigidbody.AddForce(forceDirection * jumpForce, ForceMode2D.Impulse);
        }

        private async UniTaskVoid IgnoreCollider(Collider2D otherCollider)
        {
            Physics2D.IgnoreCollision(_handCollider, otherCollider, true);
            await UniTask.Delay(TimeSpan.FromSeconds(3f));
            Physics2D.IgnoreCollision(_handCollider, otherCollider, false);
        }

        // Debug için
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            if (_currentHandedObstacle != null)
            {
                // Attachment normal'i göster
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(transform.position, _attachmentNormal * 2f);

                // Pompa yönünü göster
                Gizmos.color = _isAligning ? Color.red : Color.green;
                Gizmos.DrawRay(transform.position, transform.up * 2f);
            }
        }
        */
    }
}