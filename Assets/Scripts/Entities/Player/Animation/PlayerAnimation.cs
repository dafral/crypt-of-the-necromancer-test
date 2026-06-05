using System.Collections;
using UnityEngine;

namespace Dafral.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerAnimation : MonoBehaviour, IPlayerAnimation
    {
        private const string DASH_LEFT_ANIMATION = "Player_Dash_L";
        private const string DASH_RIGHT_ANIMATION = "Player_Dash_R";
        private const float FLASHING_ANIMATION_DURATION = 0.1f;
        
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Coroutine _flashingAnimationCoroutine;

        public void Initialize()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator.SetBool("grounded", true);
        }

        public void OnDash(Vector2Int direction)
        {
            string animation = direction.x < 0 ? DASH_LEFT_ANIMATION : DASH_RIGHT_ANIMATION;
            _animator.Play(animation);
        }

        public void OnJumped()
        {
            _animator.SetBool("grounded", false);
        }

        public void OnLanded()
        {
            _animator.SetBool("grounded", true);
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
