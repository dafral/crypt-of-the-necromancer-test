using System.IO;
using Dafral.Events;
using Dafral.Game.Map;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float _smoothSpeed = 5f;

        private Vector3 _targetPosition;
        private bool _hasTarget;
        private int _debugFrameCount;

        private void Start()
        {
            var eventService = ServiceLocator.Instance.GetService<IEventService>();
            eventService.Subscribe<OnPlayerSpawned>(OnPlayerSpawned);
            eventService.Subscribe<OnEntityGridPositionChanged>(OnEntityGridPositionChanged);
        }

        private void OnDestroy()
        {
            if (!ServiceLocator.Instance.Contains<IEventService>()) return;
            var eventService = ServiceLocator.Instance.GetService<IEventService>();
            eventService.Unsubscribe<OnPlayerSpawned>(OnPlayerSpawned);
            eventService.Unsubscribe<OnEntityGridPositionChanged>(OnEntityGridPositionChanged);
        }

        private void OnPlayerSpawned(OnPlayerSpawned e)
        {
            SnapToTarget(e.Player.transform.position);
        }

        private void OnEntityGridPositionChanged(OnEntityGridPositionChanged e)
        {
            if (e.Entity.EntityType != GridEntityType.Player) return;
            var gridService = ServiceLocator.Instance.GetService<IGridService>();
            _targetPosition = gridService.GetWorldPosition(e.NewPosition);
            _hasTarget = true;
        }

        private void LateUpdate()
        {
            if (!_hasTarget) return;
            var desired = new Vector3(_targetPosition.x, _targetPosition.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, desired, _smoothSpeed * Time.deltaTime);

            // #region agent log
            if (_debugFrameCount < 5)
            {
                _debugFrameCount++;
                var pos = transform.position;
                DbgLog(
                    "C",
                    "CameraController.LateUpdate",
                    "camera sub-pixel position",
                    $"{{\"frame\":{_debugFrameCount},\"posX\":{pos.x:F6},\"posY\":{pos.y:F6},\"fracX\":{pos.x - Mathf.Round(pos.x):F6},\"fracY\":{pos.y - Mathf.Round(pos.y):F6}}}");
            }
            // #endregion
        }

        private void SnapToTarget(Vector3 target)
        {
            _targetPosition = target;
            _hasTarget = true;
            transform.position = new Vector3(target.x, target.y, transform.position.z);
        }

        // #region agent log
        private static void DbgLog(string hypothesisId, string location, string message, string dataJson)
        {
            try
            {
                var path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "debug-7de642.log");
                var timestamp = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                File.AppendAllText(
                    path,
                    $"{{\"sessionId\":\"7de642\",\"hypothesisId\":\"{hypothesisId}\",\"location\":\"{location}\",\"message\":\"{message}\",\"data\":{dataJson},\"timestamp\":{timestamp}}}\n");
            }
            catch
            {
            }
        }
        // #endregion
    }
}
