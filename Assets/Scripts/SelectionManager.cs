using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public string selectableTag = "Selectable";
    public Material highLightMaterial;
    public Transform hand;
    public Transform handRayPoint;
    public Transform handGrabPlaceHolder;

    private Material _selectedMaterial;
    private bool _holding = false;
    private Transform _currentHolding;
    private Transform _selection;
    
    void Update()
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = _selectedMaterial;
            _selection = null;
        }

        var ray = new Ray(handRayPoint.position, handRayPoint.up);
        Debug.DrawRay(ray.origin, ray.direction*3, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3))
        {
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    _selectedMaterial = selection.GetComponent<Renderer>().material;
                    selectionRenderer.material = highLightMaterial;
                }

                _selection = selection;
                if (!HandController.IsGrabbing() && _holding)
                {
                    offHand();
                    _currentHolding = null;
                    _holding = false;
                }
            }
        }
        if (!HandController.IsGrabbing() && _holding)
        {
            offHand();
            _currentHolding = null;
            _holding = false;
        }
    }
    
    private void toHand()
    {
        _selection.GetComponent<BoxCollider>().enabled = false;
        _selection.GetComponent<Rigidbody>().useGravity = false;
        _selection.GetComponent<Rigidbody>().freezeRotation = true;
        _selection.transform.position = handGrabPlaceHolder.position;
        _selection.transform.parent = handGrabPlaceHolder.transform;
    }

    private void offHand()
    {
        _currentHolding.GetComponent<BoxCollider>().enabled = true;
        _currentHolding.transform.parent = null;
        _currentHolding.GetComponent<Rigidbody>().freezeRotation = false;
        _currentHolding.GetComponent<Rigidbody>().useGravity = true;
    }
}