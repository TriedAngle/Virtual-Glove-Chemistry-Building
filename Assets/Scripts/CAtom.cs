using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAtom : MonoBehaviour
{
    private const float RayDistance = 2.0f;

    private bool _hasUp;
    private bool _hasRight;
    private bool _hasDown;
    private bool _hasLeft;

    void Start()
    {
    }

    void Update()
    {
        var ray0 = new Ray(transform.position, transform.up);
        var ray1 = new Ray(transform.position, transform.forward);
        var ray2 = new Ray(transform.position, -transform.up);
        var ray3 = new Ray(transform.position, -transform.forward);

        Debug.DrawRay(ray0.origin, ray0.direction * RayDistance, Color.green);
        Debug.DrawRay(ray1.origin, ray1.direction * RayDistance, Color.green);
        Debug.DrawRay(ray2.origin, ray2.direction * RayDistance, Color.green);
        Debug.DrawRay(ray3.origin, ray3.direction * RayDistance, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(ray0, out hit, RayDistance))
        {
            _hasUp = hit.transform.CompareTag("Selectable");
        }

        if (Physics.Raycast(ray1, out hit, RayDistance))
        {
            _hasRight = hit.transform.CompareTag("Selectable");
        }

        if (Physics.Raycast(ray2, out hit, RayDistance))
        {
            _hasDown = hit.transform.CompareTag("Selectable");
        }

        if (Physics.Raycast(ray3, out hit, RayDistance))
        {
            _hasLeft = hit.transform.CompareTag("Selectable");
        }
    }

    public bool IsFull()
    {
        return _hasUp && _hasRight && _hasDown && _hasLeft;
    }
}