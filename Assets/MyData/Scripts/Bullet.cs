using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;

    private Transform _target;
    private Vector3 _startPoint;
    private bool _selfHoming;
    public string findEnemyTag;

    public void Initialization(float lifetime, Transform target, bool selfHoming = true)
    {
        _target = target;
        _startPoint = transform.position;
        _selfHoming = selfHoming;

        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        //transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.fixedDeltaTime);

        transform.position += transform.forward * _speed * Time.fixedDeltaTime;
    }
}
