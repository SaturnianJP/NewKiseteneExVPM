#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Saturnian_NewKiseteneEx_Package
{
    public partial class NewKiseteneEx
    {
        /// <summary>
        /// 変更確認用
        /// </summary>
        /// <param name="action">もし変更された場合の動作</param>
        /// <returns></returns>
        public bool _BeginChangeCheck(Action action)
        {
            EditorGUI.BeginChangeCheck();
            action();
            return EditorGUI.EndChangeCheck();
        }
    }
}

#endif