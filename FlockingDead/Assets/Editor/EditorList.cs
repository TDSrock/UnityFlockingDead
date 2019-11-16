using UnityEditor;
using UnityEngine;
using System;
using System.Text;

[Flags]
public enum EditorListOption
{
    None = 0,
    ListSize = 1,
    ListLabel = 2,
    ElementLabels = 4,
    Buttons = 8,
    NamedElementLabels = 16,
    ShowMultiListOverride = 32,
    Default = ListSize | ListLabel | ElementLabels,
    NoElementLabels = ListSize | ListLabel,
    All = Default | Buttons
}

public static class EditorList
{

    private static GUIContent
        moveButtonContent = new GUIContent("\u2193", "move down"),
        duplicateButtonContent = new GUIContent("+", "duplicate"),
        deleteButtonContent = new GUIContent("-", "delete"),
        clearButtonContent = new GUIContent("Clear", "Clear the list");


    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

    public static void Show(SerializedProperty list, EditorListOption options = EditorListOption.Default)
    {
        if(list == null)
        {
            EditorGUILayout.HelpBox("The list provided is null", MessageType.Error);
            return;
        }
        if (!list.isArray)
        {
            EditorGUILayout.HelpBox(list.name + " is neither an array nor a list!", MessageType.Error);
            return;
        }

        bool showListLabel = (options & EditorListOption.ListLabel) != 0,
            showListSize = (options & EditorListOption.ListSize) != 0,
            showListOverride = (options & EditorListOption.ShowMultiListOverride) != 0;

        if (showListLabel)
        {
            EditorGUILayout.PropertyField(list);
            EditorGUI.indentLevel += 1;
        }
        if (!showListLabel || list.isExpanded)
        {
            SerializedProperty size = list.FindPropertyRelative("Array.size");
            if (showListSize)
            {
                EditorGUILayout.PropertyField(size);
            }
            if (size.hasMultipleDifferentValues && showListOverride)
            {
                EditorGUILayout.HelpBox("Not showing lists with different sizes.", MessageType.Info);
            }
            else
            {
                ShowElements(list, options);
            }
        }
        if (showListLabel)
        {
            EditorGUI.indentLevel -= 1;
        }
    }

    /// <summary>
    /// Shows the elements.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="options">The options.</param>
    private static void ShowElements(SerializedProperty list, EditorListOption options)
    {
        bool showElementLabels = (options & EditorListOption.ElementLabels) != 0,
            showButtons = (options & EditorListOption.Buttons) != 0,
            namedElementLabels = (options & EditorListOption.NamedElementLabels) != 0;

        int digitCount = (int)Math.Floor(Math.Log10(list.arraySize - 1) + 1);
        if (digitCount < 1)
            digitCount = 1;
        string numberFormat = "[{0," + digitCount + "}]";
        for (int i = 0; i < list.arraySize; i++)
        {
            GUIContent content = new GUIContent();
            if (showButtons || (namedElementLabels && showElementLabels))
            {
                EditorGUILayout.BeginHorizontal();
            }
            if (showElementLabels)
            {
                if (namedElementLabels)
                {
                    var elementAtI = list.GetArrayElementAtIndex(i);
                    if (elementAtI != null)
                        if (elementAtI.objectReferenceValue != null)
                        {
                            GUILayout.Label(string.Format(numberFormat + " {1}", i, list.GetArrayElementAtIndex(i).objectReferenceValue.name));
                            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), GUIContent.none, GUILayout.Width(150f));
                        }
                        else
                            content.text = string.Format(numberFormat + " {1}", i, "Null");
                    else
                        content.text = string.Format(numberFormat + " {1}", i, "Null");
                }
                else
                {
                    content.text = string.Format(numberFormat, i);
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), content);
                }
            }
            else
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), content);

            if (showButtons)
            {
                ShowButtons(list, i);
                EditorGUILayout.EndHorizontal();
            }
            else if (namedElementLabels && showElementLabels)
            {
                EditorGUILayout.EndHorizontal();
            }
        }
        if (showButtons)
        {
            if (GUILayout.Button(clearButtonContent))
                list.ClearArray();
        }
    }

    private static void ShowButtons(SerializedProperty list, int index)
    {
        if (GUILayout.Button(moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth))
        {
            list.MoveArrayElement(index, index + 1);
        }
        if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
        {
            list.InsertArrayElementAtIndex(index);
        }
        if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
        {
            int oldSize = list.arraySize;
            list.DeleteArrayElementAtIndex(index);
            if (list.arraySize == oldSize)
            {
                list.DeleteArrayElementAtIndex(index);
            }
        }
    }
}