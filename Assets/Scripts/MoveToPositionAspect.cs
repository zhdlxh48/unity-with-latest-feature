using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public readonly partial struct MoveToPositionAspect : IAspect
{
    private readonly Entity entity;
    
    private readonly TransformAspect transformAspect;
    private readonly RefRO<Speed> speed;
    private readonly RefRW<TargetPosition> targetPosition;

    public void Move(float deltaTime)
    {
        // Calculate direction
        var direction = math.normalize(targetPosition.ValueRW.value - transformAspect.Position);
        // Move position
        transformAspect.Position += direction * speed.ValueRO.value * deltaTime;
    }
}