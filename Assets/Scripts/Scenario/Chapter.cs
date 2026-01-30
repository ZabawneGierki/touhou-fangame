using System;
using UnityEngine;


[Serializable]
public class Chapter
{
    public string title;
    [TextArea] public string description;
    public Wave[] waves;
    public Boss bossPrefab;
    public MiniBoss miniBossPrefab;


}
