using System;
using Unity.Entities;
using UnityEngine;


public struct SimpleSharedSpriteData : ISharedComponentData, IEquatable<SimpleSharedSpriteData>
{
    public Mesh      mesh;
    public Material  material;
    public Texture2D sprite;

    [ColorUsage(true, false)] public Color tintColor;
    public bool Equals(SimpleSharedSpriteData other)
    {
        return Equals(mesh, other.mesh) && Equals(material, other.material) && Equals(sprite, other.sprite) &&
               tintColor.Equals(other.tintColor);
    }

    public override bool Equals(object obj)
    {
        return obj is SimpleSharedSpriteData other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (mesh != null ? mesh.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (material != null ? material.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (sprite != null ? sprite.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ tintColor.GetHashCode();
            return hashCode;
        }
    }
}