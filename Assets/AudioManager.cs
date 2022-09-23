using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

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
    public int currentSongPackIndex = 0;
    public AudioClip[] currentSongPack;
    private float songTimer = 100.0f;
    public InputSystem controls;
    public AudioClip[] paperSounds;
    private int currentPaperSound;
    public DialogueRunner dialogueRunner;
    private void Awake()
    {
        controls = new InputSystem();
        dialogueRunner.AddCommandHandler("StartMusic", StartMusic);
        dialogueRunner.AddCommandHandler("ResetMusic", ResetMusic);
    }
    void Start()
    {
        source = GetComponents<AudioSource>();
        GameManager.game.drawLines.OnDraw += PlayDrawSound;
        // PlayBGM();
        songTimer -= 3.0f;
        GameManager.game.camControls.POISeen += POISongIndex;
        GameManager.game.OnStoryTold += AdvanceSongIndex; // instead of audio taking queues from input, take it from dialogue
        //GameManager.game.dialogueRunner.OnDialogueBegin += EndingSongIndex;
        //GameManager.game.dialogueRunner.OnDialogueEnd += AmbientSongIndex;
        controls.CameraStates.ChangeView.performed += ctx => PlayPaperSound();
       
        
    }
    void StartMusic(){
        Debug.Log("Start music executed.");
        PlayBGM();
        nextSongIndex = 0;
    }
    void ResetMusic(){
        nextSongIndex = 0;
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    // [YarnCommand("StartMusic")]
    
    // Update is called once per frame
    bool playOnceCheck = true;
    void Update()
    {
        songTimer -= Time.deltaTime;
        if (songTimer < 0)
            PlayBGM();
        if (playText)
            PlayTextNoise();
                      
        
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
        Debug.Log("Set song index to 1.");
        // TestCrossFade = 1;
    }
    
    void AdvanceSongIndex(){
        switch(nextSongIndex){
            case 1:
                nextSongIndex = 2;
                Debug.Log("Set song index to 2.");
            break;
            // case 2:
            //     nextSongIndex = 0; we dont want this to happen when you submit lol
            // break;
        }
    }
    [YarnCommand("AdvanceMusic")]
    void ResetSongIndex(){
        currentSongPackIndex++; // DONT SET THIS HERE IN FINAL GAME, SET IT IN THE DIALOGUE
        nextSongIndex = 0;
        Debug.Log("Reset song index and advanced song pack.");
    }
    public bool isItMe = false;
    public AudioClip[] textingSounds;
    public bool playText = false;
    void PlayTextNoise(){
        switch(isItMe){
            case true:
                source[4].PlayOneShot(textingSounds[0]);
            break;
            case false:
                source[4].PlayOneShot(textingSounds[1]);
            break;
        }
        playText = false;
    }
    // void DisableSongIndex(){
    //     nextSongIndex = 3;
        
    // }
    // void CrossfadeSources(int sourceFrom, int sourceTo){
    //     source[sourceFrom].volume = Mathf.Lerp(source[sourceFrom].volume, 0.0f, crossfadeAmount);
    //     source[sourceTo].volume = Mathf.Lerp(source[sourceTo].volume, 1.0f, crossfadeAmount);
    //     if(source[sourceTo].volume == 1.0f){
    //         TestCrossFade = 0;
    //     }
    // }
    [YarnCommand("PlayMusic")]
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
        if (nextSongIndex != 3)
            songTimer = currentSongPack[nextSongIndex].length;
        else
            songTimer = 10.0f;
        
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
            nextSongIndex = 3;
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
        Debug.Log("Played BGM: The current song pack is " + currentSongPackIndex + ", the current song timer is " + songTimer + ", and the current song index is" + nextSongIndex + ".");
    }
}
