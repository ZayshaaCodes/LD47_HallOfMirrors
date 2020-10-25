using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace StuckInALoop.Components
{
    [GenerateAuthoringComponent]
    public struct MoveOverTimeComponent : IComponentData
    {
        public float3 speed;
        public float3 rotation;
    }

    public class MoveOverTimeSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;

            Entities.ForEach(
                (MoveOverTimeComponent moveOverTimeData, ref Translation posData, ref Rotation rotData) =>
                {
                    posData.Value += moveOverTimeData.speed * dt;
                    rotData.Value =  math.mul(rotData.Value, quaternion.Euler(moveOverTimeData.rotation));
                }).Schedule();
        }
    }
}