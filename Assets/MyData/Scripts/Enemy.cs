using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private NavMeshAgent _agent;

    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_target == null) return;

        _agent.SetDestination(_target.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_target == null) return;


        float distance = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
            new Vector3(waypoints[m_CurrentWaypointIndex].position.x, 0, waypoints[m_CurrentWaypointIndex].position.z));
        if (distance < _agent.stoppingDistance)
        {
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
            _agent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }

        Vector3 direction = _target.position - _head.position;
        //direction.Set(direction.x, _head.position.y, direction.z);

        Vector3 stepDirection = Vector3.RotateTowards(_head.forward, direction, _speed * Time.fixedDeltaTime, 0.0f);

        _head.transform.rotation = Quaternion.LookRotation(stepDirection);
    }

    private void Update()
    {
        if (NavMesh.SamplePosition(_agent.transform.position, out NavMeshHit navMeshHit, 0.2f, NavMesh.AllAreas))
            print(NavMesh.GetAreaCost((int)Mathf.Log(navMeshHit.mask, 2)));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _target = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _target = null;
    }
}
