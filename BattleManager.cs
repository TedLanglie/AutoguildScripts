using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    /*
    | PURPOSE OF SCRIPT:
    | Manages an auto-battle. Should take a List of both player team and enemy team. Coin flip to decide who attacks first (50/50). Iterate turn attacks.
    | Always check if game is won with players dead. Script should activate List[i] players attack, wait until that player says the attack is done, then go to the next.
    */
    [Header("TESTING BOOL CHECK")]
    [SerializeField] private bool isTesting = false;
    // DELEGATES (GLOBAL EVENT CALLS)
    public delegate void OnRoundStart();
    public static OnRoundStart onRoundStart;
    public delegate void OnGameStart();
    public static OnGameStart onGameStart;
    public delegate void OnGameEnd();
    public static OnGameEnd onGameEnd;
    [Header("Flee Vars")]
    private bool playerFled = false;

    [Header("Do set manually")]
    [SerializeField] private GameObject _HealthbarPrefab;
    [SerializeField] private GameObject _HealthbarCanvas;
    [SerializeField] private TextMeshProUGUI RoundCounter;

    [Header("Do not set manually")]
    Camera mainCam;
    GameObject Banner;
    GameObject VSBanner;
    public List<GameObject> PlayerTeam;
    public List<GameObject> EnemyTeam;
    public List<string> PlayerTeamAllTraitsList;
    public List<string> EnemyTeamAllTraitsList;
    public int InitialPlayerTeamLength;
    public int InitialEnemyTeamLength;
    public int turnIndex = 0;
    public int currentPlayerIndex;
    public int currentEnemyIndex;
    public bool isPlayerTurn;
    private bool playersAreAllDead = false;
    private bool enemiesAreAllDead = false;
    void Start()
    {
        StartCoroutine(InitializeGameComponents());
    }

    private IEnumerator InitializeGameComponents()
    {
        yield return new WaitForSeconds(.3f);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // SORTING THE PLAYER AND ENEMY ARRAYS
        PlayerTeam = new List<GameObject>();
        if(!isTesting) EnemyTeam = new List<GameObject>();
        // Get the list of units from player and put it into our array
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        List<GameObject> InputPlayerTeam = player.GetComponent<Player>().LineupUnitList;
        if(!isTesting) 
        {
            List<GameObject> InputEnemyTeam = GameObject.FindGameObjectWithTag("EnemyGen").GetComponent<EnemyGenerator>().EnemyGenList;
            for(int i = 0; i < InputEnemyTeam.Count; i++)
            {
                Debug.Log("Loggy");
                if(InputEnemyTeam[i] != null) EnemyTeam.Add(InputEnemyTeam[i]);
            }
        }
        // LOOP THROUGH INPUT LIST, COP NON-NULLS TO OUR LIST (enemy part done in IF statement becuz testing bool, could change)
        for(int i = 0; i < InputPlayerTeam.Count; i++)
        {
            if(InputPlayerTeam[i] != null) PlayerTeam.Add(InputPlayerTeam[i]);
        }
        // -----------------------------------------------------------------

        // set the new Player and Enemy Trait lists  (mainly for Shapeshift trait atm)
        PlayerTeamAllTraitsList = new List<string>();
        EnemyTeamAllTraitsList = new List<string>();
        // populate PlayerListTraits
        foreach(GameObject unit in PlayerTeam)
        {
            foreach(string trait in unit.GetComponent<UnitStats>().traits)
            {
                PlayerTeamAllTraitsList.Add(trait);
            }
        }
        // populate EnemyListTraits
        foreach(GameObject unit in EnemyTeam)
        {
            foreach(string trait in unit.GetComponent<UnitStats>().traits)
            {
                EnemyTeamAllTraitsList.Add(trait);
            }
        }
        // -----------------------------------------------------------------

        // record the initial length of both team arrays, this is what we loop through for attacks
        InitialPlayerTeamLength = PlayerTeam.Count;
        InitialEnemyTeamLength = EnemyTeam.Count;
        
        // This block of code will set each unit battle script in scene to its correct index
        int indexCounter = 0;
        foreach(GameObject unit in PlayerTeam)
        {
            if(unit == null) continue;
            unit.GetComponent<UnitBattle>().Initialize(indexCounter, true);
            indexCounter++;
        }
        indexCounter = 0;
        foreach(GameObject unit in EnemyTeam)
        {
            if(unit == null) continue;
            unit.GetComponent<UnitBattle>().SpecialStatsShift(player.GetComponent<Player>().DayCount);
            unit.GetComponent<UnitBattle>().Initialize(indexCounter, false);
            indexCounter++;
        }
        // ---------------
        // Set units in the correct spot in the world --------------
        GameObject firstPlayerUnit = null;
        int indexOfFirstPlayerUnit = -1;
        for(int i = 0; i < PlayerTeam.Count; i++)
        {
            if(firstPlayerUnit != null) break;
            if(PlayerTeam[i] != null)
            {
                firstPlayerUnit = PlayerTeam[i];
                indexOfFirstPlayerUnit = i;
            }
        }
        int numOfNonNullPlayerUnits = 0;
        for(int i = 0; i < PlayerTeam.Count; i++)
        {
            if(PlayerTeam[i] == null) continue;
            numOfNonNullPlayerUnits++;
        }

        GameObject firstEnemyUnit = null;
        int indexOfFirstEnemyUnit = -1;
        for(int i = 0; i < EnemyTeam.Count; i++)
        {
            if(firstEnemyUnit != null) break;
            if(EnemyTeam[i] != null)
            {
                firstEnemyUnit = EnemyTeam[i];
                indexOfFirstEnemyUnit = i;
            }
        }
        int numOfNonNullEnemyUnits = 0;
        for(int i = 0; i < EnemyTeam.Count; i++)
        {
            if(EnemyTeam[i] == null) continue;
            numOfNonNullEnemyUnits++;
        }

        // TODO: set cam size of 1.2 if max units on both side is 3 or less, this bool will change positions of units too
        bool camSizeReduced = false;
        if(numOfNonNullEnemyUnits <= 3 && numOfNonNullPlayerUnits <= 3)
        {
            camSizeReduced = true;
            mainCam.orthographicSize = 1.2f;
        }

        // rest positions of first unit for both sides
        firstPlayerUnit.transform.position = new Vector3(0, 0, 0);
        EnemyTeam[0].transform.position = new Vector3(0, 0, 0); // BREAKING RIGHT HERE

        // first set position of first unit
        switch(numOfNonNullPlayerUnits-1)
        {
                    case 0:
                        if(!camSizeReduced) firstPlayerUnit.transform.position += new Vector3(-1.2f, .0f, 0);
                        else firstPlayerUnit.transform.position += new Vector3(-0.8f, -0.4f, 0);
                        break;
                    case 1:
                        if(!camSizeReduced) firstPlayerUnit.transform.position += new Vector3(-1.2f, .3f, 0);
                        else firstPlayerUnit.transform.position += new Vector3(-0.8f, -0.15f, 0);
                        break;
                    case 2:
                        if(!camSizeReduced) firstPlayerUnit.transform.position += new Vector3(-1.2f, .6f, 0);
                        else firstPlayerUnit.transform.position += new Vector3(-0.8f, .05f, 0);
                        break;
                    case 3:
                        firstPlayerUnit.transform.position += new Vector3(-1.2f, .45f, 0);
                        break;
                    case 4:
                        firstPlayerUnit.transform.position += new Vector3(-1.2f, .6f, 0);
                        break;
                    default:
                        // code block
                        break;
        }
        // now loop through the other objects and place them below the previous one
        for(int i = indexOfFirstPlayerUnit+1; i < PlayerTeam.Count; i++)
        {
            if(PlayerTeam[i] == null) continue;
            bool validPreviousUnitSearch = true;
            int stepCounter = 1;
            while(validPreviousUnitSearch)
            {
                if(PlayerTeam[i-stepCounter] == null)
                {
                    stepCounter++;
                    continue;
                }
                PlayerTeam[i].transform.position = PlayerTeam[i-stepCounter].transform.position; // teleport unit to where previous unit is
                validPreviousUnitSearch = false;
            }
            PlayerTeam[i].transform.position += new Vector3(0, -.5f, 0); // move it down
        }
        // :: NOW DO SAME FOR ENEMY:
        switch(numOfNonNullEnemyUnits-1)
        {
                    case 0:
                        if(!camSizeReduced) firstEnemyUnit.transform.position += new Vector3(1.2f, .0f, 0);
                        else firstEnemyUnit.transform.position += new Vector3(0.8f, -0.4f, 0);
                        break;
                    case 1:
                        if(!camSizeReduced) firstEnemyUnit.transform.position += new Vector3(1.2f, .3f, 0);
                        else firstEnemyUnit.transform.position += new Vector3(0.8f, -0.15f, 0);
                        break;
                    case 2:
                        if(!camSizeReduced) firstEnemyUnit.transform.position += new Vector3(1.2f, .6f, 0);
                        else firstEnemyUnit.transform.position += new Vector3(0.8f, .05f, 0);
                        break;
                    case 3:
                        firstEnemyUnit.transform.position += new Vector3(1.2f, .45f, 0);
                        break;
                    case 4:
                        firstEnemyUnit.transform.position += new Vector3(1.2f, .6f, 0);
                        break;
                    default:
                        // code block
                        break;
        }
        // now loop through the other objects and place them below the previous one
        if(EnemyTeam.Count != 1)
        {
            for(int i = 1; i < EnemyTeam.Count; i++)
            {
                EnemyTeam[i].transform.position = EnemyTeam[i-1].transform.position; // teleport unit to where previous unit is
                EnemyTeam[i].transform.position += new Vector3(0, -.5f, 0); // move it down
            }
        }
        // ------------------------------------------
        // create and set health bars to each unit
        foreach(GameObject unit in PlayerTeam)
        {
            if(unit == null) continue;
            GameObject currentlyAssigningHealthbar = Instantiate(_HealthbarPrefab, unit.transform.position, Quaternion.identity, _HealthbarCanvas.transform);
            currentlyAssigningHealthbar.GetComponentInChildren<HealthBar>().setAssigningUnit(unit);
        }
        foreach(GameObject unit in EnemyTeam)
        {
            GameObject currentlyAssigningHealthbar = Instantiate(_HealthbarPrefab, unit.transform.position, Quaternion.identity, _HealthbarCanvas.transform);
            currentlyAssigningHealthbar.GetComponentInChildren<HealthBar>().setAssigningUnit(unit);
        }

        StartCoroutine(StartOfGame());
    }

    private IEnumerator StartOfGame()
    {
        yield return new WaitForSeconds(1f);
        // Trigger banner animation
        Banner = GameObject.FindGameObjectWithTag("Banner");
        TextMeshProUGUI BannerText = Banner.GetComponentInChildren<TextMeshProUGUI>();
        BannerText.text = "- START OF GAME -";
        Banner.GetComponent<Animator>().SetTrigger("ShowBanner");
        yield return new WaitForSeconds(.5f);
        onGameStart?.Invoke(); //<------ START OF ROUND EFFECTS TRIGGER HERE
        yield return new WaitForSeconds(.5f);
        CoinFlip(); // start coinflip event, this starts the game
    }

    public void CoinFlip()
    {
        int result = Random.Range(0, 1);
        if(result == 0) isPlayerTurn = true;
        else isPlayerTurn = false;
        GoNextRound(); // begins game
    }

    /*
    | PURPOSE OF METHOD:
    | This function is what UnitBattle script should call after it is done attacking.
    | Essentially this runs between each units action
    */
    public void Round()
    {
        RoundCounter.text = "" + (turnIndex+1);
        // check if everyone on one side is dead, if so end game
        isOneSideAllDead();
        if(playersAreAllDead == true || enemiesAreAllDead == true) EndGame();

        // else, both sides are still alive and we run a round of the game
        else
        {
            onRoundStart?.Invoke(); //<------ START OF ROUND EFFECTS TRIGGER HERE
            // increment turn index to next int
            turnIndex++;
            // check to see whos turn it is
            Debug.Log("Setting new attacker " + turnIndex);
            if(isPlayerTurn)
            {
                // this function call will set the current player index to a valid player that is not dead and increments to the correct player
                setValidPlayerAttacker();
                // activate player units attack
                PlayerTeam[currentPlayerIndex].GetComponent<UnitBattle>().Attack(); 
                // increment current player index so that when this is called again it goes to next player
                currentPlayerIndex++;
            }
            else
            {
                // this function call will set the current player index to a valid player that is not dead and increments to the correct player
                setValidEnemyAttacker();
                // activate player units attack
                EnemyTeam[currentEnemyIndex].GetComponent<UnitBattle>().Attack(); 
                // increment current player index so that when this is called again it goes to next player
                currentEnemyIndex++;
            }

            // swap to other sides turn for when this function is called next
            if(isPlayerTurn == true) isPlayerTurn = false;
            else isPlayerTurn = true;
        }
    }

    public void EndGame()
    {
        char result;
        // 3 total results that can happen, tie, player win, enemy win
        if(playersAreAllDead && enemiesAreAllDead) result = 'T'; // Tie
        else if(enemiesAreAllDead) result = 'W'; // Won
        else if(playerFled) result = 'F'; // Fled
        else result = 'L'; // Loss

        onGameEnd?.Invoke(); //<------ START OF ROUND EFFECTS TRIGGER HERE
        GetComponent<ResultsManager>().MatchOver(result);
    }

    // HELPER FUNCTIONS TO FIND VALID PLAYER AND ENEMY ATTACKERS FOR THEIR TURNS
    private void setValidPlayerAttacker()
    {
        bool foundValidAttacker = false;

        if(currentPlayerIndex > InitialPlayerTeamLength - 1)
        currentPlayerIndex = 0;

        while(foundValidAttacker == false)
        {
            // check if null
            if(PlayerTeam[currentPlayerIndex] == null)
            {
                currentPlayerIndex++;

                if(currentPlayerIndex > InitialPlayerTeamLength - 1)
                currentPlayerIndex = 0;
                continue;
            }
            // check if dead
            if(PlayerTeam[currentPlayerIndex].GetComponent<UnitBattle>().isDead == true)
            {
                currentPlayerIndex++;

                if(currentPlayerIndex > InitialPlayerTeamLength - 1)
                currentPlayerIndex = 0;
            } 
            else foundValidAttacker = true;
        }
    }

    private void setValidEnemyAttacker()
    {
        bool foundValidAttacker = false;

        if(currentEnemyIndex > InitialEnemyTeamLength - 1)
        currentEnemyIndex = 0;

        while(foundValidAttacker == false)
        {
            if(EnemyTeam[currentEnemyIndex].GetComponent<UnitBattle>().isDead == true)
            {
                currentEnemyIndex++;

                if(currentEnemyIndex > InitialEnemyTeamLength - 1)
                currentEnemyIndex = 0;
            } 
            else foundValidAttacker = true;
        }
    }
    // --------------------------------------------

    // HELPER FUNCTION TO SEE IF ONE SIDE HAS EVERYONE DEAD
    private void isOneSideAllDead()
    {
        // start out by assuming both sides are dead
        playersAreAllDead = true;
        enemiesAreAllDead = true;

        // these loops will search through each team, if they find one alive, it'll set that side to false for the bool
        for(int i = 0; i < PlayerTeam.Count; i++)
        {
            if(PlayerTeam[i] == null) continue;
            if(PlayerTeam[i].GetComponent<UnitBattle>().isDead == false) playersAreAllDead = false;
        }
        for(int i = 0; i < EnemyTeam.Count; i++)
        {
            if(EnemyTeam[i].GetComponent<UnitBattle>().isDead == false) enemiesAreAllDead = false;
        }
    }

    // --------------------------------------------
    // Coroutine to delay the next round so everything isnt instant
    public void GoNextRound()
    {
        StartCoroutine(DelayStartNextRound());
    }
    private IEnumerator DelayStartNextRound()
    {
        if(playerFled) EndGame(); // if player has fled, end game
        yield return new WaitForSeconds(.01f); // time between rounds [SHOULD BE AROUND .5F, SETTING TO LOWER FOR TESTING]
        if(playerFled) EndGame();
        Debug.Log("Next round");
        if(!playerFled) Round(); // player has not fled, continue
    }

    public void PlayerFlee()
    {
        playerFled = true;
    }

    // ======= HELPER FUNCTIONS FOR OTHER SCRIPTS TO GET INFO =============
    public GameObject GetRandomUnit(GameObject CulledUnit, bool TargetSideIsPlayer)
    {
        GameObject returnedUnit = null;
        List<GameObject> possibleTargets = new List<GameObject>();
        // find a unit that is NOT the culled unit, and is on the correct side
        if(TargetSideIsPlayer)
        {
            foreach(GameObject unit in PlayerTeam)
            {
                if(!unit.GetComponent<UnitBattle>().isDead) possibleTargets.Add(unit);
            }
        }
        else
        {
            foreach(GameObject unit in EnemyTeam)
            {
                if(!unit.GetComponent<UnitBattle>().isDead) possibleTargets.Add(unit);
            }
        }
        // now we have a list of all all alive units on the given side
        if(possibleTargets.Contains(CulledUnit)) possibleTargets.Remove(CulledUnit);
        // we have now removed the culled unit, return null if our list is empty
        if(possibleTargets.Count == 0) return null;

        // get a random unit from list
        int IndexToGrab = Random.Range(0, possibleTargets.Count);
        returnedUnit = possibleTargets[IndexToGrab];
        
        return returnedUnit;
    }

    public GameObject GetRandomInjuredUnit(GameObject CulledUnit, bool TargetSideIsPlayer)
    {
        GameObject returnedUnit = null;
        List<GameObject> possibleTargets = new List<GameObject>();
        // find a unit that is NOT the culled unit, and is on the correct side
        if(TargetSideIsPlayer)
        {
            foreach(GameObject unit in PlayerTeam)
            {
                if(!unit.GetComponent<UnitBattle>().isDead && unit.GetComponent<UnitBattle>().getInjuryStatus()) possibleTargets.Add(unit);
            }
        }
        else
        {
            foreach(GameObject unit in EnemyTeam)
            {
                if(!unit.GetComponent<UnitBattle>().isDead && unit.GetComponent<UnitBattle>().getInjuryStatus()) possibleTargets.Add(unit);
            }
        }
        // now we have a list of all all alive units on the given side
        if(possibleTargets.Contains(CulledUnit)) possibleTargets.Remove(CulledUnit);
        // we have now removed the culled unit, return null if our list is empty
        if(possibleTargets.Count == 0) return null;

        // get a random unit from list
        int IndexToGrab = Random.Range(0, possibleTargets.Count);
        returnedUnit = possibleTargets[IndexToGrab];
        
        return returnedUnit;
    }
}
