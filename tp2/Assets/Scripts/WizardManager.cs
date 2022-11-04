using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardManager : MonoBehaviour
{
    public enum WizardStateToSwitch { Normal, Intrepid, RunAway, Hide, Secured }


    [SerializeField] private GameObject[] enemyTowers;

    private SpriteRenderer sprite;
    private WizardState state;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        state = GetComponent<WizardState>();
    }

    
    public void ChangeState(WizardStateToSwitch newState)
    {

    }

    void Update()
    {
        
    }

    public Transform GetRandomActiveTower()
    {
        List<GameObject> temp = new();

        foreach(GameObject tower in enemyTowers)
            if(tower.activeSelf)
                temp.Add(tower);

        if(temp.Count > 0)
        {
            return temp[Random.Range(0, temp.Count)].transform;
        }
        return null;
    }
}
