using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemObjectParent {

    public Transform GetItemHoldLocation();

    public void SetItem(Item item);

    public Item GetItem();

    public void ClearItem();

    public bool HasItem();


}
