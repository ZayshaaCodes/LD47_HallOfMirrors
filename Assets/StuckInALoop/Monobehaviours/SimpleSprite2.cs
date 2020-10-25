using StuckInALoop;
using Unity.Entities;
using UnityEngine;

[ExecuteAlways]
public class SimpleSprite2 : MonoBehaviour, IDestroyOnClone, IConvertGameObjectToEntity
{
    public MeshRenderer           mr;
    public SharedRenderDefinition srd;

    public bool isStatic;

    private void OnEnable()
    {
        mr = GetComponent<MeshRenderer>();

        if (srd != null)
        {
            mr.SetPropertyBlock(srd.PropBlock);

            srd.PropertyChangedEvent += SrdOnPropertyChangedEvent;
        }
    }

    private void OnDisable()
    {
        srd.PropertyChangedEvent -= SrdOnPropertyChangedEvent;
    }

    private void OnValidate()
    {
        srd.PropertyChangedEvent -= SrdOnPropertyChangedEvent;
        srd.PropertyChangedEvent += SrdOnPropertyChangedEvent;
        SrdOnPropertyChangedEvent();
    }

    private void SrdOnPropertyChangedEvent()
    {
        mr.SetPropertyBlock(srd.PropBlock);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
    }
}