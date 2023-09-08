#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace satania.fxmergetool
{
    public partial class SimplePocket : EditorWindow
    {
        VRCAvatarDescriptor _avatar;
        AnimatorController _srcController;

        /// <summary>
        /// エディタのタイトル
        /// </summary>
        public static string EditorTitle = "FX 合体ツール";

        [MenuItem("さたにあしょっぴんぐ/FX 合体ツール", priority = 12)]
        private static void Init()
        {
            //ウィンドウのインスタンスを生成
            SimplePocket window = GetWindow<SimplePocket>();

            //ウィンドウサイズを固定
            window.maxSize = window.minSize = new Vector2(512, 200);

            //タイトルを変更
            window.titleContent = new GUIContent(EditorTitle);
        }
        private Vector2 getWindowSize()
        {
            return position.size;
        }

        private bool isDarkmode()
        {
            return EditorGUIUtility.isProSkin;
        }

        private void drawLinkLabel(Rect rect, string msg, string link, GUIStyle style)
        {
            GUI.Label(rect, msg, style);
            Rect _rect = rect;
            EditorGUIUtility.AddCursorRect(_rect, MouseCursor.Link);
            Event nowEvent = Event.current;
            if (nowEvent.type == EventType.MouseDown && _rect.Contains(nowEvent.mousePosition))
            {
                Help.BrowseURL(link);
            }
        }

        /// <summary>
        /// OKのみのメッセージボックス
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="ok"></param>
        public static void MessageBox(string message, string ok)
        {
            EditorUtility.DisplayDialog(EditorTitle, message, ok);
        }

        private void ShowGUI()
        {
            _avatar = EditorGUILayout.ObjectField("導入するアバター", _avatar, typeof(VRCAvatarDescriptor), true) as VRCAvatarDescriptor;
            _srcController = EditorGUILayout.ObjectField("統合するコントローラ", _srcController, typeof(AnimatorController), true) as AnimatorController;

            if (GUILayout.Button("導入する"))
            {
                Doit();
            }

            var size = getWindowSize();

            size.x -= 310;
            size.y -= 45;

            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.UpperLeft;

            style.normal.textColor = isDarkmode() ? Color.white : Color.black;

            Rect position = new Rect { x = size.x, y = size.y, width = 310, height = 40 };
            drawLinkLabel(position, "アニメータの統合処理をVRCAvatar3Toolsからお借りしました！\nありがとうございました。\nhttps://booth.pm/ja/items/2207020", "https://booth.pm/ja/items/2207020", style);
          
        }

        private void Doit()
        {
            AnimatorController avatarController = _avatar.GetBaseLayer(ExtensionClasses.BaseLayerNums.FX);
            AnimatorController srcController = _srcController;

            if (avatarController != null && srcController != null)
                AnimatorControllerUtility.CombineAnimatorController(srcController, avatarController);

            MessageBox("完了しました！", "OK");
        }

        /// <summary>
        /// GUI描画用
        /// </summary>
        public void OnGUI()
        {
            ShowGUI();
        }
    }
}
#endif