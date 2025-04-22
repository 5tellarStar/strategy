using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public struct Move
{
    public Piece piece;
    public Vector2Int target;
}

public class AIBase
{
    public virtual Move calculateMove(Piece[,] pieces)
    {
        return new Move();
    }
}

public class RandomMove : AIBase
{
    public override Move calculateMove(Piece[,] pieces)
    {
        List<Move> list = new();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (pieces[x,y] != null)
                {
                    if (pieces[x,y].faction == Faction.enemy)
                    {
                        bool[,] possibleMoves = pieces[x, y].piece.moves(pieces, pieces[x,y].coordinates,Faction.enemy);
                        bool[,] possibleAttacks = pieces[x, y].piece.attacks(pieces, pieces[x, y].coordinates, Faction.enemy);

                        for (int bx = 0; bx < 8; bx++)
                        {
                            for(int by = 0;by < 8; by++)
                            {
                                if(possibleAttacks[bx,by] || possibleMoves[bx, by])
                                {
                                    Move move = new Move();
                                    move.piece = pieces[x,y];
                                    move.target = new Vector2Int(bx,by);
                                    list.Add(move);
                                }              
                            }
                        }
                    }
                }
            }
        }
        return list[Random.Range(0,list.Count)];
    }
}

