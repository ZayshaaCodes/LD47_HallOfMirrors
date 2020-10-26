using ECS.Components;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Systems
{
    public class RotateOverTimeSystem : JobComponentSystem
    {
        protected override void OnCreate()
        {
        }

        protected override void OnStartRunning()
        {
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            inputDeps = Entities.ForEach((RotateOverTimeData rotData, ref Rotation rotation) =>
            {
                rotation.Value = math.mul(quaternion.Euler(rotData.angularVelocity), rotation.Value);
            }).Schedule(inputDeps);

            return inputDeps;
        }
    }
}