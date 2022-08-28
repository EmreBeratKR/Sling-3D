namespace PathSystem
{
    public enum PathMoveDirection
    {
        Forward,
        Backward
    }


    public static class PathMoveDirectionExtensions
    {
        public static PathMoveDirection Inverted(this PathMoveDirection direction)
        {
            return direction == PathMoveDirection.Forward
                ? PathMoveDirection.Backward
                : PathMoveDirection.Forward;
        }
    }
}