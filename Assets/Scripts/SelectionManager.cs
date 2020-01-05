using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SelectionManager : MonoBehaviour
{
    public string selectableTag = "Selectable";
    public string genC = "GenC";
    public string genH = "GenH";
    public string genO = "GenO";
    public Material highLightMaterial;
    public Transform hand;
    public Transform handRayPoint;
    public Transform handGrabPlaceHolder;
    public Transform glassPane;
    private Vector3 _selectionNewPos;

    private Material _selectedMaterial;
    private bool _holding = false;
    private Transform _currentHolding;
    private Transform _selection;

    public GameObject cAtom;
    private GameObject _newAtom;

    void Update()
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = _selectedMaterial;
            _selection = null;
        }

        var ray = new Ray(handRayPoint.position, handRayPoint.up);
        Debug.DrawRay(ray.origin, ray.direction * 3, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Pane"))
            {
                _selectionNewPos = hit.point;
            }

            var selection = hit.transform;

            if (selection.CompareTag(genC))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null && !_holding)
                {
                    _selectedMaterial = selection.GetComponent<Renderer>().material;
                    selectionRenderer.material = highLightMaterial;
                }

                _selection = selection;
                if (!_holding && HandController.IsGrabbing())
                {
                    selectionRenderer.material = _selectedMaterial;
                    _newAtom = (GameObject) Instantiate(cAtom, hit.point, Quaternion.identity);
                    _selection = _newAtom.transform;
                    _holding = true;
                    _currentHolding = _selection;
                    ToHand();
                }
            }
                
            if (selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (!_holding && selectionRenderer != null)
                {
                    _selectedMaterial = selection.GetComponent<Renderer>().material;
                    selectionRenderer.material = highLightMaterial;
                }

                _selection = selection;
                if (!_holding && HandController.IsGrabbing())
                {
                    ToHand();
                    _currentHolding = _selection;
                    _holding = true;
                }
            }
        }

        if (!HandController.IsGrabbing() && _holding)
        {
            OffHand();
            _currentHolding = null;
            _holding = false;
        }
    }

    private void ToHand()
    {
//        _selection.GetComponent<BoxCollider>().enabled = false;
//        _selection.GetComponent<Rigidbody>().useGravity = false;
//        _selection.GetComponent<Rigidbody>().freezeRotation = true;
        _selection.transform.position = handGrabPlaceHolder.position;
        _selection.transform.parent = handGrabPlaceHolder.transform;
    }

    private void OffHand()
    {
        _currentHolding.transform.position = _selectionNewPos;
        _currentHolding.transform.rotation = Quaternion.identity;
        _currentHolding.transform.parent = null;
//        _currentHolding.GetComponent<Rigidbody>().freezeRotation = false;
//        _currentHolding.GetComponent<Rigidbody>().useGravity = true;
    }
}