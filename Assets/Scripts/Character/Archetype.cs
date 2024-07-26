using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public static class Archetype{
    public enum ArchetypeList {
        None,
        MageRanged,
        MageMelee,
        Tank,
        Support,
        Assassin,
        Summoner,
        Fighter,
        Ranger
    }

    //Change the look of character depending on items and abilities(Abilities Influence More)
    public static void applyArchetypeLook(this Character character) {
        //Number of archetypes
        int numOfArch = 9;
        //A 2D array, first index holds the number of votes, second index holds the characterIndex from the characterFactory
        int[,] archetypeVotes = new int[numOfArch, 2];
        archetypeVotes[(int)Archetype.ArchetypeList.None, 1] = 0;
        archetypeVotes[(int)Archetype.ArchetypeList.MageRanged, 1] = 1;
        archetypeVotes[(int)Archetype.ArchetypeList.MageMelee, 1] = 1;
        archetypeVotes[(int)Archetype.ArchetypeList.Tank, 1] = 2;
        archetypeVotes[(int)Archetype.ArchetypeList.Support, 1] = 3;
        archetypeVotes[(int)Archetype.ArchetypeList.Assassin, 1] = 0;
        archetypeVotes[(int)Archetype.ArchetypeList.Summoner, 1] = 0;
        archetypeVotes[(int)Archetype.ArchetypeList.Fighter, 1] = 2;
        archetypeVotes[(int)Archetype.ArchetypeList.Ranger, 1] = 4;


        //Collect the votes from items and also abilities
        foreach (Item i in character.items) {
            //If it is None Increment Everything 
            if (i.archetypePrimary == Archetype.ArchetypeList.None) {
                //Increment all the votes
                for (int j = 0; j < numOfArch; j++) {
                    archetypeVotes[j, 0]++;
                }
            }
            else {
                archetypeVotes[(int)i.archetypePrimary, 0]++;
                //If there is a secondary archetype, increment that as well
                if (i.archetypeSecondary != Archetype.ArchetypeList.None) {
                    archetypeVotes[(int)i.archetypeSecondary, 0]++;
                }
            }
        }

        foreach (Ability a in character.abilities) {
            //If it is None Increment Everything 
            if (a.archetypePrimary == Archetype.ArchetypeList.None) {
                //Increment all the votes
                for (int j = 0; j < numOfArch; j++) {
                    archetypeVotes[j, 0]++;
                }
            }
            else {
                archetypeVotes[(int)a.archetypePrimary, 0]+=2;
                //If there is a secondary archetype, increment that as well
                if (a.archetypeSecondary != Archetype.ArchetypeList.None) {
                    archetypeVotes[(int)a.archetypeSecondary, 0]+=2;
                }
            }
        }

        //Find the archetype with the most votes
        int maxVotes = 0;
        int maxArchetypeNumber = 0;
        for (int i = 0; i < numOfArch; i++) {
            if (archetypeVotes[i, 0] > maxVotes) {
                maxVotes = archetypeVotes[i, 0];
                maxArchetypeNumber = i;
            }
            else if (archetypeVotes[i, 0] == maxVotes) {
                //If there is a tie, prefer keeping the current archetype
                if (archetypeVotes[i, 1] == character.prefabIndex) {
                    maxVotes = archetypeVotes[i, 0];
                    maxArchetypeNumber = i;
                }
                else
                    continue;


            }
        }

        if (maxVotes >= 3) {

            character.prefabIndex = archetypeVotes[maxArchetypeNumber, 1];



            //Modify the character's look
            Character temp = UIManager.singleton.characterFactory.characters[character.prefabIndex].GetComponent<Character>();
            character.animationManager.animator.runtimeAnimatorController = temp.animationManager.animator.runtimeAnimatorController;
        }
        else {
            character.prefabIndex = 0;
            //Modify the character's look
            Character temp = UIManager.singleton.characterFactory.characters[character.prefabIndex].GetComponent<Character>();
            character.animationManager.animator.runtimeAnimatorController = temp.animationManager.animator.runtimeAnimatorController;
        }

        Debug.Log("MaxVotes" + maxVotes + "MaxArchetypeNumber" + maxArchetypeNumber + "PrefabIndex" + character.prefabIndex);

    }
}
