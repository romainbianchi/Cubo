using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectibleItem : InteractiveItem
{

    // The idea of object we can collect is that the default interaction is the collection
    // by the player of the object followed by the destruction of the item in the scene.

    public override void interacted_with(MainPlayerController player)
    {
        self_collected_by(player);
    }

    public void self_collected_by(MainPlayerController player)
    {
        Destroy(gameObject);
    }
    
}
