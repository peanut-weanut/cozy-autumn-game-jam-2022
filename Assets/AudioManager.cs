using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip[] DrawSounds;
    public AudioSource[] source;
    public AudioListener listener;
    public int sourceID;
    public int soundCount, maxSoundCount;
    void Start()
    {
        source = GetComponents<AudioSource>();
        GameManager.game.drawLines.OnDraw += PlayDrawSound;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void PlayDrawSound(){
        if (soundCount > maxSoundCount){
            int soundPlay = (int)Random.Range(0, DrawSounds.Length-1);
            source[sourceID].PlayOneShot(DrawSounds[soundPlay]);
            soundCount = 0;
        } else{
            soundCount++;
        }
    }
}
