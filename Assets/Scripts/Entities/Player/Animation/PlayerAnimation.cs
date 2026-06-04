using UnityEngine;

namespace Dafral.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour, IPlayerAnimation
    {
        private const string DASH_LEFT_ANIMATION = "Player_Dash_L";
        private const string DASH_RIGHT_ANIMATION = "Player_Dash_R";
        
        private Animator _animator;

        public void Initialize()
        {
            _animator = GetComponent<Animator>();
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
        }
        
        public void OnDied()
        {
        }
    }
}
