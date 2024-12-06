using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private HashSet<string> keys = new HashSet<string>();

    private void Start()
    {
        AddKey("password");
    }

    public void AddKey(string keyName)
    {
        keys.Add(keyName);
    }

    public bool HasKey(string keyName)
    {
        return keys.Contains(keyName);
    }
}
