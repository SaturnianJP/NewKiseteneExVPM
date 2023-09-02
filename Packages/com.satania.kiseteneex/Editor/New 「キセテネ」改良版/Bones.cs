
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.Rendering;

namespace Saturnian_NewKiseteneEx
{
    public partial class NewKiseteneEx
    {
        Vector3 m_armRotate = Vector3.zero;
        Vector3 m_armScale = Vector3.one;
        Vector3 m_hipsPos = Vector3.zero;
        Vector3 m_hipScale = Vector3.one;
        Vector3 m_legRotate = Vector3.zero;
        Vector3 m_legScale = Vector3.one;

        float m_SpineRotate = 0;

        //初期値保持
        Vector3 m_defaultHipsPos;
        Quaternion m_defaultLArmQuat;
        Quaternion m_defaultRArmQuat;
        Quaternion m_defaultSpineQuat;
        Quaternion m_defaultLLegQuat;
        Quaternion m_defaultRLegQuat;

        private void UpdateBones()
        {
            //ボーンリストを初期化
            m_bone.Clear();
            
            //一度nullを代入
            for (int i = 0; i <= (int)HumanBodyBones.LastBone; i++)
                m_bone.Add((HumanBodyBones)i, null);

            //服が入っていない場合はそのまま返す
            if (m_cloth == null)
                return;

            //先にアーマチュアを取得
            m_armature = FindBone(m_cloth.transform, armature_regex, (HumanBodyBones)(-1));

            //アニメーターを取得
            Animator cloth_animator = m_cloth.GetComponent<Animator>();

            //Humanoidの場合はそっちから取得
            if (cloth_animator != null && cloth_animator.isHuman)
            {
                //最後まで回す
                for (int i = (int)HumanBodyBones.Hips; i < (int)HumanBodyBones.LastBone; i++)
                    m_bone[(HumanBodyBones)i] = cloth_animator.GetBoneTransform((HumanBodyBones)i);
            }

            //Hipsが取得出来てない場合は名前から取得
            if (m_bone[HumanBodyBones.Hips] == null)
                m_bone[HumanBodyBones.Hips] = FindBone(m_armature, m_boneRegex[HumanBodyBones.Hips], HumanBodyBones.Hips);

            //Hipsが取得出来てない場合 (靴や頭すげかえの場合)
            if (m_bone[HumanBodyBones.Hips] == null && m_armature != null)
            {
                Transform root = m_armature == null ? m_cloth.transform.root : m_armature;

                var neck = FindBone(root, m_boneRegex[HumanBodyBones.Neck], HumanBodyBones.Neck);
                if (neck != null)
                {
                    m_bone[HumanBodyBones.Neck] = neck;
                    m_bone[HumanBodyBones.Head] = FindBone(neck, m_boneRegex[HumanBodyBones.Head], HumanBodyBones.Head);
                }
                else
                {
                    m_bone[HumanBodyBones.Head] = FindBone(root, m_boneRegex[HumanBodyBones.Head], HumanBodyBones.Head);
                }

                //靴用に左足を取得
                getLeftLeg(root);

                //靴用に右足を取得
                getRightLeg(root);
            }

            //MOMOの場合
            if (m_bone[HumanBodyBones.Hips] == null && m_armature != null)
            {
                m_bone[HumanBodyBones.Hips] = m_armature.Find(ChMd_momo_Hip_Regex);
            }

            //Hips_LとHips_Rを取得
            if (m_bone[HumanBodyBones.Hips] != null)
            {
                //揺れる
                Hips_L = FindBone_FullMatch(m_bone[HumanBodyBones.Hips], Hips_L_Regex);

                //揺れる
                Hips_R = FindBone_FullMatch(m_bone[HumanBodyBones.Hips], Hips_R_Regex);
            }

            //spineを取得
            m_bone[HumanBodyBones.Spine] = FindBone(m_bone[HumanBodyBones.Hips], m_boneRegex[HumanBodyBones.Spine], HumanBodyBones.Spine);

            //chest upperchestがある場合はそちらから取得
            m_bone[HumanBodyBones.Chest] = FindBone( m_bone[HumanBodyBones.Spine], m_boneRegex[HumanBodyBones.Chest], HumanBodyBones.Chest);

            //Upperchestがある場合は取得
            m_bone[HumanBodyBones.UpperChest] = FindBone(m_bone[HumanBodyBones.Chest], m_boneRegex[HumanBodyBones.UpperChest], HumanBodyBones.UpperChest);

            var chest = m_bone[HumanBodyBones.UpperChest] != null ? m_bone[HumanBodyBones.UpperChest] : m_bone[HumanBodyBones.Chest];
            if (chest != null)
            {
                //胸のボーンとってみる
                Breast_R = FindBone_LeftRight(chest, Breast_Regex, true, RegexOptions.None);

                //胸のボーンとってみる
                Breast_L = FindBone_LeftRight(chest, Breast_Regex, false, RegexOptions.None);

                //肺のボーンもとってみる
                Lung_L = FindBone_LeftRight(chest, Lung_Regex, false);
                Lung_R = FindBone_LeftRight(chest, Lung_Regex, true);

                //肺,,,
                if (Lung_L != null)
                {
                    Lung_Upper_L = FindBone_FullMatch(Lung_L, Lung_Upper_Regex);
                    if (Lung_Upper_L != null)
                        Lung_Lower_L = FindBone_FullMatch(Lung_Upper_L, Lung_Lower_Regex);
                }

                if (Lung_R != null)
                {
                    Lung_Upper_R = FindBone_FullMatch(Lung_R, Lung_Upper_Regex);
                    if (Lung_Upper_R != null)
                        Lung_Lower_R = FindBone_FullMatch(Lung_Upper_R, Lung_Lower_Regex);
                }
            }

            m_bone[HumanBodyBones.Neck] = FindBone(chest, m_boneRegex[HumanBodyBones.Neck], HumanBodyBones.Neck, Neck_NegativeRegex);
            m_bone[HumanBodyBones.Head] = FindBone(m_bone[HumanBodyBones.Neck], m_boneRegex[HumanBodyBones.Head], HumanBodyBones.Head);

            //肩 左
            m_bone[HumanBodyBones.LeftShoulder] = FindBone_LeftRight(chest, m_boneRegex[HumanBodyBones.LeftShoulder], false, HumanBodyBones.LeftShoulder);
            m_bone[HumanBodyBones.LeftUpperArm] = FindBone(m_bone[HumanBodyBones.LeftShoulder], m_boneRegex[HumanBodyBones.LeftUpperArm], HumanBodyBones.LeftUpperArm);

            m_bone[HumanBodyBones.LeftLowerArm] = FindBone(m_bone[HumanBodyBones.LeftUpperArm], m_boneRegex[HumanBodyBones.LeftLowerArm], HumanBodyBones.LeftLowerArm);
            m_bone[HumanBodyBones.LeftHand] = FindBone(m_bone[HumanBodyBones.LeftLowerArm], m_boneRegex[HumanBodyBones.LeftHand], HumanBodyBones.LeftHand);

            //右腕
            m_bone[HumanBodyBones.RightShoulder] = FindBone_LeftRight(chest, m_boneRegex[HumanBodyBones.RightShoulder], true, HumanBodyBones.RightShoulder);
            m_bone[HumanBodyBones.RightUpperArm] = FindBone(m_bone[HumanBodyBones.RightShoulder], m_boneRegex[HumanBodyBones.RightUpperArm], HumanBodyBones.RightUpperArm);
            m_bone[HumanBodyBones.RightLowerArm] = FindBone(m_bone[HumanBodyBones.RightUpperArm], m_boneRegex[HumanBodyBones.RightLowerArm], HumanBodyBones.RightLowerArm);
            m_bone[HumanBodyBones.RightHand] = FindBone(m_bone[HumanBodyBones.RightLowerArm], m_boneRegex[HumanBodyBones.RightHand], HumanBodyBones.RightHand);

            //Rushka用指ボーン
            Rushka.GetFingers(m_bone);

            //Grus用指ボーン
            Grus.GetFingers(m_bone);

            //森羅用処理
            if (m_bone[HumanBodyBones.LeftUpperArm] != null)
            {
                Shinra.XCArmTwistL = m_bone[HumanBodyBones.LeftUpperArm].Find(Shinra.XC_ArmTwist_L);
            }

            //森羅用処理
            if (m_bone[HumanBodyBones.RightUpperArm] != null)
            {
                Shinra.XCArmTwistR = m_bone[HumanBodyBones.RightUpperArm].Find(Shinra.XC_ArmTwist_R);
            }

            //森羅用処理
            if (m_bone[HumanBodyBones.LeftLowerArm] != null)
            {
                Shinra.XCWristTwistL = m_bone[HumanBodyBones.LeftLowerArm].Find(Shinra.XC_WristTwist_L);
            }

            //森羅用処理
            if (m_bone[HumanBodyBones.RightLowerArm] != null)
            {
                Shinra.XCWristTwistR = m_bone[HumanBodyBones.RightLowerArm].Find(Shinra.XC_WristTwist_R);
            }

            if (cloth_animator == null || (cloth_animator != null && !cloth_animator.isHuman))
            {
                getLeftIndexies();
                getRightIndexies();
            }

            //しずくちゃん用
            Shizuku.GetFingers(m_bone);

            //左足
            m_bone[HumanBodyBones.LeftUpperLeg] = FindBone_FullMatch(m_bone[HumanBodyBones.Hips], m_boneRegex[HumanBodyBones.LeftUpperLeg]);

            //くろなつ用処理
            if (m_bone[HumanBodyBones.LeftUpperLeg] != null)
            {
                Kuronatu.ThingsL = FindBone_FullMatch(m_bone[HumanBodyBones.LeftUpperLeg], Kuronatu.Thighs_L);
                if (Kuronatu.ThingsL != null)
                    Kuronatu.ThingsL_001 = FindBone_FullMatch(Kuronatu.ThingsL, Kuronatu.Thighs_L_001);
            }

            m_bone[HumanBodyBones.LeftLowerLeg] = FindBone(m_bone[HumanBodyBones.LeftUpperLeg], m_boneRegex[HumanBodyBones.LeftLowerLeg], HumanBodyBones.LeftLowerLeg);
            m_bone[HumanBodyBones.LeftFoot] = FindBone(m_bone[HumanBodyBones.LeftLowerLeg], m_boneRegex[HumanBodyBones.LeftFoot], HumanBodyBones.LeftFoot);
            m_bone[HumanBodyBones.LeftToes] = FindBone(m_bone[HumanBodyBones.LeftFoot], m_boneRegex[HumanBodyBones.LeftToes], HumanBodyBones.LeftToes);

            //右足
            m_bone[HumanBodyBones.RightUpperLeg] = FindBone_FullMatch(m_bone[HumanBodyBones.Hips], m_boneRegex[HumanBodyBones.RightUpperLeg]);

            //くろなつ用処理
            if (m_bone[HumanBodyBones.RightUpperLeg] != null)
            {
                Kuronatu.ThingsR = FindBone_FullMatch(m_bone[HumanBodyBones.RightUpperLeg], Kuronatu.Thighs_R);
                if (Kuronatu.ThingsR != null)
                    Kuronatu.ThingsR_001 = FindBone_FullMatch(Kuronatu.ThingsR, Kuronatu.Thighs_R_001);
            }

            m_bone[HumanBodyBones.RightLowerLeg] = FindBone(m_bone[HumanBodyBones.RightUpperLeg], m_boneRegex[HumanBodyBones.RightLowerLeg], HumanBodyBones.RightLowerLeg);
            m_bone[HumanBodyBones.RightFoot] = FindBone(m_bone[HumanBodyBones.RightLowerLeg], m_boneRegex[HumanBodyBones.RightFoot], HumanBodyBones.RightFoot);
            m_bone[HumanBodyBones.RightToes] = FindBone(m_bone[HumanBodyBones.RightFoot], m_boneRegex[HumanBodyBones.RightToes], HumanBodyBones.RightToes);

            //調整用のrotationを取得
            SetDefaultQuaternion();

            //しずくさん用処理
            Shizuku.GetSubBones(m_bone);
        }
        
