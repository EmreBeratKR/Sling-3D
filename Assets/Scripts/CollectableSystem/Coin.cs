using Goal_System;
using Sling;
using SoundSystem;
using UnityEngine;

namespace CollectableSystem
{
    public class Coin : MonoBehaviour, ICollectable
    {
        [SerializeField] private GameObject visual;
        [SerializeField] private SfxPlayer collectSfx;


        private Collider m_Collider;


        private void Awake()
        {
            m_Collider = GetComponent<Collider>();
        }


        public void Collect(SlingBehaviour collector)
        {
            FindObjectOfType<CoinGoal>().AddCoin(1);
            FindObjectOfType<CollectableSpawner>().Spawn(1);
            collectSfx.PlayRandomClip();
            visual.SetActive(false);
            m_Collider.enabled = false;
            Destroy(gameObject, 1f);
        }
    }
}