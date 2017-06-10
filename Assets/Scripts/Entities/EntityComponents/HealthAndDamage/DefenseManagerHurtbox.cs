using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseManagerHurtbox : Hurtbox {

    [SerializeField]
    private DefenseManager defenseManager;

    public DefenseManager DefenseManager
    {
        get
        {
            return defenseManager;
        }

        set
        {
            defenseManager = value;
        }
    }
}
