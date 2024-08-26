using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Energy Regeneration")]
    [SerializeField] private float energyRegenerationTimer = 0;
    private float energyRegenAmount = 20;
    [SerializeField] float energyRegenerationDelay = 5;
    float energy;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        energyRegenerationTimer = energyRegenerationDelay;
    }

    public int CalculateEnergyBasedOnGenerator(int power)
    {
        

        energy = power * 10;

        return Mathf.RoundToInt(energy);
    }

    public virtual void RegenerateEnergy()
    {
        // ONLY OWNERS CAN EDIT THEIR NETWORK VARIABLES
        if (character.isFlying)
            return;

        if (character.currentEnergy <= 0)
        {
            character.currentEnergy = 0;
            energyRegenerationTimer -= Time.deltaTime;
            if (character.currentEnergy < character.maxEnergy && energyRegenerationTimer <= 0)
            {
                character.currentEnergy += energyRegenAmount * Time.deltaTime;
                energyRegenerationTimer = energyRegenerationDelay;
            }
        }
        else if (character.currentEnergy < character.maxEnergy)
        {
            character.currentEnergy += energyRegenAmount * Time.deltaTime;
        }
    }

    
}
