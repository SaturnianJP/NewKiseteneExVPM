
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace Saturnian_flyavatarsetup
{
    // 拡張するクラスを指定する
    [CustomEditor(typeof(FlyAvatarSetupTool))]
    public class flyavatarsetup_UI : Editor
    {
        bool isLoop = false;

        public void Awake()
        {

        }

        /// <summary>
        /// OKのみのメッセージボックス
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="ok"></param>
        public static void MessageBox(string message, string ok)
        {
            EditorUtility.DisplayDialog("FlyAvatar Setup", message, ok);
        }

        public override void OnInspectorGUI()
        {
            // targetを変換して対象スクリプトの参照を取得する
            FlyAvatarSetupTool flyavatar_target = target as FlyAvatarSetupTool;

            EditorGUI.BeginChangeCheck();
            flyavatar_target.flying_height = EditorGUILayout.FloatField("浮かせる高さ", flyavatar_target.flying_height);

            isLoop = EditorGUILayout.Toggle("自動で反映", isLoop);

            Transform root = flyavatar_target.getRoottransform();

            if (EditorGUI.EndChangeCheck() && isLoop)
            {
                flyavatar_target.changeArmatureHeight(root, flyavatar_target.flying_height);
            }

            if (GUILayout.Button("実行"))
            {
                flyavatar_target.changeArmatureHeight(root, flyavatar_target.flying_height);

                bool isYes = EditorUtility.DisplayDialog("Fly Avatar Setup", "コンポーネントを削除しますか？\n(VRChatにアップロードする場合は [はい] を押してください。)", "はい", "いいえ");
                if (isYes)
                {
                    if (!EditorApplication.isPlaying)
                        DestroyImmediate(flyavatar_target);
                    else
                        Destroy(flyavatar_target);
                }


                EditorUtility.SetDirty(root);
            }
        }

    }
}
