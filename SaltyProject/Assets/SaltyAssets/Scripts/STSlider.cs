using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    // Update is called once per frame
    void Update()
    {
        slider.value = (float)PlayerInfo.Stamina;
    }
    private void FixedUpdate()
    {
        PlayerInfo.RestoreStaminaOnTick();
    }
}
