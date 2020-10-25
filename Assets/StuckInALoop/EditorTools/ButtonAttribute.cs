using System;

namespace StuckInALoop.EditorTools
{
    public class ButtonAttribute : Attribute
    {
        public ButtonAttribute(bool playmodeOnly = false, params object[] args)
        {
        }
    }
}