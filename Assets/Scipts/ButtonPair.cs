using UnityEngine;

public class ButtonPair : MonoBehaviour
{
    public WorldButton b1;
    public WorldButton b2;

    private void Start()
    {
        b1.ButtonActivateEvent.AddListener(() =>
        {
            b2.DepressButton();
        });
        
        b2.ButtonActivateEvent.AddListener(() =>
        {
            b1.DepressButton();
        });
    }
}
