using System.Collections;
using DG.Tweening;
using Handle_System;
using NaughtyAttributes;
using ScriptableEvents.Core.Channels;
using SoundSystem;
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
    }

    public void SetGoldVisual()
    {
        normalVisual.SetActive(false);
        goldVisual.SetActive(true);
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
    }

    private void InstantClose()
    {
        if (state == State.Close) return;

        state = State.Close;
        handle.SetActive(false);
        
        model.localScale = Vector3.zero;
    }
    
    private enum State
    {
        Open,
        Close
    }
}
