using Unity.Entities;
using UnityEngine;

[ExecuteAlways]
public class SimpleSpriteDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public Mesh      mesh;
    public Material  material;
    public Texture2D sprite;

    [ColorUsage(true, true)] public Color tintColor;

    private static readonly int MainTex   = Shader.PropertyToID("_MainTex");
    private static readonly int TintColor = Shader.PropertyToID("_TintColor");

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var newSharedSpriteData = new SimpleSharedSpriteData()
        {
            material  = material,
            mesh      = mesh,
            sprite    = sprite,
            tintColor = tintColor
        };

        dstManager.AddSharedComponentData(entity, newSharedSpriteData);
    }

    private void Update()
    {
        if (Application.isPlaying) return;

        var mpb = new MaterialPropertyBlock();
        mpb.SetTexture(MainTex,    sprite);
        mpb.SetColor(TintColor,  tintColor);
        
        Graphics.DrawMesh(mesh, transform.localToWorldMatrix, material, 0, null, 0, mpb);
    }
}