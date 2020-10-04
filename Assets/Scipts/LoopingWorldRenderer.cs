using System;
using Unity.Mathematics;
using UnityEngine;

[ExecuteAlways]
public class LoopingWorldRenderer : MonoBehaviour
{
    
    
    public Vector2 bSize = new Vector2(20, 10);

    public int depthCount = 3;

    public Transform statics;
    public Transform dynamics;

    public int ghostLayer;

    Camera cam;

    private Matrix4x4 WorldToZto =>
        Matrix4x4.Scale(new Vector3(bSize.x, bSize.y, 1)).inverse * Matrix4x4.Translate(bSize / 2);
    // Update is called once per frame

    private void OnEnable()
    {
        if (!Application.isPlaying) return;

        cam = Camera.main;

        for (int i = -1; i < 2; i++)
        for (int j = -1; j < 2; j++)
        {
            if (i == 0 & j == 0) continue;

            var xform = Matrix4x4.Translate(new Vector3(i * bSize.x, j * bSize.y));
            foreach (Transform stat in statics)
            {
                if (!stat.gameObject.activeInHierarchy) continue;
                
                var newStat = Instantiate(stat);

                newStat.transform.position = xform.MultiplyPoint(stat.transform.position);
                var mr = newStat.GetComponent<MeshRenderer>();
                var mf = newStat.GetComponent<MeshFilter>();
                var ss = newStat.GetComponent<SimpleSprite>();
                var wb = newStat.GetComponentsInChildren<WorldButton>();
                if (mr != null) Destroy(mr);
                if (mf != null) Destroy(mf);
                if (ss != null) Destroy(ss);
                
                foreach (var worldButton in wb) Destroy(worldButton);
            }

            foreach (Transform dyn in dynamics)
            {
                if (!dyn.gameObject.activeInHierarchy) continue;
                var newDyn = Instantiate(dyn);
                
                var mr = newDyn.GetComponent<MeshRenderer>();
                var mf = newDyn.GetComponent<MeshFilter>();
                var ss = newDyn.GetComponent<SimpleSprite>();
                if (mr != null) Destroy(mr);
                if (mf != null) Destroy(mf);
                if (ss != null) Destroy(ss);
                
                newDyn.gameObject.layer = 6;
                var link = newDyn.gameObject.AddComponent<LinkedRb>();
                link.linkOffset = new Vector3(i * bSize.x, j * bSize.y);
                link.linked = dyn.GetComponent<Rigidbody2D>();
                link.FixedUpdate();
            }
        }
    }

    void Update()
    {
        var meshRenderers = GetComponentsInChildren<SimpleSprite>();

        foreach (var sprite in meshRenderers)
        {
            var spriteTransform = sprite.transform;

            Bounds bounds = new Bounds(Vector3.zero, this.bSize);
            var rb = spriteTransform.GetComponent<Rigidbody2D>();
            var particleAnchor = spriteTransform.GetComponentInChildren<ParticleAnchor>();

            if (rb)
            {
                
                float3 pos = new float3(rb.position.x, rb.position.y, 0);
                var vel = rb.velocity;
                
                float3 zto = WorldToZto.MultiplyPoint(pos);
                zto -= math.floor(zto);
                float3 newPos = WorldToZto.inverse.MultiplyPoint(zto);
                rb.transform.position = newPos;
                
                ILoopBehaviour iloop = rb.GetComponent<ILoopBehaviour>();
                if (iloop != null)
                {
                    var delta = newPos - pos;
                    if (math.length(delta) > 5)
                    {
                        iloop.Loop(-(Vector2)delta.xy);
                    }
                }
            }

            for (int k = 0; k < depthCount; k++)
            {
                var kpo = k + 1;
                for (int i = -kpo; i < kpo + 1; i++)
                for (int j = -kpo; j < kpo + 1; j++)
                {
                    if (i == 0 && j == 0 && k == 0) continue;

                    var xform = Matrix4x4.Translate(new Vector3(i * bSize.x, j * bSize.y, k * 20));

                    var mr = sprite.GetComponent<MeshRenderer>();

                    if (mr.enabled)
                    {
                        Graphics.DrawMesh(sprite.GetComponent<MeshFilter>().sharedMesh,
                            xform * Matrix4x4.TRS(spriteTransform.position, spriteTransform.rotation,
                                spriteTransform.lossyScale),
                            sprite.mr.sharedMaterial, 0, null, 0, sprite._materialPropertyBlock);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 1f, 1f, 0.31f);
        Gizmos.DrawWireCube(Vector3.zero, bSize);
        //
        // Gizmos.matrix = WorldToZto;
        // Gizmos.color = new Color(1f, 1f, 1f, 0.31f);
        // Gizmos.DrawWireCube(Vector3.zero, bSize);
    }
}