        private void getLeftIndexies()
        {
            var index_root = m_bone[HumanBodyBones.LeftHand] != null ? m_bone[HumanBodyBones.LeftHand] : m_armature;
            if (index_root == null)
                index_root = m_cloth.transform.root;

            //左手
            m_bone[HumanBodyBones.LeftIndexProximal] = FindIndex(index_root, Left_Index_Regex, Proximal_Regex, HumanBodyBones.LeftIndexProximal);
            m_bone[HumanBodyBones.LeftIndexIntermediate] = FindIndex(m_bone[HumanBodyBones.LeftIndexProximal], Left_Index_Regex, Intermediate_Regex, HumanBodyBones.LeftIndexIntermediate);
            m_bone[HumanBodyBones.LeftIndexDistal] = FindIndex(m_bone[HumanBodyBones.LeftIndexIntermediate], Left_Index_Regex, Distal_Regex, HumanBodyBones.LeftIndexDistal);

            m_bone[HumanBodyBones.LeftMiddleProximal] = FindIndex(index_root, Left_Middle_Regex, Proximal_Regex, HumanBodyBones.LeftMiddleProximal);
            m_bone[HumanBodyBones.LeftMiddleIntermediate] = FindIndex(m_bone[HumanBodyBones.LeftMiddleProximal], Left_Middle_Regex, Intermediate_Regex, HumanBodyBones.LeftMiddleIntermediate);
            m_bone[HumanBodyBones.LeftMiddleDistal] = FindIndex(m_bone[HumanBodyBones.LeftMiddleIntermediate], Left_Middle_Regex, Distal_Regex, HumanBodyBones.LeftMiddleDistal);

            m_bone[HumanBodyBones.LeftThumbProximal] = FindIndex(index_root, Left_Thumb_Regex, Proximal_Regex, HumanBodyBones.LeftThumbProximal);
            m_bone[HumanBodyBones.LeftThumbIntermediate] = FindIndex(m_bone[HumanBodyBones.LeftThumbProximal], Left_Thumb_Regex, Intermediate_Regex, HumanBodyBones.LeftThumbIntermediate);
            m_bone[HumanBodyBones.LeftThumbDistal] = FindIndex(m_bone[HumanBodyBones.LeftThumbIntermediate], Left_Thumb_Regex, Distal_Regex, HumanBodyBones.LeftThumbDistal);

            m_bone[HumanBodyBones.LeftRingProximal] = FindIndex(index_root, Left_Ring_Regex, Proximal_Regex, HumanBodyBones.LeftRingProximal);

            //ダージェリン用処理
            if (m_bone[HumanBodyBones.LeftRingProximal] == null)
            {
                Transform palm = index_root.Find(Darjelling.Left_Hand_Palm);
                if (palm != null)
                    m_bone[HumanBodyBones.LeftRingProximal] = palm.Find(Darjelling.Left_Hand_Ring_Proximal);
            }

            m_bone[HumanBodyBones.LeftRingIntermediate] = FindIndex(m_bone[HumanBodyBones.LeftRingProximal], Left_Ring_Regex, Intermediate_Regex, HumanBodyBones.LeftRingIntermediate);
            m_bone[HumanBodyBones.LeftRingDistal] = FindIndex(m_bone[HumanBodyBones.LeftRingIntermediate], Left_Ring_Regex, Distal_Regex, HumanBodyBones.LeftRingDistal);

            m_bone[HumanBodyBones.LeftLittleProximal] = FindIndex(index_root, Left_Little_Regex, Proximal_Regex, HumanBodyBones.LeftLittleProximal);

            //ダージェリン用処理
            if (m_bone[HumanBodyBones.LeftLittleProximal] == null)
            {
                Transform palm = index_root.Find(Darjelling.Left_Hand_Palm);
                if (palm != null)
                    m_bone[HumanBodyBones.LeftLittleProximal] = palm.Find(Darjelling.Left_Hand_Pinky_Proximal);
            }

            m_bone[HumanBodyBones.LeftLittleIntermediate] = FindIndex(m_bone[HumanBodyBones.LeftLittleProximal], Left_Little_Regex, Intermediate_Regex, HumanBodyBones.LeftLittleIntermediate);
            m_bone[HumanBodyBones.LeftLittleDistal] = FindIndex(m_bone[HumanBodyBones.LeftLittleIntermediate], Left_Little_Regex, Distal_Regex, HumanBodyBones.LeftLittleDistal);

            
        }

