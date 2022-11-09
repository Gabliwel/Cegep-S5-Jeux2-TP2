using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardManager : MonoBehaviour
{
    public enum WizardStateToSwitch { Normal, Intrepid, RunAway, Hide, Secured }

    [SerializeField] private GameObject[] enemyTowers;
    public string enemyTowerTag;

    private SpriteRenderer sprite;
    private WizardState state;
    private LineController lineController;

    private float nbLives = 100f;
    private bool isInBush = false;

    public const float bushReduction = 0.75f;
    public const float maxNbLives = 100f;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        state = GetComponent<WizardState>();
        lineController = GetComponentInChildren<LineController>();
        enemyTowers = GameObject.FindGameObjectsWithTag(enemyTowerTag);
    }

    public void ChangeState(WizardStateToSwitch newState)
    {
        state.enabled = false;
        switch (newState)
        {
            case WizardStateToSwitch.Normal:
                {
                    //state = gameObject.GetComponent<WizardStateNormal>();
                    break;
                }
            case WizardStateToSwitch.RunAway:
                {
                    //state = gameObject.GetComponent<WizardStateRunAway>();
                    break;
                }
            case WizardStateToSwitch.Hide:
                {
                    //state = gameObject.GetComponent<WizardStateHide>();
                    break;
                }
            case WizardStateToSwitch.Intrepid:
                {
                    //state = gameObject.GetComponent<WizardStateIntrepid>();
                    break;
                }
            case WizardStateToSwitch.Secured:
                {
                    //state = gameObject.GetComponent<WizardStateSecured>();
                    break;
                }
        }
        state.enabled = true;
    }


    public GameObject GetRandomActiveEnemyTower()
    {
        List<GameObject> temp = new();

        foreach(GameObject tower in enemyTowers)
            if(tower.activeSelf)
                temp.Add(tower);

        if(temp.Count > 0)
        {
            return temp[Random.Range(0, temp.Count)];
        }
        return null;
    }

    public void Attack(Transform from, GameObject target)
    {
        lineController.DrawLine(from, target);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Forest")
        {
            isInBush = true;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.65f);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Forest")
        {
            isInBush = false;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
    }

    public bool IsInBush()
    {
        return isInBush;
    }

    public float getNbLives()
    {
        return nbLives;
    }

    public void AddRegenLives(float value)
    {
        nbLives += value;
        if(nbLives > maxNbLives)
        {
            nbLives = maxNbLives;
        }
    }

    public bool Damage(float attackValue)
    {
        if (isInBush)
        {
            nbLives -= attackValue * bushReduction;
        }
        else
        {
            nbLives -= attackValue;
        }

        return (nbLives <= 0);
    }
}
