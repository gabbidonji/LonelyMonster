using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FootstepAudio : MonoBehaviour
{
    private AudioSource audioSource;
    public List<AudioClip> steps;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void Footstep(){
        AudioClip step = steps[Random.Range(0,steps.Count)];
        audioSource.PlayOneShot(step);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
