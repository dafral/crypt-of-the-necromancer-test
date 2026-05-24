using UnityEngine;
using UnityEngine.UI;

namespace Dafral.Game.UI
{
    public class RhythmBarView : MonoBehaviour
    {
        [SerializeField] private Image _tempoIndicator;
        [SerializeField] private float _baseScale = 1f;
        [SerializeField] private float _pulseScale = 1.2f;
        [SerializeField] private float _pulseLerpSpeed = 12f;

        private float _currentScale = 1f;

        public void TriggerTempoAnimation()
        {
            _currentScale = _pulseScale;
            ApplyScale();
        }

        public void Render(float beatProgress)
        {

            _tempoIndicator.fillAmount = Mathf.Clamp01(beatProgress);
            _currentScale = Mathf.Lerp(_currentScale, _baseScale, _pulseLerpSpeed * Time.deltaTime);
            ApplyScale();
        }

        private void ApplyScale()
        {
            _tempoIndicator.rectTransform.localScale = Vector3.one * _currentScale;
        }
    }
}