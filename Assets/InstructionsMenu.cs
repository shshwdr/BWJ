using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsMenu : MonoBehaviour
{
    PlayerCubeGridMove player;
    public Button instruction1;
    public Button instruction2;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerCubeGridMove>();
        instruction1.onClick.AddListener(delegate { turnAround(); });
        instruction2.onClick.AddListener(delegate { ignoreSign(); });
    }

    public void turnAround()
    {
        player.turnAround();
    }
    public void ignoreSign()
    {
        player.ignoreSign();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
