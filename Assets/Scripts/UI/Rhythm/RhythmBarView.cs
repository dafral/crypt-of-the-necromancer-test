using UnityEngine;
using UnityEngine.UI;

namespace Dafral.Game.UI
{
    public class RhythmBarView : MonoBehaviour
    {
        [Header("Tempo Indicator")]
        [SerializeField] private Image _tempoIndicator;
        [SerializeField] private float _baseScale = 1f;
        [SerializeField] private float _pulseScale = 1.2f;
        [SerializeField] private float _pulseLerpSpeed = 12f;

        [Header("Lateral Beat Indicators")]
        [SerializeField] private RectTransform _leftBeatIndicator;
        [SerializeField] private RectTransform _rightBeatIndicator;
        [SerializeField] private float _lateralTravelDistance = 600f;

        private float _currentScale = 1f;

        public void TriggerTempoAnimation()
        {
            _currentScale = _pulseScale;
            ApplyScale();
        }

        public void Render(float beatProgress)
        {
            float clampedProgress = Mathf.Clamp01(beatProgress);

            _tempoIndicator.fillAmount = clampedProgress;
            _currentScale = Mathf.Lerp(_currentScale, _baseScale, _pulseLerpSpeed * Time.deltaTime);
            ApplyScale();

            UpdateLateralIndicators(clampedProgress);
        }

        private void UpdateLateralIndicators(float beatProgress)
        {
            float currentDistance = Mathf.Lerp(_lateralTravelDistance, 0f, beatProgress);
            SetLateralPositionX(_leftBeatIndicator, -currentDistance);
            SetLateralPositionX(_rightBeatIndicator, currentDistance);
        }

        private void SetLateralPositionX(RectTransform indicator, float x)
        {
            if (indicator == null) return;
            Vector2 position = indicator.anchoredPosition;
            position.x = x;
            indicator.anchoredPosition = position;
        }

        private void ApplyScale()
        {
            _tempoIndicator.rectTransform.localScale = Vector3.one * _currentScale;
        }
    }
}