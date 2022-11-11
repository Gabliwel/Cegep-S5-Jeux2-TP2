using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardManager : MonoBehaviour
{
    public enum WizardStateToSwitch { Normal, Intrepid, RunAway, Hide, Secured }

    private SpriteRenderer sprite;
    private WizardState state;
    private LineController lineController;
    private WizardTeamManager teamManager;

    protected List<GameObject> possibleTargets = new();
    protected List<GameObject> possibleHiddingSpot = new();

    private float nbLives = 100f;
    private GameObject bush = null;

    public const float bushReduction = 0.75f;
    public const float maxNbLives = 100f;

    [SerializeField] private string teamManagerTag;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        state = GetComponent<WizardState>();
        lineController = GetComponentInChildren<LineController>();
        teamManager = GameObject.FindGameObjectWithTag(teamManagerTag).GetComponent<WizardTeamManager>();
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

    //**************** Team Manager *************************//

    public GameObject GetRandomActiveEnemyTower()
    {
        return teamManager.GetRandomActiveTeamTower();
    }

    public GameObject GetClosestTower()
    {
        return teamManager.GetClosestActiveTeamTower(gameObject);
    }


    //**************** Collision *************************//

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Forest")
        {
            bush = collision.gameObject;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.65f);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Forest")
        {
            bush = null;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
    }

    //****************** Trigger ******************************//
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.EndsWith("Wizard") && gameObject.tag != collision.gameObject.tag && !collision.isTrigger)
        {
            possibleTargets.Add(collision.gameObject);

            state.ManageEnemyEnter(collision.gameObject);

            /*if (target.tag.EndsWith("Tower"))
            {
                target = collision.gameObject;
            }*/
        }
        else if (collision.gameObject.tag == "Forest" && !collision.isTrigger)
        {
            possibleHiddingSpot.Add(collision.gameObject);
            state.ManageHidingSpotEnter(collision.gameObject);
            Debug.Log(state);
            Debug.Log("ouiiui");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.EndsWith("Wizard") && gameObject.tag != collision.gameObject.tag && !collision.isTrigger)
        {
            possibleTargets.Remove(collision.gameObject);

            state.ManageEnemyExit(collision.gameObject);

            /*if (collision.gameObject == target)
            {
                isAttacking = false;
                SearchNewTarget();
            }*/
        }
        else if (collision.gameObject.tag == "Forest" && !collision.isTrigger)
        {
            possibleHiddingSpot.Remove(collision.gameObject);
        }
    }

    //********************* Basic *********************//

    public bool IsInBush()
    {
        return bush != null;
    }

    public GameObject GetBush()
    {
        return bush;
    }

    public List<GameObject> GetPossibleTargets()
    {
        return possibleTargets;
    }
    public List<GameObject> GetPossibleHidingSpots()
    {
        return possibleTargets;
    }

    public float getNbLives()
    {
        return nbLives;
    }

    public void AddRegenLives(float value)
    {
        Debug.Log(gameObject.name + " regen");
        nbLives += value;
        if(nbLives > maxNbLives)
        {
            nbLives = maxNbLives;
        }
    }

    public bool Damage(float attackValue)
    {
        if (IsInBush())
        {
            nbLives -= attackValue * bushReduction;
        }
        else
        {
            nbLives -= attackValue;
        }

        return (nbLives <= 0);
    }

    public void Attack(Transform from, GameObject target)
    {
        lineController.DrawLine(from, target);
    }
}
