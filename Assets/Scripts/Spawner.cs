using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    public int spawnCountMax;
    private int currentSpawnCount = 0;
    public EnemyController enemy1;
    public EnemyController enemy2;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn(){


        while (true)
        {
            yield return new WaitForSeconds(2);
            currentSpawnCount++;
            if (currentSpawnCount < spawnCountMax)
            {

                EnemyController e2 = Instantiate(enemy1, transform.position, Quaternion.identity);
                e2.GetComponent<NetworkObject>().Spawn();

                yield return new WaitForSeconds(5);
                if (enemy2 != null)
                {
                    EnemyController e = Instantiate(enemy2, transform.position, Quaternion.identity);
                    e.GetComponent<NetworkObject>().Spawn();
                }
            }
            else
            {
                currentSpawnCount = -5;
                //On Cooldown
            }
                
        }


    }

}
