using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardManager : MonoBehaviour
{
    public enum WizardStateToSwitch { Normal, Intrepid, RunAway, Hide, Secured, Inert }

    private SpriteRenderer sprite;
    private WizardState state;
    private LineController lineController;
    private WizardTeamManager teamManager;
    private WizardStateToSwitch currentState;

    protected List<GameObject> possibleTargets = new();
    protected List<GameObject> possibleHiddingSpot = new();

    [SerializeField] private float nbLives = 100f;
    private GameObject bush = null;
    private GameObject tower = null;

    private int nbKill = 0;

    public const float bushReduction = 0.75f;

    public const float maxNbLives = 100f;

    [SerializeField] private string teamManagerTag;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        state = GetComponent<WizardState>();
        lineController = GetComponentInChildren<LineController>();
        teamManager = GameObject.FindGameObjectWithTag(teamManagerTag).GetComponent<WizardTeamManager>();
        currentState = WizardStateToSwitch.Normal;
        nbLives = 100;
    }

    private void Update()
    {
        if (WizardTeamManager.gameOver)
        {
            GameOver();
        }
    }

    public void ChangeState(WizardStateToSwitch newState)
    {
        state.enabled = false;
        switch (newState)
        {
            case WizardStateToSwitch.Normal:
                {
                    state = gameObject.GetComponent<WizardStateNormal>();
                    currentState = WizardStateToSwitch.Normal;
                    break;
                }
            case WizardStateToSwitch.RunAway:
                {
                    state = gameObject.GetComponent<WizardStateRunAway>();
                    currentState = WizardStateToSwitch.RunAway;
                    break;
                }
            case WizardStateToSwitch.Hide:
                {
                    state = gameObject.GetComponent<WizardStateHide>();
                    currentState = WizardStateToSwitch.Hide;
                    break;
                }
            case WizardStateToSwitch.Intrepid:
                {
                    state = gameObject.GetComponent<WizardStateIntrepid>();
                    currentState = WizardStateToSwitch.Intrepid;
                    break;
                }
            case WizardStateToSwitch.Secured:
                {
                    state = gameObject.GetComponent<WizardStateSecured>();
                    currentState = WizardStateToSwitch.Secured;
                    break;
                }
            case WizardStateToSwitch.Inert:
                {
                    state = gameObject.GetComponent<WizardStateInert>();
                    currentState = WizardStateToSwitch.Inert;
                    break;
                }
        }
        state.enabled = true;
        state.Init();
    }

    public void Init()
    {
        nbLives = 100;
        nbKill = 0;
        ChangeState(WizardStateToSwitch.Normal);
    }

    public void GameOver()
    {
        ChangeState(WizardStateToSwitch.Inert);
    }

    //**************** Team Manager *************************//

    public GameObject GetRandomActiveEnemyTower()
    {
        return teamManager.GetRandomActiveTeamTower();
    }

    public GameObject GetClosestEnemyTower()
    {
        return teamManager.GetClosestActiveEnemyTower(gameObject);
    }

    public GameObject GetClosestTower()
    {
        return teamManager.GetClosestActiveTeamTower(gameObject);
    }


    //**************** Collision *************************//

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Forest"))
        {
            bush = collision.gameObject;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.65f);
        }
        else if (collision.gameObject.tag.EndsWith("Tower"))
        {
            tower = collision.gameObject;
            sprite.color = new Color(255,255,255);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Forest"))
        {
            bush = null;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
        else if (collision.gameObject.tag.EndsWith("Tower"))
        {
            tower = collision.gameObject;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
    }

    //****************** Trigger ******************************//
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.EndsWith("Wizard") && !gameObject.CompareTag(collision.gameObject.tag) && !collision.isTrigger)
        {
            possibleTargets.Add(collision.gameObject);

            state.ManageEnemyEnter(collision.gameObject);

            /*if (target.tag.EndsWith("Tower"))
            {
                target = collision.gameObject;
            }*/
        }
        else if (collision.gameObject.CompareTag("Forest") && !collision.isTrigger)
        {
            possibleHiddingSpot.Add(collision.gameObject);
            state.ManageHidingSpotEnter(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.EndsWith("Wizard") && !gameObject.CompareTag(collision.gameObject.tag) && !collision.isTrigger)
        {
            possibleTargets.Remove(collision.gameObject);

            state.ManageEnemyExit(collision.gameObject);

            /*if (collision.gameObject == target)
            {
                isAttacking = false;
                SearchNewTarget();
            }*/
        }
        else if (collision.gameObject.CompareTag("Forest") && !collision.isTrigger)
        {
            possibleHiddingSpot.Remove(collision.gameObject);
        }
    }

    //********************* Basic *********************//

    public bool IsInBush()
    {
        return bush != null;
    }
    public bool IsInTower()
    {
        return tower != null;
    }

    public GameObject GetBush()
    {
        return bush;
    }

    public GameObject GetTower()
    {
        return tower;
    }

    public int GetNbKill()
    {
        return nbKill;
    }

    public void AddKill()
    {
        nbKill++;
    }

    public List<GameObject> GetPossibleTargets()
    {
        return possibleTargets;
    }
    public List<GameObject> GetPossibleHidingSpots()
    {
        return possibleHiddingSpot;
    }

    public float GetNbLives()
    {
        return nbLives;
    }

    public void AddRegenLives(float value)
    {
        Debug.Log(gameObject.name + " regen" + nbLives);
        nbLives += value;
        if(nbLives > maxNbLives)
        {
            nbLives = maxNbLives;
        }
    }

    public bool Damage(float attackValue, GameObject from)
    {
        if (IsInBush())
        {
            nbLives -= attackValue * bushReduction;
        }
        else
        {
            nbLives -= attackValue;
        }

        state.ManageIsAttackBy(from);
        return (nbLives <= 0);
    }

    public void Attack(Transform from, GameObject target)
    {
        lineController.DrawLine(from, target);
    }

    public WizardStateToSwitch GetWizardState()
    {
        return currentState;
    }

}
