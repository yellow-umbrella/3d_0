using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    
    private AudioSource audioSource;
    private float warningSoundTimer;
    private bool playWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Frying 
            || e.state == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play();
            audioSource.volume = SoundManager.Instance.GetVolume();
        } else
        {
            audioSource.Pause();
        }

        playWarningSound = e.state == StoveCounter.State.Fried;
    }

    private void Update()
    {
        if (playWarningSound)
        {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer <= 0f)
            {
                float timerMax = .2f;
                warningSoundTimer = timerMax;

                SoundManager.Instance.PlayWarningSound(transform.position);
            }
        }
            
    }
}
