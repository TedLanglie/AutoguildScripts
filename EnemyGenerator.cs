using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [Header("Generator Settings")]
    [SerializeField] private int levelOfUnits;
    [SerializeField] private List<GameObject> CustomEnemyList;
    [Header("Enemy List DONT TOUCH")]
    public List<GameObject> EnemyGenList;
    public string EnemyGroupName;
    
    void Start()
    {
        EnemyGroupName = "TESTING TESTING!";
        // this if is saying if we have a custom enemy list to override this, use that instead
        /* THIS BROKEY BROKEY AS A MUH FUH
        if(CustomEnemyList != null || CustomEnemyList.Count > 0)
        {
            Debug.Log("hit?");
            EnemyGenList = CustomEnemyList;
            return;
        }
        */
        // Generate X enemies based on player team size
        List<GameObject> PlayerTeam = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().LineupUnitList;

        foreach(GameObject unit in PlayerTeam)
        {
            if(unit != null) GenerateUnit();
        }
    }

    void GenerateUnit()
    {
        int levelToPassToGenerator = 0;
        // get avg player level
        List<GameObject> PlayerTeam = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().LineupUnitList;
        int PlayerLevelTotal = 0;
        int AveragePlayerLevel = 0;
        int numOfNonNullUnits = 0;
        foreach(GameObject unit in PlayerTeam)
        {
            if(unit != null)
            {
                PlayerLevelTotal += unit.GetComponent<UnitStats>().level;
                numOfNonNullUnits++;
            }
        }
        AveragePlayerLevel = PlayerLevelTotal / numOfNonNullUnits; // calculate average level

        if(AveragePlayerLevel < 2) levelToPassToGenerator = 1;
        else if(AveragePlayerLevel == 2)
        {
            int randomRoll = Random.Range(1, 3);
            levelToPassToGenerator = randomRoll;
        }
        else 
        {
            int randomRoll = Random.Range(AveragePlayerLevel - 2, AveragePlayerLevel + 2);
            levelToPassToGenerator = randomRoll;
        }
        // -- done calcing level

        if(levelToPassToGenerator > 10) levelToPassToGenerator = 10;

        GameObject EnemyUnit = GetComponent<GenerateUnit>().GenerateRandomUnit(levelToPassToGenerator, true, null, null, null); // generate unit
        EnemyGenList.Add(EnemyUnit); // add unit to the list

        Vector3 localScale = EnemyUnit.transform.localScale; // flip Object
        localScale.x *= -1;
        EnemyUnit.transform.localScale = localScale;
    }
}