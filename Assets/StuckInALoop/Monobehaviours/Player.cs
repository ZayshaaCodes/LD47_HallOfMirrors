using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace StuckInALoop
{
    [SelectionBase]
    public class Player : MonoBehaviour, ILoopBehaviour, IDestroyOnClone
    {
        [SerializeField] private float jumpForce = 5;
        [SerializeField] private float moveForce;
        [SerializeField] private bool  _grounded;

        public GameObject jumpParticles;
        public AudioClip  jumpSound;

        public Camera      _cam;
        public AudioSource _audio;

        public List<Collider2D> touching = new List<Collider2D>();

        private readonly AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Vector2       _input = Vector2.zero;
        private Rigidbody2D   _rb;
        private TrailRenderer _tr;

        private IEnumerator Start()
        {
            _cam = Camera.main;
            touching.Clear();

            _rb = GetComponent<Rigidbody2D>();
            _tr = GetComponent<TrailRenderer>();

            transform.localScale = Vector3.zero;
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * 2;

                transform.localScale = Vector3.one * _curve.Evaluate(t);
                yield return null;
            }
        }

        private void Update()
        {
            _input = Vector2.zero;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) _input    += (Vector2) _cam.transform.up;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) _input  -= (Vector2) _cam.transform.right;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) _input  -= (Vector2) _cam.transform.up;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) _input += (Vector2) _cam.transform.right;

            if (_grounded)
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Vector3 gravityDir = Physics2D.gravity.normalized;
                    _audio.PlayOneShot(jumpSound);
                    Instantiate(jumpParticles,
                                transform.position + gravityDir * .5f,
                                quaternion.LookRotation(Vector3.forward, -gravityDir));
                    // Debug.Log("jump");
                    _rb.AddForce(-Physics2D.gravity.normalized * jumpForce, ForceMode2D.Impulse);
                    _grounded = false;
                }
        }

        private void FixedUpdate()
        {
            _rb.AddForce(_input * moveForce);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            touching.Add(other.collider);
            HandleCollision(other);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            touching.Remove(other.collider);
            if (touching.Count == 0)
            {
            }
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            HandleCollision(other);
        }

        public void Loop(Vector2 offset)
        {
            if (_tr)
            {
                var nativeData = new NativeArray<Vector3>(_tr.positionCount, Allocator.TempJob);

                _tr.GetPositions(nativeData);

                new ShiftPoints
                    {
                        data  = nativeData,
                        shift = -offset
                    }.ScheduleParallel(_tr.positionCount, 16, default)
                     .Complete();

                _tr.SetPositions(nativeData);
            }
        }

        private void HandleCollision(Collision2D other)
        {
            foreach (var contact in other.contacts)
            {
                var angle                 = Vector2.Angle(contact.normal, -Physics2D.gravity);
                if (angle < 60) _grounded = true;
            }
        }

        public struct ShiftPoints : IJobFor
        {
            public NativeArray<Vector3> data;
            public Vector2              shift;

            public void Execute(int index)
            {
                data[index] += (Vector3) shift;
            }
        }
    }
}