﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DishEffect : BaseEffect
{
    public override void ApplyEffect()
    {
        // repopulate
        GameManager.Instance.ReloadLevel();
    }
}
