using Dafral.Game.Combat;
using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Enemies
{
    public class Enemy : GridEntity
    {
        [SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private EntityInteract _entityInteract;

        private IEnemyMovement EnemyMovement => _enemyMovement;
        private IEntityInteract EntityInteract => _entityInteract;

        public override GridEntityType EntityType => GridEntityType.Enemy;

        public void Initialize(EnemyData enemyData)
        {
            RegisterOnGrid();
            Health.Initialize(enemyData.Health);
            Health.OnDied += OnDeath;
            EnemyMovement.Initialize(this, enemyData);
            EntityInteract.Initialize(GridEntityType.Player, new Combat(enemyData.Damage));
        }

        public override void Interact(IGridEntity otherEntity)
        {
            EntityInteract.Interact(otherEntity);
        }

        private void OnDeath()
        {
            Despawn();
        }

        protected override void Despawn()
        {
            Health.OnDied -= OnDeath;
            EnemyMovement.Dispose();
            EntityInteract.Dispose();
            base.Despawn();
        }


    }
}
