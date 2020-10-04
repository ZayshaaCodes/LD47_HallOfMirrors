using UnityEngine;

public class ButtonPair : MonoBehaviour
{
    public WorldButton b1;
    public WorldButton b2;

    private void Start()
    {
        b1.ButtonActivateEvent.AddListener((i) =>
        {
            b2.DepressButton();
        });
        
        b2.ButtonActivateEvent.AddListener((id) =>
        {
            b1.DepressButton();
        });
    }
}
