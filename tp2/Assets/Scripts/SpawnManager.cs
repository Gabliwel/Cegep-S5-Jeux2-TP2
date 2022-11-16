using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject wizard;
    [SerializeField] private string towerTag;

    private const int MAX_WIZARDS = 50;
    private GameObject[] wizards = new GameObject[MAX_WIZARDS]; 
    private const float SPAWN_INTERVAL = 3f;
    private const float DEAD_TOWER_ACCELERATION = 0.75f;
    private float currentTimer = 0f;
    private GameObject[] towers;

    void Start()
    {
        PreLoadActors();
    }

    // Update is called once per frame
    void Update()
    {
        if (!WizardTeamManager.gameOver)
        {
            ManageSpawn();
        }
        
    }

    void ManageSpawn()
    {
        List<GameObject> temp = new();

        foreach (GameObject tower in towers)
            if (tower.activeSelf)
                temp.Add(tower);

        int towersRemaining = temp.Count;



        //For each broken tower reduce spawn interval as a comeback factor
        if (currentTimer >= SPAWN_INTERVAL - (DEAD_TOWER_ACCELERATION * (towers.Length - towersRemaining)))
        {
            int randomSpawner = Random.Range(0, towersRemaining);
            for (int i = 0; i < MAX_WIZARDS; i++)
            {
                if (!wizards[i].activeSelf)
                {
                    

                    if (temp.Count > 0)
                    {
                        wizards[i].transform.position = temp[randomSpawner].transform.position;
                        wizards[i].SetActive(true);
                        wizards[i].GetComponent<WizardManager>().Init();
                        currentTimer = 0;
                        break;
                    }

                }
            }
        }

        
        currentTimer += Time.deltaTime;
    }

    void PreLoadActors()
    {
        for (int i = 0; i < MAX_WIZARDS; i++)
        {
            wizards[i] = Instantiate(wizard);
            wizards[i].transform.parent = gameObject.transform;
            wizards[i].SetActive(false);
        }

        towers = GameObject.FindGameObjectsWithTag(towerTag);
    }
}
