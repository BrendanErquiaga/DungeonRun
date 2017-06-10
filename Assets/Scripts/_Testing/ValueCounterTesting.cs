using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueCounterTesting : MonoBehaviour
{
    HealthComponent hc;
	// Use this for initialization
	void Start () {
        hc = this.gameObject.GetComponent<HealthComponent>();
        hc.HealthValueChanged += handleHealthValueChanged;
        hc.Died += itDied;
    }

    void handleValueChanged()
    {
        Debug.Log("Value Changed: Current: " + hc.CurrentValue.ToString() + "| Starting: " + hc.StartingValue.ToString());
    }

    void handleHealthValueChanged()
    {
        Debug.Log("Health Value Changed: Current: " + hc.CurrentValue.ToString() + "| Starting: " + hc.StartingValue.ToString());
    }

    void itDied()
    {
        Debug.Log("It Died: Current: " + hc.CurrentValue.ToString() + "| Starting: " + hc.StartingValue.ToString());
        this.gameObject.GetComponent<Animator>().enabled = false;
    }
}
