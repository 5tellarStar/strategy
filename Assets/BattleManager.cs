using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject piecePrefab;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject ragdollPrefab;
    [SerializeField] private CursorController cursor;
    public Color selectedColor;
    public Color selectedPColor;

    [SerializeField] private Enemy enemy;


    public bool Playerturn = true;
    public bool placingKing = false;



    public Piece[,] pieces = new Piece[8, 8];
    public Tile[,] tiles = new Tile[8, 8];

    public Piece selectedPiece;

    public List<Tile> possibleMoves;
    public List<Piece> possibleAttacks;


    private void Start()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (GlobalVariables.StartingPieces[x,y] != null)
                {
                    AddPiece(new Vector2Int(x, y), Faction.player, GlobalVariables.StartingPieces[x, y]); 
                }
                GameObject tile = Instantiate(tilePrefab);
                tile.transform.position = new Vector3(x - 3.5f, 3.8125f - y * 0.75f, 0);
                tile.GetComponent<Tile>().battleManager = this;
                tile.GetComponent<Tile>().coords = new Vector2Int(x, y);
                tiles[x, y] = tile.GetComponent<Tile>();
            }
        }

    }
    public void AddPiece(Vector2Int coords, Faction fac, PieceBase p)
    {
        Piece piece = Instantiate(piecePrefab).GetComponent<Piece>();
        piece.New(coords, fac, p, this);
        MovePiece(piece, coords);
        
    }

    public void ClickedPiece(Piece piece)
    {
        if (Playerturn && selectedPiece == null && piece.faction == Faction.player)
        {
            selectedPiece = piece;
            cursor.Grab(piece);
            piece.Grab();

            bool[,] possibleM = piece.piece.moves(pieces, piece.coordinates, Faction.player);
            bool[,] possibleA = piece.piece.attacks(pieces, piece.coordinates, Faction.player);
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (possibleM[x, y])
                    {
                        possibleMoves.Add(tiles[x, y]);
                    }
                    if (possibleA[x, y])
                    {
                        possibleAttacks.Add(pieces[x, y]);
                    }
                }
            }
            for (int i = 0; i < possibleMoves.Count; i++)
            {
                possibleMoves[i].spriteRenderer.color = selectedColor;
            }
            for (int i = 0; i < possibleAttacks.Count; i++)
            {
                possibleAttacks[i].spriteRenderer.color = selectedPColor;
            }
        }
        else if(selectedPiece != null)
        {
            bool possible = false;
            foreach (Piece p in possibleAttacks)
            {
                if(p == piece)
                {
                    possible = true;
                    break;
                }
            }

            if (possible)
            {
                MovePiece(selectedPiece, new Vector2Int(piece.coordinates.x, piece.coordinates.y));
                cursor.LetGo();
                EnemyTurn();
            }
            else
            {
                selectedPiece.Return();
                cursor.LetGo();
            }

            selectedPiece = null;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    tiles[x, y].spriteRenderer.color = Color.clear;
                    if (pieces[x, y] != null)
                        pieces[x, y].spriteRenderer.color = Color.white;
                }
            }
            possibleMoves = new();
            possibleAttacks = new();
        }
        else if (placingKing)
        {
            bool possible = false;
            foreach (Piece possibles in possibleAttacks)
            {
                if (piece == possibles)
                {
                    possible = true;
                    break;
                }
            }
            if (possible)
            {
                AddPiece(piece.coordinates, Faction.player, new King());
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        tiles[x, y].spriteRenderer.color = Color.clear;
                        if (pieces[x, y] != null)
                            pieces[x, y].spriteRenderer.color = Color.white;
                    }
                }
                possibleMoves = new();
                possibleAttacks = new();
            }

        }

    }

    public void ClickedTile(Tile tile)
    {
        if (selectedPiece != null)
        {
            bool possible = false;
            foreach (Tile possibles in possibleMoves)
            {
                if (tile == possibles)
                {
                    possible = true;
                    break;
                }
            }
            if (possible)
            {
                MovePiece(selectedPiece, new Vector2Int(tile.coords.x, tile.coords.y));
                cursor.LetGo();
                EnemyTurn();
            }
            else
            {
                selectedPiece.Return();
                cursor.LetGo();
            }
            selectedPiece = null;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    tiles[x, y].spriteRenderer.color = Color.clear;
                    if (pieces[x, y] != null)
                        pieces[x, y].spriteRenderer.color = Color.white;
                }
            }
            possibleMoves = new();
            possibleAttacks = new();
        }
        else if (placingKing)
        {
            bool possible = false;
            foreach (Tile possibles in possibleMoves)
            {
                if (tile == possibles)
                {
                    possible = true;
                    break;
                }
            }
            if (possible)
            {
                AddPiece(tile.coords, Faction.player, new King());
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        tiles[x, y].spriteRenderer.color = Color.clear;
                        if (pieces[x, y] != null)
                            pieces[x, y].spriteRenderer.color = Color.white;
                    }
                }
                possibleMoves = new();
                possibleAttacks = new();
            }

        }
    }

    public void MovePiece(Piece piece, Vector2Int coords)
    {
        if (piece.piece.name == "King" && piece.coordinates == new Vector2(piece.faction == Faction.player ? 0 : 7, 4))
        {
            if (coords == new Vector2Int(piece.faction == Faction.player ? 0 : 7, 6))
            {
                MovePiece(pieces[piece.faction == Faction.player ? 0 : 7, 7], new Vector2Int(piece.faction == Faction.player ? 0 : 7, 5));
            }
            if (coords == new Vector2Int(piece.faction == Faction.player ? 0 : 7, 2))
            {
                MovePiece(pieces[piece.faction == Faction.player ? 0 : 7, 0], new Vector2Int(piece.faction == Faction.player ? 0 : 7, 3));
            }
        }
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (pieces[x, y] == piece)
                {
                    pieces[x, y] = null;
                }
            }
        }
        if (pieces[coords.x, coords.y] != null)
        {
            if (pieces[coords.x, coords.y].faction == Faction.player && pieces[coords.x, coords.y].piece.name == "King")
            {
                if (GlobalVariables.Lives == 0)
                {

                }
                else
                {

                    GlobalVariables.Lives--;
                    placingKing = true;

                    for (int x = 0; x < GlobalVariables.SpawnArea; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            if (pieces[x, y] == null)
                            {
                                possibleMoves.Add(tiles[x, y]);
                                tiles[x, y].spriteRenderer.color = selectedColor;
                            }
                            else
                            {
                                if (pieces[x, y].faction == Faction.enemy)
                                {
                                    possibleAttacks.Add(pieces[x, y]);
                                    pieces[x, y].spriteRenderer.color = selectedPColor;
                                }
                            }
                        }
                    }
                }
            }
            GameObject ragdoll = Instantiate(ragdollPrefab);
            ragdoll.transform.position = pieces[coords.x, coords.y].transform.position;
            ragdoll.GetComponent<SpriteRenderer>().sprite = pieces[coords.x, coords.y].faction == Faction.player ? GetComponent<SpriteLib>().spritesPSmall[pieces[coords.x, coords.y].piece.sprite] : GetComponent<SpriteLib>().spritesESmall[pieces[coords.x, coords.y].piece.sprite];
            Destroy(pieces[coords.x, coords.y].gameObject);
        }
        pieces[coords.x, coords.y] = piece;
        piece.Place(coords);
    }
    
    public void EnemyTurn()
    {
        Playerturn = false;
        enemy.StartMove();
    }
}



public enum Faction
{
    player,
    enemy
}