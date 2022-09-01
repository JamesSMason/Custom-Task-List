using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System;

namespace GameDevTV.Tasks
{
    public class TaskListEditor : EditorWindow
    {
        VisualElement container;

        TextField taskText;
        Button addTaskButton;
        ScrollView taskListScrollView;

        public const string path = "Assets/GameDev.tv Assets/TaskList/Editor/EditorWindow/";

        [MenuItem("GameDev.tv/Task List")]
        public static void ShowWindow()
        {
            TaskListEditor window = GetWindow<TaskListEditor>();
            window.titleContent = new GUIContent("Task List");
        }

        public void CreateGUI()
        {
            container = rootVisualElement;
            VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path + "TaskListEditor.uxml");
            container.Add(original.Instantiate());

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path + "TaskListEditor.uss");
            container.styleSheets.Add(styleSheet);

            taskText = container.Q<TextField>("taskText");

            addTaskButton = container.Q<Button>("addTaskButton");
            addTaskButton.clicked += AddTask;

            taskListScrollView = container.Q<ScrollView>("taskList");

        }

        private void AddTask()
        {
            Debug.Log("Task added.");
        }
    }
}