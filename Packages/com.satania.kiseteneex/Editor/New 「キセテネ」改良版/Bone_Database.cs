
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Saturnian_NewKiseteneEx
{
    public static class Kuronatu
    {
        /// <summary>
        /// UpperLeg直下
        /// </summary>
        public static string Thighs_L = "Thighs.L";

        /// <summary>
        /// UpperLeg直下
        /// </summary>
        public static string Thighs_L_001 = "Thighs.L.001";

        /// <summary>
        /// UpperLeg直下
        /// </summary>
        public static string Thighs_R = "Thighs.R";

        /// <summary>
        /// UpperLeg直下
        /// </summary>
        public static string Thighs_R_001 = "Thighs.R.001";

        public static Transform ThingsL = null, ThingsL_001 = null;
        public static Transform ThingsR = null, ThingsR_001 = null;
    }

    public static class Darjelling
    {
        public static string Left_Hand_Palm = "Left_Hand_Palm";
        public static string Right_Hand_Palm = "Right_Hand_Palm";

        public static string Left_Hand_Pinky_Proximal = "Left_Hand_Pinky_Proximal";
        public static string Right_Hand_Pinky_Proximal = "Right_Hand_Pinky_Proximal";

        public static string Left_Hand_Ring_Proximal = "Left_Hand_Ring_Proximal";
        public static string Right_Hand_Ring_Proximal = "Right_Hand_Ring_Proximal";

        public static Transform LeftHandPalm = null;
        public static Transform RightHandPalm = null;

        public static bool isDarjeling;
    }

    public static class Shinra
    {
        public static string XC_ArmTwist_L = "XC_ArmTwist_L";
        public static string XC_ArmTwist_R = "XC_ArmTwist_R";

        public static string XC_WristTwist_L = "XC_WristTwist_L";
        public static string XC_WristTwist_R = "XC_WristTwist_R";

        public static Transform XCArmTwistL = null;
        public static Transform XCArmTwistR = null;

        public static Transform XCWristTwistL = null;
        public static Transform XCWristTwistR = null;
    }

    public static class Shizuku
    {
        public static string bone_lung = "bone_lung";
     
        /// <summary>
        /// bone_root/bone_pelvis/bone_spine01/bone_spine02/bone_lung
        /// </summary>
        public static Transform bonelung = null;

        public static string bone_wrist_twist_L = "bone_wrist_twist_L";
        public static string bone_forearm_twist_L = "bone_forearm_twist_L";
        public static string bone_arm_twist_L = "bone_arm_twist_L";
        public static string bone_shoulder_twist_L = "bone_shoulder_twist_L";
        public static string bone_elbow_L = "bone_elbow_L";
        public static Transform bonewristtwistL;
        public static Transform boneforearmtwistL;
        public static Transform bonearmtwistL;
        public static Transform boneshouldertwistL;
        public static Transform bonewlbowL;

        public static string bone_hip_twist_L = "bone_hip_twist_L";
        public static string bone_upleg_twist_L = "bone_upleg_twist_L";
        public static string bone_knee_L = "bone_knee_L";
        public static Transform bonehiptwistL;
        public static Transform boneuplegtwistL;
        public static Transform bonekneeL;

        public static string bone_wrist_twist_R = "bone_wrist_twist_R";
        public static string bone_forearm_twist_R = "bone_forearm_twist_R";
        public static string bone_arm_twist_R = "bone_arm_twist_R";
        public static string bone_shoulder_twist_R = "bone_shoulder_twist_R";
        public static string bone_elbow_R = "bone_elbow_R";
        public static Transform bonewristtwistR;
        public static Transform boneforearmtwistR;
        public static Transform bonearmtwistR;
        public static Transform boneshouldertwistR;
        public static Transform boneelbowR;

        public static string bone_hip_twist_R = "bone_hip_twist_R";
        public static string bone_upleg_twist_R = "bone_upleg_twist_R";
        public static string bone_knee_R = "bone_knee_R";
        public static Transform bonehiptwistR;
        public static Transform boneuplegtwistR;
        public static Transform bonekneeR;

        public enum Hand
        {
            Thumb = 1,
            Index = 2,
            Middle = 3,
            Ring = 4,
            Little = 5
        }

        public enum Joint
        {
            Proximal = 1,
            Intermediate = 2,
            Distal = 3
        }

        public enum LeftRight
        {
            Left = 0,
            Right = 1
        }

        public static string GetFingerStr(Hand hand, Joint joint, LeftRight leftright)
        {
            int _Hand = (int)hand;
            int _Joint = (int)joint;
            string _LR = leftright == 0 ? "L" : "R";

            return $"bone_finger0{_Hand}_0{_Joint}_{_LR}";
        }

        public static Transform GetFinger(Transform parent, Hand hand, Joint joint, LeftRight leftright)
        {
            if (parent == null)
                return null;

            string str = GetFingerStr(hand, joint, leftright);

            return parent.Find(str);
        }

        public static void GetLeftFingers(Dictionary<HumanBodyBones, Transform> m_bone)
        {
            if (m_bone[HumanBodyBones.LeftThumbProximal] == null)
                m_bone[HumanBodyBones.LeftThumbProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Hand.Thumb, Joint.Proximal, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftIndexProximal] == null)
                m_bone[HumanBodyBones.LeftIndexProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Hand.Index, Joint.Proximal, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftMiddleProximal] == null)
                m_bone[HumanBodyBones.LeftMiddleProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Hand.Middle, Joint.Proximal, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftRingProximal] == null)
                m_bone[HumanBodyBones.LeftRingProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Hand.Ring, Joint.Proximal, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftLittleProximal] == null)
                m_bone[HumanBodyBones.LeftLittleProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Hand.Little, Joint.Proximal, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftThumbIntermediate] == null)
                m_bone[HumanBodyBones.LeftThumbIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftThumbProximal], Hand.Thumb, Joint.Intermediate, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftIndexIntermediate] == null)
                m_bone[HumanBodyBones.LeftIndexIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftIndexProximal], Hand.Index, Joint.Intermediate, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftMiddleIntermediate] == null)
                m_bone[HumanBodyBones.LeftMiddleIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftMiddleProximal], Hand.Middle, Joint.Intermediate, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftRingIntermediate] == null)
                m_bone[HumanBodyBones.LeftRingIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftRingProximal], Hand.Ring, Joint.Intermediate, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftLittleIntermediate] == null)
                m_bone[HumanBodyBones.LeftLittleIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftLittleProximal], Hand.Little, Joint.Intermediate, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftThumbDistal] == null)
                m_bone[HumanBodyBones.LeftThumbDistal] = GetFinger(m_bone[HumanBodyBones.LeftThumbIntermediate], Hand.Thumb, Joint.Distal, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftIndexDistal] == null)
                m_bone[HumanBodyBones.LeftIndexDistal] = GetFinger(m_bone[HumanBodyBones.LeftIndexIntermediate], Hand.Index, Joint.Distal, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftMiddleDistal] == null)
                m_bone[HumanBodyBones.LeftMiddleDistal] = GetFinger(m_bone[HumanBodyBones.LeftMiddleIntermediate], Hand.Middle, Joint.Distal, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftRingDistal] == null)
                m_bone[HumanBodyBones.LeftRingDistal] = GetFinger(m_bone[HumanBodyBones.LeftRingIntermediate], Hand.Ring, Joint.Distal, LeftRight.Left);

            if (m_bone[HumanBodyBones.LeftLittleDistal] == null)
                m_bone[HumanBodyBones.LeftLittleDistal] = GetFinger(m_bone[HumanBodyBones.LeftLittleIntermediate], Hand.Little, Joint.Distal, LeftRight.Left);
        }

        public static void GetRightFingers(Dictionary<HumanBodyBones, Transform> m_bone)
        {
            if (m_bone[HumanBodyBones.RightThumbProximal] == null)
                m_bone[HumanBodyBones.RightThumbProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Hand.Thumb, Joint.Proximal, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightIndexProximal] == null)
                m_bone[HumanBodyBones.RightIndexProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Hand.Index, Joint.Proximal, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightMiddleProximal] == null)
                m_bone[HumanBodyBones.RightMiddleProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Hand.Middle, Joint.Proximal, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightRingProximal] == null)
                m_bone[HumanBodyBones.RightRingProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Hand.Ring, Joint.Proximal, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightLittleProximal] == null)
                m_bone[HumanBodyBones.RightLittleProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Hand.Little, Joint.Proximal, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightThumbIntermediate] == null)
                m_bone[HumanBodyBones.RightThumbIntermediate] = GetFinger(m_bone[HumanBodyBones.RightThumbProximal], Hand.Thumb, Joint.Intermediate, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightIndexIntermediate] == null)
                m_bone[HumanBodyBones.RightIndexIntermediate] = GetFinger(m_bone[HumanBodyBones.RightIndexProximal], Hand.Index, Joint.Intermediate, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightMiddleIntermediate] == null)
                m_bone[HumanBodyBones.RightMiddleIntermediate] = GetFinger(m_bone[HumanBodyBones.RightMiddleProximal], Hand.Middle, Joint.Intermediate, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightRingIntermediate] == null)
                m_bone[HumanBodyBones.RightRingIntermediate] = GetFinger(m_bone[HumanBodyBones.RightRingProximal], Hand.Ring, Joint.Intermediate, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightLittleIntermediate] == null)
                m_bone[HumanBodyBones.RightLittleIntermediate] = GetFinger(m_bone[HumanBodyBones.RightLittleProximal], Hand.Little, Joint.Intermediate, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightThumbDistal] == null)
                m_bone[HumanBodyBones.RightThumbDistal] = GetFinger(m_bone[HumanBodyBones.RightThumbIntermediate], Hand.Thumb, Joint.Distal, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightIndexDistal] == null)
                m_bone[HumanBodyBones.RightIndexDistal] = GetFinger(m_bone[HumanBodyBones.RightIndexIntermediate], Hand.Index, Joint.Distal, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightMiddleDistal] == null)
                m_bone[HumanBodyBones.RightMiddleDistal] = GetFinger(m_bone[HumanBodyBones.RightMiddleIntermediate], Hand.Middle, Joint.Distal, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightRingDistal] == null)
                m_bone[HumanBodyBones.RightRingDistal] = GetFinger(m_bone[HumanBodyBones.RightRingIntermediate], Hand.Ring, Joint.Distal, LeftRight.Right);

            if (m_bone[HumanBodyBones.RightLittleDistal] == null)
                m_bone[HumanBodyBones.RightLittleDistal] = GetFinger(m_bone[HumanBodyBones.RightLittleIntermediate], Hand.Little, Joint.Distal, LeftRight.Right);
        }

        public static void GetFingers(Dictionary<HumanBodyBones, Transform> m_bone)
        {
            GetLeftFingers(m_bone);

            GetRightFingers(m_bone);
        }

        public static void GetSubBones(Dictionary<HumanBodyBones, Transform> m_bone)
        {
            if (m_bone[HumanBodyBones.Hips] != null)
            {
                //しずくさん用
                Shizuku.bonehiptwistL = m_bone[HumanBodyBones.Hips].Find(Shizuku.bone_hip_twist_L);
                Shizuku.bonehiptwistR = m_bone[HumanBodyBones.Hips].Find(Shizuku.bone_hip_twist_R);
            }

            if (m_bone[HumanBodyBones.Chest] != null)
                Shizuku.bonelung = m_bone[HumanBodyBones.Chest].Find(Shizuku.bone_lung);

            if (m_bone[HumanBodyBones.LeftUpperArm] != null)
            {
                Shizuku.bonearmtwistL = m_bone[HumanBodyBones.LeftUpperArm].Find(Shizuku.bone_arm_twist_L);
                Shizuku.boneshouldertwistL = m_bone[HumanBodyBones.LeftUpperArm].Find(Shizuku.bone_shoulder_twist_L);
                Shizuku.bonewlbowL = m_bone[HumanBodyBones.LeftUpperArm].Find(Shizuku.bone_elbow_L);
            }

            if (m_bone[HumanBodyBones.LeftLowerArm] != null)
            {
                Shizuku.bonewristtwistL = m_bone[HumanBodyBones.LeftLowerArm].Find(Shizuku.bone_wrist_twist_L);
                Shizuku.boneforearmtwistL = m_bone[HumanBodyBones.LeftLowerArm].Find(Shizuku.bone_forearm_twist_L);
            }

            if (m_bone[HumanBodyBones.LeftUpperLeg] != null)
            {
                Shizuku.bonekneeL = m_bone[HumanBodyBones.LeftUpperLeg].Find(Shizuku.bone_knee_L);
                Shizuku.boneuplegtwistL = m_bone[HumanBodyBones.LeftUpperLeg].Find(Shizuku.bone_upleg_twist_L);
            }

            if (m_bone[HumanBodyBones.RightUpperArm] != null)
            {
                Shizuku.bonearmtwistR = m_bone[HumanBodyBones.RightUpperArm].Find(Shizuku.bone_arm_twist_R);
                Shizuku.boneshouldertwistR = m_bone[HumanBodyBones.RightUpperArm].Find(Shizuku.bone_shoulder_twist_R);
                Shizuku.boneelbowR = m_bone[HumanBodyBones.RightUpperArm].Find(Shizuku.bone_elbow_R);
            }

            if (m_bone[HumanBodyBones.RightLowerArm] != null)
            {
                Shizuku.bonewristtwistR = m_bone[HumanBodyBones.RightLowerArm].Find(Shizuku.bone_wrist_twist_R);
                Shizuku.boneforearmtwistR = m_bone[HumanBodyBones.RightLowerArm].Find(Shizuku.bone_forearm_twist_R);
            }

            if (m_bone[HumanBodyBones.RightUpperLeg] != null)
            {
                Shizuku.bonekneeR = m_bone[HumanBodyBones.RightUpperLeg].Find(Shizuku.bone_knee_R);
                Shizuku.boneuplegtwistR = m_bone[HumanBodyBones.RightUpperLeg].Find(Shizuku.bone_upleg_twist_R);
            }
        }

        public static void SetSubBones(Animator _avatar, Dictionary<HumanBodyBones, Transform> m_bone)
        {
            if (!_avatar.isHuman)
                return;

            Transform Hips = _avatar.GetBoneTransform(HumanBodyBones.Hips);
            Transform Chest = _avatar.GetBoneTransform(HumanBodyBones.Chest);
            Transform LeftUpperArm = _avatar.GetBoneTransform(HumanBodyBones.LeftUpperArm);
            Transform LeftLowerArm = _avatar.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            Transform LeftUpperLeg = _avatar.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
            Transform RightUpperArm = _avatar.GetBoneTransform(HumanBodyBones.RightUpperArm);
            Transform RightLowerArm = _avatar.GetBoneTransform(HumanBodyBones.RightLowerArm);
            Transform RightUpperLeg = _avatar.GetBoneTransform(HumanBodyBones.RightUpperLeg);

            if (Hips != null)
            {
                Transform avatar_bonehiptwistL = Hips.Find(Shizuku.bone_hip_twist_L);
                Transform avatar_bonehiptwistR = Hips.Find(Shizuku.bone_hip_twist_R);

                if (Shizuku.bonehiptwistL && avatar_bonehiptwistL)
                    Shizuku.bonehiptwistL.SetParent(avatar_bonehiptwistL);

                if (Shizuku.bonehiptwistR && avatar_bonehiptwistR)
                    Shizuku.bonehiptwistR.SetParent(avatar_bonehiptwistR);
            }

            if (Chest != null)
            {
                Transform avatar_bonelung = Chest.Find(Shizuku.bone_lung);

                if (Shizuku.bonelung && avatar_bonelung)
                    Shizuku.bonelung.SetParent(avatar_bonelung);
            }

            if (LeftUpperArm != null)
            {
                Transform avatar_bonearmtwistL = LeftUpperArm.Find(Shizuku.bone_arm_twist_L);
                Transform avatar_boneshouldertwistL = LeftUpperArm.Find(Shizuku.bone_shoulder_twist_L);
                Transform avatar_bonewlbowL = LeftUpperArm.Find(Shizuku.bone_elbow_L);

                if (Shizuku.bonearmtwistL && avatar_bonearmtwistL)
                    Shizuku.bonearmtwistL.SetParent(avatar_bonearmtwistL);

                if (Shizuku.boneshouldertwistL && avatar_boneshouldertwistL)
                    Shizuku.boneshouldertwistL.SetParent(avatar_boneshouldertwistL);

                if (Shizuku.bonewlbowL && avatar_bonewlbowL)
                    Shizuku.bonewlbowL.SetParent(avatar_bonewlbowL);
            }

            if (LeftLowerArm != null)
            {
                Transform avatar_bonewristtwistL = LeftLowerArm.Find(Shizuku.bone_wrist_twist_L);
                Transform avatar_boneforearmtwistL = LeftLowerArm.Find(Shizuku.bone_forearm_twist_L);

                if (Shizuku.bonewristtwistL && avatar_bonewristtwistL)
                    Shizuku.bonewristtwistL.SetParent(avatar_bonewristtwistL);

                if (Shizuku.boneforearmtwistL && avatar_boneforearmtwistL)
                    Shizuku.boneforearmtwistL.SetParent(avatar_boneforearmtwistL);
            }

            if (LeftUpperLeg != null)
            {
                Transform avatar_bonekneeL = LeftUpperLeg.Find(Shizuku.bone_knee_L);
                Transform avatar_boneuplegtwistL = LeftUpperLeg.Find(Shizuku.bone_upleg_twist_L);

                if (bonekneeL && avatar_bonekneeL)
                    bonekneeL.SetParent(avatar_bonekneeL);

                if (boneuplegtwistL && avatar_boneuplegtwistL)
                    boneuplegtwistL.SetParent(avatar_boneuplegtwistL);
            }

            if (RightUpperArm != null)
            {
                Transform avatar_bonearmtwistR = RightUpperArm.Find(Shizuku.bone_arm_twist_R);
                Transform avatar_boneshouldertwistR = RightUpperArm.Find(Shizuku.bone_shoulder_twist_R);
                Transform avatar_bonewlbowR = RightUpperArm.Find(Shizuku.bone_elbow_R);

                if (Shizuku.bonearmtwistR && avatar_bonearmtwistR)
                    Shizuku.bonearmtwistR.SetParent(avatar_bonearmtwistR);

                if (Shizuku.boneshouldertwistR && avatar_boneshouldertwistR)
                    Shizuku.boneshouldertwistR.SetParent(avatar_boneshouldertwistR);

                if (Shizuku.boneelbowR && avatar_bonewlbowR)
                    Shizuku.boneelbowR.SetParent(avatar_bonewlbowR);
            }

            if (RightLowerArm != null)
            {
                Transform avatar_bonewristtwistR = LeftLowerArm.Find(Shizuku.bone_wrist_twist_R);
                Transform avatar_boneforearmtwistR = LeftLowerArm.Find(Shizuku.bone_forearm_twist_R);

                if (Shizuku.bonewristtwistR && avatar_bonewristtwistR)
                    Shizuku.bonewristtwistR.SetParent(avatar_bonewristtwistR);

                if (Shizuku.boneforearmtwistR && avatar_boneforearmtwistR)
                    Shizuku.boneforearmtwistR.SetParent(avatar_boneforearmtwistR);
            }

            if (RightUpperLeg != null)
            {
                Transform avatar_bonekneeR = LeftUpperLeg.Find(Shizuku.bone_knee_R);
                Transform avatar_boneuplegtwistR = LeftUpperLeg.Find(Shizuku.bone_upleg_twist_R);

                if (bonekneeR && avatar_bonekneeR)
                    bonekneeR.SetParent(avatar_bonekneeR);

                if (boneuplegtwistR && avatar_boneuplegtwistR)
                    boneuplegtwistR.SetParent(avatar_boneuplegtwistR);
            }
        }
    }

    public static class Grus
    {
        public enum Joint
        {
            Proximal = 1,
            Intermediate = 2,
            Distal = 3,
        }

        public enum Hand
        {
            Left,
            Right
        }

        public enum Finger
        {
            Index,
            Middle,
            Little,
            Ring,
            Thumb
        }

        public static string[] FingerStr = new string[]
        {
            "Index",
            "Middle",
            "Pinky",
            "Ring",
            "Thumb"
        };

        public static string[] HandStr = new string[]
        {
            "_L",
            "_R"
        };

        public static Transform GetFinger(Transform handbone, Finger finger, Joint joint, Hand hand)
        {
            if (handbone == null)
                return null;

            string str = $"{FingerStr[(int)finger]}{(int)joint}{HandStr[(int)hand]}";
            Transform bone = handbone.Find(str);

            //Debug.Log(bone);
            return bone;
        }

        public static void GetFingers(Dictionary<HumanBodyBones, Transform> m_bone)
        {
            if (m_bone[HumanBodyBones.LeftThumbProximal] == null)
                m_bone[HumanBodyBones.LeftThumbProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Finger.Thumb, Joint.Proximal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftIndexProximal] == null)
                m_bone[HumanBodyBones.LeftIndexProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Finger.Index, Joint.Proximal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftMiddleProximal] == null)
                m_bone[HumanBodyBones.LeftMiddleProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Finger.Middle, Joint.Proximal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftRingProximal] == null)
                m_bone[HumanBodyBones.LeftRingProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Finger.Ring, Joint.Proximal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftLittleProximal] == null)
                m_bone[HumanBodyBones.LeftLittleProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Finger.Little, Joint.Proximal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftThumbIntermediate] == null)
                m_bone[HumanBodyBones.LeftThumbIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftThumbProximal], Finger.Thumb, Joint.Intermediate, Hand.Left);

            if (m_bone[HumanBodyBones.LeftIndexIntermediate] == null)
                m_bone[HumanBodyBones.LeftIndexIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftIndexProximal], Finger.Index, Joint.Intermediate, Hand.Left);

            if (m_bone[HumanBodyBones.LeftMiddleIntermediate] == null)
                m_bone[HumanBodyBones.LeftMiddleIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftMiddleProximal], Finger.Middle, Joint.Intermediate, Hand.Left);

            if (m_bone[HumanBodyBones.LeftRingIntermediate] == null)
                m_bone[HumanBodyBones.LeftRingIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftRingProximal], Finger.Ring, Joint.Intermediate, Hand.Left);

            if (m_bone[HumanBodyBones.LeftLittleIntermediate] == null)
                m_bone[HumanBodyBones.LeftLittleIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftLittleProximal], Finger.Little, Joint.Intermediate, Hand.Left);

            if (m_bone[HumanBodyBones.LeftThumbDistal] == null)
                m_bone[HumanBodyBones.LeftThumbDistal] = GetFinger(m_bone[HumanBodyBones.LeftThumbIntermediate], Finger.Thumb, Joint.Distal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftIndexDistal] == null)
                m_bone[HumanBodyBones.LeftIndexDistal] = GetFinger(m_bone[HumanBodyBones.LeftIndexIntermediate], Finger.Index, Joint.Distal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftMiddleDistal] == null)
                m_bone[HumanBodyBones.LeftMiddleDistal] = GetFinger(m_bone[HumanBodyBones.LeftMiddleIntermediate], Finger.Middle, Joint.Distal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftRingDistal] == null)
                m_bone[HumanBodyBones.LeftRingDistal] = GetFinger(m_bone[HumanBodyBones.LeftRingIntermediate], Finger.Ring, Joint.Distal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftLittleDistal] == null)
                m_bone[HumanBodyBones.LeftLittleDistal] = GetFinger(m_bone[HumanBodyBones.LeftLittleIntermediate], Finger.Little, Joint.Distal, Hand.Left);

            if (m_bone[HumanBodyBones.RightThumbProximal] == null)
                m_bone[HumanBodyBones.RightThumbProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Finger.Thumb, Joint.Proximal, Hand.Right);

            if (m_bone[HumanBodyBones.RightIndexProximal] == null)
                m_bone[HumanBodyBones.RightIndexProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Finger.Index, Joint.Proximal, Hand.Right);

            if (m_bone[HumanBodyBones.RightMiddleProximal] == null)
                m_bone[HumanBodyBones.RightMiddleProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Finger.Middle, Joint.Proximal, Hand.Right);

            if (m_bone[HumanBodyBones.RightRingProximal] == null)
                m_bone[HumanBodyBones.RightRingProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Finger.Ring, Joint.Proximal, Hand.Right);

            if (m_bone[HumanBodyBones.RightLittleProximal] == null)
                m_bone[HumanBodyBones.RightLittleProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Finger.Little, Joint.Proximal, Hand.Right);

            if (m_bone[HumanBodyBones.RightThumbIntermediate] == null)
                m_bone[HumanBodyBones.RightThumbIntermediate] = GetFinger(m_bone[HumanBodyBones.RightThumbProximal], Finger.Thumb, Joint.Intermediate, Hand.Right);

            if (m_bone[HumanBodyBones.RightIndexIntermediate] == null)
                m_bone[HumanBodyBones.RightIndexIntermediate] = GetFinger(m_bone[HumanBodyBones.RightIndexProximal], Finger.Index, Joint.Intermediate, Hand.Right);

            if (m_bone[HumanBodyBones.RightMiddleIntermediate] == null)
                m_bone[HumanBodyBones.RightMiddleIntermediate] = GetFinger(m_bone[HumanBodyBones.RightMiddleProximal], Finger.Middle, Joint.Intermediate, Hand.Right);

            if (m_bone[HumanBodyBones.RightRingIntermediate] == null)
                m_bone[HumanBodyBones.RightRingIntermediate] = GetFinger(m_bone[HumanBodyBones.RightRingProximal], Finger.Ring, Joint.Intermediate, Hand.Right);

            if (m_bone[HumanBodyBones.RightLittleIntermediate] == null)
                m_bone[HumanBodyBones.RightLittleIntermediate] = GetFinger(m_bone[HumanBodyBones.RightLittleProximal], Finger.Little, Joint.Intermediate, Hand.Right);

            if (m_bone[HumanBodyBones.RightThumbDistal] == null)
                m_bone[HumanBodyBones.RightThumbDistal] = GetFinger(m_bone[HumanBodyBones.RightThumbIntermediate], Finger.Thumb, Joint.Distal, Hand.Right);

            if (m_bone[HumanBodyBones.RightIndexDistal] == null)
                m_bone[HumanBodyBones.RightIndexDistal] = GetFinger(m_bone[HumanBodyBones.RightIndexIntermediate], Finger.Index, Joint.Distal, Hand.Right);

            if (m_bone[HumanBodyBones.RightMiddleDistal] == null)
                m_bone[HumanBodyBones.RightMiddleDistal] = GetFinger(m_bone[HumanBodyBones.RightMiddleIntermediate], Finger.Middle, Joint.Distal, Hand.Right);

            if (m_bone[HumanBodyBones.RightRingDistal] == null)
                m_bone[HumanBodyBones.RightRingDistal] = GetFinger(m_bone[HumanBodyBones.RightRingIntermediate], Finger.Ring, Joint.Distal, Hand.Right);

            if (m_bone[HumanBodyBones.RightLittleDistal] == null)
                m_bone[HumanBodyBones.RightLittleDistal] = GetFinger(m_bone[HumanBodyBones.RightLittleIntermediate], Finger.Little, Joint.Distal, Hand.Right);
        }
    }

    public static class Rushka
    {
        public enum Joint
        {
            Proximal = 1,
            Intermediate = 2,
            Distal = 3,
        }

        public enum Hand
        {
            Left,
            Right
        }

        public enum Finger
        {
            Index,
            Middle,
            Little,
            Ring,
            Thumb
        }

        public static string[] FingerStr = new string[]
        {
            "Index",
            "Middle",
            "Little",
            "Ring",
            "Thumb"
        };

        public static string[] HandStr = new string[]
        {
            "_L",
            "_R"
        };

        public static Transform GetFinger(Transform handbone, Finger finger, Joint joint, Hand hand)
        {
            if (handbone == null)
                return null;

            string str = $"{FingerStr[(int)finger]}{(int)joint}{HandStr[(int)hand]}";
            Transform bone = handbone.Find(str);

            //Debug.Log(bone);
            return bone;
        }

        public static void GetFingers(Dictionary<HumanBodyBones, Transform> m_bone)
        {
            if (m_bone[HumanBodyBones.LeftThumbProximal] == null)
                m_bone[HumanBodyBones.LeftThumbProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Finger.Thumb, Joint.Proximal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftIndexProximal] == null)
                m_bone[HumanBodyBones.LeftIndexProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Finger.Index, Joint.Proximal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftMiddleProximal] == null)
                m_bone[HumanBodyBones.LeftMiddleProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Finger.Middle, Joint.Proximal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftRingProximal] == null)
                m_bone[HumanBodyBones.LeftRingProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Finger.Ring, Joint.Proximal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftLittleProximal] == null)
                m_bone[HumanBodyBones.LeftLittleProximal] = GetFinger(m_bone[HumanBodyBones.LeftHand], Finger.Little, Joint.Proximal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftThumbIntermediate] == null)
                m_bone[HumanBodyBones.LeftThumbIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftThumbProximal], Finger.Thumb, Joint.Intermediate, Hand.Left);

            if (m_bone[HumanBodyBones.LeftIndexIntermediate] == null)
                m_bone[HumanBodyBones.LeftIndexIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftIndexProximal], Finger.Index, Joint.Intermediate, Hand.Left);

            if (m_bone[HumanBodyBones.LeftMiddleIntermediate] == null)
                m_bone[HumanBodyBones.LeftMiddleIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftMiddleProximal], Finger.Middle, Joint.Intermediate, Hand.Left);

            if (m_bone[HumanBodyBones.LeftRingIntermediate] == null)
                m_bone[HumanBodyBones.LeftRingIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftRingProximal], Finger.Ring, Joint.Intermediate, Hand.Left);

            if (m_bone[HumanBodyBones.LeftLittleIntermediate] == null)
                m_bone[HumanBodyBones.LeftLittleIntermediate] = GetFinger(m_bone[HumanBodyBones.LeftLittleProximal], Finger.Little, Joint.Intermediate, Hand.Left);

            if (m_bone[HumanBodyBones.LeftThumbDistal] == null)
                m_bone[HumanBodyBones.LeftThumbDistal] = GetFinger(m_bone[HumanBodyBones.LeftThumbIntermediate], Finger.Thumb, Joint.Distal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftIndexDistal] == null)
                m_bone[HumanBodyBones.LeftIndexDistal] = GetFinger(m_bone[HumanBodyBones.LeftIndexIntermediate], Finger.Index, Joint.Distal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftMiddleDistal] == null)
                m_bone[HumanBodyBones.LeftMiddleDistal] = GetFinger(m_bone[HumanBodyBones.LeftMiddleIntermediate], Finger.Middle, Joint.Distal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftRingDistal] == null)
                m_bone[HumanBodyBones.LeftRingDistal] = GetFinger(m_bone[HumanBodyBones.LeftRingIntermediate], Finger.Ring, Joint.Distal, Hand.Left);

            if (m_bone[HumanBodyBones.LeftLittleDistal] == null)
                m_bone[HumanBodyBones.LeftLittleDistal] = GetFinger(m_bone[HumanBodyBones.LeftLittleIntermediate], Finger.Little, Joint.Distal, Hand.Left);

            if (m_bone[HumanBodyBones.RightThumbProximal] == null)
                m_bone[HumanBodyBones.RightThumbProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Finger.Thumb, Joint.Proximal, Hand.Right);

            if (m_bone[HumanBodyBones.RightIndexProximal] == null)
                m_bone[HumanBodyBones.RightIndexProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Finger.Index, Joint.Proximal, Hand.Right);

            if (m_bone[HumanBodyBones.RightMiddleProximal] == null)
                m_bone[HumanBodyBones.RightMiddleProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Finger.Middle, Joint.Proximal, Hand.Right);

            if (m_bone[HumanBodyBones.RightRingProximal] == null)
                m_bone[HumanBodyBones.RightRingProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Finger.Ring, Joint.Proximal, Hand.Right);

            if (m_bone[HumanBodyBones.RightLittleProximal] == null)
                m_bone[HumanBodyBones.RightLittleProximal] = GetFinger(m_bone[HumanBodyBones.RightHand], Finger.Little, Joint.Proximal, Hand.Right);

            if (m_bone[HumanBodyBones.RightThumbIntermediate] == null)
                m_bone[HumanBodyBones.RightThumbIntermediate] = GetFinger(m_bone[HumanBodyBones.RightThumbProximal], Finger.Thumb, Joint.Intermediate, Hand.Right);

            if (m_bone[HumanBodyBones.RightIndexIntermediate] == null)
                m_bone[HumanBodyBones.RightIndexIntermediate] = GetFinger(m_bone[HumanBodyBones.RightIndexProximal], Finger.Index, Joint.Intermediate, Hand.Right);

            if (m_bone[HumanBodyBones.RightMiddleIntermediate] == null)
                m_bone[HumanBodyBones.RightMiddleIntermediate] = GetFinger(m_bone[HumanBodyBones.RightMiddleProximal], Finger.Middle, Joint.Intermediate, Hand.Right);

            if (m_bone[HumanBodyBones.RightRingIntermediate] == null)
                m_bone[HumanBodyBones.RightRingIntermediate] = GetFinger(m_bone[HumanBodyBones.RightRingProximal], Finger.Ring, Joint.Intermediate, Hand.Right);

            if (m_bone[HumanBodyBones.RightLittleIntermediate] == null)
                m_bone[HumanBodyBones.RightLittleIntermediate] = GetFinger(m_bone[HumanBodyBones.RightLittleProximal], Finger.Little, Joint.Intermediate, Hand.Right);

            if (m_bone[HumanBodyBones.RightThumbDistal] == null)
                m_bone[HumanBodyBones.RightThumbDistal] = GetFinger(m_bone[HumanBodyBones.RightThumbIntermediate], Finger.Thumb, Joint.Distal, Hand.Right);

            if (m_bone[HumanBodyBones.RightIndexDistal] == null)
                m_bone[HumanBodyBones.RightIndexDistal] = GetFinger(m_bone[HumanBodyBones.RightIndexIntermediate], Finger.Index, Joint.Distal, Hand.Right);

            if (m_bone[HumanBodyBones.RightMiddleDistal] == null)
                m_bone[HumanBodyBones.RightMiddleDistal] = GetFinger(m_bone[HumanBodyBones.RightMiddleIntermediate], Finger.Middle, Joint.Distal, Hand.Right);

            if (m_bone[HumanBodyBones.RightRingDistal] == null)
                m_bone[HumanBodyBones.RightRingDistal] = GetFinger(m_bone[HumanBodyBones.RightRingIntermediate], Finger.Ring, Joint.Distal, Hand.Right);

            if (m_bone[HumanBodyBones.RightLittleDistal] == null)
                m_bone[HumanBodyBones.RightLittleDistal] = GetFinger(m_bone[HumanBodyBones.RightLittleIntermediate], Finger.Little, Joint.Distal, Hand.Right);
        }
    }

    public partial class NewKiseteneEx
    {
        static string armature_regex = "Armature|armature|root|skelton|Armature.001|Armature.002|Armature.003|Biped|Ash|Ciel Root|bone_root";

        //regex by http://rtilabs.rti-giken.jp/files/2011_11_02/index.php

        
        /// <summary>
        ///Breast_root.L
        ///Breast_root_L
        ///Breast_L
        ///Breast.L
        ///breast_L
        ///breast.L
        ///BustDynamicBone.L.001
        ///boobs.L
        ///boob_w.001_L.002
        ///boob_w.001_L.003
        ///boob_w.001_L.001
        ///bust.001.L
        ///Breast_Root_L
        ///Breast_Root.L
        ///Breast_1_L
        ///Breast_2_L
        ///Butt_L
        /// </summary>
        string Breast_L_Regex = "(?:B(?:reast(?:_(?:[Rr]oot[._]|[12]_)?|\\.)L|u(?:stDynamicBone\\.L\\.001|tt_L))|b(?:oob(?:_w\\.001_L\\.00[123]|s\\.L)|(?:ust\\.001\\.|reast[._])L))";

        /*
Breast_root.R
Breast_root_R
Breast_R
Breast.R
breast_R
BustDynamicBone.R.001
boobs.R
boob_w.001_R.002
boob_w.001_R.003
boob_w.001_R.001
bust.001.R
Breast_Root_R
Breast_Root.R
Breast_1_R
Breast_2_R
Butt_R
*/
        string Breast_R_Regex = "(?:B(?:reast(?:_(?:(?:root[._]|[12]_)R|R(?:oot[._]R)?)|\\.R)|u(?:stDynamicBone\\.R\\.001|tt_R))|b(?:oob(?:_w\\.001_R\\.00[123]|s\\.R)|(?:ust\\.001\\.|reast_)R))";

        string Lung_L_Regex = "hai.L";
        string Lung_Upper_L_Regex = "munemoto.L";
        string Lung_Lower_L_Regex = "mune.L";

        string Lung_R_Regex = "hai.R";
        string Lung_Upper_R_Regex = "munemoto.R";
        string Lung_Lower_R_Regex = "mune.R";


        /*
Breast_root.R
Breast_root_R
Breast_R
Breast.R
breast_R
Breast_R.001
breast_r.001
breast_R.001
BustDynamicBone.R.001
boobs.R
boob_w.001_R.002
boob_w.001_R.003
boob_w.001_R.001
bust.001.R
Breast_Root_R
Breast_Root.R
Breast_1_R
Breast_2_R
Butt_R
breast.R
Right_Bust
Right_Bust2
Right_BustEnd
Right Breast1
Right Breast2
Right Breast2_end
Breast_1.R
Breast_2.R
bone_breast01_R
bone_breast03_R

Breast_root.L
Breast_root_L
Breast_L
Breast.L
breast_L
breast.L
Breast_L.001
breast_l.001
breast_L.001
BustDynamicBone.L.001
boobs.L
boob_w.001_L.002
boob_w.001_L.003
boob_w.001_L.001
bust.001.L
Breast_Root_L
Breast_Root.L
Breast_1_L
Breast_2_L
Butt_L
Left_Bust
Left_Bust2
Left_BustEnd
Left Breast1
Left Breast2
Left Breast2_end
Breast_1.L
Breast_2.L
bone_breast01_L
bone_breast03_L
        */
        string Breast_Regex = "(?:B(?:reast(?:_(?:R(?:oot(?:.[LR]|_[LR])|.001)?|root(?:.[LR]|_[LR])|1(?:.[LR]|_[LR])|2(?:.[LR]|_[LR])|L(?:.001)?)|.[LR])|u(?:stDynamicBone.[LR].001|tt_[LR]))|b(?:o(?:ob(?:_w.001_(?:L.00[123]|R.00[123])|s.[LR])|ne_breast0(?:1_[LR]|3_[LR]))|reast(?:_(?:L(?:.001)?|R(?:.001)?|[lr].001)|.[LR])|ust.001.[LR])|Right(?: Breast(?:2(?:_end)?|1)|_Bust(?:End|2)?|Breast_0[12])|Left(?: Breast(?:2(?:_end)?|1)|_Bust(?:End|2)?|Breast_0[12]))";
/*      
Hips_L
*/
        public static string Hips_L_Regex = "Hips_L|Hips.L";

/*      
Hips_R
*/
        public static string Hips_R_Regex = "Hips_R|Hips.R";

        /*
hai.L
hai.R
        */
        string Lung_Regex = "hai\\.[LR]";

        /*
munemoto.L
munemoto.R
        */  
        string Lung_Upper_Regex = @"munemoto\.[LR]";

        /*
mune.L
mune.R
        */
        string Lung_Lower_Regex = @"mune\.[LR]";

        string Proximal_Regex = "(?:[Pp]ro|_1)";
        string Intermediate_Regex = "(?:[Ii]nter|_2)";
        string Distal_Regex = "(?:[Dd]is|_3)";

        /*
L_IndexFinger
Index
index
Left_Hand_Index_
*/
        string Left_Index_Regex = "(?:L(?:eft_Hand_Index_|_IndexFinger)|[Ii]ndex)";

        /*
L_LittleFinger
Little
little
Left_Hand_Pinky_
        */
        string Left_Little_Regex = "(?:L(?:eft_Hand_Pinky_|_LittleFinger|ittle)|little)";

        /*
L_MiddleFinger
Middle
middle
Left_Hand_Middle_
        */
        string Left_Middle_Regex = "(?:L(?:eft_Hand_Middle_|_MiddleFinger)|[Mm]iddle)";

        /*
L_RingFinger
Ring
ring
Left_Hand_Ring_
        */
        string Left_Ring_Regex = "(?:L(?:eft_Hand_Ring_|_RingFinger)|[Rr]ing)";

        /*
Left_Hand_Thumb_
Thumb
thumb
L_ThumbFinger
        */
        string Left_Thumb_Regex = "(?:L(?:eft_Hand_Thumb_|_ThumbFinger)|[Tt]humb)";

        /*
R_IndexFinger
Index
index
Right_Hand_Index_
        */
        string Right_Index_Regex = "(?:R(?:ight_Hand_Index_|_IndexFinger)|[Ii]ndex)";

        /*
R_LittleFinger
Little
little
Right_Hand_Pinky_
        */
        string Right_Little_Regex = "(?:R(?:ight_Hand_Pinky_|_LittleFinger)|[Ll]ittle)";

        /*
R_MiddleFinger
Middle
middle
Right_Hand_Middle_
        */
        string Right_Middle_Regex = "(?:R(?:ight_Hand_Middle_|_MiddleFinger)|[Mm]iddle)";

        /*
R_RingFinger
Ring
ring
Right_Hand_Ring_
        */
        string Right_Ring_Regex = "(?:R(?:i(?:ght_Hand_Ring_|ng)|_RingFinger)|ring)";

        /*
Right_Hand_Thumb_
Thumb
thumb
R_ThumbFinger
        */
        string Right_Thumb_Regex = "(?:R(?:ight_Hand_Thumb_|_ThumbFinger)|[Tt]humb)";

        string UpperLeg_Regex = "upper";

        string Neck_NegativeRegex = "_necklace";

        Dictionary<HumanBodyBones, string> m_boneRegex = new Dictionary<HumanBodyBones, string>()
        {
            { HumanBodyBones.Hips, "hip|pelvis|bone_pelvis" },
            { HumanBodyBones.Spine, "spine|bone_spine01" },

            /*
upper chest
upper
Upperchest
            */
            { HumanBodyBones.UpperChest, "(?:upper(?:chest)?|Upperchest)" },
            { HumanBodyBones.Chest, "chest|bone_spine02" },

            { HumanBodyBones.LeftShoulder, "_Shoulder|shoulder|Sholder" },
            { HumanBodyBones.LeftUpperArm, "upper|arm|Arm" },
            { HumanBodyBones.LeftLowerArm,  "lower|elbow|ForeArm" },
            { HumanBodyBones.LeftHand, "hand|wrist|Hand" },

            { HumanBodyBones.RightShoulder, "_Shoulder|shoulder|Sholder" },
            { HumanBodyBones.RightUpperArm, "upper|arm|Arm" },
            { HumanBodyBones.RightLowerArm,  "lower|elbow|ForeArm" },
            { HumanBodyBones.RightHand, "hand|wrist|Hand" },

            { HumanBodyBones.Neck, "neck|bone_neck" },
            { HumanBodyBones.Head, "head|bone_head" },
            /*
bone_upleg_L
UpperLeg_L
Left leg
upper_leg_L
Upper_leg.L
upper_leg.l
upper_Leg_L
Upper_Leg.L
upper_Leg.l
Upper Leg.L
upper leg.l
upper leg.L
upper Leg.l
Upper Leg.l
Left_UpLeg
LeftUpperLeg
Left Upper Leg
UpperLeg.L
Upper_Leg_L
upper_leg_l
upper_leg_L
LeftUpLeg
upper_leg.L
            */
            { HumanBodyBones.LeftUpperLeg, "(?:upper(?:_(?:leg(?:.[Ll]|_[Ll])|Leg(?:.l|_L))| (?:leg.[Ll]|Leg.l))|Upper(?:(?:_(?:Leg[._]|leg.)|Leg[._])L| Leg.[Ll])|Left(?:(?:Up(?:per)?|_Up)L| (?:Upper L|l))eg|bone_upleg_L)" },


            /*
lowerleg.L
lowerleg_L
LowerLeg.L
LowerLeg_L
lowerLeg.L
lowerLeg_L
Lowerleg.L
Lowerleg_L
lowerleg.l
lowerleg.l
LowerLeg.l
LowerLeg_l
lowerLeg.l
lowerLeg_l
Lowerleg.l
Lowerleg_l
bone_leg_L
Lower Leg.L
Lower Leg_L
Left knee
lower_leg_L
Lower_leg.L
lower_leg.l
Lower Leg.L
lower leg.l
Left_Leg
LeftLowerLeg
Left Lower Leg
Lower_Leg_L
lower_leg_l
lower_leg_L
LeftLeg
            */
            { HumanBodyBones.LeftLowerLeg, "(?:L(?:ower(?:(?:_(?:leg.|Leg_)| Leg[._])L|Leg(?:.[Ll]|_[Ll])|leg(?:.[Ll]|_[Ll]))|eft(?: (?:Lower Leg|knee)|(?:(?:Lower)?|_)Leg))|lower(?:Leg(?:.[Ll]|_[Ll])|_leg(?:_[Ll]|.l)|leg(?:.[Ll]|_L)| leg.l)|bone_leg_L)" },


            /*
foot.L
foot_L
foot.l
foot_l
Foot.L
Foot_L
Foot.l
Foot_l
bone_foot_L
Foot.L.001
Left ankle
Left_Foot
LeftFoot
Left Foot
             */
            { HumanBodyBones.LeftFoot, "(?:Foot(?:\\.(?:L(?:\\.001)?|l)|_[Ll])|Left(?: (?:ankle|Foot)|_?Foot)|foot(?:\\.[Ll]|_[Ll])|bone_foot_L)" },


            /*
Toe.L
Toe_L
toe_l
toe.l
Toe.l
Toe_l
toe_L
toe.L
bone_toe_L
ToeBase_L
Foot.L.002
Left Toe
Left_Toes
LeftToes
Left Toes
LeftToeBase
            */
            { HumanBodyBones.LeftToes, "(?:Left(?:Toe(?:Base|s)| Toes?|_Toes)|Toe(?:Base_L|.[Ll]|_[Ll])|toe(?:.[Ll]|_[Ll])|Foot.L.002|bone_toe_L)" },


            /*
bone_upleg_R
UpperLeg_R
Right leg
upper_leg_R
Upper_leg.R
upper_leg.r
upper_Leg_R
Upper_Leg.R
upper_Leg.r
Upper Leg.R
upper leg.r
upper leg.R
upper Leg.r
Upper Leg.r
Right_UpLeg
RightUpperLeg
Right Upper Leg
UpperLeg.R
upperleg.r
upperleg.R
Upper_Leg_R
upper_leg_r
upper_leg_R
RightUpLeg
upper_leg.R
            */
            { HumanBodyBones.RightUpperLeg, "(?:upper(?:_(?:leg(?:.[Rr]|_[Rr])|Leg(?:.r|_R))| (?:leg.[Rr]|Leg.r)|leg.[Rr])|Upper(?:(?:_(?:Leg[._]|leg.)|Leg[._])R| Leg.[Rr])|Right(?:(?:Up(?:per)?|_Up)L| (?:Upper L|l))eg|bone_upleg_R)" },


            /*
lowerleg.R
lowerleg_R
LowerLeg.R
LowerLeg_R
lowerLeg.R
lowerLeg_R
Lowerleg.R
Lowerleg_R
lowerleg.r
lowerleg.r
LowerLeg.r
LowerLeg_r
lowerLeg.r
lowerLeg_r
Lowerleg.r
Lowerleg_r
bone_leg_R
Lower Leg.R
Lower Leg_R
Right knee
lower_leg_R
Lower_leg.R
lower_leg.r
Right_Leg
RightLowerLeg
Right Lower Leg
RightLeg
            */
            { HumanBodyBones.RightLowerLeg, "(?:Lower(?:(?: Leg[._]|_leg.)R|Leg(?:.[Rr]|_[Rr])|leg(?:.[Rr]|_[Rr]))|lower(?:Leg(?:.[Rr]|_[Rr])|leg(?:.[Rr]|_R)|_leg(?:.r|_R))|Right(?: (?:Lower Leg|knee)|(?:(?:Lower)?|_)Leg)|bone_leg_R)" },


            /*
foot.R
foot_R
foot.r
foot_r
Foot.R
Foot_R
Foot.r
Foot_r
bone_foot_R
Foot.R.001
Right ankle
Right_Foot
RightFoot
Right Foot
            */
            { HumanBodyBones.RightFoot, "(?:Foot(?:\\.(?:R(?:\\.001)?|r)|_[Rr])|Right(?: (?:ankle|Foot)|_?Foot)|foot(?:\\.[Rr]|_[Rr])|bone_foot_R)" },


            /*
Toe.R
Toe_R
toe_r
toe.r
Toe.r
Toe_r
toe_R
toe.R
bone_toe_R
ToeBase_R
Foot.R.002
Right Toe
Right_Toes
RightToes
Right Toes
RightToeBase
            */
            { HumanBodyBones.RightToes, "(?:Right(?:Toe(?:Base|s)| Toes?|_Toes)|Toe(?:Base_R|.[Rr]|_[Rr])|toe(?:.[Rr]|_[Rr])|Foot.R.002|bone_toe_R)" },
        
        };

        //Hipsが通常のボーン構造と違うため
        string ChMd_momo_Hip_Regex = "Root/Uper_Root/Hip";
    }
}

