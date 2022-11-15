using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer line;
    private new Collider2D collider;
    private WizardManager wizard;
 
    private const float activeTime = 0.2f;
    private float timer = 0f;
    private const float attackValue = 8;

    private GameObject target;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        collider = GetComponent<BoxCollider2D>();
        wizard = GetComponentInParent<WizardManager>();
    }

    private void Start()
    {
        line.enabled = false;
        collider.enabled = false;
    }

    public void DrawLine(Transform from, GameObject to)
    {
        target = to;

        line.enabled = true;
        Vector2 position = from.position;
        position.y += 0.1f;
        line.SetPosition(0, position);
        line.SetPosition(1, to.transform.position);

        collider.transform.position = to.transform.position;
        collider.enabled = true;
    }

    void Update()
    {
        if(line.enabled)
        {
            timer += Time.deltaTime;
            if(timer >= activeTime)
            {
                Deactivate();
            }
        }
    }

    private void OnDisable()
    {
        Deactivate();
    }

    private void Deactivate()
    {
        timer = 0f;
        line.ResetBounds();
        line.enabled = false;
        collider.enabled = false;
        target = null;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (target == null)
        {
            return;
        }
        //Valide donc les collisions meme si des enemies sont collé
        if (collision.gameObject == target)
        {
            if(target.tag.EndsWith("Wizard"))
            {
                bool isDead = target.GetComponent<WizardManager>().Damage(attackValue, transform.parent.gameObject);

                if(isDead)
                {
                    target.SetActive(false);
                    target = null;
                    wizard.AddKill();
                }
            }
            else if(target.tag.EndsWith("Tower")){
                bool isDead = target.GetComponent<TowerManager>().Damage(attackValue);

                if (isDead)
                {
                    target.SetActive(false);
                    target = null;
                }
            }
        }
        
    }
}
