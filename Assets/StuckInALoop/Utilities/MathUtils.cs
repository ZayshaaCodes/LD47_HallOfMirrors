using Unity.Mathematics;

namespace StuckInALoop.Maths
{
    public class MathUtils
    {
        public float3 MultiplyPoint(float4x4 mtx, float3 point)
        {
            float3 f3;
            f3.x = mtx.c0.x * point.x + mtx.c1.x * point.y + mtx.c2.x * point.z + mtx.c3.x;
            f3.y = mtx.c0.y * point.x + mtx.c1.y * point.y + mtx.c2.y * point.z + mtx.c3.y;
            f3.z = mtx.c0.z * point.x + mtx.c1.z * point.y + mtx.c2.z * point.z + mtx.c3.z;

            var num = 1f / (mtx.c0.w * point.x + mtx.c1.w * point.y + mtx.c2.w * point.z + mtx.c3.w);
            f3.x *= num;
            f3.y *= num;
            f3.z *= num;
            return f3;
        }
    }
}