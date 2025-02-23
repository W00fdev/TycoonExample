 #if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Editor
{
    public class StraightenCamera
    {
        [MenuItem("Edit/Straighten Camera")]
        public static void StraightenCameraLogic()
        {
            SceneView last = SceneView.lastActiveSceneView;
            Vector3 forward = last.rotation * Vector3.forward;
            last.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }
    }
}
#endif