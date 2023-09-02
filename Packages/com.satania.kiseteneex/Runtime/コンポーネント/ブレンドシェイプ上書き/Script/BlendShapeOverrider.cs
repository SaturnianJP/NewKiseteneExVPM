using sataniashoping.component;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using VRC.SDKBase;

namespace sataniashoping
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]

    public class BlendShapeOverrider : MonoBehaviour, IEditorOnly
    {
        public SkinnedMeshRenderer mesh;
        public string mesh_path;

        public List<string> override_datas_name;
        public List<float> override_datas_weight;
        public List<float> override_datas_maxweight;
        public List<bool> override_datas_isoverride = new List<bool>();

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public Transform getRoottransform()
        {
            return transform.root;
        }

        public Transform getTransform()
        {
            return transform;
        }

        public void getBlendshapedatas()
        {
            if (override_datas_name == null)
                override_datas_name = new List<string>();

            if (override_datas_weight == null)
                override_datas_weight = new List<float>();

            if (override_datas_maxweight == null)
                override_datas_maxweight = new List<float>();

            override_datas_name.Clear();
            override_datas_weight.Clear();
            override_datas_maxweight.Clear();
            override_datas_isoverride.Clear();

            if (mesh == null || mesh.sharedMesh == null)
                return;

            for (int i = 0; i < mesh.sharedMesh.blendShapeCount; i++)
            {
                var Mes = mesh.sharedMesh;
                int frameCount = Mes.GetBlendShapeFrameCount(i);
                var frameWeight = Mes.GetBlendShapeFrameWeight(i, frameCount - 1);
                float weight = mesh.GetBlendShapeWeight(i);

                override_datas_name.Add(mesh.sharedMesh.GetBlendShapeName(i));
                override_datas_weight.Add(weight);
                override_datas_maxweight.Add(frameWeight);
                override_datas_isoverride.Add(weight != 0);

            }
        }

        public string getObjectPath(GameObject source, Transform Top)
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

        public void Awake()
        {

        }

        public int getDataLen()
        {
            if (override_datas_name == null)
                return 0;

            return override_datas_name.Count;
        }

        public string[] getBlendshapeNames(SkinnedMeshRenderer skindmesh)
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
        public void MessageBox(string message, string ok)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("BlendShape Override", message, ok);
#endif
        }

        public void SetBlendshape()
        {
#if UNITY_EDITOR
            if (PrefabUtility.GetPrefabAssetType(getTransform()) != PrefabAssetType.NotAPrefab)
                PrefabUtility.UnpackPrefabInstance(getTransform().gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
#endif

            Transform _root = getRoottransform();
            if (_root != null && !string.IsNullOrEmpty(mesh_path))
            {
                Transform target_mesh = _root.transform.Find(mesh_path);
                if (target_mesh == null)
                    return;

                SkinnedMeshRenderer _target_skinnedmesh = target_mesh.GetComponent<SkinnedMeshRenderer>();
                if (_target_skinnedmesh == null)
                    return;

                string[] blendShapeNames = getBlendshapeNames(_target_skinnedmesh);
                for (int i = 0; i < override_datas_name.Count; i++)
                {
                    string name = override_datas_name[i];

                    int index = blendShapeNames.ToList().IndexOf(name);
                    float weight = override_datas_weight[i];

                    if (weight < 0)
                        continue;

                    if (override_datas_isoverride[i])
                        _target_skinnedmesh.SetBlendShapeWeight(index, override_datas_weight[i]);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


