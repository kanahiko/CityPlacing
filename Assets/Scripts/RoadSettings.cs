using UnityEngine;
using System.Collections;

public class RoadSettings: ScriptableObject
{
    public int smallCurvePolyCount = 3;
    public int bigCurvePolyCount = 5;

    [Range(0.1f,1f)]
    [Tooltip(tooltip:"Full road width in percent")]
    public float fullWidth = 0.8f;
    public float roadHeight = 0.1f;

    [Range(0.1f, 1f)]
    [Tooltip(tooltip: "Side walk width in percent")]
    public float sideWidth = 0.2f;
    public float sideWalkHeight = 0.1f;

    public Color roadColor = Color.black;
    public Color sideWalkColor = Color.gray;
}
