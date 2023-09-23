#if UNITY_EDITOR
using sataniashoping;
using sataniashoping.component.kisetenecomponent_package;
//using Saturnian_flyavatarsetup;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

using VRC.SDK3.Dynamics.PhysBone.Components;

namespace Saturnian_NewKiseteneEx_Package
{
    public partial class NewKiseteneEx
    {
        /// <summary>
        /// 削除する際にここにためる
        /// </summary>
        private static List<Action> removeActions = new List<Action>();

        /// <summary>
        /// 指定されたオブジェクトがプレハブの場合 true を返します
        /// </summary>
        public static bool IsPrefab(UnityEngine.GameObject self)
        {
            var type = PrefabUtility.GetPrefabAssetType(self);

            return
                type == PrefabAssetType.Model ||
                type == PrefabAssetType.MissingAsset ||
                type == PrefabAssetType.Regular ||
                type == PrefabAssetType.Variant;
        }

        void bone_renamer(Transform trans, string name)
        {
            trans.gameObject.name += " " + name;

            if (trans.childCount == 0)
            {
                return;
            }

            foreach (Transform ob in trans.transform)
            {
                bone_renamer(ob, name);
            }
        }

        private void puton()
        {
            if (m_cloth == null)
                return;

            if (m_Avatar == null)
                return;

            if (m_cloth == m_Avatar.gameObject)
                return;

            if (!m_Avatar.isHuman)
                return;

            if (IsPrefab(m_cloth))
                PrefabUtility.UnpackPrefabInstance(m_cloth, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            if (BoneRenameToggle)
                bone_renamer(m_cloth.transform, BoneRenameWord);

            //アバターを複製
            Animator m_AvatarInstance = Instantiate(m_Avatar);

            //元の名前に
            m_AvatarInstance.name = m_Avatar.name;

            //服の名前つける
            m_AvatarInstance.name += $" ({m_cloth.name})";

            //元のアバターを非アクティブに
            m_Avatar.gameObject.SetActive(false);

            //一応Sceneのルートになるように
            m_AvatarInstance.transform.SetParent(null);

            //衣装をアバターのルートに
            m_cloth.transform.SetParent(m_AvatarInstance.transform);

            //キセテネコンポーネントをつける
            SetAllKiseteneComponent(m_cloth, m_cloth.name);

            //着せる
            SetBoneListParent(m_AvatarInstance);
            
            //しずくボーン用
            Shizuku.SetSubBones(m_AvatarInstance, m_bone);

            //ブレンドシェイプ絞るコンポーネントがついているなら実行する
            if (isAutoBlendshape)
            {
                var blendshapeoverrider = m_cloth.GetComponent<BlendShapeOverrider>();
                if (blendshapeoverrider != null)
                {
                    blendshapeoverrider.SetBlendshape();

                    if (!EditorApplication.isPlaying)
                        DestroyImmediate(blendshapeoverrider);
                    else
                        Destroy(blendshapeoverrider);
                }
            }

            //浮遊アバターコンポーネントついているなら実行
            //if (isAutoFlying)
            //{
            //    var flyingavatar = m_cloth.GetComponent<FlyAvatarSetupTool>();
            //    if (flyingavatar != null)
            //    {
            //        flyingavatar.changeArmatureHeight(flyingavatar.getRoottransform(), flyingavatar.flying_height);

            //        if (!EditorApplication.isPlaying)
            //            DestroyImmediate(flyingavatar);
            //        else
            //            Destroy(flyingavatar);
            //    }
            //}

            //複製後のオブジェクトをアクティブ化
            m_AvatarInstance.gameObject.SetActive(true);
        }

        void getPhysBoneObj(GameObject Object, ref Transform outObj)
        {
            outObj = null;

            var Comp = Object.GetComponent<VRCPhysBone>();
            if (Comp != null)
            {
                outObj = Comp.transform;
                return;
            }

            if (Object.transform.childCount == 0)
            {
                return;
            }

            foreach (Transform ob in Object.transform)
            {
                getPhysBoneObj(ob.gameObject, ref outObj);
            }
        }

        void getPhysBoneObj(GameObject Object, ref VRCPhysBone physbone)
        {
            physbone = null;

            var Comp = Object.GetComponent<VRCPhysBone>();
            if (Comp != null)
            {
                physbone = Comp;
                return;
            }

            if (Object.transform.childCount == 0)
            {
                return;
            }

            foreach (Transform ob in Object.transform)
            {
                getPhysBoneObj(ob.gameObject, ref physbone);
            }
        }

        /// <summary>
        /// 胸をペアレントでくっつける場合の処理
        /// </summary>
        /// <param name="Bone">操作するボーン</param>
        /// <param name="Target">くっつける先</param>
        void SetParentConstraintForBreast(Transform Bone, Transform Target)
        {
            if (Bone == null)
                return;

            Vector3 DefaultPos = Target.transform.position;
            Quaternion DefaultQua = Target.transform.rotation;

            ParentConstraint parent = Bone.gameObject.AddComponent<ParentConstraint>();
            ConstraintSource source = new ConstraintSource();
            if (Bone.GetComponent<VRCPhysBone>() != null)
                DestroyImmediate(Bone.GetComponent<VRCPhysBone>());

            //ターゲットを追加
            source.sourceTransform = Target;
            source.weight = 1;

            //コンポーネントに設定
            parent.AddSource(source);
            parent.constraintActive = true;
            parent.translationAtRest = DefaultPos;
            parent.rotationAtRest = DefaultQua.eulerAngles;

            Transform children = Bone.GetComponentInChildren<Transform>();
            if (children.childCount == 0)
            {
                return;
            }

            int index = 0;

            foreach (Transform tf in children)
            {
                if (Target.childCount == 0)
                    break;

                if (Target.childCount <= index)
                    break;

                Transform TargetNextTransform = Target.GetChild(index);
                if (TargetNextTransform != null)
                    SetParentConstraintForBreast(tf, TargetNextTransform);

                index++;
            }
        }

        /// <summary>
        /// 胸をペアレントでくっつける場合の処理
        /// </summary>
        /// <param name="bone">操作するボーン</param>
        void SetBreastParentConstraint(Transform bone)
        {
            if (Breast_L == Breast_R)
                return;

            Transform AvatarBreastL = FindBone_FullMatch(bone, Breast_L_Regex);
            if (AvatarBreastL != null)
            {
                var BreLComp = AvatarBreastL.GetComponent<VRCPhysBone>();
                if (AvatarBreastL != null && BreLComp == null && AvatarBreastL.name != Breast_L.name)
                {
                    Transform outObj = null;
                    getPhysBoneObj(AvatarBreastL.gameObject, ref outObj);

                    if (outObj != null)
                        AvatarBreastL = outObj.transform;
                }

                if (AvatarBreastL != null)
                    SetParentConstraintForBreast(Breast_L, AvatarBreastL);
            }

            Transform AvatarBreastR = FindBone_FullMatch(bone, Breast_R_Regex);
            if (AvatarBreastR != null)
            {
                var BreRComp = AvatarBreastR.GetComponent<VRCPhysBone>();
                if (AvatarBreastR != null && BreRComp == null && AvatarBreastR.name != Breast_R.name)
                {
                    Transform outObj = null;
                    getPhysBoneObj(AvatarBreastR.gameObject, ref outObj);

                    if (outObj != null)
                        AvatarBreastR = outObj.transform;
                }

                if (AvatarBreastR != null)
                    SetParentConstraintForBreast(Breast_R, AvatarBreastR);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="physbone"></param>
        /// <param name="bone">服のボーン</param>
        /// <param name="Target">アバター側のボーン</param>
        void SetIgnoreTransformForBreast(VRCPhysBone physbone, Transform bone, Transform Target, bool firstchild = true)
        {
            if (bone == null || physbone == null || Target == null)
                return;

            if (Target.childCount == 0)
                return;

            if (firstchild)
            {
                physbone.ignoreTransforms.Add(bone);
            }

            Transform children = bone.GetComponentInChildren<Transform>();
            if (children.childCount == 0)
            {
                return;
            }

            int index = 0;

            foreach (Transform child in children)
            {
                if (Target.childCount <= index)
                    break;

                Transform TargetNextTransform = Target.GetChild(index);
                index++;

                if (TargetNextTransform == null)
                    continue;

                SetIgnoreTransformForBreast(physbone, child, TargetNextTransform, false);
            }
        }

        private static void _DestroyObject(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            if (Application.isPlaying)
                Destroy(gameObject);
            else
                DestroyImmediate(gameObject);

            gameObject = null;
        }

        private static void _DestroyObject(UnityEngine.Object _Object)
        {
            if (_Object == null)
                return;

            if (Application.isPlaying)
                Destroy(_Object);
            else
                DestroyImmediate(_Object);
        }

        void SetAll_Breast_Parent(Transform cloth_breast, Transform avatar_breast, ref VRCPhysBone physbone)
        {
            //中身が無い場合はスキップ
            if (cloth_breast == null || avatar_breast == null)
                return;

            //たぶんあり得ないとこ
            if (cloth_breast == avatar_breast)
                return;

            //胸ボーンを子入れ
            cloth_breast.SetParent(avatar_breast);

            //PhysBoneを取得
            var avatar_physbone = avatar_breast.GetComponent<VRCPhysBone>();

            //Physboneがある場合Ignoreボーンに追加
            if (avatar_physbone != null)
            {
                avatar_physbone.ignoreTransforms.Add(cloth_breast.transform);

                VRCPhysBone cloth_breast_physbone = cloth_breast.GetComponent<VRCPhysBone>();
                if (cloth_breast_physbone)
                    _DestroyObject(cloth_breast_physbone);

                physbone = avatar_physbone;
            }
            else if (physbone != null)
            {
                physbone.ignoreTransforms.Add(cloth_breast.transform);

                VRCPhysBone cloth_breast_physbone = cloth_breast.GetComponent<VRCPhysBone>();
                if (cloth_breast_physbone)
                    _DestroyObject(cloth_breast_physbone);
            }

            //Debug.Log($"cloth_breast = {cloth_breast.name}\navatar_breast = {avatar_breast.name}");

            if (cloth_breast.childCount == 0 || avatar_breast.childCount == 0)
                return;

            var avatar_breast_child = avatar_breast.GetChild(0);
            if (avatar_breast_child == null)
                return;

            for (int j = 0; j < cloth_breast.childCount; j++)
            {
                var cloth_breast_child = cloth_breast.GetChild(j);
                SetAll_Breast_Parent(cloth_breast_child, avatar_breast_child, ref physbone);
            }
        }

        void SetHipsBoneParent(Transform bone)
        {
            if (Hips_L == null || Hips_R == null)
                return;

            if (Hips_L == Hips_R) 
                return;

            //アバターのお尻を取得;
            Transform AvatarHips_L = FindBone_FullMatch(bone, Hips_L_Regex, RegexOptions.None);
            Transform AvatarHips_R = FindBone_FullMatch(bone, Hips_R_Regex, RegexOptions.None);

            if (AvatarHips_L == null || AvatarHips_R == null)
                return;

            //服側のお尻のボーン
            Transform Cloth_Hips_L = Hips_L;
            Transform Cloth_Hips_R = Hips_R;

            //お尻のボーン同士が同名であるかを判定;
            bool isSameName_Hips_L = AvatarHips_L.name == Cloth_Hips_L.name;
            bool isSameName_Hips_R = AvatarHips_R.name == Cloth_Hips_R.name;

            if (!isSameName_Hips_L || !isSameName_Hips_R)
            {
                if (!EditorUtility.DisplayDialog(EditorTitle, "服のお尻のボーンとアバターのお尻のボーンの名前が一致しません。\nそのままボーンをくっつけますか？", "はい", "いいえ"))
                {
                    return;
                }
            }

            VRCPhysBone physbone = null;

            SetAll_Breast_Parent(Cloth_Hips_L, AvatarHips_L, ref physbone);
            SetAll_Breast_Parent(Cloth_Hips_R, AvatarHips_R, ref physbone);
        }

        void SetBreastIgnoreTransform(Transform bone)
        {
            if (Breast_L == null || Breast_R == null)
                return;

            if (Breast_L == Breast_R)
                return;

            Transform AvatarBreast_L = FindBone_LeftRight(bone, Breast_Regex, false, RegexOptions.None);
            Transform AvatarBreast_R = FindBone_LeftRight(bone, Breast_Regex, true, RegexOptions.None);

            if (AvatarBreast_L == null || AvatarBreast_R == null)
                return;

            Transform ClothBreast_L = Breast_L;
            Transform ClothBreast_R = Breast_R;

            bool isSameName_Breast_L = AvatarBreast_L.name == ClothBreast_L.name;
            bool isSameName_Breast_R = AvatarBreast_R.name == ClothBreast_R.name;

            if (!isSameName_Breast_L || !isSameName_Breast_R)
            {
                Transform sub_cloth_breast_L = ClothBreast_L.Find(AvatarBreast_L.name);
                Transform sub_cloth_breast_R = ClothBreast_R.Find(AvatarBreast_R.name);

                if (sub_cloth_breast_L != null)
                    ClothBreast_L = sub_cloth_breast_L;

                if (sub_cloth_breast_R != null)
                    ClothBreast_R = sub_cloth_breast_R;

                if (sub_cloth_breast_L == null || sub_cloth_breast_R == null)
                {
                    if (!EditorUtility.DisplayDialog(EditorTitle, "服の胸のボーンとアバターの胸のボーンの名前が一致しません。\nそのままボーンをくっつけますか？", "はい", "いいえ"))
                    {
                        return;
                    }
                }
            }

            VRCPhysBone physbone = null;

            SetAll_Breast_Parent(ClothBreast_L, AvatarBreast_L, ref physbone);

            physbone = null;
            SetAll_Breast_Parent(ClothBreast_R, AvatarBreast_R, ref physbone);
        }

        void SetBoneListParent(Animator m_animator)
        {
            for (int i = (int)HumanBodyBones.Hips; i < (int)HumanBodyBones.LastBone; i++)
            {
                var bone = (HumanBodyBones)i;
                var baseBone = m_bone[bone];

                Transform p = m_animator.GetBoneTransform(bone);

                if (baseBone == null || p == null)
                {
                    continue;
                }

                //ボーンをアバター側の子に入れる
                baseBone.SetParent(p);

                if (i == (int)HumanBodyBones.Hips)
                    SetHipsBoneParent(p);

                //胸のボーン用
                if (i == (int)HumanBodyBones.Chest || i == (int)HumanBodyBones.UpperChest)
                {
                    SetBreastIgnoreTransform(p);
                }

                //肺のボーン用 MOMO用
                if (i == (int)HumanBodyBones.Chest)
                {
                    var avatar_lung_L = FindBone_FullMatch(p, Lung_L_Regex);
                    var avatar_lung_R = FindBone_FullMatch(p, Lung_R_Regex);

                    var avatar_lung_upper_L = FindBone_FullMatch(avatar_lung_L, Lung_Upper_L_Regex);
                    var avatar_lung_upper_R = FindBone_FullMatch(avatar_lung_R, Lung_Upper_R_Regex);

                    var avatar_lung_lower_L = FindBone_FullMatch(avatar_lung_upper_L, Lung_Lower_L_Regex);
                    var avatar_lung_lower_R = FindBone_FullMatch(avatar_lung_upper_R, Lung_Lower_R_Regex);

                    if (avatar_lung_L != null && Lung_L != null)
                        avatar_lung_L.SetParent(Lung_L);

                    if (avatar_lung_R != null && Lung_R != null)
                        avatar_lung_R.SetParent(Lung_R);

                    if (avatar_lung_upper_L != null && Lung_Upper_L != null)
                        avatar_lung_upper_L.SetParent(Lung_Upper_L);

                    if (avatar_lung_upper_R != null && Lung_Upper_L != null)
                        avatar_lung_upper_R.SetParent(Lung_Upper_R);

                    if (avatar_lung_lower_L != null && Lung_Lower_L != null)
                        avatar_lung_lower_L.SetParent(Lung_Lower_L);

                    if (avatar_lung_lower_R != null && Lung_Lower_R != null)
                        avatar_lung_lower_R.SetParent(Lung_Lower_R);
                }

                if (i == (int)HumanBodyBones.LeftUpperLeg)
                {
                    if (Kuronatu.ThingsL != null)
                    {
                        var avatar_thing_L = FindBone_FullMatch(p, Kuronatu.Thighs_L);
                        if (avatar_thing_L != null)
                        {
                            Kuronatu.ThingsL.SetParent(avatar_thing_L);
                            var avatar_thing_L_001 = FindBone_FullMatch(avatar_thing_L, Kuronatu.Thighs_L_001);
                            if (avatar_thing_L_001 != null && Kuronatu.ThingsL_001 != null)
                            {
                                Kuronatu.ThingsL_001.SetParent(avatar_thing_L_001);
                            }
                        }
                    }
                }

                if (i == (int)HumanBodyBones.RightUpperLeg)
                {
                    if (Kuronatu.ThingsR != null)
                    {
                        var avatar_thing_R = FindBone_FullMatch(p, Kuronatu.Thighs_R);
                        if (avatar_thing_R != null)
                        {
                            Kuronatu.ThingsR.SetParent(avatar_thing_R);
                            var avatar_thing_R_001 = FindBone_FullMatch(avatar_thing_R, Kuronatu.Thighs_R_001);
                            if (avatar_thing_R_001 != null && Kuronatu.ThingsR_001 != null)
                            {
                                Kuronatu.ThingsR_001.SetParent(avatar_thing_R_001);
                            }
                        }
                    }
                }

                //腕の補助ボーン 森羅用
                if (i == (int)HumanBodyBones.LeftUpperArm)
                {
                    Transform subbone = p.Find(Shinra.XC_ArmTwist_L);
                    if (subbone != null && Shinra.XCArmTwistL != null)
                        Shinra.XCArmTwistL.SetParent(subbone);
                }
                if (i == (int)HumanBodyBones.RightUpperArm)
                {
                    Transform subbone = p.Find(Shinra.XC_ArmTwist_R);
                    if (subbone != null && Shinra.XCArmTwistR != null)
                        Shinra.XCArmTwistR.SetParent(subbone);
                }
                if (i == (int)HumanBodyBones.LeftLowerArm)
                {
                    Transform subbone = p.Find(Shinra.XC_WristTwist_L);
                    if (subbone != null && Shinra.XCWristTwistL)
                        Shinra.XCWristTwistL.SetParent(subbone);
                }
                if (i == (int)HumanBodyBones.RightLowerArm)
                {
                    Transform subbone = p.Find(Shinra.XC_WristTwist_R);
                    if (subbone != null && Shinra.XCWristTwistR)
                        Shinra.XCWristTwistR.SetParent(subbone);
                }
            }
        }

        /// <summary>
        /// 引用 : https://qiita.com/Milcia/items/ff7d9e1dffa28004efb7
        /// </summary>
        /// <param name="targetObj"></param>
        /// <returns></returns>
        private static string GetHierarchyPath(GameObject targetObj, bool parentPath = false)
        {
            List<GameObject> objPath = new List<GameObject>();
            objPath.Add(targetObj);
            for (int i = 0; objPath[i].transform.parent != null; i++)
                objPath.Add(objPath[i].transform.parent.gameObject);
            string path = objPath[objPath.Count - (parentPath ? 1 : 2)].gameObject.name; //今回の場合avatar(先頭のオブジェクトが不要)なのでCount - 2にする。必要な場合は - 1 に変更
            for (int i = objPath.Count - (parentPath ? 2 : 3); i >= 0; i--) //こっちもCount - 3にする。必要な場合は - 2にする
                path += "/" + objPath[i].gameObject.name;
            return path;
        }

        private static string GetMD5Hash(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            var targetBytes = Encoding.UTF8.GetBytes(str);

            // MD5ハッシュを計算
            var csp = new MD5CryptoServiceProvider();
            var hashBytes = csp.ComputeHash(targetBytes);

            // バイト配列を文字列に変換
            var hashStr = new StringBuilder();
            foreach (var hashByte in hashBytes)
            {
                hashStr.Append(hashByte.ToString("x2"));
            }

            return hashStr.ToString();
        }

        /// <summary>
        /// オブジェクトに専用コンポーネントをつけます
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        private static void SetKiseteneComponent(GameObject obj, string name)
        {
            if (obj == null)
                return;

            if (string.IsNullOrEmpty(name))
                return;

            var kisetene_component = obj.GetComponent<KiseteneComponent>();
            if (kisetene_component == null)
            {
                kisetene_component = obj.AddComponent<KiseteneComponent>();
            }

            var nowTime = DateTime.Now;
            var timeStr = nowTime.ToString();

            kisetene_component.hash = GetMD5Hash(timeStr);
            kisetene_component.cloth_name = name;
            kisetene_component.hideFlags = HideFlags.HideInInspector;
        }

        private static void SetAllKiseteneComponent(GameObject obj, string name)
        {
            Transform children = obj.GetComponentInChildren<Transform>();

            SetKiseteneComponent(obj, name);

            //子要素がいなければ終了
            if (children.childCount == 0)
            {
                return;
            }

            foreach (Transform ob in children)
            {
                SetAllKiseteneComponent(ob.gameObject, name);
            }
        }



        private static void RemoveCloths(GameObject Avatar, string name = "", string hash = "")
        {
            removeActions.Clear();

            if (Avatar == null)
                return;

            GameObject copyAvatar = Instantiate(Avatar);
            copyAvatar.name += " (Backup)";
            Avatar.SetActive(true);
            copyAvatar.SetActive(false);
            copyAvatar = Avatar;

            RemoveCloth(Avatar, name, hash);

            foreach (var action in removeActions)
            {
                action.Invoke();
            }

            removeActions.Clear();
        }

        private static void RemoveCloth(GameObject cloth, string name = "", string hash = "")
        {
            Transform children = cloth.GetComponentInChildren<Transform>();
            KiseteneComponent kisetenecomponent = cloth.GetComponent<KiseteneComponent>();
            if (kisetenecomponent)
            {
                if (kisetenecomponent.hash == hash)
                {
                    removeActions.Add(() =>
                    {
                        _DestroyObject(cloth);
                    });
                }
            }

            //子要素がいなければ終了
            if (children.childCount == 0)
            {
                return;
            }

            foreach (Transform ob in children)
            {
                RemoveCloth(ob.gameObject, name, hash);
            }
        }

        private static void CheckCloths(GameObject avatar)
        {
            if (avatar == null)
                return;

            Transform children = avatar.GetComponentInChildren<Transform>();
            KiseteneComponent kisetenecomponent = avatar.GetComponent<KiseteneComponent>();
            if (kisetenecomponent)
            {
                if (!_takeoffList.ContainsKey(kisetenecomponent.hash))
                    _takeoffList.Add(kisetenecomponent.hash, kisetenecomponent.cloth_name);
            }

            //子要素がいなければ終了
            if (children.childCount == 0)
            {
                return;
            }

            foreach (Transform ob in children)
            {
                CheckCloths(ob.gameObject);
            }
        }
    }
}

#endif