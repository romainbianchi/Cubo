using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : CollectibleItem
{

    // Trigger the action on the player : Tell the player to acquire the blue grasp
    public override void interacted_with(MainPlayerController player)
    {
        player.aquire_item(this);
        
        // The player has acquired the blue grasp
        base.self_collected_by(player);
    }
}
