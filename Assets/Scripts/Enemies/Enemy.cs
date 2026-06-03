using Dafral.Game.Combat;
using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Enemies
{
    public class Enemy : GridEntity
    {
        [SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private EntityInteract _entityInteract;

        public override GridEntityType EntityType => GridEntityType.Enemy;

        public void Initialize(EnemyData enemyData)
        {
            RegisterOnGrid();
            _health.Initialize(enemyData.Health);
            _health.OnDied += OnDeath;
            _enemyMovement.Initialize(this, enemyData);
            _entityInteract.Initialize(GridEntityType.Player, new Combat(enemyData.Damage));
        }

        public override void Interact(IGridEntity otherEntity)
        {
            _entityInteract.Interact(otherEntity);
        }

        private void OnDeath()
        {
            Dispose();
            _gridService.RemoveEntity(this);
            Destroy(gameObject);
        }

        public void Dispose()
        {
            _health.OnDied -= OnDeath;
            _enemyMovement.Dispose();
        }
    }
}