        private void getRightIndexies()
        {
            var index_root = m_bone[HumanBodyBones.RightHand] != null ? m_bone[HumanBodyBones.RightHand] : m_armature;
            if (index_root == null)
                index_root = m_cloth.transform.root;

            m_bone[HumanBodyBones.RightIndexProximal] = FindIndex(index_root, Right_Index_Regex, Proximal_Regex, HumanBodyBones.RightIndexProximal);
            m_bone[HumanBodyBones.RightIndexIntermediate] = FindIndex(m_bone[HumanBodyBones.RightIndexProximal], Right_Index_Regex, Intermediate_Regex, HumanBodyBones.RightIndexIntermediate);
            m_bone[HumanBodyBones.RightIndexDistal] = FindIndex(m_bone[HumanBodyBones.RightIndexIntermediate], Right_Index_Regex, Distal_Regex, HumanBodyBones.RightIndexDistal);

            m_bone[HumanBodyBones.RightThumbProximal] = FindIndex(index_root, Right_Thumb_Regex, Proximal_Regex, HumanBodyBones.RightThumbProximal);
            m_bone[HumanBodyBones.RightThumbIntermediate] = FindIndex(m_bone[HumanBodyBones.RightThumbProximal], Right_Thumb_Regex, Intermediate_Regex, HumanBodyBones.RightThumbIntermediate);
            m_bone[HumanBodyBones.RightThumbDistal] = FindIndex(m_bone[HumanBodyBones.RightThumbIntermediate], Right_Thumb_Regex, Distal_Regex, HumanBodyBones.RightThumbDistal);

            m_bone[HumanBodyBones.RightMiddleProximal] = FindIndex(index_root, Right_Middle_Regex, Proximal_Regex, HumanBodyBones.RightMiddleProximal);
            m_bone[HumanBodyBones.RightMiddleIntermediate] = FindIndex(m_bone[HumanBodyBones.RightMiddleProximal], Right_Middle_Regex, Intermediate_Regex, HumanBodyBones.RightMiddleIntermediate);
            m_bone[HumanBodyBones.RightMiddleDistal] = FindIndex(m_bone[HumanBodyBones.RightMiddleIntermediate], Right_Middle_Regex, Distal_Regex, HumanBodyBones.RightMiddleDistal);

            m_bone[HumanBodyBones.RightRingProximal] = FindIndex(index_root, Right_Ring_Regex, Proximal_Regex, HumanBodyBones.RightRingProximal);
            if (m_bone[HumanBodyBones.RightRingProximal] == null)
            {
                Transform palm = index_root.Find(Darjelling.Right_Hand_Palm);
                if (palm != null)
                    m_bone[HumanBodyBones.RightRingProximal] = palm.Find(Darjelling.Right_Hand_Ring_Proximal);
            }

            m_bone[HumanBodyBones.RightRingIntermediate] = FindIndex(m_bone[HumanBodyBones.RightRingProximal], Right_Ring_Regex, Intermediate_Regex, HumanBodyBones.RightRingIntermediate);
            m_bone[HumanBodyBones.RightRingDistal] = FindIndex(m_bone[HumanBodyBones.RightRingIntermediate], Right_Ring_Regex, Distal_Regex, HumanBodyBones.RightRingDistal);

            m_bone[HumanBodyBones.RightLittleProximal] = FindIndex(index_root, Right_Little_Regex, Proximal_Regex, HumanBodyBones.RightLittleProximal);
            if (m_bone[HumanBodyBones.RightLittleProximal] == null)
            {
                Transform palm = index_root.Find(Darjelling.Right_Hand_Palm);
                if (palm != null)
                    m_bone[HumanBodyBones.RightLittleProximal] = palm.Find(Darjelling.Right_Hand_Pinky_Proximal);
            }

            m_bone[HumanBodyBones.RightLittleIntermediate] = FindIndex(m_bone[HumanBodyBones.RightLittleProximal], Right_Little_Regex, Intermediate_Regex, HumanBodyBones.RightLittleIntermediate);
            m_bone[HumanBodyBones.RightLittleDistal] = FindIndex(m_bone[HumanBodyBones.RightLittleIntermediate], Right_Little_Regex, Distal_Regex, HumanBodyBones.RightLittleDistal);

        }

