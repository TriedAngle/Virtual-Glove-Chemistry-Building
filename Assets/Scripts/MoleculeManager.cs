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
        int i = 0;
        bool allTrue = true;
        cAtoms = FindObjectsOfType<CAtom>();
        foreach (CAtom atom in cAtoms)
        {
            if (atom.IsFull())
            {
                i++;
            }
            else
            {
                allTrue = false;
            }
        }

        if (allTrue)
        {
            switch (i)
            {
                case 5:
                    Debug.Log("Pentan");
                    break;
                case 4:
                    Debug.Log("Buthan");
                    break;
                case 3:
                    Debug.Log("Propan");
                    break;
                case 2:
                    Debug.Log("Ethan");
                    break;
                case 1:
                    Debug.Log("Methan");
                    break;
            } 
        }
        
    }
}
