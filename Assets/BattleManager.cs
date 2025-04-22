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


    public bool Playerturn;

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
                GameObject tile = Instantiate(tilePrefab);
                tile.transform.position = new Vector3(x - 3.5f, 3.8125f - y * 0.75f, 0);
                tile.GetComponent<Tile>().battleManager = this;
                tile.GetComponent<Tile>().coords = new Vector2Int(x, y);
                tiles[x, y] = tile.GetComponent<Tile>();
            }
        }

        AddPiece(new Vector2Int(0, 0), Faction.player, new Rook());
        AddPiece(new Vector2Int(0, 1), Faction.player, new Knight());
        AddPiece(new Vector2Int(0, 2), Faction.player, new Bishop());
        AddPiece(new Vector2Int(0, 3), Faction.player, new Queen());
        AddPiece(new Vector2Int(0, 4), Faction.player, new King());
        AddPiece(new Vector2Int(0, 5), Faction.player, new Bishop());
        AddPiece(new Vector2Int(0, 6), Faction.player, new Knight());
        AddPiece(new Vector2Int(0, 7), Faction.player, new Rook());

        AddPiece(new Vector2Int(1, 0), Faction.player, new Pawn());
        AddPiece(new Vector2Int(1, 1), Faction.player, new Pawn());
        AddPiece(new Vector2Int(1, 2), Faction.player, new Pawn());
        AddPiece(new Vector2Int(1, 3), Faction.player, new Pawn());
        AddPiece(new Vector2Int(1, 4), Faction.player, new Pawn());
        AddPiece(new Vector2Int(1, 5), Faction.player, new Pawn());
        AddPiece(new Vector2Int(1, 6), Faction.player, new Pawn());
        AddPiece(new Vector2Int(1, 7), Faction.player, new Pawn());

        AddPiece(new Vector2Int(7, 0), Faction.enemy, new Rook());
        AddPiece(new Vector2Int(7, 1), Faction.enemy, new Knight());
        AddPiece(new Vector2Int(7, 2), Faction.enemy, new Bishop());
        AddPiece(new Vector2Int(7, 3), Faction.enemy, new Queen());
        AddPiece(new Vector2Int(7, 4), Faction.enemy, new King());
        AddPiece(new Vector2Int(7, 5), Faction.enemy, new Bishop());
        AddPiece(new Vector2Int(7, 6), Faction.enemy, new Knight());
        AddPiece(new Vector2Int(7, 7), Faction.enemy, new Rook());

        AddPiece(new Vector2Int(6, 0), Faction.enemy, new Pawn());
        AddPiece(new Vector2Int(6, 1), Faction.enemy, new Pawn());
        AddPiece(new Vector2Int(6, 2), Faction.enemy, new Pawn());
        AddPiece(new Vector2Int(6, 3), Faction.enemy, new Pawn());
        AddPiece(new Vector2Int(6, 4), Faction.enemy, new Pawn());
        AddPiece(new Vector2Int(6, 5), Faction.enemy, new Pawn());
        AddPiece(new Vector2Int(6, 6), Faction.enemy, new Pawn());
        AddPiece(new Vector2Int(6, 7), Faction.enemy, new Pawn());
    }
    private void AddPiece(Vector2Int coords, Faction fac, PieceBase p)
    {
        Piece piece = Instantiate(piecePrefab).GetComponent<Piece>();
        piece.New(coords, fac, p, this);
        pieces[coords.x, coords.y] = piece;
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
    }

    public void MovePiece(Piece piece,Vector2Int coords)
    {
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