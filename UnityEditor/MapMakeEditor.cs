using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MapMakeEditor : EditorWindow
{
    private List<GameObject> _targetObjList = new List<GameObject>();       // 選択したObjectの配列
    private Vector2 scrollPosition = Vector2.zero;                          // スクロールする部分の座標

    [SerializeField]
    float mul = 0;                                                          // Positionを拡大するために使用

    SerializedObject so;

    private List<Vector3> _positionList = new List<Vector3>();              // オブジェクトのPositionの配列
    private List<Vector3> _rotateList = new List<Vector3>();                // オブジェクトのRotateの配列
    private List<Vector3> _scaleList = new List<Vector3>();                 // オブジェクトのScaleの配列

    SerializedProperty mulProp;

    // inspectorウィンドウの隣にEditor拡張ウィンドウを作成
    [MenuItem("Tools/MapMakeEditor")]
    private static void OpenWindow()
    {
        GetWindow<MapMakeEditor>("MapMakeEditor",
            typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow"));
    }

    private void OnEnable()
    {
        so = new SerializedObject(this);
        mulProp = so.FindProperty("mul");
    }

    // 対象オブジェクト取得
    void GetTarget()
    {
        // 一度全部消去
        _targetObjList.Clear();

        // inspectorで選択したオブジェクトの数だけSelection.gameObjectsに格納される
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            // カメラとライトを除外する
            if(Selection.gameObjects[i].gameObject.GetComponent<Light>() == false &&
               Selection.gameObjects[i].gameObject.GetComponent<Camera>() == false)
            {
                // 配列に格納
                _targetObjList.Add(Selection.gameObjects[i]);
            }
        }
    }

    // 取得オブジェクトをスクロールできる画面に表示
    void ShowSelectObjects()
    {
        using (new EditorGUILayout.VerticalScope("box"))
        {
            for (int i = 0; i < _targetObjList.Count; i++)
            {
                if (_targetObjList[i] == null) return;

                // リストから解除する用のbool
                bool toggle = true;

                using (new EditorGUILayout.HorizontalScope())
                {
                    // チェックを外されたら解除
                    if (GUILayout.Toggle(toggle, "") == false)
                    {
                        _targetObjList.Remove(_targetObjList[i].gameObject);
                        return;
                    }
                    
                    GUILayout.Label(_targetObjList[i].name);
                    GUILayout.FlexibleSpace();
                }
            }
        }
    }

    // 出力先パスを生成
    private string MakeOutputFilePath()
    {
        string resultPath = "";
        resultPath = Application.dataPath;
        return resultPath + "/" +"MapDate" + ".txt";
    }

    // txtにstringを書き込む
    private void MakeMapDate(string str)
    {
        string path = MakeOutputFilePath();
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(str);
        writer.Flush();
        writer.Close();
    }

    // txtに書き込むためのstringを作成する
    private string MakeStringDate()
    {
        // 一度リセットのために消去
        _positionList.Clear();
        _rotateList.Clear();
        _scaleList.Clear();

        string sringDate = "";

        // リストに選択したオブジェクトの分のデータを格納
        for (int i = 0; i < _targetObjList.Count; i++)
        {
            _positionList.Add(_targetObjList[i].gameObject.transform.position * mulProp.floatValue);
            _rotateList.Add(_targetObjList[i].gameObject.transform.localEulerAngles);
            _scaleList.Add(_targetObjList[i].gameObject.transform.localScale);
        }

        // stringを作成
        for (int i = 0; i < _targetObjList.Count; i++)
        {
            // デバッグ用
            Debug.Log($"VGet({(int)_positionList[i].x}.0f, {(int)_positionList[i].y}.0f, {(int)_positionList[i].z}.0f), " +
                $"VGet({(int)_rotateList[i].x}.0f, {(int)_rotateList[i].y}.0f, {(int)_rotateList[i].z}.0f), " +
                $"VGet({(int)_scaleList[i].x}.0f, {(int)_scaleList[i].y}.0f, {(int)_scaleList[i].z}.0f)");

            // string
            // <<オブジェクト名>> VGet(0.0f, 0.0f, 0.0f), VGet(0.0f, 0.0f, 0.0f), VGet(1.0f, 1.0f, 1.0f)
            // となる
            sringDate += $"<<{_targetObjList[i].gameObject.name}>>  " +
                $"VGet({(int)_positionList[i].x}.0f, {(int)_positionList[i].y}.0f, {(int)_positionList[i].z}.0f), " +
                $"VGet({(int)_rotateList[i].x}.0f, {(int)_rotateList[i].y}.0f, {(int)_rotateList[i].z}.0f), " +
                $"VGet({(int)_scaleList[i].x}.0f, {(int)_scaleList[i].y}.0f, {(int)_scaleList[i].z}.0f)\n";
        }
        return sringDate;
    }

    // ウィンドウを描画
    private void OnGUI()
    {
        // ===================================================================
        GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
        // ===================================================================

        // Editor拡張のバージョン等を表記
        GUILayout.Label("Information", EditorStyles.boldLabel);
        using (new EditorGUILayout.VerticalScope("box"))
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("Version");
                GUILayout.Label("Version 1.0.0");
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Label("How to use (Japanese)");
                if (GUILayout.Button("How to use (Japanese)"))
                {
                    System.Diagnostics.Process.Start("https://github.com/ayaha401/DxLibFlightGame/wiki/MapMakeEditor");
                }
            }
        }

        // ===================================================================
        GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
        // ===================================================================

        // 選択したオブジェクトを取得する
        GUILayout.Label("GetObject", EditorStyles.boldLabel);
        using (new EditorGUILayout.VerticalScope("box"))
        {
            if (GUILayout.Button("GetObject"))
            {
                GetTarget();
            }
        }

        // ===================================================================
        GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
        // ===================================================================

        // 選択したオブジェクトを一覧表示する
        GUILayout.Label("SelectedObject", EditorStyles.boldLabel);
        using (new EditorGUILayout.VerticalScope("box"))
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            {
                ShowSelectObjects();
            }
            EditorGUILayout.EndScrollView();
        }

        // ===================================================================
        GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
        // ===================================================================

        // データを作成するボタン
        GUILayout.Label("MakeMapDate", EditorStyles.boldLabel);
        using (new EditorGUILayout.VerticalScope("box"))
        {
            if (GUILayout.Button("MakeMapDate"))
            {
                MakeMapDate(MakeStringDate());
            }
        }

        // ===================================================================
        GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
        // ===================================================================

        // Positionを拡大する数値
        GUILayout.Label("Expansion", EditorStyles.boldLabel);
        using (new EditorGUILayout.VerticalScope("box"))
        {
            EditorGUI.BeginChangeCheck();

            mulProp.floatValue = EditorGUILayout.FloatField(mulProp.floatValue);
        }

        // 内部キャッシュに値を保存する
        so.ApplyModifiedProperties();
    }
}
