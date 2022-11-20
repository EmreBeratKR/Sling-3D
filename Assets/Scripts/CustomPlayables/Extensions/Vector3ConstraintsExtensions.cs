namespace CustomPlayables.Extensions
{
    public static class Vector3ConstraintsExtensions
    {
        public static bool Contains(this TransformClip.Vector3Constraints lhs, TransformClip.Vector3Constraints rhs)
        {
            return (lhs & rhs) == rhs;
        }

        public static bool ContainsX(this TransformClip.Vector3Constraints constraints)
        {
            return constraints.Contains(TransformClip.Vector3Constraints.X);
        }
        
        public static bool ContainsY(this TransformClip.Vector3Constraints constraints)
        {
            return constraints.Contains(TransformClip.Vector3Constraints.Y);
        }
        
        public static bool ContainsZ(this TransformClip.Vector3Constraints constraints)
        {
            return constraints.Contains(TransformClip.Vector3Constraints.Z);
        }
        
        public static bool ContainsXY(this TransformClip.Vector3Constraints constraints)
        {
            return constraints.Contains(TransformClip.Vector3Constraints.X | TransformClip.Vector3Constraints.Y);
        }
        
        public static bool ContainsXZ(this TransformClip.Vector3Constraints constraints)
        {
            return constraints.Contains(TransformClip.Vector3Constraints.X | TransformClip.Vector3Constraints.Z);
        }
        
        public static bool ContainsYZ(this TransformClip.Vector3Constraints constraints)
        {
            return constraints.Contains(TransformClip.Vector3Constraints.Y | TransformClip.Vector3Constraints.Z);
        }
        
        public static bool ContainsXYZ(this TransformClip.Vector3Constraints constraints)
        {
            return constraints.Contains(TransformClip.Vector3Constraints.X | TransformClip.Vector3Constraints.Y | TransformClip.Vector3Constraints.Z);
        }

        public static bool IsNone(this TransformClip.Vector3Constraints constraints)
        {
            return !constraints.ContainsX() && !constraints.ContainsY() && !constraints.ContainsZ();
        }
    }
}