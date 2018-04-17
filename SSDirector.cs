using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSDirector : Object
{
    private static SSDirector instance;
    public FirstSceneController currentSceneController { get; set; }
    public static SSDirector getInstance()
    {
        if (instance == null)
        {
            instance = new SSDirector();
        }
        return instance;
    }

    private SSDirector()
    {

    }
}