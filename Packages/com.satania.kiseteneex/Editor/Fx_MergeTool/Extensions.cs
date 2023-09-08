#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using static VRC.SDK3.Avatars.Components.VRCAvatarDescriptor;

namespace satania.fxmergetool
{
    public static class ExtensionClasses
    {
        #region [AvatarDescriptorFunctions]
        public enum BaseLayerNums
        {
            Locomotion,
            Additive,
            Gesture,
            Action,
            FX
        }

        public static AnimatorController GetBaseLayer(this VRCAvatarDescriptor _avatar, BaseLayerNums num)
        {
            AnimLayerType type = default(AnimLayerType);
            if (num == BaseLayerNums.Locomotion)
                type = AnimLayerType.Base;
            else if (num == BaseLayerNums.Additive)
                type = AnimLayerType.Additive;
            else if (num == BaseLayerNums.Gesture)
                type = AnimLayerType.Gesture;
            else if (num == BaseLayerNums.Action)
                type = AnimLayerType.Action;
            else if (num == BaseLayerNums.FX)
                type = AnimLayerType.FX;

            for (int i = 0; i < _avatar.baseAnimationLayers.Length; i++)
            {
                var layer = _avatar.baseAnimationLayers[i];
                if (layer.type != type)
                    continue;

                if (layer.animatorController == null)
                    return null;

                //Debug.Log(i);

                return (AnimatorController)layer.animatorController;
            }

            return null;
        }

        public static VRCAvatarDescriptor BackupAvatar(this VRCAvatarDescriptor _avatar, string name)
        {
            _avatar.gameObject.SetActive(false);

            VRCAvatarDescriptor ret = GameObject.Instantiate(_avatar);
            ret.name = _avatar.name;

            if (!ret.name.Contains(name))
                ret.name += name;

            ret.gameObject.SetActive(true);

            return ret;
        }
        #endregion

        #region [File Save Utility]
        public static UnityEngine.Object SaveFile(this UnityEngine.Object asset, string filepath, string ex)
        {
            Type type = asset.GetType();

            string AssetPath = AssetDatabase.GetAssetPath(asset);
            if (AssetPath == null || string.IsNullOrEmpty(AssetPath))
                AssetDatabase.CreateAsset(asset, filepath + $".{ex}");
            else
                AssetDatabase.CopyAsset(AssetPath, filepath + $".{ex}");

            if (!File.Exists(filepath + $".{ex}"))
                return default;

            AssetDatabase.Refresh();

            return AssetDatabase.LoadAssetAtPath(filepath + $".{ex}", type);
        }
        #endregion
    }
}
#endif
