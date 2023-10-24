
#if UNITY_EDITOR

using UnityEditor;

using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace Saturnian_flyavatarsetup
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]

    public class FlyAvatarSetupTool : MonoBehaviour, IEditorOnly
    {
        public float flying_height = 0.0f;
        void Start()
        {

        }

        public Transform getRoottransform()
        {
            return transform.root;
        }

        public Transform getTransform()
        {
            return transform;
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
            avatar_descriptor.ViewPosition = new Vector3(avatar_descriptor.ViewPosition.x, avatar_descriptor.ViewPosition.y + height_diff, avatar_descriptor.ViewPosition.z);
            EditorUtility.SetDirty(armature);
        }
    }
}
#endif