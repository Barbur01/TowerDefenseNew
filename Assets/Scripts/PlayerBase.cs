using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public delegate void PlayerBaseTouched(int damage);
    public static event PlayerBaseTouched OnPlayerBaseTouched;

    void OnTriggerEnter(Collider other)
    {
        if (OnPlayerBaseTouched != null)
        {
            int damage = other.gameObject.GetComponent<Creep>().GetDamage();
            OnPlayerBaseTouched(damage);
        }

        Destroy(other.gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
