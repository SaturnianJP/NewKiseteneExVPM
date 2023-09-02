
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

namespace Saturnian_NewKiseteneEx
{
    public partial class NewKiseteneEx
    {
        private GameObject RemoveCloth_Avatar = null;

        //ボーン詳細設定のスクロール
        private Vector2 _boneSettingsScroll = Vector2.zero;

        //脱がす機能のスクロール
        private Vector2 _takeoffMenuScroll = Vector2.zero;

        /// <summary>
        /// 服のリスト
        /// </summary>
        private static Dictionary<string, string> _takeoffList = new Dictionary<string, string>();

        int selectedTab = 1;

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
        /// <summary>
        /// 質問形メッセージボックス
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="yes"></param>
        /// <param name="no"></param>
        /// <returns></returns>
        public static bool MessageBox(string message, string yes, string no)
        {
            return EditorUtility.DisplayDialog(EditorTitle, message, yes, no);
        }

        private void draw_boneList()
        {
            //表示する数が多いのでスクロールに
            _boneSettingsScroll = EditorGUILayout.BeginScrollView(_boneSettingsScroll);

            var richstyle = new GUIStyle(GUI.skin.button);
            richstyle.richText = true;

            //ボーンを取得するボタンを表示
            if (GUILayout.Button("<color=#ff0000ff><b>[Beta]</b></color>アバターのボーン配列からボーンを取得", richstyle, GUILayout.Width(300))) { GetBonesFromAvatar(); }

            drawSizeLabel("Armature", 15);
            m_armature = EditorGUILayout.ObjectField("Armature", m_armature, typeof(Transform), true) as Transform;
            GUILayout.Space(10);

            drawSizeLabel("Hips", 15);
            EditorGUI.BeginChangeCheck();
            m_bone[HumanBodyBones.Hips] = EditorGUILayout.ObjectField("Hips", m_bone[HumanBodyBones.Hips], typeof(Transform), true) as Transform;
            if (EditorGUI.EndChangeCheck())
            {
                if (m_bone[HumanBodyBones.Hips] != null)
                {
                    //揺れる
                    Hips_R = FindBone_FullMatch(m_bone[HumanBodyBones.Hips], Hips_R_Regex);

                    //揺れる
                    Hips_L = FindBone_FullMatch(m_bone[HumanBodyBones.Hips], Hips_L_Regex);
                }
            }

            Hips_L = EditorGUILayout.ObjectField("Hips Left", Hips_L, typeof(Transform), true) as Transform;
            Hips_R = EditorGUILayout.ObjectField("Hips Right", Hips_R, typeof(Transform), true) as Transform;

            GUILayout.Space(5);

            //上半身 枠
            drawSizeLabel(Localized.upperbody, 15);

            m_bone[HumanBodyBones.Spine] = EditorGUILayout.ObjectField("Spine", m_bone[HumanBodyBones.Spine], typeof(Transform), true) as Transform;

            EditorGUI.BeginChangeCheck();
            m_bone[HumanBodyBones.Chest] = EditorGUILayout.ObjectField("Chest", m_bone[HumanBodyBones.Chest], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.UpperChest] = EditorGUILayout.ObjectField("UpperChest", m_bone[HumanBodyBones.UpperChest], typeof(Transform), true) as Transform;
            if (EditorGUI.EndChangeCheck())
            {
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
            }

            m_bone[HumanBodyBones.Neck] = EditorGUILayout.ObjectField("Neck", m_bone[HumanBodyBones.Neck], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.Head] = EditorGUILayout.ObjectField("Head", m_bone[HumanBodyBones.Head], typeof(Transform), true) as Transform;
            GUILayout.Space(5);

            drawSizeLabel("Breast", 15);
            Breast_L = EditorGUILayout.ObjectField("Breast Left", Breast_L, typeof(Transform), true) as Transform;
            Breast_R = EditorGUILayout.ObjectField("Breast Right", Breast_R, typeof(Transform), true) as Transform;

            GUILayout.Space(5);
            drawSizeLabel("Lung", 15);
            Lung_L = EditorGUILayout.ObjectField("Lung Left", Lung_L, typeof(Transform), true) as Transform;
            Lung_R = EditorGUILayout.ObjectField("Lung Right", Lung_R, typeof(Transform), true) as Transform;
            Lung_Upper_L = EditorGUILayout.ObjectField("Lung Upper Left", Lung_Upper_L, typeof(Transform), true) as Transform;
            Lung_Upper_R = EditorGUILayout.ObjectField("Lung Upper Right", Lung_Upper_R, typeof(Transform), true) as Transform;
            Lung_Lower_L = EditorGUILayout.ObjectField("Lung Lower Left", Lung_Lower_L, typeof(Transform), true) as Transform;
            Lung_Lower_R = EditorGUILayout.ObjectField("Lung Lower Right", Lung_Lower_R, typeof(Transform), true) as Transform;
            GUILayout.Space(10);

            //左手 枠
            drawSizeLabel(Localized.leftarm, 15);
            m_bone[HumanBodyBones.LeftShoulder] = EditorGUILayout.ObjectField("LeftShoulder", m_bone[HumanBodyBones.LeftShoulder], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftUpperArm] = EditorGUILayout.ObjectField("LeftUpperArm", m_bone[HumanBodyBones.LeftUpperArm], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftLowerArm] = EditorGUILayout.ObjectField("LeftLowerArm", m_bone[HumanBodyBones.LeftLowerArm], typeof(Transform), true) as Transform;
            if (_BeginChangeCheck(() =>
            {
                m_bone[HumanBodyBones.LeftHand] = EditorGUILayout.ObjectField("LeftHand", m_bone[HumanBodyBones.LeftHand], typeof(Transform), true) as Transform;
            }))
            {
                if (m_bone[HumanBodyBones.LeftHand] != null)
                {
                    getLeftIndexies();
                    Shizuku.GetLeftFingers(m_bone);
                }
            }

            GUILayout.Space(5);
            
            m_bone[HumanBodyBones.LeftIndexProximal] = EditorGUILayout.ObjectField("LeftIndexProximal", m_bone[HumanBodyBones.LeftIndexProximal], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftIndexIntermediate] = EditorGUILayout.ObjectField("LeftIndexIntermediate", m_bone[HumanBodyBones.LeftIndexIntermediate], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftIndexDistal] = EditorGUILayout.ObjectField("LeftIndexDistal", m_bone[HumanBodyBones.LeftIndexDistal], typeof(Transform), true) as Transform;

            m_bone[HumanBodyBones.LeftLittleProximal] = EditorGUILayout.ObjectField("LeftLittleProximal", m_bone[HumanBodyBones.LeftLittleProximal], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftLittleIntermediate] = EditorGUILayout.ObjectField("LeftLittleIntermediate", m_bone[HumanBodyBones.LeftLittleIntermediate], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftLittleDistal] = EditorGUILayout.ObjectField("LeftLittleDistal", m_bone[HumanBodyBones.LeftLittleDistal], typeof(Transform), true) as Transform;

            m_bone[HumanBodyBones.LeftMiddleProximal] = EditorGUILayout.ObjectField("LeftMiddleProximal", m_bone[HumanBodyBones.LeftMiddleProximal], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftMiddleIntermediate] = EditorGUILayout.ObjectField("LeftMiddleIntermediate", m_bone[HumanBodyBones.LeftMiddleIntermediate], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftMiddleDistal] = EditorGUILayout.ObjectField("LeftMiddleDistal", m_bone[HumanBodyBones.LeftMiddleDistal], typeof(Transform), true) as Transform;

            m_bone[HumanBodyBones.LeftRingProximal] = EditorGUILayout.ObjectField("LeftRingProximal", m_bone[HumanBodyBones.LeftRingProximal], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftRingIntermediate] = EditorGUILayout.ObjectField("LeftRingIntermediate", m_bone[HumanBodyBones.LeftRingIntermediate], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftRingDistal] = EditorGUILayout.ObjectField("LeftRingDistal", m_bone[HumanBodyBones.LeftRingDistal], typeof(Transform), true) as Transform;

            m_bone[HumanBodyBones.LeftThumbProximal] = EditorGUILayout.ObjectField("LeftThumbProximal", m_bone[HumanBodyBones.LeftThumbProximal], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftThumbIntermediate] = EditorGUILayout.ObjectField("LeftThumbIntermediate", m_bone[HumanBodyBones.LeftThumbIntermediate], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftThumbDistal] = EditorGUILayout.ObjectField("LeftThumbDistal", m_bone[HumanBodyBones.LeftThumbDistal], typeof(Transform), true) as Transform;

            GUILayout.Space(10);

            //右手 枠
            drawSizeLabel(Localized.rightarm, 15);
            m_bone[HumanBodyBones.RightShoulder] = EditorGUILayout.ObjectField("RightShoulder", m_bone[HumanBodyBones.RightShoulder], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightUpperArm] = EditorGUILayout.ObjectField("RightUpperArm", m_bone[HumanBodyBones.RightUpperArm], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightLowerArm] = EditorGUILayout.ObjectField("RightLowerArm", m_bone[HumanBodyBones.RightLowerArm], typeof(Transform), true) as Transform;
            if (_BeginChangeCheck(() =>
            {
                m_bone[HumanBodyBones.RightHand] = EditorGUILayout.ObjectField("RightHand", m_bone[HumanBodyBones.RightHand], typeof(Transform), true) as Transform;
            }))
            {
                if (m_bone[HumanBodyBones.RightHand] != null)
                {
                    getRightIndexies();
                    Shizuku.GetRightFingers(m_bone);
                }
            }
            GUILayout.Space(5);

            m_bone[HumanBodyBones.RightIndexProximal] = EditorGUILayout.ObjectField("RightIndexProximal", m_bone[HumanBodyBones.RightIndexProximal], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightIndexIntermediate] = EditorGUILayout.ObjectField("RightIndexIntermediate", m_bone[HumanBodyBones.RightIndexIntermediate], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightIndexDistal] = EditorGUILayout.ObjectField("RightIndexDistal", m_bone[HumanBodyBones.RightIndexDistal], typeof(Transform), true) as Transform;

            m_bone[HumanBodyBones.RightLittleProximal] = EditorGUILayout.ObjectField("RightLittleProximal", m_bone[HumanBodyBones.RightLittleProximal], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightLittleIntermediate] = EditorGUILayout.ObjectField("RightLittleIntermediate", m_bone[HumanBodyBones.RightLittleIntermediate], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightLittleDistal] = EditorGUILayout.ObjectField("RightLittleDistal", m_bone[HumanBodyBones.RightLittleDistal], typeof(Transform), true) as Transform;

            m_bone[HumanBodyBones.RightMiddleProximal] = EditorGUILayout.ObjectField("RightMiddleProximal", m_bone[HumanBodyBones.RightMiddleProximal], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightMiddleIntermediate] = EditorGUILayout.ObjectField("RightMiddleIntermediate", m_bone[HumanBodyBones.RightMiddleIntermediate], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightMiddleDistal] = EditorGUILayout.ObjectField("RightMiddleDistal", m_bone[HumanBodyBones.RightMiddleDistal], typeof(Transform), true) as Transform;

            m_bone[HumanBodyBones.RightRingProximal] = EditorGUILayout.ObjectField("RightRingProximal", m_bone[HumanBodyBones.RightRingProximal], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightRingIntermediate] = EditorGUILayout.ObjectField("RightRingIntermediate", m_bone[HumanBodyBones.RightRingIntermediate], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightRingDistal] = EditorGUILayout.ObjectField("RightRingDistal", m_bone[HumanBodyBones.RightRingDistal], typeof(Transform), true) as Transform;

            m_bone[HumanBodyBones.RightThumbProximal] = EditorGUILayout.ObjectField("RightThumbProximal", m_bone[HumanBodyBones.RightThumbProximal], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightThumbIntermediate] = EditorGUILayout.ObjectField("RightThumbIntermediate", m_bone[HumanBodyBones.RightThumbIntermediate], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightThumbDistal] = EditorGUILayout.ObjectField("RightThumbDistal", m_bone[HumanBodyBones.RightThumbDistal], typeof(Transform), true) as Transform;

            GUILayout.Space(10);

            //左足 枠
            drawSizeLabel(Localized.leftleg, 15);
            m_bone[HumanBodyBones.LeftUpperLeg] = EditorGUILayout.ObjectField("LeftUpperLeg", m_bone[HumanBodyBones.LeftUpperLeg], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftLowerLeg] = EditorGUILayout.ObjectField("LeftLowerLeg", m_bone[HumanBodyBones.LeftLowerLeg], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftFoot] = EditorGUILayout.ObjectField("LeftFoot", m_bone[HumanBodyBones.LeftFoot], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.LeftToes] = EditorGUILayout.ObjectField("LeftToes", m_bone[HumanBodyBones.LeftToes], typeof(Transform), true) as Transform;

            GUILayout.Space(10);

            //右足 枠
            drawSizeLabel(Localized.rightleg, 15);
            m_bone[HumanBodyBones.RightUpperLeg] = EditorGUILayout.ObjectField("RightUpperLeg", m_bone[HumanBodyBones.RightUpperLeg], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightLowerLeg] = EditorGUILayout.ObjectField("RightLowerLeg", m_bone[HumanBodyBones.RightLowerLeg], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightFoot] = EditorGUILayout.ObjectField("RightFoot", m_bone[HumanBodyBones.RightFoot], typeof(Transform), true) as Transform;
            m_bone[HumanBodyBones.RightToes] = EditorGUILayout.ObjectField("RightToes", m_bone[HumanBodyBones.RightToes], typeof(Transform), true) as Transform;

            GUILayout.Space(5);
            drawSizeLabel("[Kuronatu] Things", 15);
            Kuronatu.ThingsL = EditorGUILayout.ObjectField("Things.L", Kuronatu.ThingsL, typeof(Transform), true) as Transform;
            Kuronatu.ThingsL_001 = EditorGUILayout.ObjectField("Things.L.001", Kuronatu.ThingsL_001, typeof(Transform), true) as Transform;

            Kuronatu.ThingsR = EditorGUILayout.ObjectField("Things.R", Kuronatu.ThingsR, typeof(Transform), true) as Transform;
            Kuronatu.ThingsR_001 = EditorGUILayout.ObjectField("Things.R.001", Kuronatu.ThingsR_001, typeof(Transform), true) as Transform;

            GUILayout.Space(5);

            drawSizeLabel("[Shinra] 補助ボーン", 15);
            Shinra.XCWristTwistL = EditorGUILayout.ObjectField("XC_WristTwist_L", Shinra.XCWristTwistL, typeof(Transform), true) as Transform;
            Shinra.XCWristTwistR = EditorGUILayout.ObjectField("XC_WristTwist_R", Shinra.XCWristTwistR, typeof(Transform), true) as Transform;

            Shinra.XCArmTwistL = EditorGUILayout.ObjectField("XC_ArmTwist_L", Shinra.XCArmTwistL, typeof(Transform), true) as Transform;
            Shinra.XCArmTwistR = EditorGUILayout.ObjectField("XC_ArmTwist_R", Shinra.XCArmTwistR, typeof(Transform), true) as Transform;

            GUILayout.Space(5);
            drawSizeLabel("[Shizuku] 補助ボーン", 15);

            Shizuku.bonelung = EditorGUILayout.ObjectField(Shizuku.bone_lung, Shizuku.bonelung, typeof(Transform), true) as Transform;

            GUILayout.Space(2.5f);

            Shizuku.bonehiptwistL = EditorGUILayout.ObjectField(Shizuku.bone_hip_twist_L, Shizuku.bonehiptwistL, typeof(Transform), true) as Transform;
            Shizuku.bonehiptwistR = EditorGUILayout.ObjectField(Shizuku.bone_hip_twist_R, Shizuku.bonehiptwistR, typeof(Transform), true) as Transform;

            Shizuku.boneuplegtwistL = EditorGUILayout.ObjectField(Shizuku.bone_upleg_twist_L, Shizuku.boneuplegtwistL, typeof(Transform), true) as Transform;
            Shizuku.boneuplegtwistR = EditorGUILayout.ObjectField(Shizuku.bone_upleg_twist_R, Shizuku.boneuplegtwistR, typeof(Transform), true) as Transform;

            Shizuku.bonekneeL = EditorGUILayout.ObjectField(Shizuku.bone_knee_L, Shizuku.bonekneeL, typeof(Transform), true) as Transform;
            Shizuku.bonekneeR = EditorGUILayout.ObjectField(Shizuku.bone_knee_R, Shizuku.bonekneeR, typeof(Transform), true) as Transform;

            GUILayout.Space(2.5f);

            Shizuku.bonewristtwistL = EditorGUILayout.ObjectField(Shizuku.bone_wrist_twist_L, Shizuku.bonewristtwistL, typeof(Transform), true) as Transform;
            Shizuku.bonewristtwistR = EditorGUILayout.ObjectField(Shizuku.bone_wrist_twist_R, Shizuku.bonewristtwistR, typeof(Transform), true) as Transform;

            Shizuku.boneforearmtwistL = EditorGUILayout.ObjectField(Shizuku.bone_forearm_twist_L, Shizuku.boneforearmtwistL, typeof(Transform), true) as Transform;
            Shizuku.boneforearmtwistR = EditorGUILayout.ObjectField(Shizuku.bone_forearm_twist_R, Shizuku.boneforearmtwistR, typeof(Transform), true) as Transform;

            Shizuku.bonearmtwistL = EditorGUILayout.ObjectField(Shizuku.bone_arm_twist_L, Shizuku.bonearmtwistL, typeof(Transform), true) as Transform;
            Shizuku.bonearmtwistR = EditorGUILayout.ObjectField(Shizuku.bone_arm_twist_R, Shizuku.bonearmtwistR, typeof(Transform), true) as Transform;

            Shizuku.boneshouldertwistL = EditorGUILayout.ObjectField(Shizuku.bone_shoulder_twist_L, Shizuku.boneshouldertwistL, typeof(Transform), true) as Transform;
            Shizuku.boneshouldertwistR = EditorGUILayout.ObjectField(Shizuku.bone_shoulder_twist_R, Shizuku.boneshouldertwistR, typeof(Transform), true) as Transform;

            Shizuku.bonewlbowL = EditorGUILayout.ObjectField(Shizuku.bone_elbow_L, Shizuku.bonewlbowL, typeof(Transform), true) as Transform;
            Shizuku.boneelbowR = EditorGUILayout.ObjectField(Shizuku.bone_elbow_R, Shizuku.boneelbowR, typeof(Transform), true) as Transform;

            //Darjelling
            EditorGUILayout.EndScrollView();
            EditorGUILayout.HelpBox(Localized.bonesetting_warinig, MessageType.Warning, true);
        }

