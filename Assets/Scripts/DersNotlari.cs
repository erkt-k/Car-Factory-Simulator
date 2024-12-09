using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DersNotlari : MonoBehaviour
{
    public int playerScore = 10;
    [SerializeField] private string playerName = "Oyuncu";
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(playerScore);
        Debug.Log(playerName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
