using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Movable : MonoBehaviour
{
    [Tooltip("Time it takes to go to the next square in seconds")]
    public int speed = 3;

    private List<Vector3> nextPositions;
    private Animator animator;
    private static readonly int Moving = Animator.StringToHash("moving");

    private void Awake()
    {
        nextPositions = new List<Vector3>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (nextPositions.Count == 0) return;
        var target = nextPositions[0];
        var step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if (Vector3.Distance(transform.position, target) > 0.001f) return;
        
        nextPositions.RemoveAt(0);
        if(nextPositions.Count == 0) animator.SetBool(Moving, false);
        else transform.LookAt(nextPositions[0]);
    }

    /**
     * Adds the new positions to the next positions list
     */
    public void MoveCharacter(List<Vector3> positions)
    {
        if (!positions.Any()) return;
        
        animator.SetBool(Moving, true);
        nextPositions.AddRange(positions);
        
        transform.LookAt(positions[0]);
    }
    
}