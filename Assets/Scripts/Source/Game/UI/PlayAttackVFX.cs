using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayAttackVFX : MonoBehaviour {

    [SerializeField]
    List<Sprite> AttackVFX = new List<Sprite>();

    [SerializeField]
    List<AudioClip> AttackSFX = new List<AudioClip>();

    [SerializeField]
    Image currentImage;

    [SerializeField]
    float fadeTime = 1;

    [SerializeField]
    float fadeSpeed = 1;

    [SerializeField]
    float fadeDelay = 0.5f;

    [SerializeField]
    AudioSource source;

    private void Start()
    {
        GameFlowManager.instance.audioManager.RegisterAudioSource(source);
    }

    public void PlaySFX(string sfxName)
    {
        AudioClip targetVFX = null;
        foreach (AudioClip clip in AttackSFX)
        {
            if (clip.name == sfxName)
            {
                targetVFX = clip;
            }
        }

        if (targetVFX == null)
        {
            Debug.LogWarning("Attack SFX Clip not found");
            return;
        }

        GameFlowManager.instance.audioManager.Play(transform.name, targetVFX, new AudioOptions(0,0,1,false,true));
    }

    public void PlayVFX(string vfxName)
    {
        Sprite targetVFX = null;
        foreach (Sprite sprite in AttackVFX)
        {
            if (sprite.name == vfxName)
            {
                targetVFX = sprite;
            }
        }

        if (targetVFX == null)
        {
            Debug.LogWarning("Attack VFX Sprite not found");
            return;
        }

        currentImage.sprite = targetVFX;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        //TODO maybe: If we want to add complicated attack animations we can create a WaitForAnimation routine instead
        currentImage.enabled = true;
        Color newColor = currentImage.color;
        float timer = 0;
        yield return new WaitForSeconds(fadeDelay);
        while (timer < fadeTime)
        {
            timer += Time.deltaTime * fadeSpeed;

            newColor.a -= Time.deltaTime;
            currentImage.color = newColor;
            yield return null;
        }

        newColor.a = 1.0f;
        currentImage.color = newColor;
        currentImage.enabled = false;
    }
}
