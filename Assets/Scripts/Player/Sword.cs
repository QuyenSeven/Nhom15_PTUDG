using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour ,IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPonit;
    //[SerializeField] private float swordAttackCD = .5f;
    [SerializeField] private WeaponInfor weaponInfor;

    private Transform weaponCollider;
    private Animator myAnimator;
    private Vector2 movement;
    private GameObject slashAnim;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPonit = GameObject.Find("SlashSpawnPoint").transform;
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public WeaponInfor GetWeaponInfor()
    {
        return weaponInfor;
    }

    public void Attack()
    {
        
            myAnimator.SetTrigger("Attack");
            weaponCollider.gameObject.SetActive(true);
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPonit.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;; 
    }

    public void DoneAttackingAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);
    }

    public void SwingUpFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(- 180,0,0);

        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }    
    public void SwingDownFlipAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0,0,0);

        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }



    private void MouseFollowWithOffset()
    {
       

        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angel = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;


            if (mousePos.x < playerScreenPoint.x)
            {
                ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angel);
                weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
            }
            else
            {
                ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angel);
                weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        
    }
}
