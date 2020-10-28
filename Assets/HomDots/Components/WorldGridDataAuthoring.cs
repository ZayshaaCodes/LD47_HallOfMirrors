using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace HomDots.Components
{
    public class WorldGridDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float3 spacing;
        public int3   counts;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new WorldGridData() {spacing = spacing, counts = counts});
        }

        private void OnDrawGizmos()
        {
            var left = -counts / 2;

            Gizmos.color = new Color(1f, 1f, 1f, 0.001f);
            for (int k = 0; k < counts.z; k++)
            for (int j = 0; j < counts.y; j++)
            for (int i = 0; i < counts.x; i++)
            {
                var pos = (left + new int3(i, j, k)) * spacing;

                Gizmos.DrawWireCube(pos, spacing);
            }

            Gizmos.color = new Color(.5f, 1f, .5f, 0.22f);
            Gizmos.DrawWireCube(Vector3.zero, counts * spacing);
        
            Gizmos.color = new Color(.5f, .5f, 1f, 0.3f);
            Gizmos.DrawWireCube(Vector3.zero, spacing);
        }
    }
}