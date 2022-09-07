using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GameDevTV.Tasks
{
    public class TaskListEditor : EditorWindow
    {
        VisualElement container;
        ObjectField savedTasksObjectField;
        Button loadTasksButton;
        TextField taskText;
        Button addTaskButton;
        ScrollView taskListScrollView;

        TaskListSO taskListSO;

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

            TemplateContainer templateContainer = original.Instantiate();
            templateContainer.style.flexGrow = 1f;

            container.Add(templateContainer);

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path + "TaskListEditor.uss");
            container.styleSheets.Add(styleSheet);

            savedTasksObjectField = container.Q<ObjectField>("savedTasksObjectField");
            savedTasksObjectField.objectType = typeof(TaskListSO);

            loadTasksButton = container.Q<Button>("loadTasksButton");
            loadTasksButton.clicked += LoadTasks;

            taskText = container.Q<TextField>("taskText");
            taskText.RegisterCallback<KeyDownEvent>(AddTask);

            addTaskButton = container.Q<Button>("addTaskButton");
            addTaskButton.clicked += AddTask;

            taskListScrollView = container.Q<ScrollView>("taskList");
        }

        private void AddTask()
        {
            if (!string.IsNullOrEmpty(taskText.value))
            {
                taskListScrollView.Add(CreateTask(taskText.value));
                SaveTask(taskText.value);
                taskText.value = "";
                taskText.Focus();
            }   
        }

        private void AddTask(KeyDownEvent e)
        {
            if (Event.current.Equals(Event.KeyboardEvent("Return")))
            {
                AddTask();
            }
        }

        private Toggle CreateTask(string taskText)
        {
            Toggle taskItem = new Toggle();
            taskItem.text = taskText;
            return taskItem;
        }

        private void LoadTasks()
        {
            taskListSO = savedTasksObjectField.value as TaskListSO;

            if (taskListSO != null)
            {
                taskListScrollView.Clear();
                List<string> tasks = taskListSO.GetTasks();

                foreach (string task in tasks)
                {
                    taskListScrollView.Add(CreateTask(task));
                }
            }
        }

        private void SaveTask(string task)
        {
            taskListSO.AddTask(task);
            EditorUtility.SetDirty(taskListSO);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}