        private void getLeftLeg(Transform parent)
        {
            Transform left_lowerleg = null, left_foot = null, left_toe = null;

            left_lowerleg = FindBone(parent, m_boneRegex[HumanBodyBones.LeftLowerLeg], HumanBodyBones.LeftLowerLeg);
            if (left_lowerleg != null)
                parent = left_lowerleg;

            left_foot = FindBone(parent, m_boneRegex[HumanBodyBones.LeftFoot], HumanBodyBones.LeftFoot);
            if (left_foot != null)
                parent = left_foot;

            left_toe = FindBone(parent, m_boneRegex[HumanBodyBones.LeftToes], HumanBodyBones.LeftToes);

            m_bone[HumanBodyBones.LeftLowerLeg] = left_lowerleg;
            m_bone[HumanBodyBones.LeftFoot] = left_foot;
            m_bone[HumanBodyBones.LeftToes] = left_toe;
        }

        private void getRightLeg(Transform parent) 
        {
            Transform right_lowerleg = null, right_foot = null, right_toe = null;

            right_lowerleg = FindBone(parent, m_boneRegex[HumanBodyBones.RightLowerLeg], HumanBodyBones.RightLowerLeg);
            if (right_lowerleg != null)
                parent = right_lowerleg;

            right_foot = FindBone(parent, m_boneRegex[HumanBodyBones.RightFoot], HumanBodyBones.RightFoot);
            if (right_foot != null)
                parent = right_foot;

            right_toe = FindBone(parent, m_boneRegex[HumanBodyBones.RightToes], HumanBodyBones.RightToes);

            m_bone[HumanBodyBones.RightLowerLeg] = right_lowerleg;
            m_bone[HumanBodyBones.RightFoot] = right_foot;
            m_bone[HumanBodyBones.LeftToes] = right_toe;
        }

