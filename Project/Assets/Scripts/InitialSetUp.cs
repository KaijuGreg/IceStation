using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialSetUp : MonoBehaviour {

    [SerializeField] private Structure Table01;
    [SerializeField] private Structure Table02; 
    [SerializeField] private Structure Table03;
    [SerializeField] private ItemSO keyPink;
    [SerializeField] private ItemSO keyBlue;
    [SerializeField] private Player player;

    [SerializeField] private bool testing;

    private void Awake() {

       // Initial setup of keys...
        Transform item01 = Instantiate(keyBlue.prefab, Table01.GetItemHoldLocation());
        item01.GetComponent<Item>().SetItemObjectParent(Table01);
        Transform item02 = Instantiate(keyPink.prefab, Table02.GetItemHoldLocation());
        item02.GetComponent<Item>().SetItemObjectParent(Table02);

        Debug.Log(item01.GetComponent<Item>().GetItemSO().itemName + " knows it is on " + Table01.GetItem().GetItemObjectParent());
        Debug.Log(item02.GetComponent<Item>().GetItemSO().itemName + " knows it is on " + Table02.GetItem().GetItemObjectParent());

    }

    private void Update() {

        if (testing && Input.GetKeyDown(KeyCode.T)) { 
            
            Table01.GetItem().GetComponent<Item>().SetItemObjectParent(Table03);
        
        }
    }




}
