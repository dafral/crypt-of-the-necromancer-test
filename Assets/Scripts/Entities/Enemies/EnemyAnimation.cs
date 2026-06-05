using System.Collections;
using UnityEngine;

namespace Dafral.Enemies
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class EnemyAnimation : MonoBehaviour, IEnemyAnimation
    {
        private const float FLASHING_ANIMATION_DURATION = 0.1f;

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Coroutine _flashingAnimationCoroutine;

        public void Initialize()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void OnHealthChanged(int currentHealth, int maxHealth)
        {
            if(_flashingAnimationCoroutine != null)
            {
                StopCoroutine(_flashingAnimationCoroutine);
                _flashingAnimationCoroutine = null;
            }

            _flashingAnimationCoroutine = StartCoroutine(FlashingAnimation());
        }

        private IEnumerator FlashingAnimation()
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(FLASHING_ANIMATION_DURATION);
            _spriteRenderer.color = Color.white;
        }

        public void OnDied()
        {
            _spriteRenderer.color = Color.white;
            StopAllCoroutines();
        }
    }
}
