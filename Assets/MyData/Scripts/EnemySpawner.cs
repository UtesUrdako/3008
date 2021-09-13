using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform[] _enemyPoint;
    private int count = 0;

    private void Start()
    {
        StopAllCoroutines();
        StartCoroutine(Spawn(3));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Spawn(3);
        }
    }

    private IEnumerator Spawn(int countEnemy)
    {
        yield return new WaitForSeconds(3f);

        while (count != countEnemy)
        {
            GameObject go = Instantiate(_enemyPrefab);
            go.transform.position = _enemyPoint[count].position;
            count = (count + 1) % _enemyPoint.Length;
            yield return Waiter();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator Waiter()
    {
        yield return new WaitUntil(() => Input.GetButtonDown("Fire1"));
    }
}
