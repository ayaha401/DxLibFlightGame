using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MapMakeEditor : EditorWindow
{
    private List<GameObject> _targetObjList = new List<GameObject>();       // �I������Object�̔z��
    private Vector2 scrollPosition = Vector2.zero;                          // �X�N���[�����镔���̍��W

    [SerializeField]
    float mul = 0;                                                          // Position���g�傷�邽�߂Ɏg�p

    SerializedObject so;

    private List<Vector3> _positionList = new List<Vector3>();              // �I�u�W�F�N�g��Position�̔z��
    private List<Vector3> _rotateList = new List<Vector3>();                // �I�u�W�F�N�g��Rotate�̔z��
    private List<Vector3> _scaleList = new List<Vector3>();                 // �I�u�W�F�N�g��Scale�̔z��

    SerializedProperty mulProp;

    // inspector�E�B���h�E�ׂ̗�Editor�g���E�B���h�E���쐬
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

    // �ΏۃI�u�W�F�N�g�擾
    void GetTarget()
    {
        // ��x�S������
        _targetObjList.Clear();

        // inspector�őI�������I�u�W�F�N�g�̐�����Selection.gameObjects�Ɋi�[�����
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            // �J�����ƃ��C�g�����O����
            if(Selection.gameObjects[i].gameObject.GetComponent<Light>() == false &&
               Selection.gameObjects[i].gameObject.GetComponent<Camera>() == false)
            {
                // �z��Ɋi�[
                _targetObjList.Add(Selection.gameObjects[i]);
            }
        }
    }

    // �擾�I�u�W�F�N�g���X�N���[���ł����ʂɕ\��
    void ShowSelectObjects()
    {
        using (new EditorGUILayout.VerticalScope("box"))
        {
            for (int i = 0; i < _targetObjList.Count; i++)
            {
                if (_targetObjList[i] == null) return;

                // ���X�g�����������p��bool
                bool toggle = true;

                using (new EditorGUILayout.HorizontalScope())
                {
                    // �`�F�b�N���O���ꂽ�����
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

    // �o�͐�p�X�𐶐�
    private string MakeOutputFilePath()
    {
        string resultPath = "";
        resultPath = Application.dataPath;
        return resultPath + "/" +"MapDate" + ".txt";
    }

    // txt��string����������
    private void MakeMapDate(string str)
    {
        string path = MakeOutputFilePath();
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(str);
        writer.Flush();
        writer.Close();
    }

    // txt�ɏ������ނ��߂�string���쐬����
    private string MakeStringDate()
    {
        // ��x���Z�b�g�̂��߂ɏ���
        _positionList.Clear();
        _rotateList.Clear();
        _scaleList.Clear();

        string sringDate = "";

        // ���X�g�ɑI�������I�u�W�F�N�g�̕��̃f�[�^���i�[
        for (int i = 0; i < _targetObjList.Count; i++)
        {
            _positionList.Add(_targetObjList[i].gameObject.transform.position * mulProp.floatValue);
            _rotateList.Add(_targetObjList[i].gameObject.transform.localEulerAngles);
            _scaleList.Add(_targetObjList[i].gameObject.transform.localScale);
        }

        // string���쐬
        for (int i = 0; i < _targetObjList.Count; i++)
        {
            // �f�o�b�O�p
            Debug.Log($"VGet({(int)_positionList[i].x}.0f, {(int)_positionList[i].y}.0f, {(int)_positionList[i].z}.0f), " +
                $"VGet({(int)_rotateList[i].x}.0f, {(int)_rotateList[i].y}.0f, {(int)_rotateList[i].z}.0f), " +
                $"VGet({(int)_scaleList[i].x}.0f, {(int)_scaleList[i].y}.0f, {(int)_scaleList[i].z}.0f)");

            // string
            // <<�I�u�W�F�N�g��>> VGet(0.0f, 0.0f, 0.0f), VGet(0.0f, 0.0f, 0.0f), VGet(1.0f, 1.0f, 1.0f)
            // �ƂȂ�
            sringDate += $"<<{_targetObjList[i].gameObject.name}>>  " +
                $"VGet({(int)_positionList[i].x}.0f, {(int)_positionList[i].y}.0f, {(int)_positionList[i].z}.0f), " +
                $"VGet({(int)_rotateList[i].x}.0f, {(int)_rotateList[i].y}.0f, {(int)_rotateList[i].z}.0f), " +
                $"VGet({(int)_scaleList[i].x}.0f, {(int)_scaleList[i].y}.0f, {(int)_scaleList[i].z}.0f)\n";
        }
        return sringDate;
    }

    // �E�B���h�E��`��
    private void OnGUI()
    {
        // ===================================================================
        GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
        // ===================================================================

        // Editor�g���̃o�[�W��������\�L
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

        // �I�������I�u�W�F�N�g���擾����
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

        // �I�������I�u�W�F�N�g���ꗗ�\������
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

        // �f�[�^���쐬����{�^��
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

        // Position���g�傷�鐔�l
        GUILayout.Label("Expansion", EditorStyles.boldLabel);
        using (new EditorGUILayout.VerticalScope("box"))
        {
            EditorGUI.BeginChangeCheck();

            mulProp.floatValue = EditorGUILayout.FloatField(mulProp.floatValue);
        }

        // �����L���b�V���ɒl��ۑ�����
        so.ApplyModifiedProperties();
    }
}
