using System;
using UnityEngine;

public class ClearUI : MonoBehaviour
{
    [SerializeField] GameObject clearUI;

    void Start()
    {
        clearUI.SetActive(false);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            clearUI.SetActive(true);
        }
    }
}
