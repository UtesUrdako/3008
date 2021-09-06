using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        Debug.DrawRay(collision.contacts[0].point, normal * 10f, Color.red, 15f);
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exit");
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Stay");
    }
}
