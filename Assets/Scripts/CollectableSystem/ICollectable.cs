using Sling;

namespace CollectableSystem
{
    public interface ICollectable
    {
        void Collect(SlingBehaviour collector);
    }
}