using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{
    // Activate the story
    public bool storyActivated = false;

    // Story chapters
    private enum Advancement { SpawnInDesk, FirstEnterInCubo, FirstComeBack, FirstRotateCubo, BreakWall, DistanceGrabTorch, LightTorch }
    private Advancement chapter = Advancement.SpawnInDesk;

    // Keep track of the loop number in each chapter
    private int loopChapter = 0;

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

    // Start is called before the first frame update
    void Start()
    {
        if (!storyActivated) return;

        // Reset the loop number
        loopChapter = 0;
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

        if (loopChapter == 0) {
            //! Voice here (TODO) to explain the situation
        }

        // Block cubo's rotation (except for the y axis)
        smallCubo.transform.rotation = Quaternion.Euler(0, smallCubo.transform.rotation.eulerAngles.y, 0);

        // Increase the loop number
        loopChapter++;
    
        // If the player tp in cubo, we enter in the next chapter
        if (playerController.transform.position.x > 10.0f) {
            chapter = Advancement.FirstEnterInCubo;
            loopChapter = 0;
        }
    }

    //------ CHAPTER 2: First enter in cubo (learn how to tp) ------//
    void ChapFirstEnterInCubo()
    {
        // The player is in cubo for the first time and enters in the grass area
        // A voice explains the situation: the player has to grab a stick to break the glass
        // According to the map, the stick is on the top island 
        // The player is told he can tp using the VR controllers (pressing A or X + index trigger)

        if (loopChapter == 0) {
            //! Voice here (TODO) to explain the situation
        }

        // Increase the loop number
        loopChapter++;

        // If the player grabs the stick, we enter in the next chapter
        if (stick.GetComponent<ObjectGrabbable>().IsAvailable() == false) {
            chapter = Advancement.FirstComeBack;
            loopChapter = 0;
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
        

        if (loopChapter == 0) {
            //! Voice here (TODO) to explain the situation
        }

        // Increase the loop number
        loopChapter++;

        // If the player comes back in the desk, we enter in the next chapter
        if (playerController.transform.position.x < 10.0f) {
            chapter = Advancement.FirstRotateCubo;
            loopChapter = 0;
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

        if (loopChapter == 0) {
            if (stick.GetComponent<ObjectGrabbable>().IsAvailable()) {
                //! Voice here (TODO) to explain to come back with the stick
            }
            else {
                //! Voice here (TODO) to explain that the player has to rotate cubo
            }
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
        }
    }

    //------ CHAPTER 5: Break wall ------//
    void ChapBreakWall()
    {
        // The player is in the graveyard area, with the stick
        // A voice explains that the player can break the wall with the stick
        // The player breaks the wall and enters in the next chapter

        if (loopChapter == 0) {
            //! Voice here (TODO) to explain that the player can break the wall
        }

        // Increase the loop number
        loopChapter++;

        // If the player breaks the wall, we enter in the next chapter (A IMPLEMENTER)
        // if (wall.GetComponent<WallBehaviour>().IsBroken()) {
        //     chapter = Advancement.DistanceGrabTorch;
        //     loopChapter = 0;
        // }
    }

    //------ CHAPTER 6: Distance grab torch (learn how to distance grab) ------//
    void ChapDistanceGrabTorch()
    {
        // The player just broke the wall and sees the torch on the other side of the fence
        // He cannot access it though
        // A voice explains that the player can distance grab the torch (pressing B or Y + index trigger)
        // The player distance grabs the torch and enters in the next chapter

        if (loopChapter == 0) {
            //! Voice here (TODO) to explain that the player can distance grab the torch
        }

        // Increase the loop number
        loopChapter++;

        // If the player distance grabs the torch, we enter in the next chapter
        if (torch.GetComponent<DistanceGrabbable>().IsAvailable() == false) {
            chapter = Advancement.LightTorch;
            loopChapter = 0;
        }
    }

    //------ CHAPTER 7: Light torch ------//
    void ChapLightTorch()
    {
        //Todo
    }
}
