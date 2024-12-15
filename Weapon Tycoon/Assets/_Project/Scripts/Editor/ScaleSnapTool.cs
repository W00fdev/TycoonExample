using UnityEditor;
using UnityEngine;

namespace _Project.Scripts.Editor
{
    /*
     Usage: https://i.imgur.com/drW9RI0.mp4
      1. Click on Tools -> Scale Snap Tool
      2. Just do normal vertex snapping (V+mouse drag)
      3. Close the window if you want to snap without scaling
     */
    public class ScaleSnapTool : EditorWindow {
        private static bool isScaling = false;
        private static Transform selectedTransform;
        private static Vector3 initialPos;

        [MenuItem("Tools/Scale Snap Tool")]
        private static void Init() {
            EditorWindow window = GetWindow<ScaleSnapTool>("Scale Snap Tool");
            window.Show();
        }

        private void OnEnable() {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable() {
            StopScaling();
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private static void StartScaling() {
            if (Selection.activeTransform != null && !isScaling) {
                selectedTransform = Selection.activeTransform;
                initialPos = selectedTransform.position;
                isScaling = true;
            }
        }

        private static void StopScaling() {
            isScaling = false;
            selectedTransform = null;
        }

        private static void OnSceneGUI(SceneView sceneView) {
            Event e = Event.current;

            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.V) {
                StartScaling();
            } else if (e.type == EventType.KeyUp && e.keyCode == KeyCode.V) {
                CalculateNewScale();
                StopScaling();
            }
        }

        public static void CalculateNewScale() {
            Vector3 newPosition = selectedTransform.position;

            Renderer renderer = selectedTransform.GetComponent<Renderer>();
            var distance = newPosition - initialPos;
            distance.x = Mathf.Abs(distance.x);
            distance.y = Mathf.Abs(distance.y);
            distance.z = Mathf.Abs(distance.z);
            float maxValue = Mathf.Max(distance.x, distance.y, distance.z);
            
            float val;
            if (distance.x == maxValue) {
                val = (renderer.bounds.size.x + Mathf.Abs(initialPos.x - newPosition.x)) * selectedTransform.localScale.x / renderer.bounds.size.x;
            } else {
                val = (renderer.bounds.size.z + Mathf.Abs(initialPos.z - newPosition.z)) * selectedTransform.localScale.z / renderer.bounds.size.z;
            }

            selectedTransform.localScale = new Vector3(distance.x == maxValue ? val : selectedTransform.localScale.x, selectedTransform.localScale.y, distance.z == maxValue ? val : selectedTransform.localScale.z);
            
            // recenter it based on where it used to be
            selectedTransform.position = (newPosition + initialPos) / 2.0f;
        }
    }
}