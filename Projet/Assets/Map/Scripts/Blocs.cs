﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bloc
{
    public bool artefact;
    public string type;
    public int rotate;
    public int xpos, zpos;
    public GameObject obj;
    
    public Bloc ()
    {
        type = null;
        rotate = Random.Range(1,4);
        artefact = Random.Range(1,11) == 1;
        obj = null;
        
    }

    
}
