using UnityEngine;

namespace TubeSystem
{
    public class SlimeTube : Tube
    {
        protected override void Charge()
        {
            Debug.Log("Slimed!");
        }
    }
}