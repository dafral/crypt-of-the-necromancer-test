using UnityEngine;
using UnityEngine.UI;

namespace Dafral.Game.UI
{
    public class PlayerHealthBarView : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Text _healthText;
        [SerializeField] private RectTransform _animatedTransform;
        [SerializeField] private float _baseScale = 1f;
        [SerializeField] private float _damagedScale = 1.2f;
        [SerializeField] private float _pulseLerpSpeed = 12f;

        private float _currentScale = 1f;

        public void TriggerDamagedAnimation()
        {
            _currentScale = _damagedScale;
            ApplyScale();
        }

        public void Render(int currentHealth, int maxHealth)
        {
            int displayedMaxHealth = Mathf.Max(maxHealth, 0);
            int displayedCurrentHealth = Mathf.Clamp(currentHealth, 0, displayedMaxHealth);

            _healthSlider.minValue = 0f;
            _healthSlider.maxValue = Mathf.Max(displayedMaxHealth, 1);
            _healthSlider.value = displayedCurrentHealth;
            _healthText.text = $"{displayedCurrentHealth} / {displayedMaxHealth}";

            _currentScale = Mathf.Lerp(_currentScale, _baseScale, _pulseLerpSpeed * Time.deltaTime);
            ApplyScale();
        }

        private void ApplyScale()
        {
            _animatedTransform.localScale = Vector3.one * _currentScale;
        }
    }
}
