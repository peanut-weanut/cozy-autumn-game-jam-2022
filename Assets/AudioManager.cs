using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip[] DrawSounds;
    public AudioSource[] source;
    public AudioClip[] songPack0;
    public AudioClip[] songPack1;
    public AudioClip[] songPack2;
    public AudioClip[] songPack3;
    public AudioClip[] songPack4;

    public AudioListener listener;
    public int sourceID;
    public int soundCount, maxSoundCount;
    private int nextSongIndex = 0;
    private int currentSongPackIndex = 0;
    private AudioClip[] currentSongPack;
    private float songTimer = 100.0f;
    public InputSystem controls;
    public AudioClip[] paperSounds;
    private int currentPaperSound;
    private void Awake()
    {
        controls = new InputSystem();
    }
    void Start()
    {
        source = GetComponents<AudioSource>();
        GameManager.game.drawLines.OnDraw += PlayDrawSound;
        PlayBGM();
        songTimer -= 3.0f;
        GameManager.game.camControls.POISeen += POISongIndex;
        controls.inputs.Submit.performed += ctx => AdvanceSongIndex();
        controls.CameraStates.ChangeView.performed += ctx => PlayPaperSound();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        songTimer -= Time.deltaTime;
        if (songTimer < 0){
            PlayBGM();
            if(nextSongIndex == 2)
                ResetSongIndex();
        }
        // if (TestCrossFade == 1){
        //     CrossfadeSources(5, 6);
        // } else if (TestCrossFade == 2){
        //     CrossfadeSources(6, 5);
        // }
    }
    void PlayPaperSound(){
        source[0].PlayOneShot(paperSounds[currentPaperSound]);
        if(currentPaperSound == 0)
            currentPaperSound = 1;
        else
            currentPaperSound = 0;
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
    // private int TestCrossFade = 0;
    // [Range(0.01f, 0.5f)]
    // public float crossfadeAmount;
    void POISongIndex(){
        nextSongIndex = 1;
        // TestCrossFade = 1;
    }
    void AdvanceSongIndex(){
        switch(nextSongIndex){
            case 1:
                nextSongIndex = 2;
            break;
            // case 2:
            //     nextSongIndex = 0; we dont want this to happen when you submit lol
            // break;
        }
    }
    void ResetSongIndex(){
        nextSongIndex = 0;
        currentSongPackIndex++; // DONT SET THIS HERE IN FINAL GAME, SET IT IN THE DIALOGUE
        // TestCrossFade = 2;
    }
    void DisableSongIndex(){
        nextSongIndex = 3;
        
    }
    // void CrossfadeSources(int sourceFrom, int sourceTo){
    //     source[sourceFrom].volume = Mathf.Lerp(source[sourceFrom].volume, 0.0f, crossfadeAmount);
    //     source[sourceTo].volume = Mathf.Lerp(source[sourceTo].volume, 1.0f, crossfadeAmount);
    //     if(source[sourceTo].volume == 1.0f){
    //         TestCrossFade = 0;
    //     }
    // }
    void PlayBGM(){
        //play two songs on different tracks (5, 6), when you get the signal, crossfade between them.
        switch(currentSongPackIndex){
            case 0:
                currentSongPack = songPack0;
            break;
            case 1:
                currentSongPack = songPack1;
            break;
            case 2:
                currentSongPack = songPack2;
            break;
            case 3:
                currentSongPack = songPack3;
            break;
            case 4:
                currentSongPack = songPack4;
            break;
        }
        songTimer = currentSongPack[nextSongIndex].length;
        switch(nextSongIndex){
        case 0:
            source[5].clip = currentSongPack[0];
            source[5].volume = 1;
            source[5].Play();
            source[6].volume = 0;
            source[6].clip = currentSongPack[1];
            source[6].Play();
        break;
        case 1:
            source[5].clip = currentSongPack[0];
            source[5].volume = 0;
            source[5].Play();
            source[6].volume = 1;
            source[6].clip = currentSongPack[1];
            source[6].Play();
        break;
        case 2:
            source[5].clip = currentSongPack[2];
            source[5].volume = 1;
            source[5].Play();
            source[6].volume = 0;
            source[6].clip = currentSongPack[1];
            source[6].Play();
        break;
        case 3:
            source[5].clip = currentSongPack[0];
            source[5].volume = 0;
            source[5].Play();
            source[6].volume = 0;
            source[6].clip = currentSongPack[1];
            source[6].Play();
        break;
        }
    }
}
