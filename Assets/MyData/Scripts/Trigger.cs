using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] private float _boostSpeed = 2f;

    public UnityEvent OnTrigger;
    public UnityEvent OffTrigger;

    private void Start()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            OnTrigger?.Invoke();
            //other.GetComponent<Player>().SetBoostSpeed(_boostSpeed);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            OffTrigger?.Invoke();
            //other.GetComponent<Player>().SetBoostSpeed(1f);
    }
}
