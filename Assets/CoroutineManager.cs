using System.Collections;
using System.Collections.Generic;
// CoroutineManager.cs
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartManagedCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }
}
