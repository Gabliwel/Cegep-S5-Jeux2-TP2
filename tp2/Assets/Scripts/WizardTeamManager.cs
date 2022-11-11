using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardTeamManager : MonoBehaviour
{
    private GameObject[] towers;
    private GameObject[] enemyTowers;
    [SerializeField] private string teamTowerTag;
    [SerializeField] private string enemyTowerTag;

    private void Awake()
    {
        towers = GameObject.FindGameObjectsWithTag(teamTowerTag);
        enemyTowers = GameObject.FindGameObjectsWithTag(enemyTowerTag);
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
        GameObject closestTour = null;
        float smallerDistance = Mathf.Infinity;

        foreach (GameObject possibleTarget in towers)
        {
            float distance = Vector2.Distance(wizard.transform.position, possibleTarget.transform.position);

            if (distance < smallerDistance)
            {
                smallerDistance = distance;
                closestTour = possibleTarget;
            }
        }

        return closestTour;
    }

    public void GetClosestActiveEnemyTower()
    {

    }
}
