using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtherealSpirit : MonoBehaviour
{
    bool used = false;
    // need to revoke the damage from unit, and then set isdead to false in unit battle
    void Awake()
    {
        BattleManager.onGameEnd += GameEnd;
        used = false;
    }
    public void Activate()
    {
        if(used)
        {
            UnSubscribe();
            return;
        }
        GetComponent<UnitBattle>().CurrentHealth = 10;
        GetComponent<UnitBattle>().CurrentDamage = 5;
        GetComponent<UnitBattle>().isDead = false;
        used = true;
        
        List<SpriteRenderer> CompositeSprites = GetComponentInChildren<UnitSpriteManager>().CompositeSprites;
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(255, 190, 0, 210);
        } 

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Ethereal Spirit", false);
        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[2], transform.position, Quaternion.identity);
    }

    void GameEnd()
    {
        List<SpriteRenderer> CompositeSprites = GetComponentInChildren<UnitSpriteManager>().CompositeSprites;
        // return all to original
        foreach(SpriteRenderer sprRend in CompositeSprites)
        {
            sprRend.color = new Color32(255, 255, 255, 255);
        }
        Color hairColorOriginal = GetComponentInChildren<UnitSpriteManager>().GetHairColor();
        Color eyeColorOriginal = GetComponentInChildren<UnitSpriteManager>().GetEyeColor();
        GetComponentInChildren<UnitSpriteManager>().SetHairColor(hairColorOriginal);
        GetComponentInChildren<UnitSpriteManager>().SetEyeColor(eyeColorOriginal);

        UnSubscribe();
    }

    void UnSubscribe()
    {
        BattleManager.onGameEnd -= GameEnd;
    }
}
