﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KYC_Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataHandler.WaterLog[] test = DataHandler.GetTempWaterData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
