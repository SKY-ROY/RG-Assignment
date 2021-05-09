using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBlock : MonoBehaviour
{
    public Transform otherBlock;

    private Transform player;
    private float halfLength = 100f;
    private float endOffset = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        halfLength = (transform.position.z + otherBlock.position.z) / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        MoveGround();
    }

    void MoveGround()
    {
        //if player crosses the offset position in particular block the position of other block is shifted by halflength*2 units in z axis
        if (transform.position.z + halfLength < player.transform.position.z - endOffset)
        {
            transform.position = new Vector3(otherBlock.position.x, otherBlock.position.y, otherBlock.position.z + halfLength * 2);
        }
    }
}