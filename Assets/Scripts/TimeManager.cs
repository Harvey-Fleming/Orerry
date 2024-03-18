using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    [SerializeField] private float timeScale;

    [SerializeField] [ReadOnly] private float simulationSecond = 1f;

    public float TimeScale { get => timeScale; }
    public float SimulationSecond { get => simulationSecond;}

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Update()
    {
        simulationSecond = 1 / timeScale;
    }
}
