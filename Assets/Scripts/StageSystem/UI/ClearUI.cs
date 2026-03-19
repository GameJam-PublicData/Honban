using System;
using UnityEngine;

public class ClearUI : MonoBehaviour
{
    [SerializeField] GameObject clearUI;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            clearUI.SetActive(true);
        }
    }
}
