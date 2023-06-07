using UnityEngine;

namespace CutsceneSystem
{
    public class ElementHolder : MonoBehaviour
    {
        [SerializeField] private Renderer coreRenderer;
        [SerializeField] private Material enabledMaterial;
        [SerializeField] private Material disabledMaterial;
        
        
        public void EnablePower()
        {
            coreRenderer.material = enabledMaterial;
        }

        public void DisablePower()
        {
            coreRenderer.material = disabledMaterial;
        }
    }
}