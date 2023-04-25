using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{

    protected List<Type> list_of_player_upgrades = new List<Type>();
    // We use type instead of Upgrade because we want to be able to store any type of upgrade

    public void aquire_item(CollectibleItem item)
    {
        // Check if the item is not already in the list of upgrades
        if(is_equiped_with(item.GetType())) {
            // If it is, we do nothing
            return;
        }

        // We add the item to the list of upgrades
        list_of_player_upgrades.Add(item.GetType());
    }

    public bool is_equiped_with(Type item_type)
    {
        // Check if the item is not already in the list of upgrades
        foreach(Type upgrade in list_of_player_upgrades) {
            if(upgrade == item_type) {
                return true;
            }
        }
        return false;
    }

    void OnTriggerEnter(Collider other) {
        // Check if the object is an interactive item
        InteractiveItem item = other.GetComponent<InteractiveItem>();
        if(item != null) {
            // If it is, we interact with it
            item.interacted_with(this);
        }
    }
}
