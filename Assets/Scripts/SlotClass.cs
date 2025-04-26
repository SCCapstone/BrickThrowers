using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Don't serizalize? Should be managed by code entirely.
[System.Serializable]
public class SlotClass {
  [SerializeField] private ItemClass item;

  public SlotClass(ItemClass item) {
    this.item = item;
  }
  public SlotClass(SlotClass slot) {
    item = slot.item;
  }
  public SlotClass() {
    item = null;
  }

  public ItemClass Item {
    get { return item; }
    set { item = value; }
  }

  public void Clear() {
    item = null;
  }
}