        void SetDefaultQuaternion()
        {
            m_armRotate = Vector3.zero;
            m_hipsPos = Vector3.zero;
            m_legRotate = Vector3.zero;
            m_armScale = Vector3.one;
            m_hipScale = Vector3.one;
            m_legScale = Vector3.one;
            m_SpineRotate = 0;

            //調整用 腕
            if (m_bone[HumanBodyBones.LeftUpperArm] != null)
                m_defaultLArmQuat = m_bone[HumanBodyBones.LeftUpperArm].rotation;

            //調整用 腕
            if (m_bone[HumanBodyBones.RightUpperArm] != null)
                m_defaultRArmQuat = m_bone[HumanBodyBones.RightUpperArm].rotation;

            //調整用 腰
            if (m_bone[HumanBodyBones.Hips] != null)
                m_defaultHipsPos = m_bone[HumanBodyBones.Hips].position;

            //調整用 お腹
            if (m_bone[HumanBodyBones.Spine] != null)
                m_defaultSpineQuat = m_bone[HumanBodyBones.Spine].rotation;

            //調整用 脚
            if (m_bone[HumanBodyBones.LeftUpperLeg] != null)
                m_defaultLLegQuat = m_bone[HumanBodyBones.LeftUpperLeg].rotation;

            //調整用 脚
            if (m_bone[HumanBodyBones.RightUpperLeg] != null)
                m_defaultRLegQuat = m_bone[HumanBodyBones.RightUpperLeg].rotation;
        }