        private void draw_tabs()
        {
            selectedTab = GUILayout.Toolbar(selectedTab, new string[] { Localized.bonesetting, Localized.fullbody_ui, Localized.upperbody_ui, Localized.bottombody_ui, Localized.test_ui, "[Beta]服を脱がす" }, EditorStyles.toolbarButton);

            switch (selectedTab)
            {
                case 0: draw_boneList(); break;
                case 1: draw_FUllBodyUI(); break;
                case 2: draw_TopBodyUI(); break;
                case 3: draw_BottomBodyUI(); break;
                case 4: draw_SettingUI(); break;
                case 5: draw_RemoveClothUI(); break;
            }
        }

        private void draw_putbutton()
        {
            //アバターを入れた場合、シーン上になければ複製
            if (_BeginChangeCheck(() =>
            {
                m_Avatar = EditorGUILayout.ObjectField(Localized.avatar, m_Avatar, typeof(Animator), true) as Animator;
            }))
            {
                if (m_Avatar != null)
                {
                    if (!m_Avatar.gameObject.IsExists())
                    {
                        Animator copy = Instantiate(m_Avatar);
                        copy.name = m_Avatar.name;
                        m_Avatar = copy;

                        MessageBox(Localized.avatarcopytoscene, Localized.ok);
                    }
                }
            }

            if (GUILayout.Button(Localized.put_cloth))
            {
                puton();
            }
        }

