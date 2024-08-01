//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]

public class Outline : MonoBehaviour
{
    private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();



    public Color OutlineColor
    {
        get { return outlineColor; }
        set
        {
            outlineColor = value;
        }
    }

    [SerializeField]
    private LayerMask OutlineMask;
    [SerializeField]
    private Color outlineColor = Color.white;
    [SerializeField,Tooltip("Not working!!!")]
    private float _outlineWidth = 3f;


    private Renderer[] renderers;
    void Awake()
    {

        // Cache renderers
        renderers = GetComponentsInChildren<Renderer>();

    }

    void OnEnable()
    {
        SetColor(outlineColor, _outlineWidth);
        gameObject.layer = LayerMask.GetMask(LayerMask.LayerToName(OutlineMask.value));
    }

    void OnValidate()
    {
        if(renderers == null || renderers.Length == 0)
        {
            renderers = GetComponentsInChildren<Renderer>();
        }
        SetColor(outlineColor, _outlineWidth);
    }
    void SetColor(Color color, float value)
    {
        var propertyBlock = new MaterialPropertyBlock();

        foreach (var renderer in renderers)
        {
            renderer.GetPropertyBlock(propertyBlock);

            // İlk materyal için _SelectionColor değerini set et
            propertyBlock.SetColor("_SelectionColor", color);
            renderer.SetPropertyBlock(propertyBlock, 0);

            //// İkinci materyal için _OutlineWidth değerini set et
            //if (renderer.sharedMaterials.Length > 1)
            //{
            //    propertyBlock.Clear();
            //    renderer.GetPropertyBlock(propertyBlock, 1);
            //    propertyBlock.SetFloat("_OutlineWidth", value);
            //    renderer.SetPropertyBlock(propertyBlock, 1);
            //}
        }
    }
    void OnDisable()
    {
        SetColor(new Color(0, 0, 0,0),0);
        gameObject.layer = 0;
    }

}
