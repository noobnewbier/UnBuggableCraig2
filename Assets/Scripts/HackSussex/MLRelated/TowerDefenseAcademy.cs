using UnityEngine;
using MLAgents;
using System;

public class TowerDefenseAcademy : Academy
{
    public static TowerDefenseAcademy Instance { get; private set; }

    public override void InitializeAcademy()
    {
        base.InitializeAcademy();
        Instance = this;
        //Monitor.SetActive(true);
    }

    public override void AcademyReset()
    {
        //mysteriously called twice, fk this method
        Debug.Log("mysteriously reset");
    }
}
