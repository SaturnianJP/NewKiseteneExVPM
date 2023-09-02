
using UnityEngine;
using VRC.SDKBase;

namespace sataniashoping.component.kisetenecomponent
{
    //Editモードでも実行
    [ExecuteAlways]

    //複数個つくのを無効化
    [DisallowMultipleComponent]

    [AddComponentMenu("さたにあしょっぴんぐ/Kisetene Component")]
    public class KiseteneComponent : MonoBehaviour, IEditorOnly
    {
        public string hash;
        public string cloth_name;

        private void OnValidate()
        {
            this.hideFlags = HideFlags.HideInInspector; 
        }

        private void Update()
        {
            //Playmodeの場合は削除
            //if (EditorApplication.isPlaying)
            //{
            //    Destroy(this);
            //}
        }
    }

    
}


