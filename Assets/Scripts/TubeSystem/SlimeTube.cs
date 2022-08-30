using UnityEngine.Events;

namespace TubeSystem
{
    public class SlimeTube : Tube
    {
        public static UnityAction OnCharged;


        protected override void Charge()
        {
            base.Charge();
            OnCharged?.Invoke();
        }
    }
}