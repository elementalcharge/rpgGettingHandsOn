using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class Portal : MonoBehaviour
{
    enum DestinationIdentifier
    {
        A, B, C, D, E
    }

    [SerializeField] int sceneToLoad=-1;
    [SerializeField] Transform spawnPoint;
    [SerializeField] DestinationIdentifier destination;
    [SerializeField] DestinationIdentifier origin;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        print("enterred");
        if (sceneToLoad < 0)
        {
            Debug.LogError("Scene to load not set.");
            yield break;
        }

        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);
        Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
        GameObject player = GameObject.FindWithTag ( "Player" );
        Transform spwanLocation= otherPortal.GetComponentInChildren<Transform>();
        player.GetComponent<TrailRenderer>().emitting = false;
        player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
        player.transform.rotation = otherPortal.spawnPoint.rotation;
        //WaitForSecondsRealtime(player.GetComponent<TrailRenderer>().time);
        player.GetComponent<TrailRenderer>().Clear();
        player.GetComponent<TrailRenderer>().emitting = true;
    }

    private Portal GetOtherPortal()
    {
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue;
            if (portal.origin == this.destination)
            {
                return portal;
            }
            
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
