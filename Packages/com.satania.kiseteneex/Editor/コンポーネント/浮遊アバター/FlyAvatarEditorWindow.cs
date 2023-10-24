
#if UNITY_EDITOR
using UnityEditor;

using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace Saturnian_FlyAvatarEditor
{
    public partial class FlyAvatarEditor : EditorWindow
    {
        public static VRCAvatarDescriptor Avatar;
        public static float Height = 0.0f;
        public static bool isLoop = false;

        /// <summary>
        /// エディタのタイトル
        /// </summary>
        public static string EditorTitle = "浮遊アバター セットアップ";

        [MenuItem("さたにあしょっぴんぐ/導入ツール/浮遊アバター セットアップ")]
        private static void Init()
        {
            //ウィンドウのインスタンスを生成
            FlyAvatarEditor window = GetWindow<FlyAvatarEditor>();

            //ウィンドウサイズを固定
            window.maxSize = window.minSize = new Vector2(512, 300);

            //タイトルを変更
            window.titleContent = new GUIContent(EditorTitle);
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

        private void ShowGUI()
        {
            Avatar = EditorGUILayout.ObjectField("アバター", Avatar, typeof(VRCAvatarDescriptor), true) as VRCAvatarDescriptor;

            EditorGUI.BeginChangeCheck();
            Height = EditorGUILayout.FloatField("浮かせる高さ", Height);

            isLoop = EditorGUILayout.Toggle("自動で反映", isLoop);

            if (EditorGUI.EndChangeCheck() && isLoop)
            {
                changeArmatureHeight(Avatar.transform, Height);
            }

            if (GUILayout.Button("実行"))
            {
                changeArmatureHeight(Avatar.transform, Height);
            }
        }

        /// <summary>
        /// GUI描画用
        /// </summary>
        public void OnGUI()
        {
            ShowGUI();
        }

        public Transform GetArmature(Transform root, Animator animator)
        {
            //Armatureの中にHipsがあると仮定してHipsを取得
            var hips = animator.GetBoneTransform(HumanBodyBones.Hips);
            if (!hips)
                return null;

            //ルート直下のオブジェクトにあたるまでループ
            var curret = hips;
            while (curret.parent != root)
            {
                curret = curret.parent;
            }

            return curret;
        }

        public Transform GetDummyArmatrure(Transform root, Transform armature)
        {
            for (int i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i);
                if (child != armature && child.name == armature.name)
                {
                    return child;
                }
            }

            return null;
        }

        public Transform GetOrCreateDummyArmature(Transform root, out Transform armature)
        {
            armature = null;

            Animator root_animator = root.GetComponent<Animator>();
            if (root_animator == null || !root_animator.isHuman)
                return null;

            //Armatureを取得
            armature = GetArmature(root, root_animator);
            if (armature == null)
                return null;

            //ダミーのArmatureがある可能性があるので探す
            var dummy = GetDummyArmatrure(root, armature);

            //ダミーがない場合は作成
            if (dummy == null)
            {
                //アーマチュアと同じ名前で作成
                GameObject Dummy = new GameObject(armature.name);

                //アバターのルート直下に移動
                Dummy.transform.SetParent(root);

                //ルート直下の一番上へ移動 (Armatureより上にないと浮遊しないため)
                Dummy.transform.SetAsFirstSibling();

                dummy = Dummy.transform;
            }

            dummy.transform.position = root.transform.position;
            dummy.transform.rotation = root.transform.rotation;

            EditorUtility.SetDirty(dummy);

            return dummy;
        }

        public void changeArmatureHeight(Transform root, float height)
        {
            VRCAvatarDescriptor avatar_descriptor = root.GetComponent<VRCAvatarDescriptor>();
            if (avatar_descriptor == null)
                return;

            if (PrefabUtility.GetPrefabAssetType(root) != PrefabAssetType.NotAPrefab)
                PrefabUtility.UnpackPrefabInstance(root.gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);

            Animator avatar_animator = root.GetComponent<Animator>();
            if (avatar_animator == null || !avatar_animator.isHuman)
                return;

            var hips_transform = avatar_animator.GetBoneTransform(HumanBodyBones.Hips);
            float beforeHipsY = hips_transform.position.y;

            Transform armature;
            Transform dummy_armature = GetOrCreateDummyArmature(root, out armature);

            Undo.RecordObject(armature, "[FlyAvatarSetup] Chnage Armature Position");
            armature.localPosition = new Vector3(armature.localPosition.x, height, armature.localPosition.z);

            float height_diff = hips_transform.position.y - beforeHipsY;

            //float viewposition_height = avatar_descriptor.ViewPosition.y - height_diff;
            Undo.RecordObject(avatar_descriptor, "[FlyAvatarSetup] Change ViewPosition");

            //ビューポイントを変更
            avatar_descriptor.ViewPosition = new Vector3(avatar_descriptor.ViewPosition.x, avatar_descriptor.ViewPosition.y - height_diff, avatar_descriptor.ViewPosition.z);
            EditorUtility.SetDirty(armature);
        }
    }
}

#endif