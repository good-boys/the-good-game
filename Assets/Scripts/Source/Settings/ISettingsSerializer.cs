using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettingsSerializer
{
    float RetrieveMusicVolume();
    float RetrieveSFXVolume();

    void SerializeMusicVolume(float volume);
    void SerializeSFXVolume(float volume);
}
