#if UNITY_EDITOR
using System.Linq;
using System.Text;

using UnityEditor;

using UnityEngine;
using static VRC.Dynamics.CollisionScene;



// 拡張するクラスを指定する
[CustomEditor(typeof(sataniashoping.BlendShapeOverrider))]
public class blendshape_inspectorUI : Editor
{
    bool group_toggle = false;
    bool slider_toggle = false;

    public override void OnInspectorGUI()
    {
        // targetを変換して対象スクリプトの参照を取得する
        sataniashoping.BlendShapeOverrider override_target = target as sataniashoping.BlendShapeOverrider;

        override_target.mesh_path = EditorGUILayout.TextField("メッシュのパス", override_target.mesh_path);

        GameObject this_object = override_target.getTransform().gameObject;

        if (GUILayout.Button("ブレンドシェイプを変更"))
        {
            override_target.SetBlendshape();
            override_target.MessageBox("変更しました。", "OK");

            DestroyImmediate(override_target);
            EditorUtility.SetDirty(this_object);
        }

        GUILayout.Space(15);

        group_toggle = EditorGUILayout.Toggle($"開発者用オプション", group_toggle);

        if (group_toggle)
        {
            override_target.mesh = EditorGUILayout.ObjectField("メッシュ", override_target.mesh, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;

            if (GUILayout.Button($"ブレンドシェイプを取得"))
            {
                override_target.getBlendshapedatas();

                if (override_target.override_datas_name.Count > 0)
                {
                    override_target.mesh_path = override_target.getObjectPath(override_target.mesh.gameObject, override_target.mesh.transform.parent);
                    override_target.mesh = null;
                }
                else
                {
                    override_target.mesh_path = "";
                }

                EditorUtility.SetDirty(override_target);
                EditorUtility.SetDirty(override_target.getRoottransform());

                override_target.MessageBox("取得しました。", "OK");
            }

            if (override_target.override_datas_name != null)
            {
                slider_toggle = EditorGUILayout.ToggleLeft("ブレンドシェイプ一覧", slider_toggle);

                if (slider_toggle)
                {
                    var keys = override_target.override_datas_name;

                    for (int i = 0; i < keys.Count; i++)
                    {
                        string name = override_target.override_datas_name[i];
                        float maxweight = override_target.override_datas_maxweight[i];

                        EditorGUILayout.BeginHorizontal();
                        EditorGUI.BeginDisabledGroup(!override_target.override_datas_isoverride[i]);
                        override_target.override_datas_weight[i] = EditorGUILayout.Slider(name, override_target.override_datas_weight[i], 0f, maxweight, GUILayout.MaxWidth(1000));
                        EditorGUI.EndDisabledGroup();

                        //GUILayout.FlexibleSpace();
                        override_target.override_datas_isoverride[i] = EditorGUILayout.Toggle(override_target.override_datas_isoverride[i], GUILayout.MaxWidth(30));
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
        }
    }
}
#endif