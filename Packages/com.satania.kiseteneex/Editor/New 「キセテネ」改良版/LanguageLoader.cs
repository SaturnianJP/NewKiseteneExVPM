#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Saturnian_NewKiseteneEx_Package
{
    public class Kisetene_localize
    {
        public string title = "New 「キセテネ」改良版";
        public string m_cloth = "着せたい服";
        public string copytoscene = "シーン上に存在しないため、シーン上へコピーしました。\n位置の調整が必要な場合はご自身で調整を行ってください。";
        public string bonesetting = "ボーン詳細設定";
        public string fullbody_ui = "調整用 (全身)";
        public string upperbody_ui = "調整用 (上半身)";
        public string bottombody_ui = "調整 (下半身)";
        public string test_ui = "テスト機能";
        public string avatar = "アバター";
        public string ok = "OK";
        public string avatarcopytoscene = "アバターがシーン上に存在しないため、シーン上へコピーしました。";
        public string put_cloth = "服を着せる";
        public string reset = "リセット";
        public string updown = "上下";
        public string frontback = "前後";
        public string zoominout = "拡大 縮小";
        public string ozigi = "お辞儀";
        public string scale_y = "袖を伸ばす";
        public string scale_x = "袖を太くする";
        public string legrotate_z = "足を開く";
        public string legrotate_y = "足を前に出す";
        public string bonerename = "ボーンの名前 文字付け足し";
        public string bonerename_info = "着せる服のボーンに文字を付け足す機能です。";
        public string bonerename_word = "付け足す文字";
        public string bonerename_word_info = "付け足す文字を入れてください。";
        public string breastbone_parent = "胸のボーン 追従";
        public string breastbone_parent_info = "Parent Constraintを使用して胸のボーンを完全追従させます。";
        public string upperbody = "上半身";
        public string leftarm = "左腕";
        public string rightarm = "右腕";
        public string leftleg = "左足";
        public string rightleg = "右足";
        public string bonesetting_warinig = "ボーン詳細設定を必ず確認する必要はありません。\n何故か服が着せれない場合などに確認してください。";
    }

    public partial class NewKiseteneEx
    {
        /// <summary>
        /// 翻訳ファイル用
        /// </summary>
        public static Kisetene_localize Localized = new Kisetene_localize();

        /// <summary>
        /// 言語ファイル
        /// </summary>
        public static string LocalizePath = "Packages/com.satania.kiseteneex/Runtime/New 「キセテネ」改良版/Localize/";

        /// <summary>
        /// 言語ファイル読み込み用
        /// </summary>
        public static string[] files = { "jp", "kr", "en", "ch", "ch2" };

        public static string[] guids = new string[]
        {
            "5ba04412443076041b104348d30a26c6",
            "4416817ae9e309746a6e556d4e13f79e",
            "27917cd9fea316e4192fa4ec4907092d",
            "6fa3a86895b68124d9adde34f52fe7b1",
            "c5c37f3fa0723b64986d84ac9fd66006",
        };

        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Kisetene_localize LoadLocalizeFile(int index)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[index]);

            //string filename = $"{LocalizePath}{files[index]}.json";
            if (!File.Exists(assetPath))
                return new Kisetene_localize();

            string jsonText = File.ReadAllText(assetPath);
            var FromJson = JsonUtility.FromJson<Kisetene_localize>(jsonText);

            if (FromJson == null)
                return new Kisetene_localize();

            return FromJson;
        }

        /// <summary>
        /// 言語の
        /// </summary>
        static int languageIndex = 0;

        private void drawLanguagePopup()
        {
            if (_BeginChangeCheck(() =>
            {
                languageIndex = EditorGUILayout.Popup("Language", languageIndex, new string[] { "日本語", "한국어", "English", "简体中文", "繁体中文" });
            }))
            {
                //言語が選択された場合ファイルがロード
                Localized = LoadLocalizeFile(languageIndex);
            }
        }
    }
}

#endif