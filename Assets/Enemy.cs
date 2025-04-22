using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SelectAi selectAi;
    [SerializeField] private Transform hand;
    [SerializeField] private Vector2 handStart;
    [SerializeField] private float handSpeed;

    public AIBase ai;

    BattleManager battleManager;

    private Move move;

    private EnemyState state;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        battleManager = GameObject.FindWithTag("BattleManager").GetComponent<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Grabing)
        {
            hand.position = Vector2.MoveTowards(hand.position, move.piece.transform.position, handSpeed * Time.deltaTime);
            if(hand.position.x == move.piece.transform.position.x && hand.position.y == move.piece.transform.position.y)
            {
                state = EnemyState.Moving;
                move.piece.spriteRenderer.sprite = battleManager.GetComponent<SpriteLib>().spritesESmall[move.piece.piece.sprite];
            }
        }
        if(state == EnemyState.Moving)
        {
            hand.position = Vector2.MoveTowards(hand.position, new Vector2(move.target.x - 3.5f, 4.8125f - move.target.y * 0.75f), handSpeed * Time.deltaTime);
            move.piece.transform.position = hand.position;
            if(hand.position.x == move.target.x - 3.5f && hand.position.y == 4.8125f - move.target.y * 0.75f)
            {
                state = EnemyState.Waiting;
                battleManager.MovePiece(move.piece,move.target);
                battleManager.Playerturn = true;
            }
        }
        if (state == EnemyState.Waiting)
        {
            if(hand.position.x != handStart.x || hand.position.y != handStart.y)
            {
                hand.position = Vector2.MoveTowards(hand.position, handStart, handSpeed * Time.deltaTime);
            }
        }
    }

    public void StartMove()
    {
        move = ai.calculateMove(battleManager.pieces);
        state = EnemyState.Grabing;
    }

    private void OnValidate()
    {
        switch (selectAi)
        {
            case SelectAi.None:
                ai = new AIBase();
                break;
            case SelectAi.Random:
                ai = new RandomMove();
                break;
            default:
                Debug.LogError("AI type not found");
                break;
        }
    }
}

public enum EnemyState
{
    Waiting,
    Grabing,
    Moving
}

public enum SelectAi{
    None,
    Random
}