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

    public Transform GetRandomActiveEnemyTower()
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision);
        if(collision.gameObject.tag == "Forest")
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.65f);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Forest")
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
        }
    }
}
