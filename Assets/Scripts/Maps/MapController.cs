using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MapController: MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkeradius;
    Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    PlayerMovement pm;
    public GameObject currentChunk;

    [Header("Optimiation")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDist; // Must be greater than the length and width of the tilemap
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDur;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pm = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        chunkoptimizer();
    }

    void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }

        if(pm.moveDir.x > 0 && pm.moveDir.y == 0) //right
        {
            if(!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkeradius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right").position;
                spawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y == 0) //left
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkeradius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left").position;
                spawnChunk();
            }
        }
        else if (pm.moveDir.x == 0 && pm.moveDir.y > 0) //up
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkeradius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Up").position;
                spawnChunk();
            }
        }
        else if (pm.moveDir.x == 0 && pm.moveDir.y < 0) //down
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkeradius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Down").position;
                spawnChunk();
            }
        }
        else if (pm.moveDir.x > 0 && pm.moveDir.y > 0) //right up
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Up").position, checkeradius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right Up").position;
                spawnChunk();
            }
        }
        else if (pm.moveDir.x > 0 && pm.moveDir.y < 0) //right down
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right Down").position, checkeradius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right Down").position;
                spawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y > 0) //left up
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Up").position, checkeradius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left Up").position;
                spawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y < 0) //left down
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left Down").position, checkeradius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left Down").position;
                spawnChunk();
            }
        }
    }

    void spawnChunk()
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk); 
    }

    void chunkoptimizer()
    {
        optimizerCooldown -= Time.deltaTime;
        if (optimizerCooldown <= 0)
        {
            optimizerCooldown = optimizerCooldownDur;
        }
        else
        {
            return;
        }
        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}