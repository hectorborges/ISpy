﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RaycastSelect : MonoBehaviour
{
    RaycastHit hit;
    public Image topEye;
    public Image bottomEye;
    public GameObject reticleHolder;
    public GameObject backpack;
    Image reticleImage;
    bool isSelecting;
    ObjectManager objectManager;
    GameObject button;
    bool isClosing;
    AudioSource source;

    public AudioClip wrongItem;
    public AudioClip foundItem;


    void Start()
    {
        objectManager = GameObject.Find("GameManager").GetComponent<ObjectManager>();
        source = GetComponent<AudioSource>();
        reticleImage = reticleHolder.GetComponent<Image>();
        reticleHolder.SetActive(false);
    }

    void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit, 500))
        {
            if(hit.transform.tag == "Collectable")
            {
                if(!isSelecting)
                    StartCoroutine(Selecting(hit.transform.gameObject));

                reticleHolder.SetActive(true);
            }
            if (hit.transform.tag == "Static")
            {
                if (!isSelecting)
                    StartCoroutine(Selecting());

                reticleHolder.SetActive(true);
            }
            if(hit.transform.tag == "StartButton")
            {
                if (!isSelecting)
                    StartCoroutine(Selecting("Game"));

                reticleHolder.SetActive(true);
            }
        }
        else
        {
            ResetFill();
            reticleHolder.SetActive(false);
        }
    }

    IEnumerator Selecting(string scene)
    {

        isSelecting = true;
        while (reticleImage.fillAmount < 1)
        {
            reticleImage.fillAmount += .01f;
            yield return new WaitForSeconds(.01f);
        }
        isSelecting = false;
        //SceneManager.LoadScene(scene);
    }
    IEnumerator Selecting(GameObject pickupable)
    {
        isSelecting = true;
        while (reticleImage.fillAmount < 1)
        {
            reticleImage.fillAmount += .01f;
            yield return new WaitForSeconds(.01f);
        }
        isSelecting = false;

        if (objectManager.RemoveObject(pickupable))
        {
            source.clip = foundItem;
            source.Play();
            pickupable.transform.position = backpack.transform.position;
        }
        else
        {
            source.clip = wrongItem;
            source.Play();
            print("That is not the item I was looking for");
        }
    }

    IEnumerator Selecting()
    {
        isSelecting = true;
        while (reticleImage.fillAmount < 1)
        {
            reticleImage.fillAmount += .01f;
            yield return new WaitForSeconds(.01f);
        }
        isSelecting = false;
        //Play audio here
    }

    void ResetFill()
    {
        reticleImage.fillAmount = 0f;
    }
}
