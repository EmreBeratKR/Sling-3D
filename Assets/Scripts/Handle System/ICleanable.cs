namespace Handle_System
{
    public interface ICleanable
    {
        bool IsDirty { get; }
        void Clean();
    }
}