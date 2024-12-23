using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDKBase;

namespace sataniashoping
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]

    public class BlendShapeOverrider : MonoBehaviour, IEditorOnly
    {
        public SkinnedMeshRenderer SkinnedMesh
        {
            get
            {
                if (string.IsNullOrEmpty(mesh_path))
                    return null;

                VRCAvatarDescriptor avatar = GetComponentInParent<VRCAvatarDescriptor>();
                if (avatar == null)
                    return null;

                Transform meshTransform = avatar.transform.Find(mesh_path);
                return meshTransform?.GetComponent<SkinnedMeshRenderer>();
            }
        }

        //public SkinnedMeshRenderer mesh;
        public string mesh_path;

        public List<string> override_datas_name;
        public List<float> override_datas_weight;
        public List<float> override_datas_maxweight;
        public List<bool> override_datas_isoverride = new List<bool>();

        public Transform getRoottransform()
        {
            return transform.root;
        }

        public Transform getTransform()
        {
            return transform;
        }

        public void GetBlendshapedatas()
        {
            override_datas_name = new List<string>();
            override_datas_weight = new List<float>();
            override_datas_maxweight = new List<float>();
            override_datas_isoverride = new List<bool>();

            if (SkinnedMesh == null || SkinnedMesh.sharedMesh == null)
                return;

            for (int i = 0; i < SkinnedMesh.sharedMesh.blendShapeCount; i++)
            {
                var Mes = SkinnedMesh.sharedMesh;
                int frameCount = Mes.GetBlendShapeFrameCount(i);
                var frameWeight = Mes.GetBlendShapeFrameWeight(i, frameCount - 1);
                float weight = SkinnedMesh.GetBlendShapeWeight(i);

                override_datas_name.Add(SkinnedMesh.sharedMesh.GetBlendShapeName(i));
                override_datas_weight.Add(weight);
                override_datas_maxweight.Add(frameWeight);
                override_datas_isoverride.Add(weight != 0);

            }
        }

        public string GetObjectPath(GameObject source, Transform Top)
        {
            var builder = new StringBuilder(source.transform.name);
            var current = source.transform.parent;
            while (current != null)
            {
                if (current == Top)
                    break;

                if (current.parent == Top)
                    break;

                builder.Insert(0, current.name + "/");
                current = current.parent;
            }

            return builder.ToString();
        }

        public int GetDataLen()
        {
            if (override_datas_name == null)
                return 0;

            return override_datas_name.Count;
        }

        public string[] GetBlendshapeNames(SkinnedMeshRenderer skindmesh)
        {
            if (skindmesh == null || skindmesh.sharedMesh == null)
                return null;

            int len = skindmesh.sharedMesh.blendShapeCount;
            string[] ret = new string[len];

            for (int i = 0; i < len; i++)
            {
                ret[i] = skindmesh.sharedMesh.GetBlendShapeName(i);
            }

            return ret;
        }

        /// <summary>
        /// OKのみのメッセージボックス
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="ok"></param>
        //        public void MessageBox(string message, string ok)
        //        {
        //#if UNITY_EDITOR
        //            EditorUtility.DisplayDialog("BlendShape Override", message, ok);
        //#endif
        //        }

        public void SetBlendshape()
        {
            Transform t = getRoottransform();
            if (t != null && !string.IsNullOrEmpty(mesh_path))
            {
                Transform mesh = t.transform.Find(mesh_path);
                if (mesh == null)
                    return;

                SkinnedMeshRenderer skinnedMesh = mesh.GetComponent<SkinnedMeshRenderer>();
                if (skinnedMesh == null)
                    return;

                string[] blendShapeNames = GetBlendshapeNames(skinnedMesh);
                for (int i = 0; i < override_datas_name.Count; i++)
                {
                    string name = override_datas_name[i];

                    int index = Array.IndexOf(blendShapeNames, name);
                    if (index == -1)
                        continue;

                    float weight = override_datas_weight[i];

                    if (weight < 0)
                        continue;

                    if (override_datas_isoverride[i])
                        skinnedMesh.SetBlendShapeWeight(index, override_datas_weight[i]);
                }
            }
        }
    }
}


