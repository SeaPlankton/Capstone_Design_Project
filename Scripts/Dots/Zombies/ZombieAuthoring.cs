using Unity.Entities;
using UnityEngine;
namespace Dots.Zombie
{
    public class ZombieAuthoring : MonoBehaviour
    {
        public int NumberZombiesToSpawn;
        public GameObject ZombiePrefab;
        public int Speed;
        public int RotatingSpeed;
        [HideInInspector]
        public uint RandomSeed;
    
    }
    public class ZombieBaker : Baker<ZombieAuthoring>
    {
        public override void Bake(ZombieAuthoring authoring)
        {
            var entityPrefab = GetEntity(authoring.ZombiePrefab, TransformUsageFlags.Dynamic);
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            authoring.RandomSeed = (uint) Random.Range(0, int.MaxValue);
            AddComponent(entity, new ZombieProperties
            {
                Speed = authoring.Speed,
                RotateSpeed = authoring.RotatingSpeed,
                ZombiePrefab = entityPrefab,
                NumberZombiesToSpawn = authoring.NumberZombiesToSpawn
            });
            AddComponent(entity, new ZombieRandom
            {
                Value = Unity.Mathematics.Random.CreateFromIndex(authoring.RandomSeed)
            });
        }
    }
    public struct ZombieProperties : IComponentData
    {
        public int Speed;
        public int RotateSpeed;
        public int NumberZombiesToSpawn;
        public Entity ZombiePrefab;
    }
    public struct ZombieRandom : IComponentData
    {
        public Unity.Mathematics.Random Value;
    }
}