        /// <summary>
        /// ボーン検索用
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="matchPattern"></param>
        /// <returns></returns>
        Transform FindBone(Transform parent, string matchPattern, HumanBodyBones bone)
        {
            if (bone != (HumanBodyBones)(-1) && m_bone[bone] != null)
                return m_bone[bone];

            if (parent == null)
                return null;

            foreach (Transform child in parent)
            {
                if (Regex.IsMatch(child.name, matchPattern, RegexOptions.IgnoreCase))
                    return child;
            }
            return null;
        }


        /// <summary>
        /// ボーン検索用
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="matchPattern"></param>
        /// <returns></returns>
        Transform FindBone(Transform parent, string matchPattern, HumanBodyBones bone, string dontmatchpattern)
        {
            if (bone != (HumanBodyBones)(-1) && m_bone[bone] != null)
                return m_bone[bone];

            if (parent == null)
                return null;

            foreach (Transform child in parent)
            {
                if (Regex.IsMatch(child.name, matchPattern, RegexOptions.IgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(dontmatchpattern))
                        return child;

                    if (!Regex.IsMatch(child.name, dontmatchpattern, RegexOptions.IgnoreCase))
                        return child;
                }
            }
            return null;
        }

        /// <summary>
        /// Regexから完全一致の場合のみ
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="matchPattern"></param>
        /// <returns></returns>
        Transform FindBone_FullMatch(Transform parent, string matchPattern, RegexOptions option = RegexOptions.None)
        {
            if (parent == null)
                return null;

            foreach (Transform child in parent)
            {
                //Debug.LogWarning(child.name);
                if (Regex.IsMatch(child.name, matchPattern, option))
                {
                    return child;
                }
            }
            return null;
        }

