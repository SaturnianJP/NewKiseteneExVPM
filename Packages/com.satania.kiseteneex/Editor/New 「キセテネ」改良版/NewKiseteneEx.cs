#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Saturnian_NewKiseteneEx_Package
{
    public partial class NewKiseteneEx : EditorWindow
    {

        public static string version = "v2.4.3(VPM)";

        /// <summary>
        /// 着せたい服
        /// </summary>
        public static GameObject m_cloth = null;

        /// <summary>
        /// 着せたい服のボーンを一括取得
        /// </summary>
        public static Dictionary<HumanBodyBones, Transform> m_bone = null;

        /// <summary>
        /// 着せるアバター
        /// </summary>
        public static Animator m_Avatar = null;

        /// <summary>
        /// アーマチュア
        /// </summary>
        public static Transform m_armature = null;

        /// <summary>
        /// 胸のボーン 左
        /// </summary>
        public static Transform Breast_L = null;

        /// <summary>
        /// 胸のボーン 右
        /// </summary>
        public static Transform Breast_R = null;

        /// <summary>
        /// 肺のボーン
        /// </summary>
        public static Transform Lung_L = null;

        /// <summary>
        /// 肺のボーン
        /// </summary>
        public static Transform Lung_R = null;

        /// <summary>
        /// 肺のボーン
        /// </summary>
        public static Transform Lung_Upper_L = null;

        /// <summary>
        /// 肺のボーン
        /// </summary>
        public static Transform Lung_Upper_R = null;

        /// <summary>
        /// 肺のボーン
        /// </summary>
        public static Transform Lung_Lower_L = null;

        /// <summary>
        /// 肺のボーン
        /// </summary>
        public static Transform Lung_Lower_R = null;

        public static Transform Hips_L = null;

        public static Transform Hips_R = null;

        /// <summary>
        /// ボーンの名前に付け足すワード
        /// </summary>
        string BoneRenameWord = "";

        /// <summary>
        /// ワードを付け足すフラグ
        /// </summary>
        bool BoneRenameToggle = false;

        /// <summary>
        /// 胸のボーンを揺れるように
        /// </summary>
        //bool BreastBoneToggle = true;

        /// <summary>
        /// 自動で揺れるお尻にセット
        /// </summary>
        bool isAutoHipsPhysbone = true;

        /// <summary>
        /// 自動でブレンドシェイプセットアップ
        /// </summary>
        bool isAutoBlendshape = true;

        /// <summary>
        /// 自動で浮遊アバターセットアップ
        /// </summary>
        bool isAutoFlying = true;

        /// <summary>
        /// エディタのタイトル
        /// </summary>
        public static string EditorTitle = "New 「キセテネ」改良版";

        [MenuItem("さたにあしょっぴんぐ/New Kisetene Ex(パッケージ版) %#T", priority = 12)]
        private static void Init()
        {
            //ウィンドウのインスタンスを生成
            NewKiseteneEx window = GetWindow<NewKiseteneEx>();

            //ウィンドウサイズを固定
            window.minSize = new Vector2(800, 500);

            //タイトルを変更
            window.titleContent = new GUIContent(EditorTitle);
        }

        private void OnEnable()
        {
            //ボーンリストを初期化
            m_bone = null;

            m_cloth = null;
            m_armature = null;

            ResetBone();
        }

        private void ShowGUI()
        {
            if (m_bone == null)
            {
                m_bone = new Dictionary<HumanBodyBones, Transform>();

                //一度nullを代入
                for (int i = 0; i <= (int)HumanBodyBones.LastBone; i++)
                    m_bone.Add((HumanBodyBones)i, null);
            }

            //テキストの描画方法をイニシャライズ
            InitializeTexts();

            //言語変更用
            drawLanguagePopup();

            //タイトル表示
            drawTitle();

            //空間を空ける
            GUILayout.Space(15);

            //服を入れてもらう
            if (_BeginChangeCheck(() =>
            {
                m_cloth = EditorGUILayout.ObjectField(Localized.m_cloth, m_cloth, typeof(GameObject), true) as GameObject;
            }))
            {
                m_bone.Clear();

                Breast_L = null;
                Breast_R = null;

                Hips_L = null;
                Hips_R = null;

                ResetBone();

                if (m_cloth != null)
                {
                    if (!m_cloth.IsExists())
                    {
                        GameObject copy = Instantiate(m_cloth);
                        copy.name = m_cloth.name;
                        m_cloth = copy;

                        MessageBox(Localized.copytoscene, Localized.ok);
                    }
                }

                //ボーンのリストを更新
                UpdateBones();
            }

            draw_tabs();
            draw_putbutton();
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