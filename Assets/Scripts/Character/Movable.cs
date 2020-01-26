using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Character))]
public class Movable : MonoBehaviour
{
    [Tooltip("Time it takes to go to the next square in seconds")] [SerializeField]
    private int speed = 3;

    private List<Square> nextPositions;
    private Animator animator;
    private Character character;
    private static readonly int Moving = Animator.StringToHash("moving");
    private Action onFinishMoving;

    private void Awake()
    {
        nextPositions = new List<Square>();
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();
    }

    /// <summary>
    /// <para>Moves the Movable to the next element of the nextPositions list. While it is moving the animation
    /// 'moving' is executed.</para>
    /// </summary>
    /// <remarks>It is called once per frame</remarks>
    private void Update()
    {
        if (nextPositions.Count == 0) return;
        var target = nextPositions[0].transform.position;
        var step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, target, step);

        if (Vector3.Distance(transform.position, target) > 0.001f) return;

        character.StepOnSquare(nextPositions[0]);
        nextPositions.RemoveAt(0);
        if (nextPositions.Count == 0)
        {
            animator.SetBool(Moving, false);
            onFinishMoving?.Invoke();
        }
        else transform.LookAt(nextPositions[0].transform.position);
    }

    /// <summary>
    /// Adds the new positions to the next positions list
    /// </summary>
    /// <param name="positions"></param>
    /// <param name="moveFinished">Action that will be called when the movable reaches the last position</param>
    public void MoveCharacter(List<Square> positions, Action moveFinished)
    {
        if (!positions.Any())
        {
            moveFinished();
            return;
        }

        animator.SetBool(Moving, true);
        positions = positions.Count > 1 ? positions.GetRange(1, positions.Count - 1) : positions;
        nextPositions.AddRange(positions);

        transform.LookAt(positions[0].transform.position);
        onFinishMoving = moveFinished;
    }
}