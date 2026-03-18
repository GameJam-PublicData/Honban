using System;
using UnityEngine;

public class ClearUI : MonoBehaviour
{
    [SerializeField] GameObject clearUI;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            clearUI.SetActive(true);
        }
    }
}
