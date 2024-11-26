using Dots.Zombie;
using Unity.Entities;
using UnityEngine;

public readonly partial struct ZombieAspect : IAspect
{
    public readonly Entity entity;
    public readonly RefRO<ZombieProperties> _zombieProperties;
    public readonly RefRW<ZombieRandom> _zombieRandom;

    public int NumberZombiesToSpawn => _zombieProperties.ValueRO.NumberZombiesToSpawn;
    public Entity ZombiePrefab => _zombieProperties.ValueRO.ZombiePrefab;
}
