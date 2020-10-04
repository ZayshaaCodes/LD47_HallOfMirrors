using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    private Vector3 _initialOffset;

    private void Start()
    {
        _initialOffset = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.transform.position + _initialOffset;
    }
}