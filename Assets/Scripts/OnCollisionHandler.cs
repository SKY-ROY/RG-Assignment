using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionHandler : MonoBehaviour
{
    public PlayerController m_Player;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == MyTags.PLAYER_TAG)
            return;
        m_Player.OnCharacterColliderHit(collision.collider);
    }
}
