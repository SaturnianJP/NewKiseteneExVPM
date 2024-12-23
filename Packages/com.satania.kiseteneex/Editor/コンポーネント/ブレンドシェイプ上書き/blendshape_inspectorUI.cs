using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace sataniashoping.editor
{
    // 拡張するクラスを指定する
    [CustomEditor(typeof(BlendShapeOverrider))]
    public class blendshape_inspectorUI : Editor
    {
        bool group_toggle = false;
        bool slider_toggle = false;
        SkinnedMeshRenderer skinnedMesh;

        // targetを変換して対象スクリプトの参照を取得する
        BlendShapeOverrider script => target as BlendShapeOverrider;

        Vector2 scrollPosition = new Vector2(0, 0);

        private void SetBlendshape()
        {
            script.SetBlendshape();
            EditorUtility.DisplayDialog("BlendShape Override", "変更しました。", "OK");

            DestroyImmediate(script);
            //EditorUtility.SetDirty(overriderGO);
        }

        private void GetBlendshapes()
        {
            script.mesh_path = AnimationUtility.CalculateTransformPath(skinnedMesh?.transform,
                                script.GetComponentInParent<VRCAvatarDescriptor>()?.transform);

            script.GetBlendshapedatas();

            EditorUtility.SetDirty(script);
            //EditorUtility.DisplayDialog("BlendShape Override", "取得しました。", "OK");
        }

        private void DrawBlendshapeList(List<string> datas)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            for (int i = 0; i < datas.Count; i++)
            {
                string name = datas[i];
                float maxweight = script.override_datas_maxweight[i];

                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(!script.override_datas_isoverride[i]);
                script.override_datas_weight[i] = EditorGUILayout.Slider(name, script.override_datas_weight[i], 0f, maxweight, GUILayout.MaxWidth(1000));
                EditorGUI.EndDisabledGroup();

                //GUILayout.FlexibleSpace();
                script.override_datas_isoverride[i] = EditorGUILayout.Toggle(script.override_datas_isoverride[i], GUILayout.MaxWidth(30));
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        private void OnEnable()
        {
            skinnedMesh = script.SkinnedMesh;
        }

        void Reset()
        {

        }

        public override void OnInspectorGUI()
        {
            //Reset
            if (skinnedMesh != null && string.IsNullOrEmpty(script.mesh_path))
                skinnedMesh = null;

            if (GUILayout.Button("ブレンドシェイプを適用"))
                SetBlendshape();

            GUILayout.Space(10);

            group_toggle = EditorGUILayout.Toggle($"開発者用オプション", group_toggle);

            if (group_toggle)
            {
                script.mesh_path = EditorGUILayout.TextField("メッシュのパス", script.mesh_path);

                EditorGUI.BeginChangeCheck();
                skinnedMesh = EditorGUILayout.ObjectField("メッシュ", skinnedMesh, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;
                if (EditorGUI.EndChangeCheck())
                {
                    script.mesh_path = AnimationUtility.CalculateTransformPath(skinnedMesh?.transform,
                        script.GetComponentInParent<VRCAvatarDescriptor>()?.transform);

                    GetBlendshapes();

                    EditorUtility.SetDirty(script);
                }

                //if (GUILayout.Button($"ブレンドシェイプを取得"))
                //    GetBlendshapes();

                var datas = script.override_datas_name;
                if (datas != null && datas.Count > 0)
                {
                    slider_toggle = EditorGUILayout.ToggleLeft("ブレンドシェイプ一覧", slider_toggle);

                    if (slider_toggle)
                        DrawBlendshapeList(datas);
                }
            }
        }
    }
}