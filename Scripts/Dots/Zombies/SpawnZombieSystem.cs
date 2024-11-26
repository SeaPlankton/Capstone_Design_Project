using UnityEngine;
using Unity.Entities;
using Dots.Zombie;
using Unity.Collections;
using Unity.Transforms;

public partial struct SpawnZombieSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ZombieProperties>();
    }
    public void OnDestroy(ref SystemState state)
    {

    }
    public void OnUpdate(ref SystemState state)
    {
        Managers.print("OnUpdate!");
        state.Enabled = false;
        /*
        var zombieEntity = SystemAPI.GetSingletonEntity<ZombieProperties>();
        var zombie = SystemAPI.GetAspect<ZombieAspect>(zombieEntity);
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        Managers.print("NumberZombiesToSpawn : " + zombie.NumberZombiesToSpawn);
        for (int i = 0; i < zombie.NumberZombiesToSpawn; i++)
        {
            var z = ecb.Instantiate(zombie.ZombiePrefab);
        }
        ecb.Playback(state.EntityManager);
        */
    }
}
