using System;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    DateTime StartingTime;
    int Size;
    public PlayerScript Player;
    BoxCollider2D boxCollider;
    public GameObject Explosion;
    void Start()
    {
        StartingTime = DateTime.Now;
        Size = 2;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan ts = DateTime.Now - StartingTime;
        if (ts.Seconds >= 3)
        {
            Explode(Size);
            if (Player != null)
            {
                Player.BombExploded();
            }
            GameManager.RemoveBomb(gameObject);
            Destroy(gameObject);

        }
        if (Player == null)
        {
            boxCollider.isTrigger = false;
        }
        else if (Math.Floor(Player.transform.position.x) != Math.Floor(transform.position.x) ||  Math.Floor(Player.transform.position.y) != Math.Floor(transform.position.y))
        {
            boxCollider.isTrigger = false;
        }

    }

    public static GameObject Deploy(GameObject bomb, Vector3 pos)
    {
        return Instantiate(bomb, pos, Quaternion.identity);
    }


    public void Explode(int size, bool left = true, bool right = true, bool up = true, bool down = true, int i = 0)
    {
        if(i <= size)
        {
            if(i == 0)
            {
                GameManager.CheckForPlayers((int)Math.Floor(transform.position.x), (int)Math.Floor(transform.position.y));
                Instantiate(Explosion, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                i++;
            }
            if(left == true && Tiles.Tilemap.GetTile(new Vector3Int((int)(transform.position.x - 0.5f) - i, (int)(transform.position.y - 0.5f), (int)Layers.Indestructible)) == null)
            {
                Tiles.Destruct((int)(transform.position.x - 0.5f) - i, (int)(transform.position.y - 0.5f));
                Instantiate(Explosion,new Vector3(transform.position.x - i, transform.position.y, transform.position.z), Quaternion.identity);
                GameManager.CheckForPlayers((int)(transform.position.x - 0.5f) - i, (int)(transform.position.y - 0.5f));
            }
            else left = false;

            if (right == true && Tiles.Tilemap.GetTile(new Vector3Int((int)(transform.position.x - 0.5f) + i, (int)(transform.position.y - 0.5f), (int)Layers.Indestructible)) == null)
            {
                Tiles.Destruct((int)(transform.position.x - 0.5f) + i, (int)(transform.position.y - 0.5f));
                Instantiate(Explosion, new Vector3(transform.position.x + i, transform.position.y, transform.position.z), Quaternion.identity);
                GameManager.CheckForPlayers((int)(transform.position.x - 0.5f) + i, (int)(transform.position.y - 0.5f));
            }

            else right = false;

            if (up == true && Tiles.Tilemap.GetTile(new Vector3Int((int)(transform.position.x - 0.5f), (int)(transform.position.y - 0.5f) + i, (int)Layers.Indestructible)) == null)
            {
                Tiles.Destruct((int)(transform.position.x - 0.5f), (int)(transform.position.y - 0.5f) + i);
                Instantiate(Explosion, new Vector3(transform.position.x, transform.position.y + i, transform.position.z), Quaternion.identity);
                GameManager.CheckForPlayers((int)(transform.position.x - 0.5f), (int)(transform.position.y - 0.5f) + i);
            }

            else up = false;

            if (down == true && Tiles.Tilemap.GetTile(new Vector3Int((int)(transform.position.x - 0.5f), (int)(transform.position.y - 0.5f) - i, (int)Layers.Indestructible)) == null)
            {
                Tiles.Destruct((int)(transform.position.x - 0.5f), (int)(transform.position.y - 0.5f) - i);
                Instantiate(Explosion, new Vector3(transform.position.x, transform.position.y - i, transform.position.z), Quaternion.identity);
                GameManager.CheckForPlayers((int)(transform.position.x - 0.5f), (int)(transform.position.y - 0.5f) - i);
            }

            else down = false;
            Explode(size, left, right, up, down, i + 1);
        }
    }
}
