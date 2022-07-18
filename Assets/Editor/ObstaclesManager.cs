using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Collections;


class GroundBool
{
    public Button button;
    public Ground ground;
    public int k;
    public GroundBool(Button button, Ground ground, int k)
    {
        this.button = button;
        this.ground = ground;
        this.k = k;
    }
}

public class ObstaclesManager : EditorWindow
{
    static GroundInfo info;
    static VisualElement root;
    static Color color = new(0.000f, 0.547f, 0.156f, 1);
    [MenuItem("Window/Obstacles Manager")]

    public static void ShowExample()
    {
        ObstaclesManager wnd = GetWindow<ObstaclesManager>();
        wnd.titleContent = new GUIContent("Obstacles Manager");
    }

    public void CreateGUI()
    {
        root = rootVisualElement;
    }

    public static void GenerateUI(Vector2 size, List<List<Ground>> groundList, GameObject sphere, GameObject point)
    {
        info = Resources.Load<GroundInfo>("GroundInfo");
        var pointingSphere = Instantiate(point);
        pointingSphere.transform.SetParent(groundList[0][0].transform);

        int k = 0;
        if (size != null && groundList != null && root != null)
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/ObstaclesManager.uss");
            root.styleSheets.Add(styleSheet);

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ObstaclesManager.uxml");
            var labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            var mainVisualElement = new VisualElement();
            mainVisualElement.AddToClassList("main");
            root.Add(mainVisualElement);

            var innerVisualElement = new VisualElement();
            innerVisualElement.AddToClassList("inner");
            mainVisualElement.Add(innerVisualElement);

            for (int i = 0; i <= size.x; i++)
            {
                var longVisualElement = new VisualElement();
                if (i == 0)
                {
                    longVisualElement.AddToClassList("first_long_visualElement");
                    innerVisualElement.Add(longVisualElement);
                }
                else
                {
                    longVisualElement.AddToClassList("last_long_visualElement");
                    innerVisualElement.Add(longVisualElement);
                }
                for (int j = 0; j <= size.y; j++)
                {
                    var smallVisualElement = new VisualElement();
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            smallVisualElement.AddToClassList("first_small_visualElement");
                            longVisualElement.Add(smallVisualElement);
                            var label = new Label();
                            label.AddToClassList("label_xy");
                            smallVisualElement.Add(label);
                            label.text = "x/y";
                        }
                        else
                        {
                            smallVisualElement.AddToClassList("last_small_visualElement");
                            longVisualElement.Add(smallVisualElement);
                            var label = new Label();
                            label.AddToClassList("label-style");
                            smallVisualElement.Add(label);
                            label.text = (j - 1) + "";
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            smallVisualElement.AddToClassList("first_small_visualElement");
                            longVisualElement.Add(smallVisualElement);
                            var label = new Label();
                            label.AddToClassList("label-style");
                            smallVisualElement.Add(label);
                            smallVisualElement.Add(label);
                            label.text = (i - 1) + "";
                        }
                        else
                        {
                            smallVisualElement.AddToClassList("last_small_visualElement");
                            longVisualElement.Add(smallVisualElement);
                            var button = new Button() { };
                            button.AddToClassList("button-style");
                            button.name = String.Format("{0}/{1}", i - 1, j - 1);
                            button.style.backgroundColor = color;
                            button.text = button.name;
                            if (groundList[i - 1][j - 1].isMovable == false)
                            {
                                button.style.backgroundColor = Color.red;
                            }
                            GroundBool obj = new(button, groundList[i - 1][j - 1], k); ;
                            button.RegisterCallback<MouseUpEvent>((evt) => OnClick(obj, sphere));
                            button.RegisterCallback<MouseEnterEvent>((evt) => OnEnter(obj, pointingSphere));
                            button.RegisterCallback<MouseOutEvent>((evt) => OnOut(obj, pointingSphere));
                            smallVisualElement.Add(button);
                            k++;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("Open Obstacles Manager Window!!");
        }
    }

    private static void OnClick(GroundBool groundBool, GameObject sphere)
    {
        if (groundBool.ground.isMovable)
        {
            info.obstacles[groundBool.k] = false;
            groundBool.ground.isMovable = false;
            var tempSphere = Instantiate(sphere);
            tempSphere.transform.SetParent(groundBool.ground.transform);
            tempSphere.transform.localPosition = new Vector3(0f, 0.5f, 0);
            groundBool.button.style.backgroundColor = Color.red;
        }
        else
        {
            info.obstacles[groundBool.k] = true;
            groundBool.ground.isMovable = true;
            DestroyImmediate(groundBool.ground.transform.GetChild(0).gameObject);
            groundBool.button.style.backgroundColor = color;
        }
    }
    private static void OnEnter(GroundBool groundBool, GameObject sphere)
    {
        if (sphere != null)
        {
            sphere.transform.SetParent(groundBool.ground.transform);
            sphere.transform.localPosition = new Vector3(0f, 0.5f, 0);
        }
        if (groundBool.ground.isMovable)
        {
            groundBool.button.style.backgroundColor = Color.red;
        }
    }
    private static void OnOut(GroundBool groundBool, GameObject sphere)
    {
        if (groundBool.ground.isMovable)
        {
            groundBool.button.style.backgroundColor = color;
        }

        if (sphere is null)
        {
            throw new ArgumentNullException(nameof(sphere));
        }
    }
    public static void ClearUI()
    {
        if (root != null)
        {
            root.Clear();
        }
    }
}