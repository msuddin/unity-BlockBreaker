using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Configuration
    [SerializeField] AudioClip blockBreakSound;
    [SerializeField] GameObject blockSparklesVFX;
    [SerializeField] Sprite[] hitSprits;

    // Cache
    GameLevel gameLevel;

    // State
    [SerializeField] int timesHit;

    void Start()
    {
        gameLevel = FindObjectOfType<GameLevel>();
        if (tag == "Breakable")
        {
            gameLevel.CountBreakableBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            HandleHits();
        }
    }

    private void HandleHits()
    {
        timesHit++;
        int maxHits = hitSprits.Length + 1;
        if (timesHit >= maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitsSprite();
        }
    }

    private void ShowNextHitsSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprits[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprits[spriteIndex];
        }
        else
        {
            Debug.LogError("Block sprite is missing from array, GameObject is " + gameObject.name);
        }
    }

    private void DestroyBlock()
    {
        // Found a bug with the line below, if I add this to the start method then the block score does not update but I don't know why
        FindObjectOfType<GameStatus>().AddToScore();
        AudioSource.PlayClipAtPoint(blockBreakSound, Camera.main.transform.position);
        gameLevel.BlockDestroyed();
        Destroy(gameObject);
        TriggerSparklesVFX();
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 2f);
    }
}
