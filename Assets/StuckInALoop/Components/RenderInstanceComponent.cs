using System;
using Unity.Entities;
using UnityEngine;

namespace StuckInALoop.Components
{
    public struct SharedRenderInstanceData : ISharedComponentData, IEquatable<SharedRenderInstanceData>
    {
        public Mesh      mesh;
        public Material  material;
        public Texture2D texture;
        public MaterialPropertyBlock propertyBlock;
        public Color     tintColor;

        public bool Equals(SharedRenderInstanceData other)
        {
            return Equals(mesh, other.mesh) && Equals(material, other.material) && Equals(texture, other.texture) &&
                   tintColor.Equals(other.tintColor);
        }

        public override bool Equals(object obj)
        {
            return obj is SharedRenderInstanceData other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (mesh != null ? mesh.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (material != null ? material.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (texture != null ? texture.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ tintColor.GetHashCode();
                return hashCode;
            }
        }
    }
}