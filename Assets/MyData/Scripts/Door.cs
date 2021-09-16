using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string _nameKey;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IStorage storage))
        {
            if (storage.IsItem(_nameKey))
                Destroy(gameObject);
        }
    }
}
