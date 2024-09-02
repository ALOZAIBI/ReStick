using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface RewarderInterface 
{
    //Sets the objects active
    public void showRewards();

    //Sets the objects inactive
    public void hideRewards();
    public void setUpRewards();

    //Adds to inventory in case of ability/items will apply to a character if it's a heal will add to gold if gold etc..
    public void receiveReward();
}
