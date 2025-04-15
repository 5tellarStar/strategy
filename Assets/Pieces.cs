using UnityEditor;
using UnityEngine;


public class PieceBase
{
    protected bool o = false;
    protected bool f = false;
    protected bool x = true;

    public string name;

    public string description;

    public int sprite;

    public bool[,] moves;

    public bool[,] attacks;
}

public class Pawn : PieceBase
{
    public Pawn()
    {
        name = "Pawn";

        description = "The weakest piece";

        sprite = 0;

        moves = new bool[17,17]
       {{f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,o,x,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f}};

        attacks = new bool[17, 17]
       {{f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,x,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,o,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,x,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f},
        {f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f,f}};
    }
}