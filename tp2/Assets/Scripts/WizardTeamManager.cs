using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizardTeamManager : MonoBehaviour
{
    private GameObject[] towers;
    private GameObject[] enemyTowers;
    [SerializeField] private string teamTowerTag;
    [SerializeField] private string enemyTowerTag;
    [SerializeField] private Text endGameText;
    private string teamNameSuffix = "Tower";

    // This is bad practice, but again this is to prevent creating an entire GameManager just to manage the gameover state.
    static public bool gameOver = false;

    private void Awake()
    {
        towers = GameObject.FindGameObjectsWithTag(teamTowerTag);
        enemyTowers = GameObject.FindGameObjectsWithTag(enemyTowerTag);
    }

    private void Update()
    {
        if (!gameOver)
        {
            gameOver = VerifyVictoryConditions();
        }
        
    }

    public GameObject GetRandomActiveTeamTower()
    {
        List<GameObject> temp = new();

        foreach (GameObject tower in enemyTowers)
            if (tower.activeSelf)
                temp.Add(tower);

        if (temp.Count > 0)
        {
            return temp[Random.Range(0, temp.Count)];
        }
        return null;
    }

    public GameObject GetClosestActiveTeamTower(GameObject wizard)
    {
        return ReturnClosestToPosition(wizard, towers);
    }

    public GameObject GetClosestActiveEnemyTower(GameObject wizard)
    {
        return ReturnClosestToPosition(wizard, enemyTowers);
    }

    private GameObject ReturnClosestToPosition(GameObject wizard, GameObject[] towers)
    {
        GameObject closestTour = null;
        float smallerDistance = Mathf.Infinity;

        foreach (GameObject possibleTarget in towers)
        {
            if (possibleTarget.activeSelf)
            {
                float distance = Vector2.Distance(wizard.transform.position, possibleTarget.transform.position);

                if (distance < smallerDistance)
                {
                    smallerDistance = distance;
                    closestTour = possibleTarget;
                }
            }
        }

        return closestTour;
    }

    // This is the kind of behavior that should usually stand in a GameManager, but as stands, it would be slightly overkill to create one JUST for this.
    private bool VerifyVictoryConditions()
    {
        

        bool enemyVanquished = true;
        foreach (GameObject tower in enemyTowers)
            if (tower.activeSelf)
            {
                enemyVanquished = false;
                break;
            }
                

        if (enemyVanquished)
        {
            string formattedTeamName = teamTowerTag;
            formattedTeamName = formattedTeamName.Remove(formattedTeamName.Length - teamNameSuffix.Length).ToUpper();
            endGameText.text = formattedTeamName + " TEAM IS VICTORIOUS";
            if(formattedTeamName == "BLUE")
            {
                endGameText.color = Color.blue;
            }
            else
            {
                endGameText.color = Color.green;
            }
            
        }
        return enemyVanquished;
    }
}
