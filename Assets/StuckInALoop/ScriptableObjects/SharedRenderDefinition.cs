using System;
using System.Collections.Generic;
using UnityEngine;

namespace StuckInALoop
{
    [CreateAssetMenu]
    public class SharedRenderDefinition : ScriptableObject
    {
        private static readonly int TintColor = Shader.PropertyToID("_TintColor");
        private static readonly int MainTex   = Shader.PropertyToID("_MainTex");

        public static Dictionary<SharedRenderDefinition, int> definitionIds;
        public static int                                     NextId = 0;

        static SharedRenderDefinition()
        {
            definitionIds = new Dictionary<SharedRenderDefinition, int>();
        }

        [ColorUsage(false, true)] public Color color;

        public Texture2D texture;
        public Mesh      mesh;
        public Material  material;
        public bool      enabled;

        private MaterialPropertyBlock _materialPropertyBlock;

        private void OnEnable()
        {
            definitionIds.Add(this, NextId++);
        }

        public MaterialPropertyBlock PropBlock
        {
            get
            {
                if (_materialPropertyBlock == null) UpdatePropBlock();
                return _materialPropertyBlock;
            }
        }

        private void OnValidate()
        {
            UpdatePropBlock();
        }

        public event Action PropertyChangedEvent;

        private void UpdatePropBlock()
        {
            _materialPropertyBlock = new MaterialPropertyBlock();
            _materialPropertyBlock.SetColor(TintColor, color);
            if (texture) _materialPropertyBlock.SetTexture(MainTex, texture);

            PropertyChangedEvent?.Invoke();
        }
    }
}