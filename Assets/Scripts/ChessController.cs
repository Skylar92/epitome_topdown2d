using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChessController : Collectable
{
    public Sprite closeChess;
    public Sprite openEmptyChess;
    public Sprite openTreasureChess;

    private SpriteRenderer _spriteRenderer;
    private bool _chessIsOpen;

    protected override void Start()
    {
        base.Start();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = closeChess;
    }

    protected override void OnCollide(Collider2D hit)
    {
        if (_chessIsOpen)
            return;

        if (hit.CompareTag("Player"))
        {
            _spriteRenderer.sprite = openEmptyChess;
            _chessIsOpen = true;
            
            var coins = Random.Range(100, 200);
            GameManager.Instance.money += coins;
            GameManager.Instance.ShowText($"+{coins} coins", 15, Color.yellow, gameObject.transform.position, Vector3.up * 75, 1.5f);
        }
    }

    protected override void OnCollect()
    {
        base.OnCollect();
    }
}