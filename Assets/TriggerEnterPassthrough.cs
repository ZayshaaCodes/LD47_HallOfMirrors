using UnityEngine;

public class TriggerEnterPassthrough : MonoBehaviour, IDestroyOnClone
{
    public WorldButton target;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        target.OnTriggerEnter2D(other);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        target.OnTriggerExit2D(other);
    }
}
