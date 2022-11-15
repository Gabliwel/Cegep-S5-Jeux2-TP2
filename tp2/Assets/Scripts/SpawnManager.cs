using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject wizard;
    [SerializeField] private string towerTag;

    private static int maxWizards = 50;
    private GameObject[] wizards = new GameObject[maxWizards];
    private float spawnInterval = 3f;
    private float deadMultiplier = 0.75f;
    private float currentTimer = 0f;
    private GameObject[] towers;

    void Start()
    {
        preLoadActors();
    }

    // Update is called once per frame
    void Update()
    {
        if (!WizardTeamManager.gameOver)
        {
            manageSpawn();
        }
        
    }

    void manageSpawn()
    {
        List<GameObject> temp = new();

        foreach (GameObject tower in towers)
            if (tower.activeSelf)
                temp.Add(tower);

        int towersRemaining = temp.Count;



        //For each broken tower reduce spawn interval as a comeback factor
        if (currentTimer >= spawnInterval - (deadMultiplier * (towers.Length - towersRemaining)))
        {
            int randomSpawner = Random.Range(0, towers.Length);
            for (int i = 0; i < maxWizards; i++)
            {
                if (!wizards[i].activeSelf)
                {
                    

                    if (temp.Count > 0)
                    {
                        wizards[i].transform.position = towers[randomSpawner].transform.position;
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
