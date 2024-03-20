﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;


public class Player_Archer : MonoBehaviour
{
    PlayerSetIKPosition playerSetIKPosition;

   
    [Header("ARROW SHOOT")]
    public float shootRate = 1;
    public float force = 20;
    [ReadOnly] public float extraForce = 0; //from shop upgrade
    //[ReadOnly]
    [Range(0.01f, 0.1f)]
    [ReadOnly] public float stepCheck = 0.1f;
    [ReadOnly] public float stepAngle = 1;
    [ReadOnly] public float gravityScale = 3.5f;
    [ReadOnly] public bool onlyShootTargetInFront = true;

    [Header("ARROW DAMAGE")]
    public ArrowProjectile arrow;
    public WeaponEffect weaponEffect;

    [Header("ARROW SETUP - SPINE")]
    [SpineBone]
    public string fireBone;
    Bone fireBoneBone;

    [Header("Sound")]
    public float soundShootVolume = 0.5f;
    public AudioClip[] soundShoot;

    SkeletonMecanim skeleton;

    private float x1, y1;
    bool isTargetRight = false;
    float lastShoot;
    ARCHER_FIRE_ARROWS numberArrow = ARCHER_FIRE_ARROWS.ONE;
    ARCHER_ABILITY ability = ARCHER_ABILITY.NONE;

    [ReadOnly] public bool isAvailable = true;

    
    Animator anim;

    bool isFacingRight { get { return transform.rotation.eulerAngles.y == 180 ? true : false; }
        set { } }

    // Start is called before the first frame update
    void Start()
    {
        skeleton = GetComponent<SkeletonMecanim>();
        fireBoneBone = skeleton.skeleton.FindBone(fireBone);
        anim = GetComponent<Animator>();

        extraForce = force * GlobalValue.LongShootExtra;
        force += extraForce;

        playerSetIKPosition = GetComponent<PlayerSetIKPosition>();
    }

    public void Victory()
    {
        anim.SetBool("victory", true);
    }

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Shoot();
    //    }
    //}

    //call by owner script
    
    public void Shoot(ARCHER_FIRE_ARROWS _numberArrow = ARCHER_FIRE_ARROWS.ONE, ARCHER_ABILITY _ablity = ARCHER_ABILITY.NONE)
    {
        if (!isAvailable)
            return;

        isTargetRight = Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x;

        
        if (onlyShootTargetInFront && ((isTargetRight && !isFacingRight) || (isFacingRight && !isTargetRight)))     //if ticked onlyShootTargetInFront, then check if target behide character or not
            return;

        numberArrow = _numberArrow;
        ability = _ablity;
        StartCoroutine(CheckTarget());
    }