        void drawButtonUI(ref float setParam, float paramDefault, float paramRatio = 1.0f)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(Localized.reset))
                setParam = paramDefault;

            if (GUILayout.Button("--", EditorStyles.miniButtonLeft, GUILayout.Height(20), GUILayout.Width(50)))
                setParam -= 0.01f * paramRatio;

            if (GUILayout.Button("-", EditorStyles.miniButtonMid, GUILayout.Height(20), GUILayout.Width(50)))
                setParam -= 0.001f * paramRatio;

            if (GUILayout.Button("+", EditorStyles.miniButtonMid, GUILayout.Height(20), GUILayout.Width(50)))
                setParam += 0.001f * paramRatio;

            if (GUILayout.Button("++", EditorStyles.miniButtonRight, GUILayout.Height(20), GUILayout.Width(50)))
                setParam += 0.01f * paramRatio;

            GUILayout.EndHorizontal();
        }

        void draw_FUllBodyUI()
        {
            if (_BeginChangeCheck(() =>
            {
                //位置調整 高さ
                drawSizeLabel(Localized.updown, 13);
                drawButtonUI(ref m_hipsPos.y, 0.0f);
                m_hipsPos.y = EditorGUILayout.Slider(m_hipsPos.y, -1, 1);

                GUILayout.Space(5);

                //位置調整 奥行き
                drawSizeLabel(Localized.frontback, 13);
                drawButtonUI(ref m_hipsPos.z, 0.0f);
                m_hipsPos.z = EditorGUILayout.Slider(m_hipsPos.z, -1, 1);

                GUILayout.Space(5);
            }))
            {
                var hips = m_bone[HumanBodyBones.Hips];
                hips.position = m_defaultHipsPos + m_hipsPos;
            }

            if (_BeginChangeCheck(() =>
            {
                //スケール変更用
                drawSizeLabel(Localized.zoominout, 13);
                drawButtonUI(ref m_hipScale.x, 1.0f);
                m_hipScale.x = EditorGUILayout.Slider(m_hipScale.x, 0.5f, 1.5f);

                GUILayout.Space(5);
            }))
            {
                m_hipScale.y = m_hipScale.z = m_hipScale.x;
                var hips = m_bone[HumanBodyBones.Hips];
                hips.localScale = m_hipScale;
            }

            if (_BeginChangeCheck(() =>
            {
                //お辞儀
                drawSizeLabel(Localized.ozigi, 13);

                drawButtonUI(ref m_SpineRotate, 0.0f, 10);
                m_SpineRotate = EditorGUILayout.Slider(m_SpineRotate, -20, 20);
            }))
            {
                var spine = m_bone[HumanBodyBones.Spine];
                if (spine != null)
                {
                    spine.rotation = m_defaultSpineQuat;
                    spine.Rotate(spine.right, m_SpineRotate);
                }
            }
        }

        void draw_TopBodyUI()
        {
            if (_BeginChangeCheck(() =>
            {
                drawSizeLabel(Localized.updown, 13);

                drawButtonUI(ref m_armRotate.z, 0.0f, 10);
                m_armRotate.z = EditorGUILayout.Slider(m_armRotate.z, -50, 50);

                GUILayout.Space(5);

                drawSizeLabel(Localized.frontback, 13);

                drawButtonUI(ref m_armRotate.y, 0.0f, 10);
                m_armRotate.y = EditorGUILayout.Slider(m_armRotate.y, -15, 15);
                GUILayout.Space(5);
            }))
            {
                var left = m_bone[HumanBodyBones.LeftUpperArm];
                if (left != null)
                {
                    //0で0に戻りたいので、回す前にいったん初期値を入れる
                    left.rotation = m_defaultLArmQuat;

                    left.Rotate(new Vector3(0, 0, 1), m_armRotate.z * -1, Space.World);
                    left.Rotate(new Vector3(0, 1, 0), m_armRotate.y, Space.World);
                }

                var right = m_bone[HumanBodyBones.RightUpperArm];
                if (right != null)
                {
                    //0で0に戻りたいので、回す前にいったん初期値を入れる
                    right.rotation = m_defaultRArmQuat;

                    right.Rotate(new Vector3(0, 0, 1), m_armRotate.z, Space.World);
                    right.Rotate(new Vector3(0, 1, 0), m_armRotate.y * -1, Space.World);
                }
            }

            if (_BeginChangeCheck(() =>
            {
                drawSizeLabel(Localized.scale_y, 13);

                drawButtonUI(ref m_armScale.y, 1.0f);
                m_armScale.y = EditorGUILayout.Slider(m_armScale.y, 0.5f, 1.5f);

                GUILayout.Space(5);
                drawSizeLabel(Localized.scale_x, 13);

                drawButtonUI(ref m_armScale.x, 1.0f);
                m_armScale.x = EditorGUILayout.Slider(m_armScale.x, 0.5f, 1.5f);
            }))
            {
                var left = m_bone[HumanBodyBones.LeftUpperArm];
                if (left != null)
                {
                    if (Mathf.Abs(left.forward.y) > Mathf.Abs(left.forward.z))
                    {
                        m_armScale.z = m_armScale.x;
                        left.localScale = m_armScale;
                    }
                    else
                    {
                        //軸が違うのでxyを入れ替える
                        if (left.forward.z > 0)
                        {
                            Vector3 tmpScale = new Vector3(m_armScale.y, m_armScale.x, m_armScale.x);
                            left.localScale = tmpScale;
                        }
                        else
                        {
                            Vector3 tmpScale = new Vector3(m_armScale.x, m_armScale.y, m_armScale.x);
                            left.localScale = tmpScale;
                        }
                    }
                }

                var right = m_bone[HumanBodyBones.RightUpperArm];
                if (right != null)
                {
                    if (Mathf.Abs(right.forward.y) > Mathf.Abs(right.forward.z))
                    {
                        m_armScale.z = m_armScale.x;
                        right.localScale = m_armScale;
                    }
                    else
                    {
                        //軸が違うのでxyを入れ替える
                        if (right.forward.z > 0)
                        {
                            Vector3 tmpScale = new Vector3(m_armScale.y, m_armScale.x, m_armScale.x);
                            right.localScale = tmpScale;
                        }
                        else
                        {
                            Vector3 tmpScale = new Vector3(m_armScale.x, m_armScale.y, m_armScale.x);
                            right.localScale = tmpScale;
                        }
                    }
                }
            }
        }

        void draw_BottomBodyUI()
        {
            if (_BeginChangeCheck(() =>
            {
            drawSizeLabel(Localized.legrotate_z, 13);

                drawButtonUI(ref m_legRotate.z, 0.0f, 10);
                m_legRotate.z = EditorGUILayout.Slider(m_legRotate.z, -10, 10);

                GUILayout.Space(5);
                drawSizeLabel(Localized.legrotate_y, 13);

                drawButtonUI(ref m_legRotate.y, 0.0f, 10);
                m_legRotate.y = EditorGUILayout.Slider(m_legRotate.y, -10, 10);

                GUILayout.Space(5);
            }))
            {
                var left = m_bone[HumanBodyBones.LeftUpperLeg];
                if (left != null)
                {
                    //0で0に戻りたいので、回す前にいったん初期値を入れる
                    left.rotation = m_defaultLLegQuat;

                    left.Rotate(left.forward, m_legRotate.z * -1);
                    left.Rotate(left.right, m_legRotate.y * -1);
                }

                var right = m_bone[HumanBodyBones.RightUpperLeg];
                if (right != null)
                {
                    //0で0に戻りたいので、回す前にいったん初期値を入れる
                    right.rotation = m_defaultRLegQuat;

                    right.Rotate(right.forward, m_legRotate.z);
                    right.Rotate(right.right, m_legRotate.y * -1);
                }
            }

            if (_BeginChangeCheck(() =>
            {
                drawSizeLabel(Localized.scale_y, 13);
                drawButtonUI(ref m_legScale.y, 1.0f);
                m_legScale.y = EditorGUILayout.Slider(m_legScale.y, 0.5f, 1.5f);

                GUILayout.Space(5);
                drawSizeLabel(Localized.scale_x, 13);

                drawButtonUI(ref m_legScale.x, 1.0f);
                m_legScale.x = EditorGUILayout.Slider(m_legScale.x, 0.5f, 1.5f);
            }))
            {
                m_legScale.z = m_legScale.x;
                var left = m_bone[HumanBodyBones.LeftUpperLeg];
                var right = m_bone[HumanBodyBones.RightUpperLeg];
                if (left != null)
                    left.localScale = m_legScale;

                if (right != null)
                    right.localScale = m_legScale;
            }
        }

        void draw_SettingUI()
        {
            BoneRenameToggle = EditorGUILayout.ToggleLeft(Localized.bonerename, BoneRenameToggle);
            drawSizeLabel(Localized.bonerename_info, 10);

            if (BoneRenameToggle)
            {
                GUILayout.Space(15);

                BoneRenameWord = EditorGUILayout.TextField(Localized.bonerename_word, BoneRenameWord);
                drawSizeLabel(Localized.bonerename_word_info, 10);
            }
            GUILayout.Space(15);

            drawSizeLabel("胸のボーン追従", 13);

            var radioStyle = new GUIStyle(EditorStyles.radioButton);
            radioStyle.richText = true;
            BreastBoneSwap = GUILayout.SelectionGrid(BreastBoneSwap, new string[] { $"<color=#ff0000ff><b>[Beta]</b></color>Ignore Transforms (Quest対応)", "Parent Constraint (Quest非対応)", "追従しない"}, 1, radioStyle);

            //BreastBoneToggle = EditorGUILayout.ToggleLeft(Localized.breastbone_parent, BreastBoneToggle);
            //drawSizeLabel(Localized.breastbone_parent_info, 10);

            GUILayout.Space(15);

            var richTextStyle = new GUIStyle(EditorStyles.label);
            richTextStyle.richText = true;

            isAutoHipsPhysbone = EditorGUILayout.ToggleLeft($"<color=#ff0000ff><b>[Beta]</b></color>自動でお尻のボーンをセットする", isAutoHipsPhysbone, richTextStyle);
            drawSizeLabel("アバターと服の両方にお尻が揺れるPhysboneが存在する場合、そのボーンを追従するように設定します。", 10);
            GUILayout.Space(15);

            isAutoBlendshape = EditorGUILayout.ToggleLeft("自動でブレンドシェイプをセットアップする", isAutoBlendshape);
            drawSizeLabel("[Blend Shape Overrider]コンポーネントが服に着いている場合、自動で素体のブレンドシェイプを変更します。", 10);

            GUILayout.Space(15);

            //isAutoFlying = EditorGUILayout.ToggleLeft("自動で浮遊アバターセットアップをする", isAutoFlying);
            //drawSizeLabel("[Fly Avatar Setup Tool]コンポーネントが服に着いている場合、自動でアバターを浮かせます。", 10);

            GUILayout.Space(15);
        }

        void draw_RemoveClothUI()
        {
            var richTextStyle = new GUIStyle(EditorStyles.label);
            richTextStyle.richText = true;

            drawSizeLabel("※この機能は <color=#ff0000ff><b>キセテネ改良版 v2.1.4</b></color> 以降で着せた服でしか使用出来ません。", 15);
            GUILayout.Space(10);

            //リストを取得する
            EditorGUI.BeginChangeCheck();
            RemoveCloth_Avatar = EditorGUILayout.ObjectField("脱がすアバター", RemoveCloth_Avatar, typeof(GameObject), true) as GameObject;
            if (EditorGUI.EndChangeCheck())
            {
                _takeoffList.Clear();
                CheckCloths(RemoveCloth_Avatar);
            }

            

            richTextStyle = new GUIStyle(GUI.skin.button);
            richTextStyle.richText = true;
            richTextStyle.alignment = TextAnchor.MiddleCenter;

            if (GUILayout.Button("<color=#ff0000ff><b>キセテネ改良版</b></color> で着せた服を全て脱がす", richTextStyle))
            {
                RemoveCloths(RemoveCloth_Avatar);

                MessageBox("キセテネ改良版で着せた服を削除しました！", Localized.ok);
            }

            EditorGUILayout.BeginScrollView(_takeoffMenuScroll);

            if (RemoveCloth_Avatar != null)
            {
                foreach (var takeoff in _takeoffList.ToArray())
                {
                    string hash = takeoff.Key;
                    string name = takeoff.Value;

                    if (GUILayout.Button($"name = {name}, hash = {hash}"))
                    {
                        RemoveCloths(RemoveCloth_Avatar, name, hash);

                        MessageBox($"{name}を削除しました！", Localized.ok);

                        _takeoffList.Clear();
                        CheckCloths(RemoveCloth_Avatar);
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }
    }
}

