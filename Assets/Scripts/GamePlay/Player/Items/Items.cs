using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour{
    


    public Animator animator;
    
    private bool useSheild;

    private PlayerControl shield;
    private GamePlayItem shieldItem;

    private PlayerControl mobilePhone;
    private GamePlayItem mobilePhoneItem;

    private GamePlayItem[] items = new GamePlayItem[7];

    private PlayerController playerController;
    private MobilePhone mobile;

    GameController instance;
    DataController data;

    IEnumerator Start() {
        
        instance = GameController.instance;
        data = instance.dataController;

        playerController = transform.root.GetComponent<PlayerController>();
        animator = GetComponent<Animator>();

        mobile = instance.GetComponentForReference<MobilePhone>();

        while (mobile == null) {

            print("WWW");

            mobile = instance.GetComponentForReference<MobilePhone>();

            yield return new WaitForSeconds(.1f);

        }
        
        shield = transform.root.Find("Controls/Controller/Inventory/SheildItem/ClickItem").GetComponent<PlayerControl>();
        shieldItem = shield.GetComponentInParent<GamePlayItem>();

        items[2] = shieldItem;

        mobilePhone = transform.root.Find("Controls/Controller/Inventory/PhoneItem/ClickItem").GetComponent<PlayerControl>();
        mobilePhoneItem = mobilePhone.GetComponentInParent<GamePlayItem>();

        items[1] = mobilePhoneItem;

        useSheild = false;
    }

    void Update() {

        if (shield.isPressed) {

            StartCoroutine("UsingShield");

            shield.isPressed = false;

        }

        if (mobilePhone.isPressed) {

            if (mobile == null)
                return;

            if (!mobile.gameObject.activeInHierarchy) {

                bool canUseItem = UseItem(1);

                if (canUseItem) {
                    mobile.UseMobilePhone(transform.position);
                }

            }

            mobilePhone.isPressed = false;
        }

    }

    public IEnumerator UsingShield() {
        
        if (!useSheild) {

            bool canUseShield = UseItem(2);
           
            if (canUseShield) {

                useSheild = true;

                playerController.playerDamageable = !useSheild;

                animator.SetBool("Shield", useSheild);

                yield return new WaitForSeconds(5f);

                useSheild = false;

                animator.SetBool("Shield", useSheild);

                playerController.playerDamageable = !useSheild;

            }
        }
        
    }


    bool UseItem(int index) {

        DataFile dta = data.dataFile;

        Inventory item = dta.items[index];

        int count = item.item_count;

        if (count > 0) {
            count--;

            item.item_count = count;

            dta.items[index] = item;

            data.dataFile = dta;

            items[index].UsedItem(count);




            return true;

        }

        return false;


    }

    




}
