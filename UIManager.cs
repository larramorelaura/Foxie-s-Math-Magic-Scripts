using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Sprite[] _liveSprites;
    [SerializeField] private Image _liveImage;
    // Start is called before the first frame update
    
    public void updateLives(int currentLives)
    {
        _liveImage.sprite=_liveSprites[currentLives];
    }

    public void doExitGame() {
        Application.Quit();
    }
}

