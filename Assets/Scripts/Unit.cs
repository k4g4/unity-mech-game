using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int health;
    public int maxHealth;
    public int actionPoints;
    public int maxActionPoints;
    public int attack;
    public int unitType = 0;
    public float walkSpeed = 2f;
    public GameObject footprint;
    public List<GameObject> initWeapons = new List<GameObject>();
    public List<Weapon> weapons = new List<Weapon>();
    public bool isWalking = false;
    Vector3 movePos = Vector3.zero;
    Animator anim;
    public UnitStatus us;
    public GameObject usObj;
    public bool setWeapons = true;
    public GameObject deathFX;
    bool isDead = false;
    public List<GameObject> aiWaypoints = new List<GameObject>();
    AudioSource audSoc;
    public AudioClip walkSFX;
    public UnitSidePanel usp;
    public GameObject unitSidePanel;

    void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        us = Instantiate(usObj,FindObjectOfType<Canvas>().transform).GetComponent<UnitStatus>();
        us.transform.SetAsFirstSibling();
        us.unit = this;
        if (health > maxHealth)
            health = maxHealth;
        audSoc = GetComponent<AudioSource>();
        audSoc.Stop();
        if(gameObject.layer==10)
        {
            GameObject clone = Instantiate(unitSidePanel, GameObject.Find("UnitInfoSidePanel").transform);
            clone.GetComponent<UnitSidePanel>().unit = this;
            usp = clone.GetComponent<UnitSidePanel>();
        }
    }

    void Start()
    {
        if(setWeapons)
            SetWeapons();
        us.UpdateInfo();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 5, 1 << 9))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + 0.5f, transform.position.z);
        }
        UpdateInfo();
    }

    void UpdateInfo()
    {
        if (usp)
            usp.UpdateInfo();
        us.UpdateInfo();
    }

    void SetWeapons()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<Weapon>())
            {
                weapons.Add(child.GetComponent<Weapon>());
            }
        }

        for(int i=0;i<initWeapons.Count;i++)
        {
            if (!initWeapons[i])
            {
                continue;
            }
            GameObject clone = Instantiate(FindObjectOfType<PartDict>().partDict[initWeapons[i].GetComponent<Weapon>().weaponID], transform);
            clone.GetComponent<Weapon>().partPos = i;
            weapons.Add(clone.GetComponent<Weapon>());
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            if(unitType==0)
            {
                switch (weapons[i].partPos)
                {
                    case 0:
                        weapons[i].transform.SetParent(transform.Find("Model/Armature.001/body.001/LArm"));
                        weapons[i].transform.localPosition = Vector3.zero;
                        break;
                    case 1:
                        weapons[i].transform.SetParent(transform.Find("Model/Armature.001/body.001/RArm"));
                        weapons[i].transform.localPosition = Vector3.zero;
                        break;
                    case 2:
                        weapons[i].transform.SetParent(transform.Find("Model/Armature.001/body.001/LShoulder"));
                        weapons[i].transform.localPosition = Vector3.zero + Vector3.up * 0.0008f + Vector3.right * 0.0004f;
                        break;
                    case 3:
                        weapons[i].transform.SetParent(transform.Find("Model/Armature.001/body.001/RShoulder"));
                        weapons[i].transform.localPosition = Vector3.zero + Vector3.up * 0.0008f + Vector3.left * 0.0004f;
                        break;
                    default:
                        Debug.Log("ERROR: Invalid part position - " + unitName + "|" + weapons[i].wepName);
                        break;
                }
            }
            else if(unitType==1)
            {
                switch (weapons[i].partPos)
                {
                    case 0:
                        weapons[i].transform.SetParent(transform.Find("Model/Armature/body.001/body/LArm"));
                        weapons[i].transform.localPosition = Vector3.zero;
                        break;
                    case 1:
                        weapons[i].transform.SetParent(transform.Find("Model/Armature/body.001/body/RArm"));
                        weapons[i].transform.localPosition = Vector3.zero;
                        break;
                    case 2:
                        weapons[i].transform.SetParent(transform.Find("Model/Armature/body.001/body/LShoulder"));
                        weapons[i].transform.localPosition = Vector3.zero;
                        break;
                    case 3:
                        weapons[i].transform.SetParent(transform.Find("Model/Armature/body.001/body/RShoulder"));
                        weapons[i].transform.localPosition = Vector3.zero;
                        break;
                    default:
                        Debug.Log("ERROR: Invalid part position - " + unitName + "|" + weapons[i].wepName);
                        break;
                }
            }
        }
    }

    void Update()
    {
        if(isWalking)
        {
            //transform.position = Vector3.MoveTowards(transform.position, movePos, walkSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
            RaycastHit hit;
            if(Physics.Raycast(transform.position,-Vector3.up,out hit,5,1<<9))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + 0.5f, transform.position.z);
            }
            if(!audSoc.isPlaying)
            {
                audSoc.Play();
            }
        }
        else
        {
            if(audSoc.isPlaying)
            {
                audSoc.Stop();
            }
        }
    }

    public void Move(Vector3 targetPos)
    {
        isWalking = true;
        IsWalking(true);
        transform.LookAt(targetPos); //Temporary fix, need to lock and lerp rotation
        movePos = targetPos;
    }

    public void IsWalking(bool walking)
    {
        anim.SetBool("isWalking", walking);
    }

    public void isHit()
    {
        if(anim)
        {
            anim.SetBool("isHit", true);
            Timing.RunCoroutine(AnimHitTimer(),"anim");
        }
    }

    public void Damage(int hp)
    {
        health -= hp;
        us.UpdateInfo();
        if(usp)
            usp.UpdateInfo();
        if(health < 1&&!isDead)
        {
            isDead = true;
            GameObject clone = Instantiate(deathFX);
            clone.transform.position = transform.position;
            gameObject.layer = 0;
            FindObjectOfType<PlayerController>().RemoveUnit(GetComponent<Unit>());
            Timing.KillCoroutines("anim");
            Timing.RunCoroutine(DeathTimer());
        }
    }

    IEnumerator<float> DeathTimer()
    {
        yield return Timing.WaitForSeconds(5f);
        GameObject clone = Instantiate(deathFX);
        clone.transform.position = transform.position;
        Destroy(us.gameObject);
        Destroy(gameObject);
    }

    IEnumerator<float> AnimHitTimer()
    {
        yield return Timing.WaitForSeconds(2f);
        if (isDead)
            yield break;
        anim.SetBool("isHit", false);
    }

    public void Attack(Unit attacker, Unit defender, int wep) //Just reduces health, add things like accuracy and rng later
    {
        IsWalking(false);
        transform.LookAt(defender.transform.position); //Temporary fix, need to lock and lerp rotation
        weapons[wep].Fire(defender);
        //defender.health -= attacker.attack;
    }
}
