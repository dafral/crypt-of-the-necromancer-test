using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _smoothTime = 0.15f;
        [SerializeField] private float _maxSpeed = Mathf.Infinity;

        private Transform _target;
        private Vector3 _velocity;

        private void Start()
        {
            var eventService = ServiceLocator.Instance.GetService<IEventService>();
            eventService.Subscribe<OnPlayerSpawned>(OnPlayerSpawned);
        }

        private void OnDestroy()
        {
            if (!ServiceLocator.Instance.Contains<IEventService>()) return;
            var eventService = ServiceLocator.Instance.GetService<IEventService>();
            eventService.Unsubscribe<OnPlayerSpawned>(OnPlayerSpawned);
        }

        private void OnPlayerSpawned(OnPlayerSpawned e)
        {
            _target = e.Player.transform;
            SnapToTarget(_target.position);
        }

        private void LateUpdate()
        {
            if (_target == null) return;

            var targetPosition = _target.position;
            var desired = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(
                transform.position,
                desired,
                ref _velocity,
                _smoothTime,
                _maxSpeed,
                Time.deltaTime);
        }

        private void SnapToTarget(Vector3 target)
        {
            _velocity = Vector3.zero;
            transform.position = new Vector3(target.x, target.y, transform.position.z);
        }
    }
}
