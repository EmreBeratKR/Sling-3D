namespace ScriptableEvents.Core
{
    public interface IEventResponder
    {
        public void Respond();
    }

    public interface IEventResponder<in T>
    {
        public void Respond(T data);
    }
}