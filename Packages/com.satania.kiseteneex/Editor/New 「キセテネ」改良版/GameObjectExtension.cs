using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Saturnian_NewKiseteneEx_Package
{
    /// <summary>
    /// GameObjectの拡張クラス
    /// </summary>
    public static class GameObjectExtension
    {

        /// <summary>
        /// シーンをまたいでも破棄されないオブジェクトか
        /// </summary>
        public static bool IsDontDestroyOnLoad(this GameObject gameObject)
        {
            return gameObject.scene.name == "DontDestroyOnLoad";
        }

        public static bool IsExists(this GameObject gameObject)
        {
            return gameObject.scene.IsValid();
        }
    }

    public partial class NewKiseteneEx
    {

    }
}