    IEnumerator CheckTarget()
    {
        Vector3 mouseTempLook = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseTempLook -= transform.position;
        mouseTempLook.x *= (isFacingRight ? -1 : 1);
        playerSetIKPosition.SetAnimIK(transform.position + mouseTempLook);
        yield return null;

        

        Vector2 fromPosition = fireBoneBone.GetWorldPosition(transform);
        Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Debug.LogError(fromPosition + "to" + target);
        //Debug.Break();

        float beginAngle = Vector2ToAngle(target - fromPosition);
        //Debug.LogError("beginAngle" + beginAngle);
        Vector2 ballPos = fromPosition;


        float cloestAngleDistance = int.MaxValue;

        bool checkingPerAngle = true;
        while (checkingPerAngle)
        {
            int k = 0;
            Vector2 lastPos = fromPosition;
            bool isCheckingAngle = true;
            float clostestDistance = int.MaxValue;

            while (isCheckingAngle)
            {
                Vector2 shotForce = force * AngleToVector2(beginAngle);
                x1 = ballPos.x + shotForce.x * Time.fixedDeltaTime * (stepCheck * k);    //X position for each point is found
                y1 = ballPos.y + shotForce.y * Time.fixedDeltaTime * (stepCheck * k) - (-(Physics2D.gravity.y * gravityScale) / 2f * Time.fixedDeltaTime * Time.fixedDeltaTime * (stepCheck * k) * (stepCheck * k));    //Y position for each point is found

                float _distance = Vector2.Distance(target, new Vector2(x1, y1));
                if (_distance < clostestDistance)
                    clostestDistance = _distance;

                if ((y1 < lastPos.y) && (y1 < target.y))
                    isCheckingAngle = false;
                else
                    k++;

                lastPos = new Vector2(x1, y1);
            }

            if (clostestDistance >= cloestAngleDistance)
                checkingPerAngle = false;
            else
            {
                cloestAngleDistance = clostestDistance;

                if (isTargetRight)
                    beginAngle += stepAngle;
                else
                    beginAngle -= stepAngle;
                //if (beginAngle <= maxAngle)
                //    checkingPerAngle = false;
            }
        }

        //set ik spine
        var lookAt = AngleToVector2(beginAngle) * 10;
        lookAt.x *= (isFacingRight ? -1 : 1);
        playerSetIKPosition.SetAnimIK((Vector2)transform.position + lookAt);

        yield return null;
        anim.SetTrigger("shoot");

       

        
        //WEAPON_EFFECT effectType;
        switch (ability)
        {
            case ARCHER_ABILITY.POSION:
                weaponEffect.effectType = WEAPON_EFFECT.POISON;
                //effectType = WEAPON_EFFECT.POISON;
                break;
            case ARCHER_ABILITY.FREEZE:
                weaponEffect.effectType = WEAPON_EFFECT.FREEZE;
                //effectType = WEAPON_EFFECT.FREEZE;
                break;
            default:
                weaponEffect.effectType = WEAPON_EFFECT.NORMAL;
                //effectType = WEAPON_EFFECT.NORMAL;
                break;
        }

        //Fire number arrow
        ArrowProjectile _tempArrow;
        switch (numberArrow)
        {
            case ARCHER_FIRE_ARROWS.DOUBLE:
                _tempArrow = Instantiate(arrow, fromPosition, Quaternion.identity);
                _tempArrow.Init(force * AngleToVector2(beginAngle + 1.5f), gravityScale, weaponEffect);
                //shot second arrow
                _tempArrow = Instantiate(arrow, fromPosition, Quaternion.identity);
                _tempArrow.Init(force * AngleToVector2(beginAngle - 1.5f), gravityScale, weaponEffect);
                break;
            case ARCHER_FIRE_ARROWS.TRIPLE:
                _tempArrow = Instantiate(arrow, fromPosition, Quaternion.identity);
                _tempArrow.Init(force * AngleToVector2(beginAngle + 1.5f), gravityScale, weaponEffect);
                //shot second arrow
                _tempArrow = Instantiate(arrow, fromPosition, Quaternion.identity);
                _tempArrow.Init(force * AngleToVector2(beginAngle), gravityScale, weaponEffect);
                //shot third arrow
                _tempArrow = Instantiate(arrow, fromPosition, Quaternion.identity);
                _tempArrow.Init(force * AngleToVector2(beginAngle - 1.5f), gravityScale, weaponEffect);
                break;
            default:
                _tempArrow = Instantiate(arrow, fromPosition, Quaternion.identity);
                _tempArrow.Init(force * AngleToVector2(beginAngle), gravityScale, weaponEffect);
                break;
        }


        SoundManager.PlaySfx(soundShoot[Random.Range(0, soundShoot.Length)], soundShootVolume);
        
        StartCoroutine(ReloadingCo());
    }

    IEnumerator ReloadingCo()
    {
        isAvailable = false;
        lastShoot = Time.time;

        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isLoading", true);

        while ( Time.time < (lastShoot + shootRate)) { yield return null; }

        anim.SetBool("isLoading", false);

        yield return new WaitForSeconds(0.2f);

        isAvailable = true;
    }


    public static Vector2 AngleToVector2(float degree)
    {
        Vector2 dir = (Vector2)(Quaternion.Euler(0, 0, degree) * Vector2.right);

        return dir;
    }

    public float Vector2ToAngle(Vector2 vec2)
    {
        var angle = Mathf.Atan2(vec2.y, vec2.x) * Mathf.Rad2Deg;
        return angle;
    }
}
