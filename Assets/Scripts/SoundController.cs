using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioClip flare;
    public AudioClip walk;
    public AudioClip scream;
    public AudioClip music;
    public AudioClip pickup;
    public AudioClip putin;
    public AudioClip musicDark;

    public AudioSource sesource;
    public AudioSource musicsource;
    public AudioSource walkLoop;

    public enum SE { FLARE, SCREAM, PU, PD };


    private void Awake()
    {
        if(sesource == null)
        {
            Debug.LogError("no Sound Effect source!");
            
        }
        if (musicsource == null)
        {
            Debug.LogError("no Music source!");

        }
        if (walkLoop == null)
        {
            Debug.LogError("no Walk source!");
        }

        // Some setup
        musicsource.loop = true;
        walkLoop.loop = true;
        walkLoop.clip = walk;

        // play the music
        musicsource.clip = music;
        musicsource.Play();

    }

    public void playSoundeffect(SE s)
    {
        switch (s)
        {
            case SE.FLARE:
                sesource.PlayOneShot(flare);
                break;
            case SE.SCREAM:
                sesource.PlayOneShot(scream);
                break;
            case SE.PU:
                sesource.PlayOneShot(pickup);
                break;
            case SE.PD:
                sesource.PlayOneShot(putin);
                break;
            default:
                Debug.LogWarning("unknown sound effect" + s);
                break;
        }
    }

    public void setWalkingLoop(bool on)
    {
        if((on && walkLoop.isPlaying) || (!on && !walkLoop.isPlaying))
        {
            // do nothing
            return;
        }

        if (on)
        {
            walkLoop.Play();
        }
        else
        {
            walkLoop.Stop();
        }
    }

    public void stopMusic()
    {
        musicsource.Stop();
    }

    public void playDarkMusic()
    {
        musicsource.clip = musicDark;
        musicsource.Play();
    }
}
