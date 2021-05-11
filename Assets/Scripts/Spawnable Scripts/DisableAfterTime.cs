using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableAfterTime : MonoBehaviour
{
    public float timer = 1f;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(MyTags.PLAYER_TAG);
        
    }

    private void Update()
    {
        if(transform.position.z < player.transform.position.z)
        {
            Invoke("DeactivateGameObject", timer);
        }
    }

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
