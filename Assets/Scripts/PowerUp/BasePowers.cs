using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerState
{
    ready,
    active,
    cooldown,
    deactivate
}

public enum PowerTypes
{
    None,
    Multiplier,
    Spring,
    TimeSlow,
    SingleClear,
    RowClear
    
}

public interface PowerUpsBaseMethods
{
    void OnMultiplierActivate();
    void OnMultiplierDeactivate();
    void OnSpringActivate();
    void OnSpringDeactivate();
    void OnTimeSlowActivate();
    void OnTimeSlowDeactivate();
    void OnSingleClearActivate();
    void OnSingleClearDeactivate();
    void OnRowClearActivate();
    void OnRowClearDeactivate();
}