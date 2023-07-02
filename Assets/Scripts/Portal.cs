using System.Collections;
using DG.Tweening;
using Handle_System;
using NaughtyAttributes;
using ScriptableEvents.Core.Channels;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Event Channels")]
    [SerializeField] private Vector3EventChannel portalSpawned;

    [Header("References")] 
    [SerializeField] private PortalSound sound;
    [SerializeField] private Transform model;
    [SerializeField] private GameObject handle;
    [SerializeField] private Transform exit;
    [SerializeField] private GameObject normalVisual;
    [SerializeField] private GameObject goldVisual;
    [SerializeField] private GameObject gemVisual;
    [SerializeField] private new Light light;
    [SerializeField] private Color normalLightColor;
    [SerializeField] private Color goldLightColor;
    [SerializeField] private Color gemLightColor;
    [SerializeField] private ParticleSystem particle;
    
    [Header("Animation Settings")]
    [SerializeField] private Ease openEasing;
    [SerializeField] private Ease closeEasing;
    [SerializeField] private float duration;


    private Vector3 ExitPosition => exit.position;
    

    private State state;
    private bool isSpawned;

    
    private IEnumerator Start()
    {
        InstantClose();

        yield return new WaitForSeconds(1f);
        
        Open();

        yield return new WaitForSeconds(1f);
        
        Close();
    }


    public void SetNormalVisual()
    {
        normalVisual.SetActive(true);
        goldVisual.SetActive(false);
        gemVisual.SetActive(false);
        
        light.color = normalLightColor;
        
        var particleMain = particle.main;
        particleMain.startColor = normalLightColor;
        particle.gameObject.SetActive(true);
        particle.Clear();
    }

    public void SetGoldVisual()
    {
        normalVisual.SetActive(false);
        goldVisual.SetActive(true);
        gemVisual.SetActive(false);
        
        light.color = goldLightColor;
        
        var particleMain = particle.main;
        particleMain.startColor = goldLightColor;
        particle.gameObject.SetActive(true);
        particle.Clear();
    }

    public void SetGemVisual()
    {
        normalVisual.SetActive(false);
        goldVisual.SetActive(false);
        gemVisual.SetActive(true);
        
        light.color = gemLightColor;
        
        var particleMain = particle.main;
        particleMain.startColor = gemLightColor;
        particle.gameObject.SetActive(false);
        particle.Clear();
    }
    
    public void OnAllGoalsAchieved()
    {
        transform.position = ExitPosition;
        Open();
    }

    public void OnLevelCompleted()
    {
        Close();
    }

    public void CloseAndGoBackToInitialPosition()
    {
        Close(() =>
        {
            transform.position -= exit.localPosition;
        });
    }

    public PortalHandle GetHandle()
    {
        return handle.GetComponent<PortalHandle>();
    }
    
    
    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void Open()
    {
        if (state == State.Open) return;

        state = State.Open;
        if (LevelSystem.IsPlaying)
        {
            handle.SetActive(true);
        }

        model.DOScale(Vector3.one, duration)
            .SetEase(openEasing)
            .OnComplete(() =>
            {
                if (isSpawned) return;

                isSpawned = true;
                var spawnPosition = transform.position;
                spawnPosition.z = 0f;
                portalSpawned.RaiseEvent(spawnPosition);
            });

        light.DOIntensity(10f, duration)
            .SetEase(openEasing);
        
        particle.Play();
        
        sound.PlayOpening();
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void Close(TweenCallback onCompleted = default)
    {
        if (state == State.Close) return;

        state = State.Close;
        handle.SetActive(false);
        
        model.DOScale(Vector3.zero, duration)
            .SetEase(closeEasing)
            .OnComplete(onCompleted);

        light.DOIntensity(0f, duration)
            .SetEase(closeEasing);
        
        particle.Stop();
    }

    private void InstantClose()
    {
        if (state == State.Close) return;

        state = State.Close;
        handle.SetActive(false);
        
        model.localScale = Vector3.zero;
        light.intensity = 0f;
    }
    
    private enum State
    {
        Open,
        Close
    }
}
