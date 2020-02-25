using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Armour {
    public string chest = "none";
    public string legs = "none";
    public string head = "none";

    public Armour(string chest, string legs, string head) {
        this.chest = chest;
        this.legs = legs;
        this.head = head;
    }

    public Armour(ArmourManager source) {
        this.chest = source.chest;
        this.legs = source.legs;
        this.head = source.head;
    }
}
