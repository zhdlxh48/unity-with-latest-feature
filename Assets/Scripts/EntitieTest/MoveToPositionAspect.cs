using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public readonly partial struct MoveToPositionAspect : IAspect
{
    private readonly Entity entity;
    
    private readonly TransformAspect transformAspect;
    private readonly RefRO<Speed> speed;
    private readonly RefRW<TargetPosition> targetPosition;

    public void Move(float deltaTime)
    {
        // Calculate direction
        var direction = math.normalize(targetPosition.ValueRW.value - transformAspect.WorldPosition);
        // Move position
        transformAspect.WorldPosition += direction * speed.ValueRO.value * deltaTime;
    }
    
    public void TestReachedTargetPosition(RefRW<RandomComponent> randomComponent)
    {
        var reachedTargetDistance = 0.5f;
        if (math.distance(transformAspect.WorldPosition, targetPosition.ValueRW.value) < reachedTargetDistance)
        {
            // Generate new random target position
            targetPosition.ValueRW.value = GetRandomPosition(randomComponent);
            var value = targetPosition.ValueRW.value;
            Debug.Log(value.ToString());
        }
    }

    private float3 GetRandomPosition(RefRW<RandomComponent> randomComponent)
    {
        return new float3(
            randomComponent.ValueRW.random.NextFloat(0f, 15f), 
            0f, 
            randomComponent.ValueRW.random.NextFloat(0f, 15f));
    }
}