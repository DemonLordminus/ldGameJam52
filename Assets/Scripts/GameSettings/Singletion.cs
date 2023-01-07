using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletion<T> : MonoBehaviour where T : Singletion<T>
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        if(instance!=null)
            Destroy(gameObject);
        else
            instance=(T)this;
    }

    public static bool IsInitialized
    {
        get { return instance!=null; }
    }

    protected virtual void OnDestroy()
    {
        if(instance==this)
        {
            instance=null;
        }
    }
}
