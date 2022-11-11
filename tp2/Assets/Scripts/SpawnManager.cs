using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject wizard;
    [SerializeField] private string towerTag;

    private static int maxWizards = 50;
    private GameObject[] wizards = new GameObject[maxWizards];
    private int spawnInterval = 5;
    private float currentTimer = 0f;
    private GameObject[] towers;

    void Start()
    {
        preLoadActors();
    }

    // Update is called once per frame
    void Update()
    {
        manageSpawn();
    }

    void manageSpawn()
    {
        int randomSpawner = Random.Range(1, towers.Length);
        if (currentTimer >= spawnInterval)
        {
            for (int i = 0; i < maxWizards; i++)
            {
                if (!wizards[i].activeSelf)
                {
                    List<GameObject> temp = new();

                    foreach (GameObject tower in towers)
                        if (tower.activeSelf)
                            temp.Add(tower);

                    if (temp.Count > 0)
                    {
                        wizards[i].transform.position = towers[randomSpawner].transform.position;
                        wizards[i].SetActive(true);
                        currentTimer = 0;
                        break;
                    }

                }
            }
        }

        
        currentTimer += Time.deltaTime;
    }

    void preLoadActors()
    {
        for (int i = 0; i < maxWizards; i++)
        {
            wizards[i] = Instantiate(wizard);
            wizards[i].transform.parent = gameObject.transform;
            wizards[i].SetActive(false);
        }

        towers = GameObject.FindGameObjectsWithTag(towerTag);
    }
}
