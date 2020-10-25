using System;
using UnityEngine;

namespace StuckInALoop.EditorTools
{
    public class AssertFieldNotNullAttribute : PropertyAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class GetNameAttribute : PropertyAttribute
    {
        public readonly string valueSource;

        public GetNameAttribute(string labelName)
        {
            valueSource = labelName;
        }
    }
}