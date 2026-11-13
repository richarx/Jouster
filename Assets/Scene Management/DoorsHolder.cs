using System;
using System.Collections.Generic;
using System.Linq;
using Map;
using UnityEngine;

public class DoorsHolder : MonoBehaviour
{
    public static DoorsHolder instance;

    private List<DoorTrigger> doors = new List<DoorTrigger>();

    private void Awake()
    {
        instance = this;
    }

    public void RegisterNewDoor(DoorTrigger door)
    {
        Debug.Log("Register new Door");
        doors.Add(door);
    }

    public void ResetDoors()
    {
        doors.Clear();
    }

    public DoorTrigger GetDoor(Cities targetCity)
    {
        return doors.FirstOrDefault((d) => d.TargetCity == targetCity);
    }
}
