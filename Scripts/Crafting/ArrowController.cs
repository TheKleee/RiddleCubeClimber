using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [Header("Riddle Craft Ref:")]
    public RiddleCraft rCraft;

    [Header("Behaviour:")]
    public bool increment;

    public void ClickArrow()
    {
        rCraft.ChangeColor(increment);
    }
}
