using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PathSystem
{
    public class Path : MonoBehaviour
    {
        public const int NullIndex = -1;
        
        
        private const int MinPointCount = 2;
    
        private static readonly Vector3 FirstDefaultPoint = Vector3.zero;
        private static readonly Vector3 SecondDefaultPoint = Vector3.right;


        [SerializeField] private List<Vector3> points = new List<Vector3>();
        [SerializeField] private bool isClosed = true;


        public Vector3 this[int index]
        {
            get => points[index];
            set => points[index] = value;
        }

        public int Size => points.Count;
        public bool IsClosed => isClosed;
        public bool IsEmpty => Size == 0;

    
        private int FirstPointIndex => 0;
        private int LastPointIndex => Size - 1;


        public Vector3? NextPoint(int index)
        {
            var nextIndex = GetNextPointIndex(index);

            return nextIndex == NullIndex
                ? null
                : this[nextIndex];
        }

        public Vector3? PreviousPoint(int index)
        {
            var previousIndex = GetPreviousPointIndex(index);

            return previousIndex == NullIndex
                ? null
                : this[previousIndex];
        }
    
        public int GetNextPointIndex(int index)
        {
            index++;

            if (index > LastPointIndex)
            {
                return IsClosed ? FirstPointIndex : NullIndex;
            }

            return index;
        }

        public int GetPreviousPointIndex(int index)
        {
            index--;

            if (index < FirstPointIndex)
            {
                return IsClosed ? LastPointIndex : NullIndex;
            }

            return index;
        }

        public int GetValidatedIndex(int index)
        {
            return Mathf.Clamp(index, FirstPointIndex, LastPointIndex);
        }

        public void AddPointByIndex(int index, Vector3 point)
        {
            if (index > LastPointIndex)
            {
                points.Add(point);
            }

            else
            {
                points.Insert(index, point);
            }
        }

        public void RemovePointByIndex(int index)
        {
            if (IsEmpty) return;
        
            if (Size <= MinPointCount) return;
        
            if (index < FirstPointIndex) return;
        
            if (index > LastPointIndex) return;
        
            points.RemoveAt(index);
        }

        public void ResetPath()
        {
            points.Clear();
            points.Add(FirstDefaultPoint);
            points.Add(SecondDefaultPoint);
        }
    
    
#if UNITY_EDITOR

        private void OnValidate()
        {
            if (Size < MinPointCount)
            {
                ResetPath();
            }
        }

#endif
    }


    [CustomEditor(typeof(Path))]
    public class PathEditor : Editor
    {
        private const int UnwantedControlID = -1;
        private const float PathPointRadius = 0.25f;

        private static readonly Color PathPointColor = Color.grey;
        private static readonly Color PathColor = Color.blue;
        private static readonly Color AddPathPointColor = Color.green;
        private static readonly Color RemovePathPointColor = Color.red;
    
    
        private Vector3 m_Point;
    
    
        private void OnSceneGUI()
        {
            if (targets.Length != 1) return;

            var path = target as Path;

            PathGUI(path);
            PathSceneToolsGUI(path);
        }

        private static void PathSceneToolsGUI(Path path)
        {
            const string resetPathText = "Reset Path";

            GUI.BeginGroup(new Rect(50, 10, 100, 60));

            if (GUI.Button(new Rect(0, 0, 100, 30), resetPathText))
            {
                Undo.RecordObject(path, $"Reset Path ({path.name})");
                path.ResetPath();
            }
            
            GUI.EndGroup();
        }

        private static void PathGUI(Path path)
        {
            PathEditGUI(path);
            PathDrawGUI(path);
            PathButtonGUI(path);
        }

        private static void PathEditGUI(Path path)
        {
            for (int i = 0; i < path.Size; i++)
            {
                var currentPosition = path[i];
                var newPosition = Handles.PositionHandle(currentPosition, Quaternion.identity);

                if (newPosition == currentPosition) continue;
                
                Undo.RecordObject(path, $"Move Point in Path ({path.name})");
                path[i] = newPosition;
            }
        }

        private static void PathDrawGUI(Path path)
        {
            for (int i = 0; i < path.Size; i++)
            {
                var currentPoint = path[i];
                var nextPoint = path.NextPoint(i);

                if (nextPoint.HasValue)
                {
                    Handles.color = PathColor;
                    Handles.DrawLine(currentPoint, nextPoint.Value, 2f);
                }
            
                Handles.color = PathPointColor;
                Handles.SphereHandleCap(UnwantedControlID, currentPoint, Quaternion.identity, PathPointRadius, EventType.Repaint);

                var labelStyle = new GUIStyle();
                labelStyle.alignment = TextAnchor.MiddleCenter;
                labelStyle.normal.textColor = Color.white;
            
                Handles.Label(currentPoint, i.ToString(), labelStyle);
            }
        }

        private static void PathButtonGUI(Path path)
        {
            for (int i = 0; i < path.Size; i++)
            {
                RemovePathPointButton(path, i);
            }
            
            for (int i = 0; i < path.Size; i++)
            {
                var currentPoint = path[i];
                var nextPoint = path.NextPoint(i);
                var previousPoint = path.PreviousPoint(i);
            
                if (nextPoint.HasValue)
                {
                    AddPathPointButton(path, i);
                }

                else
                {
                    if (previousPoint != null)
                    {
                        var direction = currentPoint - previousPoint.Value;
                        var position = currentPoint + direction * 0.5f;
                        AddPathPointButton(path, i, position);
                    }
                }
            }
        }

        private static void AddPathPointButton(Path path, int index)
        {
            var point = path[index];
            var nextPoint = path[path.GetNextPointIndex(index)];
            var position = Vector3.Lerp(point, nextPoint, 0.5f);
            
            AddPathPointButton(path, index, position);
        }

        private static void AddPathPointButton(Path path, int index, Vector3 position)
        {
            const string addText = "Add";

            Handles.color = AddPathPointColor;
            if (Handles.Button(
                    position,
                    Quaternion.identity,
                    PathPointRadius,
                    PathPointRadius,
                    Handles.SphereHandleCap))
            {
                Undo.RecordObject(path, $"Add Point to Path ({path.name})");
                path.AddPointByIndex(index + 1, position);
            }
        
            var labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.normal.textColor = Color.white;
        
            Handles.Label(position, addText, labelStyle);
        }
    
        private static void RemovePathPointButton(Path path, int index)
        {
            const string removeText = "Remove";
        
            var point = path[index];
            var position = point + Vector3.down * (PathPointRadius * 2f);
        
            Handles.color = RemovePathPointColor;
            if (Handles.Button(
                    position,
                    Quaternion.identity,
                    PathPointRadius,
                    PathPointRadius,
                    Handles.SphereHandleCap))
            {
                Undo.RecordObject(path, $"Remove Point from Path ({path.name})");
                path.RemovePointByIndex(index);
            }
        
            var labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.normal.textColor = Color.white;
        
            Handles.Label(position, removeText, labelStyle);
        }
    }
}