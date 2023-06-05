using DG.Tweening;
using UnityEngine;

namespace Elements
{
    public class MagicElement : MonoBehaviour
    {
        private static readonly int ShaderPowerID = Shader.PropertyToID("_Power");
        
        
        [SerializeField] private Renderer renderer;


        private void Awake()
        {
            DOTween.To(GetPower, SetPower, -0.15f, 1f)
                .From(-0.5f)
                .SetLoops(-1, LoopType.Yoyo);
        }


        private void SetPower(float value)
        {
            renderer.material.SetFloat(ShaderPowerID, value);
        }

        private float GetPower()
        {
            return renderer.material.GetFloat(ShaderPowerID);
        }
    }
}