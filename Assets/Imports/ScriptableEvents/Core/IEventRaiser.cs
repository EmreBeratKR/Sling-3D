namespace ScriptableEvents.Core
{
    public interface IEventRaiser
    {
        public void RaiseEvent();
    }
    
    public interface IEventRaiser<in T>
    {
        public void RaiseEvent(T data);
    }
}