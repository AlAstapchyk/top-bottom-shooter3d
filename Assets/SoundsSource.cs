using UnityEngine;

public class SoundsSource : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip buttonClick;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        source= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonSound()
    {
        //Debug.Log("Playing button sound");
        source.clip = buttonClick;
        source.Play();
    }
    
    public void DeathSound(AudioClip deathClip)
    {
        Debug.Log(deathClip.name);
        source.clip = deathClip;
        source.Play();
    }
}
