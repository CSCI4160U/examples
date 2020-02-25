using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Launcher))]
public class LauncherEditor : Editor {
    [DrawGizmo(GizmoType.Pickable | GizmoType.Selected)]
    static void DrawGizmoSelected(Launcher launcher, GizmoType gizmoType) {
        // draw the starting point
        var offsetPosition = launcher.transform.TransformPoint(launcher.offset);
        Handles.DrawDottedLine(launcher.transform.position, offsetPosition, 3.0f);
        Handles.Label(offsetPosition, "Offset");

        // draw the trajectory
        var velocity = launcher.transform.forward * launcher.launchVelocity;
        var positions = new List<Vector3>();
        var position = offsetPosition;
        var deltaT = 0.1f;
        for (var t = 0.0f; t <= 1.0f; t += deltaT) {
            positions.Add(position);
            position += velocity * deltaT;
            velocity += Physics.gravity * deltaT;
        }

        using (new Handles.DrawingScope(Color.yellow)) {
            Handles.DrawAAPolyLine(positions.ToArray());
            var lastPos = positions[positions.Count - 1];
            Gizmos.DrawWireSphere(lastPos, launcher.blastRadius);
            Handles.Label(lastPos, "Estimated Position (1 sec)");
        }
    }

    private void OnSceneGUI() {
        var launcher = target as Launcher;
        using (var cc = new EditorGUI.ChangeCheckScope()) {
            var offset = launcher.transform.TransformPoint(launcher.offset);

            var newOffset = Handles.PositionHandle(offset, launcher.transform.rotation);
            launcher.offset = launcher.transform.InverseTransformPoint(newOffset);
        }

        Handles.BeginGUI();
        var rectMin = Camera.current.WorldToScreenPoint(launcher.transform.position +
                                                        launcher.offset);
        var rect = new Rect();
        rect.xMin = rectMin.x;
        rect.yMin = SceneView.currentDrawingSceneView.position.height - rectMin.y;
        rect.width = 64;
        rect.height = 18;

        GUILayout.BeginArea(rect);
        using (new EditorGUI.DisabledGroupScope(!Application.isPlaying)) {
            if (GUILayout.Button("Fire")) {
                launcher.Fire();
            }
        }
        GUILayout.EndArea();

        Handles.EndGUI();
    }
}
