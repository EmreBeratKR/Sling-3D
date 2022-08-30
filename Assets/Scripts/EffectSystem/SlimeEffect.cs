using TubeSystem;
using UnityEngine;

namespace EffectSystem
{
    public class SlimeEffect : Effect
    {
        [Header("Slime Effect")]
        [SerializeField] private GameObject main;
        [SerializeField] private GameObject visual;


        private void OnEnable()
        {
            SlimeTube.OnCharged += OnSlimeTubeCharged;
        }

        private void OnDisable()
        {
            SlimeTube.OnCharged -= OnSlimeTubeCharged;
        }

        private void OnSlimeTubeCharged()
        {
            Activate();
        }


        protected override void OnActivated()
        {
            main.SetActive(true);
        }

        protected override void OnDeactivated()
        {
            main.SetActive(false);
        }

        protected override void OnBlinkVisible()
        {
            visual.SetActive(true);
        }

        protected override void OnBlinkInvisible()
        {
            visual.SetActive(false);
        }
    }
}