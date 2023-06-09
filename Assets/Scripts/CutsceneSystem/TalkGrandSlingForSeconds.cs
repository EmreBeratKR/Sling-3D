using System.Collections;
using GrandSling;
using UnityEngine;

namespace CutsceneSystem
{
    public class TalkGrandSlingForSeconds : MonoBehaviour
    {
        [SerializeField] private GrandSlingBehaviour grandSling;
        [SerializeField] private float seconds;


        public void Talk()
        {
            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                grandSling.SetIsTalking(true);
                yield return new WaitForSeconds(seconds);
                grandSling.SetIsTalking(false);
            }
        }
    }
}