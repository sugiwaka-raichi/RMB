﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAttack : AttackBase
{
    // Start is called before the first frame update
    void Start()
    {
        //有効時間
        Destroy(this.gameObject, timer);
        //攻撃属性
        atkType = ATK_TYPE.AT_WARTER;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
