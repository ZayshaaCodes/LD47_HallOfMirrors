using UnityEngine;

namespace StuckInALoop
{
    public class ButtonGroup : MonoBehaviour, IDestroyOnClone
    {
        public int activeButtonIndex;

        public WorldButton[] buttons;

        private void OnEnable()
        {
            buttons = GetComponentsInChildren<WorldButton>();

            for (var i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                button.buttonindex = i;
                button.ButtonActivateEvent.AddListener(OnButtonPressed);
            }

            // buttons[activeButtonIndex].PressButton(false);
        }

        private void OnButtonPressed(int buttonIndex)
        {
            for (var i = 0; i < buttons.Length; i++)
                if (i != buttonIndex)
                    buttons[i].DepressButton(false);
        }
    }
}