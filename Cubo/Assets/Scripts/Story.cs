using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------- //
    // ---------------------------------------------------- STORY PARAMETERS ----------------------------------------------------- //
    // --------------------------------------------------------------------------------------------------------------------------- //

    [Header("Story parameters")]

    // Activate the story
    public bool storyActivated = false;

    // Activate dialogues
    public bool dialoguesActivated = false;

    // Story chapters
    private enum Advancement { SpawnInDesk, LoadingScene, FirstEnterInCubo, FirstComeBack, ForgotStick, FirstRotateCubo, BreakWall, DistanceGrabTorch, LightTorch, MeltEntrance, Heated, GrabCup, TakeIce, MeltIce, PutOutFire, GrabKey, OpenChest, End}
    private Advancement chapter = Advancement.SpawnInDesk;

    // Keep track of the loop number in each chapter
    private int loopChapter = 0;

    // Repeat the audio clips every X seconds
    public float repeatAudioEveryXSeconds = 60.0f;   

    // Keep track of the time
    private float time = 0.0f;

    // Audio source for dialogues
    private AudioSource audioSource;

    // OVR Screen Fade
    public GameObject centerEyeAnchor;

    // First time teleporting into the cube
    public float firstFadeTime = 15f;
    public float fadeTime = 0.5f;
    

    // --------------------------------------------------------------------------------------------------------------------------- //
    // --------------------------------------------------- INTERCTABLE OBJECTS --------------------------------------------------- //
    // --------------------------------------------------------------------------------------------------------------------------- //

    [Header("Interactable objects")]

    // Player controller
    public PlayerControllerPers playerController;

    // Small cubo
    public GameObject smallCubo;

    // Stick
    public GameObject stick;

    // Wall area 
    public GameObject wallArea;

    // Wall
    public GameObject wall;

    // Torch
    public GameObject torch;

    // Cup
    public GameObject cup;

    // Cup Area
    public GameObject cupArea;

    // Fire
    public GameObject fire;

    // Key
    public GameObject key;

    // Chest
    public GameObject chest;

    // --------------------------------------------------------------------------------------------------------------------------- //
    // -------------------------------------------------------- DIALOGUES  ------------------------------------------------------- //
    // --------------------------------------------------------------------------------------------------------------------------- //

    [Header("Dialogues (Audio clips)")]

    // audio for SpawnInDesk chapter
    public AudioClip audioSpawnInDesk;

    // Voice for inside cubo instruction
    public AudioClip voiceInsideCuboInstruction;

    // Audio for FirstEnterInCubo chapter
    public AudioClip audioFirstEnterInCubo;

    //Voice for Tp instruction
    public AudioClip voiceTpInstruction;

    // Voice for FirstComeBack chapter
    public AudioClip voiceFirstComeBack;

    // Voice for FirstRotateCubo chapter
    public AudioClip voiceFirstRotateCubo;
    public AudioClip voiceFirstRotateCuboForgotStick;

    // Voice for BreakWall chapter
    public AudioClip voiceBreakWall;

    // Voice for DistanceGrabTorch chapter
    public AudioClip voiceDistanceGrabTorch;

    // Voice for LightTorch chapter
    public AudioClip voiceLightTorch;

    // Voice for MeltEntrance chapter
    public AudioClip voiceMeltEntrance;

    // Voice for Heated chapter
    public AudioClip voiceHeated;

    // Voice for GrabCup chapter
    public AudioClip voiceGrabCup;

    // Voice for TakeIce chapter
    public AudioClip voiceTakeIce;

    // Voice for MeltIce chapter
    public AudioClip voiceMeltIce;

    // Voice for PutOutFire chapter
    public AudioClip voicePutOutFire;

    // Voice for GrabKey chapter
    public AudioClip voiceGrabKey;

    // Voice for OpenChest chapter
    public AudioClip voiceOpenChest;

    // Voice for End chapter
    public AudioClip voiceEnd;


    // --------------------------------------------------------------------------------------------------------------------------- //
    // ----------------------------------------------------- START & UPDATE  ----------------------------------------------------- //
    // --------------------------------------------------------------------------------------------------------------------------- //

    // Start is called before the first frame update
    void Start()
    {
        if (!storyActivated) {
            playerController.developerMode = true;
            return;
        }

        // Reset the loop number
        loopChapter = 0;

        // Start timer for audio clips
        time = Time.time;

        // Get the audio source
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!storyActivated) return;

        switch (chapter)
        {
            case Advancement.SpawnInDesk: ChapSpawnInDesk(); break;
            case Advancement.LoadingScene: ChapLoadingScene(); break;
            case Advancement.FirstEnterInCubo: ChapFirstEnterInCubo(); break;
            case Advancement.FirstComeBack: ChapFirstComeBack(); break;
            case Advancement.ForgotStick: ChapForgotStick(); break;
            case Advancement.FirstRotateCubo: ChapFirstRotateCubo(); break;
            case Advancement.BreakWall: ChapBreakWall(); break;
            case Advancement.DistanceGrabTorch: ChapDistanceGrabTorch(); break;
            case Advancement.LightTorch: ChapLightTorch(); break;
            case Advancement.MeltEntrance: ChapMeltEntrance(); break;
            case Advancement.Heated: ChapHeated(); break;
            case Advancement.GrabCup: ChapGrabCup(); break;
            case Advancement.TakeIce: ChapTakeIce(); break;
            case Advancement.MeltIce: ChapMeltIce(); break;
            case Advancement.PutOutFire: ChapPutOutFire(); break;
            case Advancement.GrabKey: ChapGrabKey(); break;
            case Advancement.OpenChest: ChapOpenChest(); break;
            case Advancement.End: ChapEnd(); break;
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------- //
    // -------------------------------------------------------- CHAPTERS  -------------------------------------------------------- //
    // --------------------------------------------------------------------------------------------------------------------------- //

    //------ CHAPTER 1: Spawn in desk (learn how to enter in cubo) ------//
    void ChapSpawnInDesk()
    {
        // The player spawn in the desk for the first time
        // A voice explains the situation (the machine is finally working)
        // A map is exposed on the left wall (the player can see what to do in cubo by looking at the map)
        // The player can see small cubo in front of him
        // Cubo's box is a bit on the right of the player
        // Cubo's rotation is blocked for now (the player will learn to rotate cubo later)
        // What the player can do: look at the map, listen to the voice, grab cubo, move cubo into the box
        // That's all for now, if the player presses the button with cubo in the box, we enter in the next chapter

        if ((loopChapter == 0) && dialoguesActivated) {
            audioSource.clip = audioSpawnInDesk;
            audioSource.Play();
            loopChapter++;
        }
        
        if (!audioSource.isPlaying) {
            audioSource.clip = voiceInsideCuboInstruction;
            if ((loopChapter == 1 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
                audioSource.Play();
            }   
            // Increase the loop number
            loopChapter++;
        }

        if (audioSource.isPlaying) time = Time.time;
         
        centerEyeAnchor.GetComponent<OVRScreenFade>().fadeTime = firstFadeTime;

        // Block cubo's rotation (except for the y axis)
        smallCubo.transform.rotation = Quaternion.Euler(0, smallCubo.transform.rotation.eulerAngles.y, 0);
         
        // If the player tp in cubo, we enter in the next chapter
        if (playerController.isInLoadingScene()) {
            chapter = Advancement.LoadingScene;
            loopChapter = 0;
            audioSource.Stop();
        }
    }

    //------ CHAPTER 1.5: Loading scene (C-U-B-O) ------//
    void ChapLoadingScene()
    { 
        audioSource.clip = audioFirstEnterInCubo;
        if (loopChapter == 0 && dialoguesActivated) {
            audioSource.Play();
        }

        // Increase the loop number
        loopChapter++;

        // Block cubo's rotation (except for the y axis)
        smallCubo.transform.rotation = Quaternion.Euler(0, smallCubo.transform.rotation.eulerAngles.y, 0);

        // If the player comes back in the desk, we enter in the next chapter
        if (!playerController.isInLoadingScene()) {
            chapter = Advancement.FirstEnterInCubo;
            loopChapter = 0;
            time = Time.time;
        }
    }
    

    //------ CHAPTER 2: First enter in cubo (learn how to tp) ------//
    void ChapFirstEnterInCubo()
    {
        // The player is in cubo for the first time and enters in the grass area
        // A voice explains the situation: the player has to grab a stick to break the glass
        // According to the map, the stick is on the top island 
        // The player is told he can tp using the VR controllers (pressing A or X + index trigger)
                
        if (Time.time - time > centerEyeAnchor.GetComponent<OVRScreenFade>().fadeTime) {
            audioSource.clip = voiceTpInstruction;
            centerEyeAnchor.GetComponent<OVRScreenFade>().fadeTime = fadeTime;
            if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
                audioSource.Play();
            }   
            // Increase the loop number
            loopChapter++;
        }

        if (audioSource.isPlaying) time = Time.time;


        // Block cubo's rotation (except for the y axis)
        smallCubo.transform.rotation = Quaternion.Euler(0, smallCubo.transform.rotation.eulerAngles.y, 0);

        // If the player grabs the stick, we enter in the next chapter
        if (stick.GetComponent<ObjectGrabbable>().IsAvailable() == false) {
            chapter = Advancement.FirstComeBack;
            loopChapter = 0;
            audioSource.Stop();
        }
    }

    
    //------ CHAPTER 3: First come back (learn to come back from cubo) ------//
    void ChapFirstComeBack()
    {
        // The player is not back in the desk yet, but holds the stick with index trigger
        // The voice explains that the player can break a wall with the stick
        // He is asked to look around to find the breakable wall in the graveyard area
        // The player can see the breakable wall but cannot access it for now
        // if only the player could rotate cubo...
        // The player is thinking he might come back in the desk scene and has to find a way to do so (respawn terminal)
        
        audioSource.clip = voiceFirstComeBack;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;

        // Increase the loop number
        loopChapter++;

        // Block cubo's rotation (except for the y axis)
        smallCubo.transform.rotation = Quaternion.Euler(0, smallCubo.transform.rotation.eulerAngles.y, 0);

        // If the player comes back in the desk, we enter in the next chapter
        if (playerController.transform.position.x < 10.0f) {
            if (stick.GetComponent<ObjectGrabbable>().IsAvailable()) {
                chapter = Advancement.ForgotStick;
                loopChapter = 0;
                audioSource.Stop();
            } else {
                chapter = Advancement.FirstRotateCubo;
                loopChapter = 0;
                audioSource.Stop();
            }
        }
    }

    //------ CHAPTER 3.5: Forgot the stick ------//
    void ChapForgotStick()
    {
        // The player comes back in the desk without the stick
        // A voice explains that he has forgotten the stick
        // The player has to come back in cubo to grab the stick
        // Once the stick grabbed, the next chapter starts

        audioSource.clip = voiceFirstRotateCuboForgotStick;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;

        // Increase the loop number
        loopChapter++;

        // Block cubo's rotation (except for the y axis)
        smallCubo.transform.rotation = Quaternion.Euler(0, smallCubo.transform.rotation.eulerAngles.y, 0);

        // If the player comes back in cubo, we enter in the next chapter
        if (playerController.transform.position.x < 10.0f
            && !stick.GetComponent<ObjectGrabbable>().IsAvailable()) {
            chapter = Advancement.FirstRotateCubo;
            loopChapter = 0;
            audioSource.Stop();
        }
    }

    //------ CHAPTER 4: First rotate cubo (learn to rotate cubo) ------//
    void ChapFirstRotateCubo()
    {
        // If the player comes back without the stick, a voice explains that he has forgotten the stick
        // If the player comes back with the stick, a voice explains that he has to rotate cubo
        // The player can rotate, and understand how the faces of small cubo are linked to the faces of cubo
        // The player has to rotate cubo to access the graveyard area
        // Once the graveyard accessed, the next chapter starts

        audioSource.clip = voiceFirstRotateCubo;

        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;
        
        // The player can now rotate cubo

        // Increase the loop number
        loopChapter++;
        
        // If the player enters cubo in the gravyard, with the stick, we enter in the next chapter
        if (playerController.transform.position.x > 10.0f 
            && stick.GetComponent<ObjectGrabbable>().IsAvailable() == false
            && smallCubo.GetComponent<SmallCuboBehaviour>().FaceDown() == "Graveyard"
            && wallArea.GetComponent<DialogTrigger>().isInArea()) {
            chapter = Advancement.BreakWall;
            loopChapter = 0;
            audioSource.Stop();
        }
    }

    //------ CHAPTER 5: Break wall ------//
    void ChapBreakWall()
    {
        // The player is in the graveyard area, with the stick
        // A voice explains that the player can break the wall with the stick
        // The player breaks the wall and enters in the next chapter

        audioSource.clip = voiceBreakWall;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;

        // Increase the loop number
        loopChapter++;

        // If the player breaks the wall, we enter in the next chapter (A IMPLEMENTER)
        if (wall.GetComponent<WallBehaviour>().IsDestroyed()) {
            chapter = Advancement.DistanceGrabTorch;
            loopChapter = 0;
            audioSource.Stop();
        }
    }

    //------ CHAPTER 6: Distance grab torch (learn how to distance grab) ------//
    void ChapDistanceGrabTorch()
    {
        // The player just broke the wall and sees the torch on the other side of the fence
        // He cannot access it though
        // A voice explains that the player can distance grab the torch (pressing B or Y + index trigger)
        // The player distance grabs the torch and enters in the next chapter

        audioSource.clip = voiceDistanceGrabTorch;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;

        // Increase the loop number
        loopChapter++;

        // If the player distance grabs the torch, we enter in the next chapter
        if (torch.GetComponent<DistanceGrabbable>().IsAvailable() == false) {
            chapter = Advancement.LightTorch;
            loopChapter = 0;
            audioSource.Stop();

        }
    }

    //------ CHAPTER 7: Light torch ------//
    void ChapLightTorch()
    {
        // The player just grabbe the torch
        // A voice explains that the player can light the torch by approaching it to the fire
        // The player lights the torch and enters in the next chapter

        audioSource.clip = voiceLightTorch;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;

        // Increase the loop number
        loopChapter++;

        // If the player lights the torch, we enter in the next chapter
        if (torch.GetComponent<TorchBehaviour>().isOnFire()) {
            chapter = Advancement.MeltEntrance;
            loopChapter = 0;
            audioSource.Stop();
        }
    }

    //------ CHAPTER 8: Melting the entrance ------//
    void ChapMeltEntrance()
    {
        // The player just lit the torch
        // A voice explains that the player can take back the torch to the desk and melt the ice entrance
        // The player melts the ice entrance and enters in the next chapter

        audioSource.clip = voiceMeltEntrance;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;

        // Increase the loop number
        loopChapter++;

        // If the player melts the ice, we enter in the next chapter
        if (smallCubo.GetComponent<SmallCuboBehaviour>().getIceIsMelted()) {
            chapter = Advancement.Heated;
            loopChapter = 0;
            audioSource.Stop();
        }

    }

    //------ CHAPTER 8.5: Cubo is heated ------//
    void ChapHeated()
    {
        // The player just lit the torch
        // A voice explains that the player can take back the torch to the desk and melt the ice entrance
        // The player melts the ice entrance and enters in the next chapter

        audioSource.clip = voiceHeated;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;

        // Increase the loop number
        loopChapter++;

        // If the player enters in ice, we enter in the next chapter
        if (playerController.transform.position.x > 10.0f 
            && smallCubo.GetComponent<SmallCuboBehaviour>().FaceDown() == "Ice"
            && cupArea.GetComponent<DialogTrigger>().isInArea()) {
            chapter = Advancement.GrabCup;
            loopChapter = 0;
            audioSource.Stop();
        }

    }


    //------ CHAPTER 9: Grabbing the cup ------//
    void ChapGrabCup()
    {
        // The player just melted the ice entrance
        // A voice explains that there might be something to do with the cup
        // The player grabs the cup and enters in the next chapter

        audioSource.clip = voiceGrabCup;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;

        // Increase the loop number
        loopChapter++;

        // If the player grabs the cup, we enter in the next chapter
        if (cup.GetComponent<ObjectGrabbable>().IsAvailable() == false) {
            chapter = Advancement.TakeIce;
            loopChapter = 0;
            audioSource.Stop();
        }

    }

    //------ CHAPTER 10: Taking ice ------//
    void ChapTakeIce()
    {
        // The player grabbed the cup
        // A voice explains that the player can fill it with something
        // The player fills the cup with ice and enters in the next chapter

        audioSource.clip = voiceTakeIce;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;

        // Increase the loop number
        loopChapter++;

        // refer to the child0
        if (cup.transform.GetChild(0).GetComponent<CupBehaviour>().fullCup()) {
            chapter = Advancement.MeltIce;
            loopChapter = 0;
            audioSource.Stop();
        }

    }

    //------ CHAPTER 11: Melting the ice ------//
    void ChapMeltIce()
    {
        // The player took ice in the cup
        // A voice explains that the player could also melt the ice to make water
        // The player melts ice and enters the next chapter

        audioSource.clip = voiceMeltIce;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;

        // Increase the loop number
        loopChapter++;

        // If the player melts ice, we enter in the next chapter
        if (cup.transform.GetChild(0).GetComponent<CupBehaviour>().isWater()) {
            chapter = Advancement.PutOutFire;
            loopChapter = 0;
            audioSource.Stop();
        }

    }

    //------ CHAPTER 12: Putting out the fire ------//
    void ChapPutOutFire()
    {
        // The player melted the ice
        // A voice explains that the player can put out the fire with the water
        // The player puts out the fire and enters the next chapter

        audioSource.clip = voicePutOutFire;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;

        // Increase the loop number
        loopChapter++;

        // If the player puts out the fire, we enter in the next chapter
        if (fire.GetComponent<FireBehaviour>().isOnFire() == false) {
            chapter = Advancement.GrabKey;
            loopChapter = 0;
            audioSource.Stop();
        }

    }

    //------ CHAPTER 13: Grabbing the key ------//
    void ChapGrabKey()
    {
        // The player put out the fire
        // A voice explains that the player can grab the key
        // The player grabs the key and enters the next chapter

        audioSource.clip = voiceGrabKey;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;

        // Increase the loop number
        loopChapter++;

        // If the player grabs the key, we enter in the next chapter
        if (key.GetComponent<ObjectGrabbable>().IsAvailable() == false) {
            chapter = Advancement.OpenChest;
            loopChapter = 0;
            audioSource.Stop();
        }

    }

    //------ CHAPTER 14: Opening the chest ------//
    void ChapOpenChest()
    {
        // The player grabbed the key
        // A voice explains that the player can open the chest
        // The player opens the chest and enters the next chapter

        audioSource.clip = voiceOpenChest;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        if (audioSource.isPlaying) time = Time.time;

        // Increase the loop number
        loopChapter++;

        // If the player opens the chest, we enter in the next chapter
        if (chest.GetComponent<ChestBehaviour>().isOpen()) {
            chapter = Advancement.End;
            loopChapter = 0;
            audioSource.Stop();
        }

    }

    //------ CHAPTER 15: End ------//
    void ChapEnd()
    {
        // The player opened the chest
        // it is over
       

        audioSource.clip = voiceEnd;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
        }

        // Increase the loop number
        loopChapter++;

        // If the player opens the chest, we enter in the next chapter

    }
}