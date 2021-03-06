using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{
    float moveSpeed = 100f;
    Vector3 currentPosition, lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.instance.nextSpawnPoint != "")
        {
            GameObject spawnPoint = GameObject.Find(GameManager.instance.nextSpawnPoint);
            transform.position = spawnPoint.transform.position;

            GameManager.instance.nextSpawnPoint = "";
        }
        else if(GameManager.instance.lastHeroPosition != Vector3.zero)
        {
            transform.position = GameManager.instance.lastHeroPosition;
            GameManager.instance.lastHeroPosition = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX,0f, moveZ);
        GetComponent<Rigidbody>().velocity = movement * moveSpeed * Time.deltaTime;

        currentPosition = transform.position;
        if (currentPosition == lastPosition)
        {
            GameManager.instance.isWalking = false;
        }
        else
        {
            GameManager.instance.isWalking = true;
        }
        lastPosition = currentPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Teleporter")
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextSpawnPoint = col.spawnPointName;
            GameManager.instance.NextScene = col.sceneToLoad;
            GameManager.instance.LoadNextScene();
        }

        if(other.tag == "EncounterZone")
        {
            RegionData region = other.gameObject.GetComponent<RegionData>();
            GameManager.instance.curRegion = region;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "EncounterZone")
        {
            GameManager.instance.canGetEncounter = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "EncounterZone")
        {
            GameManager.instance.canGetEncounter = false;
        }
    }
}
