using System;
using UnityEngine;

namespace Dafral.Enemies
{
    [Serializable]
    public struct EnemyData
    {
        [Header("Movement")]
        public EnemyMovementType MovementStrategy;
        public int BeatsToMove;

        [Header("Combat")]
        public int Health;
        public int Damage;
    }
}
