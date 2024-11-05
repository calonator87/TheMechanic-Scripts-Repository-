using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clockSound;

    public float totalTime = 180f;
    private float currentTime;

    public Slider slider;

    void Start()
    {
        currentTime = totalTime;
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.clip = clockSound;
        }
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        slider.value = currentTime / totalTime;

        if (currentTime > 0)
        {
            audioSource.Play();
        }

        if (currentTime <= 0)
        {
            Debug.Log("Time's up");
            currentTime = 0;
            SceneManager.LoadScene("GAMEOVER");
        }
    }
}