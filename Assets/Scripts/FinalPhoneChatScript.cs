using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class FinalPhoneChatScript : DialogueViewBase
{
    DialogueRunner runner;

    public TMPro.TextMeshProUGUI text;

    public Texture2D[] images;

    private Image chatImage;
    public GameObject parent;
    public GameObject optionsContainer;
    public OptionView optionPrefab;
    public GameObject dialogueBubblePrefab;
    bool isFirstMessage = true;
    bool isRightAlignment = true;
}
