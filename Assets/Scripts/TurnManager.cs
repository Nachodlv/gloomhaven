using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private List<Character> charactersOrdered;
    private int currentTurn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTurn(List<Character> characters)
    {
        charactersOrdered = characters;
        currentTurn = 0;
    }
    
    /**
     * Returns the characters that is playing at the moment
     */
    public Character GetCurrentCharacter()
    {
        return charactersOrdered[currentTurn];
    }
}
