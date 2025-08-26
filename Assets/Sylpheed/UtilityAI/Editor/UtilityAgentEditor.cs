using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Sylpheed.UtilityAI.Editor
{
    [CustomEditor(typeof(UtilityAgent))]
    public class UtilityAgentEditor : UnityEditor.Editor
    {
        private struct LabelColors
        {
            public static readonly Color Best = Color.green;
            public static Color Scored => GUI.color;
            public static readonly Color Skipped = Color.orange;
            public static readonly Color Unscored = Color.gray;
        }
        
        private static readonly Dictionary<int, bool> DecisionResultFoldoutStates = new();
        private static readonly Dictionary<int, bool> ConsiderationFoldoutStates = new();
        
        private UtilityAgent _agent;
        private Color _defaultLabelColor;
        
        private void OnEnable()
        {
            _agent = (UtilityAgent)target;
            _defaultLabelColor = GUI.color;
            RequiresConstantRepaint();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); // Draw the default inspector

            EditorGUILayout.Space();
            if (Application.isPlaying)
            {
                // Sort results
                var results = _agent.DecisionResults
                    .OrderByDescending(d => d.Best)
                    .ThenByDescending(d => d.Scored)
                    .ThenBy(d => d.Skipped)
                    .ThenByDescending(d => d.Score)
                    .ToList();

                foreach (var result in results)
                {
                    DrawDecisionResult(result);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Enter Play mode to view utility values.", MessageType.Info);
            }
        }

        private void DrawDecisionResult(DecisionResult result)
        {
            if (result.Best) GUI.color = LabelColors.Best;
            else if (result.Scored)
            {
                if (result.Skipped) GUI.color = LabelColors.Skipped;
                else GUI.color = LabelColors.Scored;
            }
            else GUI.color = LabelColors.Unscored;

            // Foldout header text
            var text = $"[{result.Decision.Score * 100:N0}] {result.Decision.Behavior.name}\t";
            if (result.Decision.Target) text += $" Target: {result.Decision.Target.name}";
            if (result.Decision.Data != null) text += $" Data: {result.Decision.Data}";
            
            // Check foldout state cache. Create a new entry if it doesn't exist yet.
            if (!DecisionResultFoldoutStates.TryGetValue(result.Hash, out _))
                DecisionResultFoldoutStates[result.Hash] = false;
            DecisionResultFoldoutStates[result.Hash] = EditorGUILayout.Foldout(DecisionResultFoldoutStates[result.Hash], text, true);
                    
            GUI.color = _defaultLabelColor;

            // Expand foldout
            if (DecisionResultFoldoutStates[result.Hash])
            {
                // Same decision
                if (result.IsSameDecision && !Mathf.Approximately(_agent.SameDecisionScoreBonus, 1f))
                {
                    GUI.color = Color.lightSeaGreen;
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField($"[{_agent.SameDecisionScoreBonus * 100:N0}] Same Decision Bonus");
                    EditorGUI.indentLevel--;
                    GUI.color = _defaultLabelColor;
                }
            
                // Draw considerations
                EditorGUI.indentLevel++;
                foreach (var consideration in result.Decision.Behavior.Considerations)
                {
                    DrawConsideration(consideration, result.Decision);
                }
                EditorGUI.indentLevel--;
            }
        }

        private void DrawConsideration(IConsideration consideration, Decision decision)
        {
            if (consideration.Ignored) return;
            
            var hash = decision.GetConsiderationHash(consideration);
            var score = _agent.GetCachedConsiderationScore(decision, consideration);
            GUI.color = score switch
            {
                >= 1f => Color.green,
                <= 0f => Color.gray,
                _ => _defaultLabelColor
            };

            // Label
            var text = $"[{_agent.GetCachedConsiderationScore(decision, consideration) * 100:N0}] {consideration.Name}";

            if (consideration.Children == null)
            {
                EditorGUILayout.LabelField(text);
            }
            else
            {
                // Check foldout state cache. Create a new entry if it doesn't exist yet.
                if (!ConsiderationFoldoutStates.TryGetValue(hash, out _))
                    ConsiderationFoldoutStates[hash] = false;
                ConsiderationFoldoutStates[hash] = EditorGUILayout.Foldout(ConsiderationFoldoutStates[hash], text, true);

                // Draw child recursively if expanded
                if (ConsiderationFoldoutStates[hash])
                {
                    EditorGUI.indentLevel++;
                    foreach (var child in consideration.Children)
                    {
                        DrawConsideration(child, decision);
                    }
                    EditorGUI.indentLevel--;
                }
            }

            // Reset label color
            GUI.color = _defaultLabelColor;
        }
    }
}