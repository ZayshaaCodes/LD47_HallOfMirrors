using StuckInALoop.EditorTools;
using UnityEditor;
using UnityEngine;

namespace StuckInALoop.EditorTools.Editor
{
    [CustomPropertyDrawer(typeof(AssertFieldNotNullAttribute))]
    public class AssertFieldNotNullAttributeDrawer : PropertyDrawer
    {
        private GUIStyle _redLabelStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //init style
            if (_redLabelStyle == null)
                _redLabelStyle = new GUIStyle(EditorStyles.largeLabel)
                {
                    normal    = {textColor = Color.red},
                    fontStyle = FontStyle.Bold
                };

            if (property.objectReferenceValue == null)
            {
                //Debug.LogError($"Value for {property.name} on {property.serializedObject.targetObject.name} is null but cannot be.");
                EditorGUI.LabelField(position, "Null", _redLabelStyle);
                position.xMin  += 35;
                position.width -= 35;
            }

            EditorGUI.PropertyField(position, property);
        }
    }


    [CustomPropertyDrawer(typeof(GetNameAttribute))]
    public class GetNameAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Debug.Log("test");
            var att = (GetNameAttribute) attribute;


            // property.stringValue = fieldProp.objectReferenceValue.ToString();
            //
            // EditorGUI.PropertyField(position, property, label);
        }
    }
}