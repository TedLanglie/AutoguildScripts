using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBattle : MonoBehaviour
{
    [Header("Stats")]
    public float CurrentHealth;
    public float CurrentDamage;
    public float CurrentDodgeChance;
    public float CurrentBlockChance;
    public float CurrentParryChance;
    public float CurrentCritChance;
    public float CurrentCritDamage;
    public float damageDelt;
    // we need to get the unitStats data in here somehow
    private UnitStats UnitData;
    [Header("Battle Components")]
    private GameObject targetedEnemy;
    protected int enemyIndexToTarget;
    private bool foundAliveTarget = false;
    private int thisUnitsIndex;
    protected BattleManager _battleManager;
    [Header("Unit States")]
    public bool isInjured = false;
    public bool isDead = false;
    public bool isPlayer;
    protected bool attacked = false;
    // DELEGATES (GLOBAL EVENT CALLS)
    public delegate void OnUnitDeath(bool isPlayer, GameObject unit);
    public static OnUnitDeath onUnitDeath;
    public delegate void OnUnitBlock(bool isPlayer, GameObject unit);
    public static OnUnitBlock onUnitBlock;
    [Header("Effects/GUI")]
    [SerializeField] GameObject CritTextEffect;
    [SerializeField] GameObject blockTextEffect;
    [SerializeField] GameObject ShieldEffect;
    [SerializeField] GameObject DodgeTextEffect;
    [SerializeField] GameObject ParryTextEffect;
    [SerializeField] GameObject HitEffect;
    [SerializeField] GameObject SlashEffect;
    [SerializeField] GameObject DeathParticleEffect;
    [SerializeField] Material FlashMaterial;
    public GameObject attackingDisplayHUD;
    public GameObject targetDisplayHUD;
    private Material originalMaterial;
    [Header("Sounds")]
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip damaged;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip block;
    [SerializeField] AudioClip dodge;
    [SerializeField] AudioClip crit;
    [SerializeField] AudioClip heal;
    [Header("Saved Colors")]
    Color32 hairColorOriginal;
    Color32 eyeColorOriginal;

    // this method is called by BattleManager at the very start of reaching a BATTLE SCENE
    public void Initialize(int index, bool areWeThePlayer)
    {
        _battleManager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();

        thisUnitsIndex = index;
        isPlayer = areWeThePlayer;

        UnitData = gameObject.GetComponent<UnitStats>(); // get the stats of this unit

        hairColorOriginal = GetComponentInChildren<UnitSpriteManager>().GetHairColor();
        eyeColorOriginal = GetComponentInChildren<UnitSpriteManager>().GetEyeColor();

        // set stats
        CurrentHealth = UnitData.maxHealth;
        CurrentDamage = UnitData.baseDamage;
        CurrentDodgeChance = UnitData.dodgeChance;
        CurrentBlockChance = UnitData.blockChance;
        CurrentParryChance = UnitData.parryChance;
        CurrentCritChance = UnitData.critChance;
        CurrentCritDamage = UnitData.critDamage;
        originalMaterial = GetComponentInChildren<SpriteRenderer>().material;

        // || THE IF-ELSE LADDER FOR TRAITS AT START OF GAME ||
        if(gameObject.GetComponent<Shapeshift>() != null) gameObject.GetComponent<Shapeshift>().Activate(_battleManager);
        if(gameObject.GetComponent<Ghoul>() != null) gameObject.GetComponent<Ghoul>().Activate();
        if(gameObject.GetComponent<HumanRacialTrait>() != null) gameObject.GetComponent<HumanRacialTrait>().Activate();
        if(gameObject.GetComponent<ArtificerOfChaos>() != null) gameObject.GetComponent<ArtificerOfChaos>().Activate(_battleManager);
        if(gameObject.GetComponent<Deflection>() != null) gameObject.GetComponent<Deflection>().Activate();
        // -----------------------------------------------------------------------------------
    }

    public void Target()
    {
        targetedEnemy = null; // Refresh the gameObject that holds the targeted enemy as null

         // get correct side/list to damage
        List<GameObject> targetList;
        if(GetComponent<UnitBattle>().isPlayer) targetList = _battleManager.EnemyTeam;
        else targetList = _battleManager.PlayerTeam;

        // this is the base TARGETING method. it will target a random alive enemy
        while(foundAliveTarget == false)
        {
            // first, target any random unit on the opposing team
            enemyIndexToTarget = Random.Range(0, _battleManager.EnemyTeam.Count);
            // check if the unit is dead, if it is, it loops
            if(targetList[enemyIndexToTarget] == null) continue;
            if(targetList[enemyIndexToTarget].GetComponent<UnitBattle>().isDead == false)
            {
                // found an alive target, set targetedEnemy object and set bool to true to break us out of loop
                targetedEnemy = targetList[enemyIndexToTarget];
                foundAliveTarget = true;
            }
        }

        foundAliveTarget = false; // reset target for next loop
    }

    public void Attack()
    {
        // target unit for attack
        Target();
        StartCoroutine(BeforeAttacking());
    }

    // currently, this is just to disable hud elements on a timer, but corutines should be MUCH more thorough throughout the script, because timing matters alot here (visually)
    private IEnumerator BeforeAttacking()
    {
        attackingDisplayHUD.SetActive(true); // set UI for this attacking unit 
        attackingDisplayHUD.GetComponent<Animator>().SetTrigger("Appear");
        yield return new WaitForSeconds(.2f);
        targetedEnemy.GetComponent<UnitBattle>().targetDisplayHUD.SetActive(true); // set UI for targeted enemy
        targetedEnemy.GetComponent<UnitBattle>().targetDisplayHUD.GetComponent<Animator>().SetTrigger("Appear");
        // target unit for attack
        // || THE IF-ELSE LADDER FOR TRAITS IN BEFORE ATTACKING TIME ||
        if(gameObject.GetComponent<SiphonPower>() != null) gameObject.GetComponent<SiphonPower>().Activate(targetedEnemy);
        if(gameObject.GetComponent<BloodKnives>() != null) gameObject.GetComponent<BloodKnives>().Activate(targetedEnemy);
        if(gameObject.GetComponent<DividedSoul>() != null) gameObject.GetComponent<DividedSoul>().Activate(targetedEnemy);
        if(gameObject.GetComponent<DemonRacialTrait>() != null) gameObject.GetComponent<DemonRacialTrait>().Activate(_battleManager);
        if(gameObject.GetComponent<Execute>() != null) gameObject.GetComponent<Execute>().Attack(_battleManager);
        if(gameObject.GetComponent<BloodBath>() != null) gameObject.GetComponent<BloodBath>().Activate(targetedEnemy);
        if(gameObject.GetComponent<EarthShaker>() != null) gameObject.GetComponent<EarthShaker>().Activate(_battleManager);
        if(gameObject.GetComponent<CripplingArrow>() != null) gameObject.GetComponent<CripplingArrow>().Activate(_battleManager);
        if(gameObject.GetComponent<Judgement>() != null) gameObject.GetComponent<Judgement>().Activate(_battleManager);
        if(gameObject.GetComponent<IceBinding>() != null) gameObject.GetComponent<IceBinding>().Activate(_battleManager);
        if(gameObject.GetComponent<Fiend>() != null) gameObject.GetComponent<Fiend>().Activate(_battleManager, thisUnitsIndex);
        // -----------------------------------------------------------------------------------
        yield return new WaitForSeconds(.5f);
        StartCoroutine(Attacking());
    }
    private IEnumerator Attacking()
    {
        // first check if player is dead, if dead just exit sequence, call battlemanager to conitnue
        if(isDead){
            // deactivate HUD things
            attackingDisplayHUD.SetActive(false);
            targetedEnemy.GetComponent<UnitBattle>().targetDisplayHUD.SetActive(false);
            // go next and end things
            _battleManager.GoNextRound();
            yield break;
        }
        // trigger anim
        GetComponentInChildren<Animator>().SetTrigger("NormalAttack");
        // || THE IF-ELSE LADDER FOR TRAITS IN ATTACKING TIME ||
        if(gameObject.GetComponent<Cleave>() != null) gameObject.GetComponent<Cleave>().Attack(_battleManager, enemyIndexToTarget, CurrentDamage, isPlayer);
        if(gameObject.GetComponent<ExplodingBlood>() != null) gameObject.GetComponent<ExplodingBlood>().Activate(targetedEnemy);
        // -----------------------------------------------------------------------------------

        // float that will pass to targets Hit script
        float attackDamage = CurrentDamage;
        bool isCrit = false;
        // calculate crit
        float critRoll = Random.Range(1, 100);
        if(critRoll <= CurrentCritChance)
        {
            // || THE IF-ELSE LADDER FOR TRAITS THAT TRIGGER ON CRIT HIT ||
            if(gameObject.GetComponent<Headshot>() != null) gameObject.GetComponent<Headshot>().Activate(targetedEnemy);
            // -----------------------------------------------------------------------------------
            attackDamage += CurrentCritDamage;
            isCrit = true;
        }

        if(gameObject.GetComponent<Freeze>() != null)
        {
            attackDamage = Mathf.Floor(damageDelt / 2); // halve damage output if frozen
            StartCoroutine(GetComponent<Freeze>().DestroySelf()); // remove frozen
        }
        damageDelt = targetedEnemy.GetComponent<UnitBattle>().Hit(attackDamage, true, true, true, true, isCrit, gameObject); // attack enemy with damage

        // trigger slash animation on targeted enemy
        Instantiate(SlashEffect, targetedEnemy.transform.position + new Vector3(0,0, -1f), Quaternion.identity);

        yield return new WaitForSeconds(.5f);
        StartCoroutine(AfterAttacking());
    }

    private IEnumerator AfterAttacking()
    {
        // first check if player is dead, if dead just exit sequence, call battlemanager to conitnue
        if(isDead){
            // deactivate HUD things
            attackingDisplayHUD.SetActive(false);
            targetedEnemy.GetComponent<UnitBattle>().targetDisplayHUD.SetActive(false);
            // go next and end
            _battleManager.GoNextRound();
            yield break;
        }
        // || THE IF-ELSE LADDER FOR TRAITS IN AFTER ATTACKING TIME ||
        if(gameObject.GetComponent<Fiend>() != null) gameObject.GetComponent<Fiend>().DeActivate();
        if(gameObject.GetComponent<SiphonPower>() != null) gameObject.GetComponent<SiphonPower>().DeActivate();
        if(gameObject.GetComponent<BloodKnives>() != null) gameObject.GetComponent<BloodKnives>().DeActivate();
        if(gameObject.GetComponent<Curse>() != null) gameObject.GetComponent<Curse>().Activate(targetedEnemy);
        if(gameObject.GetComponent<HolyBell>() != null) GetComponent<HolyBell>().Activate(damageDelt, _battleManager);
        if(gameObject.GetComponent<GuardianAngel>() != null) gameObject.GetComponent<GuardianAngel>().Activate(_battleManager);
        if(gameObject.GetComponent<HolyNova>() != null) gameObject.GetComponent<HolyNova>().Activate(_battleManager, thisUnitsIndex);
        if(gameObject.GetComponent<Retreat>() != null) gameObject.GetComponent<Retreat>().Activate(4);
        if(gameObject.GetComponent<DualShot>() != null) GetComponent<DualShot>().Activate(_battleManager);
        if(gameObject.GetComponent<VoidArrow>() != null) GetComponent<VoidArrow>().Activate(targetedEnemy);
        if(gameObject.GetComponent<Warcry>() != null) GetComponent<Warcry>().Activate(_battleManager, thisUnitsIndex);
        if(gameObject.GetComponent<Eviscerate>() != null && damageDelt > 0) gameObject.GetComponent<Eviscerate>().Activate(targetedEnemy);
        if(gameObject.GetComponent<StormBringer>() != null && damageDelt > 0) gameObject.GetComponent<StormBringer>().Activate(_battleManager, targetedEnemy.GetComponent<UnitBattle>().thisUnitsIndex);
        if(gameObject.GetComponent<BloodMagic>() != null) gameObject.GetComponent<BloodMagic>().Activate(targetedEnemy);
        // -----------------------------------------------------------------------------------

        // deactivate HUD things
        attackingDisplayHUD.SetActive(false);
        targetedEnemy.GetComponent<UnitBattle>().targetDisplayHUD.SetActive(false);
        
        yield return new WaitForSeconds(.5f);

        // speak to battle manager to begin new round
        _battleManager.GoNextRound();
    }

    // returns Float Damage delt, so the attacker can know how much damage was delt
    public float Hit(float damage, bool parryable, bool blockable, bool dodgeable, bool wasAnAttack, bool wasCrit, GameObject Attacker)
    {
        // || THE IF-ELSE LADDER FOR TRAITS FOR BEING HIT ||
        if(gameObject.GetComponent<BearTrap>() != null && wasAnAttack) gameObject.GetComponent<BearTrap>().Activate(Attacker);
        if(gameObject.GetComponent<IceShield>() != null && wasAnAttack) gameObject.GetComponent<IceShield>().Activate(Attacker);
        // -----------------------------------------------------------------------------------
        // dodge
        float dodgeroll = Random.Range(1, 101);
        bool dodged = false;
        if(dodgeroll <= CurrentDodgeChance && dodgeable)
        {
            SoundManager.instance.PlaySound(dodge);
            Debug.Log("DODGED");
            Instantiate(DodgeTextEffect, transform.position, Quaternion.identity);
            StartCoroutine(DodgeOpacityEffect());
            if(Attacker.GetComponent<SweepingStrike>() != null && wasAnAttack) Attacker.GetComponent<SweepingStrike>().Activate(damage, gameObject, _battleManager);
            damage = 0;
            dodged = true;
        }
        // || THE IF-ELSE LADDER FOR TRAITS FOR BLOCKING DAMAGE ||
        bool DomForceBlock = false;
        if(gameObject.GetComponent<DominatingForce>() != null) DomForceBlock = gameObject.GetComponent<DominatingForce>().DomForceBlock(Attacker);
        if(DomForceBlock == true) CurrentBlockChance += 50f;
        // -----------------------------------------------------------------------------------
        // block
        float blockroll = Random.Range(1, 101);
        if(blockroll <= CurrentBlockChance && blockable && !dodged)
        {
            SoundManager.instance.PlaySound(block);
            onUnitDeath?.Invoke(isPlayer, gameObject); //<------ BLOCKED UNIT EFFECTS TRIGGER HERE (FOR TRAITS LIKE BULWORK)
            Debug.Log("BLOCKED");
            Instantiate(blockTextEffect, transform.position, Quaternion.identity);
            Instantiate(ShieldEffect, transform.position, Quaternion.identity);
            if(Attacker.GetComponent<SweepingStrike>() != null && wasAnAttack) Attacker.GetComponent<SweepingStrike>().Activate(damage, gameObject, _battleManager);
            damage = Mathf.Round(damage / 2);
            if(gameObject.GetComponent<Tank>() != null) Mathf.Round(damage/2);
            if(gameObject.GetComponent<ArcaneShield>() != null) gameObject.GetComponent<ArcaneShield>().Activate(_battleManager);
        }
        if(DomForceBlock == true) CurrentBlockChance -= 50f;
        // parry
        float parryroll = Random.Range(1, 101);
        if(parryroll <= CurrentParryChance && parryable)
        {
            Debug.Log("PARRY");
            Instantiate(ParryTextEffect, transform.position, Quaternion.identity);
            Attacker.GetComponent<UnitBattle>().Hit(CurrentDamage, false, true, true, false, false, gameObject); // attack enemy with damage
            // || THE IF-ELSE LADDER FOR TRAITS FOR PARRYING DAMAGE ||
            if(Attacker.GetComponent<MasterSwordsman>() != null) Attacker.GetComponent<MasterSwordsman>().Activate(gameObject);
            // -----------------------------------------------------------------------------------
        }

        if(damage > 0)
        {
            // sound
            if(wasAnAttack && !wasCrit) SoundManager.instance.PlaySound(hit);
            else if(wasAnAttack && wasCrit) SoundManager.instance.PlaySound(crit);
            else SoundManager.instance.PlaySound(damaged);
            // ---
            if(wasAnAttack && !wasCrit) StartCoroutine(UnitHitEffect()); // play hit effect if damage taken
            if(wasAnAttack && wasCrit) StartCoroutine(UnitCritHitEffect()); // play hit effect if damage taken
            if(!wasAnAttack) StartCoroutine(UnitDamagedEffect());
            GetComponent<StatusNumbersEffect>().ActivateAmount(damage, "damage", wasCrit); // text effect
            // || THE IF-ELSE LADDER FOR TRAITS FOR TAKING DAMAGE ||
            if(gameObject.GetComponent<Deflection>() != null && wasAnAttack)
            {
                damage = Mathf.Floor(damage / 2);
                GetComponent<Deflection>().DisableShield(damage, _battleManager);
            }
            if(gameObject.GetComponent<CurseEffect>() != null)
            {
                damage = damage * 2;
                Destroy(GetComponent<CurseEffect>());
            }
            if(gameObject.GetComponent<Conquerer>() != null) gameObject.GetComponent<Conquerer>().Activate();
            // -----------------------------------------------------------------------------------
        } 
        CurrentHealth -= damage;
        if(CurrentHealth <= 0) if(gameObject.GetComponent<UnrelentingWarrior>() != null) gameObject.GetComponent<UnrelentingWarrior>().Activate(damage);
        if(CurrentHealth <= 0)
        {
            isDead = true;
            // || THE IF-ELSE LADDER FOR TRAITS FOR DEATH ||
            if(gameObject.GetComponent<EtherealSpirit>() != null) gameObject.GetComponent<EtherealSpirit>().Activate();
            if(gameObject.GetComponent<ExplodingBloodEffect>() != null) gameObject.GetComponent<ExplodingBloodEffect>().Explode(_battleManager, thisUnitsIndex);
            if(Attacker.GetComponent<SpiritBeast>() != null) Attacker.GetComponent<SpiritBeast>().Activate(CurrentDamage);
            if(gameObject.GetComponent<FromTheGrave>() != null && wasAnAttack) gameObject.GetComponent<FromTheGrave>().Activate(Attacker);
            // -----------------------------------------------------------------------------------
            if(isDead == true) StartCoroutine(UnitDeathEffect());
            onUnitDeath?.Invoke(isPlayer, gameObject); //<------ DEATH OF UNIT EFFECTS TRIGGER HERE (FOR TRAITS LIKE GHOUL)
        }
        // || THE IF-ELSE LADDER FOR TRAITS FOR TAKING THAT RUN AFTER BEING HIT AT ALL ||
        if(gameObject.GetComponent<Evasion>() != null) gameObject.GetComponent<Evasion>().Activate();
        // -----------------------------------------------------------------------------------

        return damage;
    }

    // this small corutine is just for after a player has dodged, turn invis, wait a bit then return to normal
    private IEnumerator DodgeOpacityEffect()
    {
        List<SpriteRenderer> CompositeSprites = GetComponentInChildren<UnitSpriteManager>().CompositeSprites;
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            Color tmp = sprRend.GetComponent<SpriteRenderer>().color;
            tmp.a = .5f;
            sprRend.GetComponent<SpriteRenderer>().color = tmp;
        } 

        yield return new WaitForSeconds(1f);
        
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            Color tmp = sprRend.GetComponent<SpriteRenderer>().color;
            tmp.a = 1f;
            sprRend.GetComponent<SpriteRenderer>().color = tmp;
        } 
    }
    // this small corutine is just for getting HIT on an ATTACK
    private IEnumerator UnitHitEffect()
    {
        // Movement
        transform.position += new Vector3(.01f, 0, 0);
        // hit effect
        Instantiate(HitEffect, transform.position + new Vector3(0,0, 0f), Quaternion.identity);
        // white flash effect
        List<SpriteRenderer> CompositeSprites = GetComponentInChildren<UnitSpriteManager>().CompositeSprites;
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(255, 255, 255, 255);
            sprRend.material = FlashMaterial;
        } 
        // more movement
        yield return new WaitForSeconds(.15f);
        if(isDead == true) yield break;
        // make black
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(0, 0, 0, 255);
        }
        GetComponentInChildren<UnitSpriteManager>().SetHairColor(hairColorOriginal);
        GetComponentInChildren<UnitSpriteManager>().SetEyeColor(eyeColorOriginal);
        
        transform.position += new Vector3(-.03f, 0, 0);
        yield return new WaitForSeconds(.05f);
        if(isDead == true) yield break;
        // white flash effect
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(255, 255, 255, 255);
        } 
        transform.position += new Vector3(.02f, 0, 0);
        yield return new WaitForSeconds(.15f);

        // return all to original
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.material = originalMaterial;
        }
        GetComponentInChildren<UnitSpriteManager>().SetHairColor(hairColorOriginal);
        GetComponentInChildren<UnitSpriteManager>().SetEyeColor(eyeColorOriginal);
    }

    // this small corutine is just for taking ANY damage
    private IEnumerator UnitDamagedEffect()
    {
        List<SpriteRenderer> CompositeSprites = GetComponentInChildren<UnitSpriteManager>().CompositeSprites;
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(200, 0, 0, 255);
            sprRend.material = FlashMaterial;
        } 
        // more movement
        yield return new WaitForSeconds(.08f);
        if(isDead == true) yield break;

        // return all to original
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(255, 255, 255, 255);
            sprRend.material = originalMaterial;
        }
        GetComponentInChildren<UnitSpriteManager>().SetHairColor(hairColorOriginal);
        GetComponentInChildren<UnitSpriteManager>().SetEyeColor(eyeColorOriginal);
    }

    private IEnumerator UnitCritHitEffect()
    {
        Instantiate(CritTextEffect, transform.position, Quaternion.identity);
        // Movement
        transform.position += new Vector3(.01f, 0, 0);
        // hit effect
        Instantiate(HitEffect, transform.position + new Vector3(0,0, 0f), Quaternion.identity);
        // white flash effect
        List<SpriteRenderer> CompositeSprites = GetComponentInChildren<UnitSpriteManager>().CompositeSprites;
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(255, 0, 120, 255);
            sprRend.material = FlashMaterial;
        } 
        // more movement
        yield return new WaitForSeconds(.15f);
        if(isDead == true) yield break;
        // make black
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(0, 0, 0, 255);
        }
        GetComponentInChildren<UnitSpriteManager>().SetHairColor(hairColorOriginal);
        GetComponentInChildren<UnitSpriteManager>().SetEyeColor(eyeColorOriginal);
        
        transform.position += new Vector3(-.03f, 0, 0);
        yield return new WaitForSeconds(.05f);
        // white flash effect
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(255, 255, 255, 255);
        } 
        transform.position += new Vector3(.02f, 0, 0);
        yield return new WaitForSeconds(.15f);

        // return all to original
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.material = originalMaterial;
        }
        GetComponentInChildren<UnitSpriteManager>().SetHairColor(hairColorOriginal);
        GetComponentInChildren<UnitSpriteManager>().SetEyeColor(eyeColorOriginal);
    }

    // this small corutine is just for taking ANY damage
    private IEnumerator UnitDeathEffect()
    {
        SoundManager.instance.PlaySound(death);
        targetDisplayHUD.SetActive(false); // set UI for urself
        // Instantiate(DeathParticleEffect, transform.position, Quaternion.identity); honestly it just looks bad, will jus comment out
        // go away health bar!
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("HealthBar");   
        foreach(GameObject healthbar in taggedObjects)
        {
            if(healthbar.GetComponent<HealthBar>().getAssignedUnit() == gameObject) healthbar.transform.parent.GetComponent<Animator>().SetTrigger("GoAway");
        }

        List<SpriteRenderer> CompositeSprites = GetComponentInChildren<UnitSpriteManager>().CompositeSprites;
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(255, 255, 255, 255);
            sprRend.material = FlashMaterial;
        } 
        // more movement
        GetComponent<Rigidbody2D>().gravityScale = 0.5f;
        Vector2 launchDirection = new Vector2(Random.Range(-9, 10)*.1f, transform.up.y);
        GetComponent<Rigidbody2D>().AddForce(launchDirection * 2.5f, ForceMode2D.Impulse);
        transform.eulerAngles = new Vector3(0,0,Random.Range(-180, 181));

        yield return new WaitForSeconds(2.5f); // WAIIIIIIT
        transform.eulerAngles = new Vector3(0,0,0);

        transform.Rotate(0, 0.0f, 0.0f, Space.Self);
        // return all to original
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(255, 255, 255, 255);
            sprRend.material = originalMaterial;
        }
        GetComponentInChildren<UnitSpriteManager>().SetHairColor(hairColorOriginal);
        GetComponentInChildren<UnitSpriteManager>().SetEyeColor(eyeColorOriginal);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = 0f;
    }

    // this small corutine is just for taking ANY damage
    private IEnumerator UnitHealedEffect()
    {
        List<SpriteRenderer> CompositeSprites = GetComponentInChildren<UnitSpriteManager>().CompositeSprites;
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(162, 255, 31, 255);
            sprRend.material = FlashMaterial;
        } 
        // more movement
        yield return new WaitForSeconds(.08f);
        if(isDead == true) yield break;

        // return all to original
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(255, 255, 255, 255);
            sprRend.material = originalMaterial;
        }
        GetComponentInChildren<UnitSpriteManager>().SetHairColor(hairColorOriginal);
        GetComponentInChildren<UnitSpriteManager>().SetEyeColor(eyeColorOriginal);
    }

    public void Healed(float amount)
    {
        if(GetComponent<AntiHeal>() != null) return; // do not heal if has anti-heal status effect
        if(CurrentHealth == UnitData.maxHealth) return; // do not heal if max health (max health should maybe be a CurrentMaxHealth var instead of using unit data)
        GetComponent<StatusNumbersEffect>().ActivateAmount(amount, "heal", false); // text effect
        StartCoroutine(UnitHealedEffect());
        //play heal animation/sound etc
        SoundManager.instance.PlaySound(heal);
        CurrentHealth += amount;
        if(CurrentHealth > UnitData.maxHealth)
        {
            CurrentHealth = UnitData.maxHealth; // if the heal took us over max health, set health to max health NO overheal
            // this methodology is bad, should have a MAX HEALTH variable that is set at the start of the game, then can increase with things like GHOUL or buffs
        }
    }

    public bool getInjuryStatus()
    {
        if(CurrentHealth < UnitData.maxHealth) isInjured = true;
        else isInjured = false;

        return isInjured;
    }
    // --------------------

    public void SpecialStatsShift(int dayNum)
    {
        switch(dayNum)
        {
            case 1:
                GetComponent<UnitStats>().baseDamage -= 2;
                GetComponent<UnitStats>().maxHealth -= 7;
                GetComponent<UnitStats>().dodgeChance = 0;
                GetComponent<UnitStats>().critChance = 0;
            break;
            case 3:
                GetComponent<UnitStats>().baseDamage -= 2;
                GetComponent<UnitStats>().maxHealth -= 6;
                GetComponent<UnitStats>().dodgeChance = 0;
                GetComponent<UnitStats>().critChance = 0;
            break;
            case 4:
                GetComponent<UnitStats>().baseDamage -= 2;
                GetComponent<UnitStats>().maxHealth -= 6;
                GetComponent<UnitStats>().dodgeChance = 0;
                GetComponent<UnitStats>().critChance = 0;
            break;
            case 6:
                GetComponent<UnitStats>().baseDamage -= 2;
                GetComponent<UnitStats>().maxHealth -= 5;
                GetComponent<UnitStats>().dodgeChance = 0;
                GetComponent<UnitStats>().critChance = 0;
            break;
            case 7:
                GetComponent<UnitStats>().baseDamage -= 1;
                GetComponent<UnitStats>().maxHealth -= 5;
                GetComponent<UnitStats>().dodgeChance = 0;
                GetComponent<UnitStats>().critChance = 0;
            break;
            default:
                //
            break;
        }
    }
}
