using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics { 
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;
        void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
        }
        // Start is called before the first frame update
        void DisableControl(PlayableDirector director)
        {
            print("DisableControl");
            
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled=false;
        }

        // Update is called once per frame
        void EnableControl(PlayableDirector director)
        {
            print("EnableControl");
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}