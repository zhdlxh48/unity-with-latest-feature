using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class MovingSystemBase : SystemBase
{
    protected override void OnUpdate()
    {
        // This syntax (use foreach) is equal with Entities.Foreach(...).Run();
        
        /*
        // When do not use Aspect
        foreach (var (transformAspect, speed, targetPosition) in SystemAPI.Query<TransformAspect, RefRO<Speed>, RefRW<TargetPosition>>())
        {
            // Calculate direction
            var direction = math.normalize(targetPosition.ValueRW.value - transformAspect.Position);
            // Move position
            transformAspect.Position += direction * speed.ValueRO.value * SystemAPI.Time.DeltaTime;
        }
        */
        
        // When use Aspect
        foreach (var moveToPositionAspect in SystemAPI.Query<MoveToPositionAspect>())
        {
            moveToPositionAspect.Move(SystemAPI.Time.DeltaTime);
        }

        // ForEach returns Jobs, this need to call Functions to register Jobs 
        // Entities.ForEach((TransformAspect transformAspect) =>
        // {
        //     transformAspect.Position += new float3(SystemAPI.Time.DeltaTime, 0, 0);
        // }).Schedule();
        // Run(): runs code on main thread, Schedule(): runs on single WorkerThread, ScheduleParallel(): runs on multiple WorkerThread
    }
}