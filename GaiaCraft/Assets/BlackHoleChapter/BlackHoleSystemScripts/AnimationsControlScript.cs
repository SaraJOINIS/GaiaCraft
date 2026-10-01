using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class WholeBlackHoleSystemAnimation : MonoBehaviour
{
    // Animators des objets présents dans la scène
    public Animator collapsingStarAnimator;
    public Animator gravityFieldAnimator;
    public Animator swallowedObjectAnimator;
    public Animator slingshottedObjectAnimator;
    public Animator orbitingObjectAnimator;

    // Audio sources
    private AudioSource collapsingStarAudioSource;
    private AudioSource swallowedObjectAudioSource;
    private AudioSource slingshottedObjectAudioSource;
    private AudioSource orbitingObjectAudioSource;

    // Pour que le toggle définissant le sens de lecture revienne à l'état "Forward" lorsqu'on clique sur le bouton "Reset"
    public ReadDirectionScript readDirectionScript;

    // Booléens pour les boutons accessibles par l'utilisateur
    private bool IsPlaying = true;
    private bool IsForward = true;

    public bool GetIsPlaying() { return IsPlaying; }
    public bool GetIsForward() { return IsForward; }

    private float currentSpeed = 0.4f;

    void Start()
    {
        SetSpeed(currentSpeed);

        // On assigne chaque source audio à chaque animation
        collapsingStarAudioSource = collapsingStarAnimator.GetComponentInParent<AudioSource>();
        collapsingStarAudioSource.Play();

        swallowedObjectAudioSource = swallowedObjectAnimator.GetComponentInParent<AudioSource>();
        swallowedObjectAudioSource.Play();

        slingshottedObjectAudioSource = slingshottedObjectAnimator.GetComponentInParent<AudioSource>();
        slingshottedObjectAudioSource.Play();

        orbitingObjectAudioSource = orbitingObjectAnimator.GetComponentInParent<AudioSource>();
        orbitingObjectAudioSource.Play();
    }

    public void TogglePlayPause()
    {
        // Debug.Log("Play-Pause toggle called");

        if (IsPlaying)
            Pause();
        else
            Play();
    }

    public void Play()
    {
        // Debug.Log("Play event called");

        if(IsPlaying)
            return;

        IsPlaying = true;
        SetSpeed(currentSpeed);

        if(IsForward)
            PlayAudio();
    }

    public void Pause()
    {
        // Debug.Log("Pause event called");

        if(!IsPlaying)
            return;

        IsPlaying = false;
        SetSpeed(0);

        if(IsForward)
            PauseAudio();
    }

    public void ToggleReadDirection()
    {
        // Debug.Log("Read Direction toggle called");
        
        if (IsForward)
            Backward();
        else
            Forward();
    }

    public void Forward()
    {
        // Debug.Log("Forward event called");

        if (IsForward)
            return;

        IsForward = true;
        currentSpeed = 0.4f;

        if (IsPlaying)
            SetSpeed(currentSpeed);
    }

    public void Backward()
    {
        // Debug.Log("Backward event called");

        if (!IsForward)
            return;

        IsForward = false;
        currentSpeed = -0.7f; // Pour jouer l'animation dans le sens inverse

        if (IsPlaying)
            SetSpeed(currentSpeed);

        // On arrête tous les audios
        collapsingStarAudioSource.Stop();
        swallowedObjectAudioSource.Stop();
        slingshottedObjectAudioSource.Stop();
        orbitingObjectAudioSource.Stop();
    }

    public void Reset()
    {
        // Debug.Log("Reset event called");

        readDirectionScript.ResetToggle(); // Pour réinitialiser le toggle de sens de lecture

        // On reset les audios
        collapsingStarAudioSource.Stop();
        swallowedObjectAudioSource.Stop();
        slingshottedObjectAudioSource.Stop();
        orbitingObjectAudioSource.Stop();

        // Pour revenir au début de l'animation
        collapsingStarAnimator.Play("CollapsingStarAnimation", -1, 0 /*temps du début de l'animation*/);
        gravityFieldAnimator.Play("GravityFieldAnimation", -1, 0);
        swallowedObjectAnimator.Play("SwallowedObjectAnimation", -1, 0);
        slingshottedObjectAnimator.Play("SlingshottedObjectAnimation", -1, 0);
        orbitingObjectAnimator.Play("OrbitingObjectAnimation", -1, 0);

        // Pour remettre les booléens à l'état initial
        IsForward = true;
        currentSpeed = 0.4f;

        if (IsPlaying)
            Play();
        else
            Pause();
    }

    private void SetSpeed(float speed)
    {
        collapsingStarAnimator.SetFloat("speed", speed);
        gravityFieldAnimator.SetFloat("speed", speed);
        swallowedObjectAnimator.SetFloat("speed", speed);
        slingshottedObjectAnimator.SetFloat("speed", speed);
        orbitingObjectAnimator.SetFloat("speed", speed);
    }

    private void PlayAudio()
    {
        if (collapsingStarAudioSource != null)
        {
            if (collapsingStarAudioSource.isPlaying)
                collapsingStarAudioSource.UnPause();
            else
            {
                if (!collapsingStarAudioSource.enabled)
                    collapsingStarAudioSource.enabled = true;
                collapsingStarAudioSource.Play();

            }
        }

        if (swallowedObjectAudioSource != null)
        {
            if (swallowedObjectAudioSource.isPlaying)
                swallowedObjectAudioSource.UnPause();
            else
            {
                if (!swallowedObjectAudioSource.enabled)
                    swallowedObjectAudioSource.enabled = true;
                swallowedObjectAudioSource.Play();

            }
        }

        if (slingshottedObjectAudioSource != null)
        {
            if (slingshottedObjectAudioSource.isPlaying)
                slingshottedObjectAudioSource.UnPause();
            else
            {
                if (!slingshottedObjectAudioSource.enabled)
                    slingshottedObjectAudioSource.enabled = true;
                slingshottedObjectAudioSource.Play();

            }
        }

        if (orbitingObjectAudioSource != null)
        {
            if (orbitingObjectAudioSource.isPlaying)
                orbitingObjectAudioSource.UnPause();
            else
            {
                if (!orbitingObjectAudioSource.enabled)
                    orbitingObjectAudioSource.enabled = true;
                orbitingObjectAudioSource.Play();

            }
        }
    }

    private void PauseAudio()
    {
        if (collapsingStarAudioSource != null)
            collapsingStarAudioSource.Pause();

        if (swallowedObjectAudioSource != null)
            swallowedObjectAudioSource.Pause();

        if (slingshottedObjectAudioSource != null)
            slingshottedObjectAudioSource.Pause();

        if (orbitingObjectAudioSource != null)
            orbitingObjectAudioSource.Pause();
    }
}