using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public GameObject explosionPrefab;

    private string explosionPrefabPoolTag;

    public int damage = 1;

    private void Awake()
    {
        explosionPrefabPoolTag = explosionPrefab.name;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == MyTags.PLAYER_TAG)
        {
            GameObject fxObj = ObjectPooler.Instance.GetPooledObject(explosionPrefabPoolTag, transform.position, Quaternion.identity);

            other.gameObject.GetComponent<PlayerHealth>().ApplyDamage(damage);

            gameObject.SetActive(false);
        }

        if (other.gameObject.tag == MyTags.PLAYER_PROJECTILE_TAG)
        {
            GameObject fxObj = ObjectPooler.Instance.GetPooledObject(explosionPrefabPoolTag, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
        }
    }

}

