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
    private enum Advancement { SpawnInDesk, FirstEnterInCubo, FirstComeBack, FirstRotateCubo, BreakWall, DistanceGrabTorch, LightTorch }
    private Advancement chapter = Advancement.SpawnInDesk;

    // Keep track of the loop number in each chapter
    private int loopChapter = 0;

    // Repeat the audio clips every X seconds
    public float repeatAudioEveryXSeconds = 60.0f;   

    // Keep track of the time
    private float time = 0.0f;

    // Audio source for dialogues
    private AudioSource audioSource;

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

    // Wall
    public GameObject wall;

    // Torch
    public GameObject torch;

    // --------------------------------------------------------------------------------------------------------------------------- //
    // -------------------------------------------------------- DIALOGUES  ------------------------------------------------------- //
    // --------------------------------------------------------------------------------------------------------------------------- //

    [Header("Dialogues (Audio clips)")]

    // Voice for SpawnInDesk chapter
    public AudioClip voiceSpawnInDesk;

    // Voice for FirstEnterInCubo chapter
    public AudioClip voiceFirstEnterInCubo;

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



    // --------------------------------------------------------------------------------------------------------------------------- //
    // ----------------------------------------------------- START & UPDATE  ----------------------------------------------------- //
    // --------------------------------------------------------------------------------------------------------------------------- //

    // Start is called before the first frame update
    void Start()
    {
        if (!storyActivated) return;

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
            case Advancement.FirstEnterInCubo: ChapFirstEnterInCubo(); break;
            case Advancement.FirstComeBack: ChapFirstComeBack(); break;
            case Advancement.FirstRotateCubo: ChapFirstRotateCubo(); break;
            case Advancement.BreakWall: ChapBreakWall(); break;
            case Advancement.DistanceGrabTorch: ChapDistanceGrabTorch(); break;
            case Advancement.LightTorch: ChapLightTorch(); break;
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

        audioSource.clip = voiceSpawnInDesk;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
            time = Time.time;
        }

        // Block cubo's rotation (except for the y axis)
        smallCubo.transform.rotation = Quaternion.Euler(0, smallCubo.transform.rotation.eulerAngles.y, 0);

        // Increase the loop number
        loopChapter++;
    
        // If the player tp in cubo, we enter in the next chapter
        if (playerController.transform.position.x > 10.0f) {
            chapter = Advancement.FirstEnterInCubo;
            loopChapter = 0;
            audioSource.Stop();
        }
    }

    //------ CHAPTER 2: First enter in cubo (learn how to tp) ------//
    void ChapFirstEnterInCubo()
    {
        // The player is in cubo for the first time and enters in the grass area
        // A voice explains the situation: the player has to grab a stick to break the glass
        // According to the map, the stick is on the top island 
        // The player is told he can tp using the VR controllers (pressing A or X + index trigger)

        audioSource.clip = voiceFirstEnterInCubo;
        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
            time = Time.time;
        }        

        // Increase the loop number
        loopChapter++;

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
            time = Time.time;
        }

        // Increase the loop number
        loopChapter++;

        // If the player comes back in the desk, we enter in the next chapter
        if (playerController.transform.position.x < 10.0f) {
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

        if (stick.GetComponent<ObjectGrabbable>().IsAvailable() == false) audioSource.clip = voiceFirstRotateCuboForgotStick;
        else audioSource.clip = voiceFirstRotateCubo;

        if ((loopChapter == 0 || Time.time - time > repeatAudioEveryXSeconds) && dialoguesActivated) {
            audioSource.Play();
            time = Time.time;
        }
        
        // The player can now rotate cubo
        smallCubo.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        // Increase the loop number
        loopChapter++;
        
        // If the player enters cubo in the gravyard, with the stick, we enter in the next chapter
        if (playerController.transform.position.x > 10.0f 
            && stick.GetComponent<ObjectGrabbable>().IsAvailable() == false
            && smallCubo.GetComponent<SmallCuboBehaviour>().FaceDown() == "Graveyard") {
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
            time = Time.time;
        }

        // Increase the loop number
        loopChapter++;

        // If the player breaks the wall, we enter in the next chapter (A IMPLEMENTER)
        // if (wall.GetComponent<WallBehaviour>().IsBroken()) {
        //     chapter = Advancement.DistanceGrabTorch;
        //     loopChapter = 0;
        //     audioSource.Stop();

        // }
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
            time = Time.time;
        }

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
        //Todo
    }
}
