using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace StuckInALoop.EditorTools.Editor
{
    [CustomEditor(typeof(BaseBehavior), true),CanEditMultipleObjects]
    public class BaseBehaviourEditor : UnityEditor.Editor
    {
        public Dictionary<string, List<Action>> CommandMethods = new Dictionary<string, List<Action>>();

        private IEnumerable<BaseBehavior> _tars;

        private void OnEnable()
        {
            _tars = targets.Cast<BaseBehavior>();

            CommandMethods.Clear();

            var methods = _tars.First().GetType()
                               .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var eca = typeof(ButtonAttribute);
            foreach (var method in methods)
            {
                var attributes = method.CustomAttributes;

                foreach (var attribute in attributes)
                    if (attribute.AttributeType == eca)
                    {
                        var ctorArgs = attribute.ConstructorArguments;

                        object[] args             = null;
                        var      playmodeOnlyFlag = false;

                        if (ctorArgs.Count > 0)
                            playmodeOnlyFlag = (bool) attribute.ConstructorArguments[0].Value;
                        if (ctorArgs.Count > 1)
                        {
                            var v =
                                attribute.ConstructorArguments[1].Value as
                                    ReadOnlyCollection<CustomAttributeTypedArgument>;
                            args = v.Select(attrib => attrib.Value).ToArray();
                        }

                        var actions = new List<Action>();
                        foreach (var tar in _tars)
                            actions.Add(() =>
                            {
                                if (playmodeOnlyFlag && !Application.isPlaying) return;
                                method.Invoke(tar, args);
                            });

                        CommandMethods.Add(method.Name, actions);
                    }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            foreach (var commandMethod in CommandMethods)
                if (GUILayout.Button(commandMethod.Key))
                    foreach (var action in commandMethod.Value)
                        action.Invoke();
        }
    }
}