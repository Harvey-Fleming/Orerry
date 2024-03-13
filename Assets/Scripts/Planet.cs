using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Orrery/Planet Data")]
public class Planet : ScriptableObject
{
    [SerializeField] private string planetName;
    [TextArea]
    [SerializeField] private string planetDesc;
    [SerializeField] private string planetTilt;
    [SerializeField] private string planetDist;

    public string PlanetName { get => planetName; }
    public string PlanetDesc { get => planetDesc; }
    public string PlanetTilt { get => planetTilt; }
    public string PlanetDist { get => planetDist; }
}
