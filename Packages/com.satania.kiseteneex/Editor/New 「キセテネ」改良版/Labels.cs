#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Saturnian_NewKiseteneEx_Package
{
    public partial class NewKiseteneEx
    {
        static GUIStyle RichText;
    
        private static void InitializeTexts()
        {
            //リッチテキストを使えるように
            if (RichText == null)
            {
                RichText = new GUIStyle(EditorStyles.label);
                RichText.richText = true;
            }
        }

        private static void drawTitle()
        {
            //タイトル表示
            GUILayout.Label($"<size=20><b>{Localized.title} {version}</b></size>", RichText);
        }

        private static void drawSizeLabel(string msg, int size, bool isBold = true)
        {
            if (isBold)
                GUILayout.Label($"<size={size}><b>{msg}</b></size>", RichText);
            else
                GUILayout.Label($"<size={size}>{msg}</size>", RichText);
        }
    }
}
#endif

