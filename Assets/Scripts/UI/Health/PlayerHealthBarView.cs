using UnityEngine;
using UnityEngine.UI;

namespace Dafral.Game.UI
{
    public class PlayerHealthBarView : MonoBehaviour
    {
        [SerializeField] private Image _healthFill;
        [SerializeField] private float _baseScale = 1f;
        [SerializeField] private float _damagedScale = 1.2f;
        [SerializeField] private float _pulseLerpSpeed = 12f;

        private float _currentScale = 1f;

        public void TriggerDamagedAnimation()
        {
            _currentScale = _damagedScale;
            ApplyScale();
        }

        public void Render(float healthRatio)
        {
            _healthFill.fillAmount = Mathf.Clamp01(healthRatio);
            _currentScale = Mathf.Lerp(_currentScale, _baseScale, _pulseLerpSpeed * Time.deltaTime);
            ApplyScale();
        }

        private void ApplyScale()
        {
            _healthFill.rectTransform.localScale = Vector3.one * _currentScale;
        }
    }
}
