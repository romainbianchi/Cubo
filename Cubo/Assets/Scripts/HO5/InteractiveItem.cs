using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveItem : MonoBehaviour
{
    
    public abstract void interacted_with(MainPlayerController player);
    // N.B. : It might be really interesting to add an additional action such playing an audio sound
    // that would be also stored in this abstract class. This would automatically make every interactive
    // items to produce a sound each time the player touches it !

}
