using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [SerializeField] private float healthRemaining;
    private float startingHealth = 18f;
    
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
}
