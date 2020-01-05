using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeManager : MonoBehaviour
{
    public CAtom[] cAtoms;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cAtoms = GameObject.FindObjectsOfType<CAtom>();
        foreach (CAtom atom in cAtoms)
        {
            if (atom.IsFull())
            {
                Debug.Log("Atom is full");
            }
        }
    }
}