        /// <summary>
        /// 肩検索用
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="matchPattern"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        Transform FindBone_LeftRight(Transform parent, string matchPattern, bool right, HumanBodyBones bone, RegexOptions option = RegexOptions.IgnoreCase)
        {
            if (bone != (HumanBodyBones)(-1) && m_bone[bone] != null)
                return m_bone[bone];

            if (parent == null)
                return null;

            Transform hit1 = null;
            Transform hit2 = null;

            foreach (Transform child in parent)
            {
                if (Regex.IsMatch(child.name, matchPattern, option))
                {
                    if (hit1 == null)
                        hit1 = child;
                    else
                        hit2 = child;
                }
            }

            if (hit1 == null || hit2 == null)
                return null;

            if (right)
            {
                if (hit1.position.x > hit2.position.x)
                    return hit1;
                else
                    return hit2;
            }
            else if (!right)
            {
                if (hit1.position.x < hit2.position.x)
                    return hit1;
                else
                    return hit2;
            }

            return null;
        }

        /// <summary>
        /// 左右分かれてる場合の検索用
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="matchPattern"></param>
        /// <param name="right">あとで定義の仕方直す</param>
        /// <param name="option"></param>
        /// <returns></returns>
        Transform FindBone_LeftRight(Transform parent, string matchPattern, bool right, RegexOptions option = RegexOptions.IgnoreCase)
        {
            if (parent == null)
                return null;

            Transform hit1 = null;
            Transform hit2 = null;

            foreach (Transform child in parent)
            {
                if (Regex.IsMatch(child.name, matchPattern, option))
                {
                    if (hit1 == null)
                        hit1 = child;
                    else
                        hit2 = child;
                }
            }

            if (hit1 == null || hit2 == null)
                return null;

            if (right)
            {
                if (hit1.position.x > hit2.position.x)
                    return hit1;
                else
                    return hit2;
            }
            else if (!right)
            {
                if (hit1.position.x < hit2.position.x)
                    return hit1;
                else
                    return hit2;
            }

            return null;
        }

        public static Transform FindIndex(Transform parent, string matchPattern, string word, HumanBodyBones bone)
        {
            if (bone != (HumanBodyBones)(-1) && m_bone[bone] != null)
                return m_bone[bone];

            if (parent == null)
                return null;

            return FindIndex_Loop(parent, matchPattern, word);
        }

