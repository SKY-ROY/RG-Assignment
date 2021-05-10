using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    public Collectable collectableType;

    public float rotationSpeed = 90f;

    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals(MyTags.PLAYER_TAG))
        {
            GameplayController.Instance.IncreaseCoinCount();
            gameObject.SetActive(false);
        }
    }
}
