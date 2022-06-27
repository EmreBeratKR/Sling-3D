using System.Collections;
using DG.Tweening;
using NaughtyAttributes;
using ScriptableEvents.Core.Channels;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Event Channels")]
    [SerializeField] private Vector3EventChannel portalSpawned;

    [Header("References")] 
    [SerializeField] private Transform model;
    [SerializeField] private GameObject handle;
    [SerializeField] private Transform exit;
    
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


    public void OnAllGoalsAchieved()
    {
        transform.position = ExitPosition;
        Open();
    }

    public void OnLevelCompleted()
    {
        Close();
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
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void Close()
    {
        if (state == State.Close) return;

        state = State.Close;
        handle.SetActive(false);
        
        model.DOScale(Vector3.zero, duration)
            .SetEase(closeEasing);
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