        public static Transform FindIndex_Loop(Transform parent, string matchPattern, string word = "")
        {
            if (parent == null)
                return null;

            foreach (Transform childbone in parent)
            {
                if (Regex.IsMatch(childbone.name, matchPattern, RegexOptions.IgnoreCase))
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        if (!Regex.IsMatch(childbone.name, word, RegexOptions.IgnoreCase))
                        {
                            if (childbone.childCount > 0)
                            {
                                Transform temp = FindIndex_Loop(childbone, matchPattern, word);
                                if (temp != null)
                                    return temp;
                            }
                        }
                    }
                    return childbone;
                }
            }
            return null;
        }

        public static void ResetBone()
        {
            Breast_L = null;
            Breast_R = null;

            Lung_L = null;
            Lung_Upper_L = null;
            Lung_Lower_L = null;

            Lung_R = null;
            Lung_Upper_R = null;
            Lung_Lower_R = null;

            Kuronatu.ThingsR = null;
            Kuronatu.ThingsL = null;

            Darjelling.LeftHandPalm = null;
            Darjelling.RightHandPalm = null;

            Shinra.XCWristTwistL = null;
            Shinra.XCWristTwistR = null;

            Shinra.XCArmTwistL = null;
            Shinra.XCArmTwistR = null;
        }

        public void GetBonesFromAvatar()
        {
            if (m_cloth == null)
            {
                MessageBox("服を入れてください！", Localized.ok);
                return;
            }

            if (m_Avatar == null)
            {
                MessageBox("アバターを入れてください！", Localized.ok);
                return;
            }

            if (!m_Avatar.isHuman)
                return;

            var armature = FindBone(m_cloth.transform, armature_regex, (HumanBodyBones)(-1));
            if (armature != null)
            {
                string armature_path = GetHierarchyPath(armature.gameObject);
                if (!string.IsNullOrEmpty(armature_path))
                {
                    var _m_armature = m_cloth.transform.Find(armature_path);
                    if (_m_armature)
                        m_armature = _m_armature;
                }
            }

            for (int i = 0; i < (int)HumanBodyBones.LastBone; i++)
            {
                var bone = m_Avatar.GetBoneTransform((HumanBodyBones)i);

                if (bone == null) 
                    continue;

                string path = GetHierarchyPath(bone.gameObject);
                if (!string.IsNullOrEmpty(path))
                {
                    var _bone = m_cloth.transform.Find(path);
                    if (_bone)
                        m_bone[(HumanBodyBones)i] = _bone;
                }

                var chest = m_bone[HumanBodyBones.UpperChest] != null ? m_bone[HumanBodyBones.UpperChest] : m_bone[HumanBodyBones.Chest];

                if (chest != null)
                {
                    //胸のボーンとってみる
                    Breast_R = FindBone_LeftRight(chest, Breast_Regex, true, RegexOptions.None);

                    //胸のボーンとってみる
                    Breast_L = FindBone_LeftRight(chest, Breast_Regex, false, RegexOptions.None);

                    //肺のボーンもとってみる
                    Lung_L = FindBone_LeftRight(chest, Lung_Regex, false);
                    Lung_R = FindBone_LeftRight(chest, Lung_Regex, true);

                    //肺,,,
                    if (Lung_L != null)
                    {
                        Lung_Upper_L = FindBone_FullMatch(Lung_L, Lung_Upper_Regex);
                        if (Lung_Upper_L != null)
                            Lung_Lower_L = FindBone_FullMatch(Lung_Upper_L, Lung_Lower_Regex);
                    }

                    if (Lung_R != null)
                    {
                        Lung_Upper_R = FindBone_FullMatch(Lung_R, Lung_Upper_Regex);
                        if (Lung_Upper_R != null)
                            Lung_Lower_R = FindBone_FullMatch(Lung_Upper_R, Lung_Lower_Regex);
                    }
                }

                //くろなつ用処理
                if (m_bone[HumanBodyBones.LeftUpperLeg] != null)
                {
                    Kuronatu.ThingsL = FindBone_FullMatch(m_bone[HumanBodyBones.LeftUpperLeg], Kuronatu.Thighs_L);
                    if (Kuronatu.ThingsL != null)
                        Kuronatu.ThingsL_001 = FindBone_FullMatch(Kuronatu.ThingsL, Kuronatu.Thighs_L_001);
                }

                //くろなつ用処理
                if (m_bone[HumanBodyBones.RightUpperLeg] != null)
                {
                    Kuronatu.ThingsR = FindBone_FullMatch(m_bone[HumanBodyBones.RightUpperLeg], Kuronatu.Thighs_R);
                    if (Kuronatu.ThingsR != null)
                        Kuronatu.ThingsR_001 = FindBone_FullMatch(Kuronatu.ThingsR, Kuronatu.Thighs_R_001);
                }
            }
        }
    }
}

