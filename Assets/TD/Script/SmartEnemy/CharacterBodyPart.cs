using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.Examples;

public class CharacterBodyPart : MonoBehaviour, ICanTakeDamageBodyPart
{
    public GameObject owner;
    public BODYPART _bodyPart;
    public int multipleDamageX = 1;
    public GameObject hitFX;
    public GameObject ownerHitFX;
    [Range(0,1)]
    public float soundHitVol = 0.5f;
    public AudioClip[] headHit;
    public Vector2 healthBarOffset = new Vector2(0, 1.5f);
    
    
    public bool allowPushBack = false;
    public bool allowKnockDown = false;
    public bool allowShock = false;

    Enemy ownerDamage;

    public void TakeDamage(float damage, Vector2 force, Vector2 hitPosition, GameObject instigator, WeaponEffect weaponEffect = null, float pushBackPercent = 0, float knockDownRagdollPercent = 0, float shockPercent = 0)
    {
        damage *= multipleDamageX;

        ownerDamage.TakeDamage(damage, hitPosition, force, instigator, _bodyPart, weaponEffect);

        if (hitFX)
        {
            Instantiate (hitFX, transform.position, hitFX.transform.rotation);
            SoundManager.PlaySfx (headHit, soundHitVol);
            //SpawnSystemHelper.GetNextObject(hitFX, true).transform.position = hitPosition;
        }

        if (ownerHitFX)
        {
            ownerHitFX.SetActive(false);
            ownerHitFX.SetActive(true);
        }
        
    }

    bool canDoAction(float percent)
    {
        if (percent == 0)
            return false;

        if (Random.Range(0f, 1f) < percent)
            return true;
        else
            return false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        ownerDamage = owner.GetComponent<Enemy>();
        if (ownerHitFX)
            ownerHitFX.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
