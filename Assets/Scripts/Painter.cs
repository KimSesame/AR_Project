using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Painter : MonoBehaviour
{
    public abstract void Paint();
    public abstract void SelectColor(Color color);
}
