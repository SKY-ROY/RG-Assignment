using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform target;
    private Vector3 offset;
    private float y;

    public float followSpeed = 5f;

    public float distance = 6.5f;
    public float height = 3.5f;

    public float height_Damping = 3.25f;
    public float rotation_Damping = 0.27f;
    public bool freeFollow = true;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag(MyTags.PLAYER_TAG).transform;
    }

    void LateUpdate()
    {
        if(freeFollow)
        {
            FollowPlayerFreely();
        }
        else
        {
            FollowPlayerClamped();
        }
    }

    void FollowPlayerFreely()
    {
        float wanted_Rotation_Angle = target.eulerAngles.y;
        float wanted_Height = target.position.y + height;

        float current_Rotation_Angle = transform.eulerAngles.y;
        float current_Height = transform.position.y;

        current_Rotation_Angle = Mathf.LerpAngle(current_Rotation_Angle, wanted_Rotation_Angle, rotation_Damping * Time.deltaTime);

        current_Height = Mathf.Lerp(current_Height, wanted_Height, height_Damping * Time.deltaTime);

        Quaternion current_Rotation = Quaternion.Euler(0f, current_Rotation_Angle, 0f);

        transform.position = target.position;
        transform.position -= current_Rotation * Vector3.forward * distance;

        transform.position = new Vector3(transform.position.x, current_Height, transform.position.z);
    }

    void FollowPlayerClamped()
    {
        Vector3 followPos = target.position + offset;
        RaycastHit hit;
        
        if(Physics.Raycast(target.position, Vector3.down, out hit, 2.5f))
        {
            y = Mathf.Lerp(y, hit.point.y, followSpeed * Time.deltaTime);               
        }
        else
        {
            y = Mathf.Lerp(y, target.position.y, followSpeed * Time.deltaTime);
        }

        followPos.y = offset.y + y;
        transform.position = followPos;
    }
}