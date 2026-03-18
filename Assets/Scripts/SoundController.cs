using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    [SerializeField] private AudioSource diceSound;
    [SerializeField] private AudioSource moveSound;
    [SerializeField] private AudioSource itemObtainSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    public void PlayDiceSound()
    {
      //  diceSound.pitch = (Random.Range(0.9f, 1.1f));
        diceSound.Play();
    }

    public void PlayMoveSound()
    {
        moveSound.pitch = (Random.Range(0.9f, 1.1f));
        moveSound.Play();
    }

    public void PlayItemObtainedSound()
    {
        itemObtainSound.Play();
    }
}
