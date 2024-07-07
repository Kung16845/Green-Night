using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
public class SpawnMonster : MonoBehaviour
{
    public float duration;
    public float treatValue = 50;
    Coroutine spawnCoroutine;
    public List<GameObject> prefabMonster;
    public List<GameObject> spawnedMonsters = new List<GameObject>();
    private void Start()
    {
        // StartCoroutine(SpawnMon());
    }

    public void StopSpawnEnemy()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
    public IEnumerator SpawnMon()
    {
        yield return new WaitForSeconds(duration);
        SpawnedMonster();
        spawnCoroutine = StartCoroutine(SpawnMon());
    }

    public void SpawnedMonster()
    {

        var transformSpawnMonster = new Vector3(this.transform.position.x, Random.Range(-10.0f, 10.0f));
        float[] probabilities = { 0.5f, 0.45f, 0.05f };
        float randomValue = Random.value;
        int n = 0;

        // Determine which monster to spawn based on the random value.
        if (randomValue < probabilities[0])
        {
            n = 0; // First monster type (50% chance)
        }
        else if (randomValue < probabilities[0] + probabilities[1])
        {
            n = 1; // Second monster type (40% chance)
        }
        else
        {
            n = 2; // Third monster type (10% chance)
        }
        
        GameObject monster = Instantiate(prefabMonster[n], transformSpawnMonster, this.transform.rotation);

        spawnedMonsters.Add(monster);
        monster.GetComponent<Enemy>().spawnMonster = this;
       
    }

}
