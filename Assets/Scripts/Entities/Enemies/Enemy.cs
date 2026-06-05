using Dafral.Game.Combat;
using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Enemies
{
    public class Enemy : GridEntity, IEnemy
    {
        [SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private EntityInteract _entityInteract;
        [SerializeField] private EnemyAnimation _enemyAnimation;

        private IEnemyMovement EnemyMovement => _enemyMovement;
        private IEntityInteract EntityInteract => _entityInteract;
        private IEnemyAnimation EnemyAnimation => _enemyAnimation;

        public override GridEntityType EntityType => GridEntityType.Enemy;

        public void Initialize(EnemyData enemyData)
        {
            RegisterOnGrid();
            InitializeControllers(enemyData);
        }

        private void InitializeControllers(EnemyData enemyData)
        {
            EnemyMovement.Initialize(this, enemyData);
            EntityInteract.Initialize(GridEntityType.Player, new Combat(enemyData.Damage));
            EnemyAnimation.Initialize();
            Health.OnHealthChanged += OnHealthChanged;
            Health.OnDied += OnDeath;
            Health.Initialize(enemyData.Health);
        }

        public override void Interact(IGridEntity otherEntity)
        {
            EntityInteract.Interact(otherEntity);
        }

        public void OnHealthChanged(int currentHealth, int maxHealth)
        {
            EnemyAnimation.OnHealthChanged(currentHealth, maxHealth);
        }

        private void OnDeath()
        {
            EnemyAnimation.OnDied();
            Despawn();
        }

        protected override void Despawn()
        {
            Health.OnDied -= OnDeath;
            EnemyMovement.Dispose();
            EntityInteract.Dispose();
            base.Despawn();
        }

        public void Dispose()
        {
            Despawn();
        }
    }
}
