using Goal_System;
using Sling;
using UnityEngine;

namespace CollectableSystem
{
    public class Coin : MonoBehaviour, ICollectable
    {
        public void Collect(SlingBehaviour collector)
        {
            FindObjectOfType<CoinGoal>().AddCoin(1);
            FindObjectOfType<CollectableSpawner>().Spawn(1);
            Destroy(gameObject);
        }
    }
}