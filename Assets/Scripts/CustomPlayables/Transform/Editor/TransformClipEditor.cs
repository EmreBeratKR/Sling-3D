using CustomPlayables.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace CustomPlayables.Editor
{
    [CustomEditor(typeof(TransformClip))]
    public class TransformClipEditor : UnityEditor.Editor
    {
        private const string AllAxisesAreConstrained = "All axises are constrained!";
        private const string ToLabel = "To";
        private const string FromLabel = "From";
        private const string PositionLabel = "Position";
        private const string RotationLabel = "Rotation";
        private const string ScaleLabel = "Scale";
        private const string ConstraintsLabel = "Constraints";
        private const string CurvesLabel = "Curves";
        private const string XLabel = "X";
        private const string YLabel = "Y";
        private const string ZLabel = "Z";


        private static bool ms_ShowFrom;
        private static bool ms_ShowTo;
        private static bool ms_ShowConstraints;
        private static bool ms_ShowCurves;
        

        public override void OnInspectorGUI()
        {
            var targetObj = (TransformClip) target;
            

            DrawFoldout(targetObj, FromLabel, ref ms_ShowFrom, _ =>
            {
                DrawTransformData(targetObj, ref targetObj.from);
            });
            
            EditorGUILayout.Space();
            
            DrawFoldout(targetObj, ToLabel, ref ms_ShowTo, _ =>
            {
                DrawTransformData(targetObj, ref targetObj.to);
            });
            
            EditorGUILayout.Space();
            
            DrawFoldout(targetObj, CurvesLabel, ref ms_ShowCurves, DrawCurves);
            
            EditorGUILayout.Space();
            
            DrawFoldout(targetObj, ConstraintsLabel, ref ms_ShowConstraints, DrawConstraints);
            
            EditorGUILayout.Space();
            
            DrawRestoreDefaultButton(targetObj);
        }
        

        private static void DrawFoldout(TransformClip target, string title, ref bool value, UnityAction<TransformClip> drawer)
        {
            value = EditorGUILayout.Foldout(value, title);
            
            if (!value) return;
            
            
            EditorGUI.indentLevel++;
            
            
            drawer?.Invoke(target);
            
            
            EditorGUI.indentLevel--;
        }
        
        private static void DrawTransformData(TransformClip target, ref TransformClip.Data data)
        {
            const string updateTransformClipTransformValuesValues = "Update TransformClip Transform values";
            
            
            Undo.RecordObject(target, updateTransformClipTransformValuesValues);
            
            
            DrawVector3Fields(PositionLabel, ref data.position, target.positionConstraints);
            DrawVector3Fields(RotationLabel, ref data.rotation, target.rotationConstraints);
            DrawVector3Fields(ScaleLabel, ref data.scale, target.scaleConstraints);
        }
        
        private static void DrawConstraints(TransformClip target)
        {
            const string usePrefix = "Use";
            const string updateTransformClipConstraints = "Update TransformClip constraints";
            
            
            Undo.RecordObject(target, updateTransformClipConstraints);
            
            
            DrawVector3Constraints($"{usePrefix} {PositionLabel}", ref target.positionConstraints);
            DrawVector3Constraints($"{usePrefix} {RotationLabel}", ref target.rotationConstraints);
            DrawVector3Constraints($"{usePrefix} {ScaleLabel}", ref target.scaleConstraints);
        }

        private static void DrawCurves(TransformClip target)
        {
            const string updateTransformClipCurves = "Update TransformClip curves";
            
            
            Undo.RecordObject(target, updateTransformClipCurves);
            
            
            DrawCurveGroup(PositionLabel, ref target.positionCurve, target.positionConstraints);
            DrawCurveGroup(RotationLabel, ref target.rotationCurve, target.rotationConstraints);
            DrawCurveGroup(ScaleLabel, ref target.scaleCurve, target.scaleConstraints);
        }

        private static void DrawRestoreDefaultButton(TransformClip target)
        {
            if (GUILayout.Button("Restore Default Values"))
            {
                target.RestoreDefaultValues();
            }
        }
        
        private static void DrawCurveGroup(string title, ref TransformClip.AnimationCurveGroup value, TransformClip.Vector3Constraints constraints)
        {
            EditorGUILayout.BeginHorizontal();
            
            
            EditorGUILayout.LabelField(title, GUILayout.Width(80f));

            
            if (constraints.IsNone())
            {
                EditorGUILayout.LabelField(AllAxisesAreConstrained);
                EditorGUILayout.EndHorizontal();
                return;
            }
            
            
            var fieldCount = 1;

            if (constraints.ContainsXYZ())
            {
                fieldCount = 3;
            }
            
            else if (constraints.ContainsXY() || constraints.ContainsXZ() || constraints.ContainsYZ())
            {
                fieldCount = 2;
            }

            var fieldWidth = (EditorGUIUtility.currentViewWidth - 180f) / fieldCount;
            

            if (constraints.ContainsXYZ())
            {
                DrawSingleCurveField(XLabel, ref value.x, fieldWidth);
                DrawSingleCurveField(YLabel, ref value.y, fieldWidth);
                DrawSingleCurveField(ZLabel, ref value.z, fieldWidth);
            }
            
            else if (constraints.ContainsXY())
            {
                DrawSingleCurveField(XLabel, ref value.x, fieldWidth);
                DrawSingleCurveField(YLabel, ref value.y, fieldWidth);
            }
            
            else if (constraints.ContainsXZ())
            {
                DrawSingleCurveField(XLabel, ref value.x, fieldWidth);
                DrawSingleCurveField(ZLabel, ref value.z, fieldWidth);
            }
            
            else if (constraints.ContainsYZ())
            {
                DrawSingleCurveField(YLabel, ref value.y, fieldWidth);
                DrawSingleCurveField(ZLabel, ref value.z, fieldWidth);
            }
            
            else if (constraints.ContainsX())
            {
                DrawSingleCurveField(XLabel, ref value.x, fieldWidth);
            }
            
            else if (constraints.ContainsY())
            {
                DrawSingleCurveField(YLabel, ref value.y, fieldWidth);
            }
            
            else if (constraints.ContainsZ())
            {
                DrawSingleCurveField(ZLabel, ref value.z, fieldWidth);
            }


            EditorGUILayout.EndHorizontal();
        }

        private static void DrawVector3Fields(string title, ref Vector3 vector, TransformClip.Vector3Constraints constraints)
        {
            EditorGUILayout.BeginHorizontal();
            
            
            EditorGUILayout.LabelField(title, GUILayout.Width(80f));


            if (constraints.IsNone())
            {
                EditorGUILayout.LabelField(AllAxisesAreConstrained);
                EditorGUILayout.EndHorizontal();
                return;
            }
            
            
            var fieldCount = 1;

            if (constraints.ContainsXYZ())
            {
                fieldCount = 3;
            }
            
            else if (constraints.ContainsXY() || constraints.ContainsXZ() || constraints.ContainsYZ())
            {
                fieldCount = 2;
            }

            var fieldWidth = (EditorGUIUtility.currentViewWidth - 180f) / fieldCount;
            

            if (constraints.ContainsXYZ())
            {
                DrawSingleVector3Field(XLabel, ref vector.x, fieldWidth);
                DrawSingleVector3Field(YLabel, ref vector.y, fieldWidth);
                DrawSingleVector3Field(ZLabel, ref vector.z, fieldWidth);
            }
            
            else if (constraints.ContainsXY())
            {
                DrawSingleVector3Field(XLabel, ref vector.x, fieldWidth);
                DrawSingleVector3Field(YLabel, ref vector.y, fieldWidth);
            }
            
            else if (constraints.ContainsXZ())
            {
                DrawSingleVector3Field(XLabel, ref vector.x, fieldWidth);
                DrawSingleVector3Field(ZLabel, ref vector.z, fieldWidth);
            }
            
            else if (constraints.ContainsYZ())
            {
                DrawSingleVector3Field(YLabel, ref vector.y, fieldWidth);
                DrawSingleVector3Field(ZLabel, ref vector.z, fieldWidth);
            }
            
            else if (constraints.ContainsX())
            {
                DrawSingleVector3Field(XLabel, ref vector.x, fieldWidth);
            }
            
            else if (constraints.ContainsY())
            {
                DrawSingleVector3Field(YLabel, ref vector.y, fieldWidth);
            }
            
            else if (constraints.ContainsZ())
            {
                DrawSingleVector3Field(ZLabel, ref vector.z, fieldWidth);
            }


            EditorGUILayout.EndHorizontal();
        }

        private static void DrawSingleVector3Field(string axis, ref float value, float width)
        {
            var labelWidth = GUILayout.MaxWidth(40f);
            var fieldWidth = GUILayout.MaxWidth(width);
            
            EditorGUILayout.LabelField(axis, labelWidth);
            value = EditorGUILayout.FloatField(value, fieldWidth);
        }

        private static void DrawSingleCurveField(string title, ref AnimationCurve curve, float width)
        {
            var labelWidth = GUILayout.MaxWidth(40f);
            var fieldWidth = GUILayout.MaxWidth(width);
            
            EditorGUILayout.LabelField(title, labelWidth);
            curve = EditorGUILayout.CurveField(curve, fieldWidth);
        }
        
        private static void DrawVector3Constraints(string title, ref TransformClip.Vector3Constraints constraints)
        {
            EditorGUILayout.BeginHorizontal();
            
            
            EditorGUILayout.LabelField(title);

            var usePositionX = GUILayout.Toggle(constraints.Contains(TransformClip.Vector3Constraints.X), XLabel);

            if (usePositionX)
            {
                constraints |= TransformClip.Vector3Constraints.X;
            }

            else
            {
                constraints &= ~TransformClip.Vector3Constraints.X;
            }
            
            var usePositionY = GUILayout.Toggle(constraints.Contains(TransformClip.Vector3Constraints.Y), YLabel);

            if (usePositionY)
            {
                constraints |= TransformClip.Vector3Constraints.Y;
            }

            else
            {
                constraints &= ~TransformClip.Vector3Constraints.Y;
            }
            
            var usePositionZ = GUILayout.Toggle(constraints.Contains(TransformClip.Vector3Constraints.Z), ZLabel);

            if (usePositionZ)
            {
                constraints |= TransformClip.Vector3Constraints.Z;
            }

            else
            {
                constraints &= ~TransformClip.Vector3Constraints.Z;
            }


            EditorGUILayout.EndHorizontal();
        }
    }
}