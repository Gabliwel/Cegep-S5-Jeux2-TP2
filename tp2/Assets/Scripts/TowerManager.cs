using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private float healthRemaining;
    private float startingHealth = 100f;
    private GameObject[] bunkeredWizards;
    
    void Start()
    {
        healthRemaining = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Damage(float attackValue)
    {
        healthRemaining -= attackValue;

        if (healthRemaining > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AlertBunkeredWizards()
    {
        for(int i = 0; i < bunkeredWizards.Length; i++)
        {
            bunkeredWizards[i].GetComponent<WizardStateSecured>().Alert();
        }
    }
}
