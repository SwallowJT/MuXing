using UnityEngine;
using System.Collections;

public class MoveBg : MonoBehaviour {
    public Transform Bg1;
    public Transform Bg2;
    public Transform Bg3;
    public Transform Player;

    private float _BgWidth;

    void Start()
    {
        SpriteRenderer sprite = Bg1.GetComponent<SpriteRenderer>();
        _BgWidth = sprite.bounds.size.x;
    }
    
	void Update () {
        Vector3 playerPos = Player.position;
        Transform minBg = GetMinBg();
        Transform midBg = GetMidBg();
        Transform maxBg = GetMaxBg();

        if(playerPos.x > midBg.position.x)
        {
            Vector3 maxPos = maxBg.position;
            maxPos.x += _BgWidth;
            minBg.position = maxPos;
        }
    }

    Transform GetMidBg()
    {
        Vector3 pos1 = Bg1.position;
        Vector3 pos2 = Bg2.position;
        Vector3 pos3 = Bg3.position;

        if (pos2.x > pos1.x && pos2.x < pos3.x)
            return Bg2;
        else if (pos3.x > pos2.x && pos3.x < pos1.x)
            return Bg3;
        else
            return Bg1;
    }

    Transform GetMinBg()
    {
        Vector3 pos1 = Bg1.position;
        Vector3 pos2 = Bg2.position;
        Vector3 pos3 = Bg3.position;

        if (pos1.x < pos2.x && pos1.x < pos3.x)
            return Bg1;
        else if (pos2.x < pos1.x && pos2.x < pos3.x)
            return Bg2;
        else
            return Bg3;
    }

    Transform GetMaxBg()
    {
        Vector3 pos1 = Bg1.position;
        Vector3 pos2 = Bg2.position;
        Vector3 pos3 = Bg3.position;

        if (pos1.x > pos2.x && pos1.x > pos3.x)
            return Bg1;
        else if (pos2.x > pos1.x && pos2.x > pos3.x)
            return Bg2;
        else
            return Bg3;
    }
}
