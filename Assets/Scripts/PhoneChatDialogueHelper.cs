using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Yarn.Unity.Example 
{
    /// <summary>
    /// clones dialogue bubbles for the ChatDialogue example
    /// </summary>
    public class PhoneChatDialogueHelper : DialogueViewBase
    {
        DialogueRunner runner;

        public TMPro.TextMeshProUGUI text;

        //this holds images printed to chat
        public Sprite[] spriteArray;

        //Chat Image Game Object
        private SpriteRenderer chatImage;

        private GameObject chatImageGameObject;

        //gets where the image needs to be put
        public GameObject parent;

        public GameObject optionsContainer;
        public OptionView optionPrefab;

        [Tooltip("This is the chat message bubble UI object (what we are cloning for each message!)... NOT the container group for all chat bubbles")]
        public GameObject dialogueBubblePrefab;
        public float lettersPerSecond = 10f;
        
        bool isFirstMessage = true;

        // current message bubble styling settings, modified by SetSender
        bool isRightAlignment = true;
        Color currentBGColor = Color.black, currentTextColor = Color.white;

        void Awake()
        {
            runner = GetComponent<DialogueRunner>();
            runner.AddCommandHandler("Me", SetSenderMe ); // registers Yarn Command <<Me>>, which sets the current message sender to "Me"
            runner.AddCommandHandler("Them", SetSenderThem ); // registers Yarn Command <<They>>, which sets the current message sender to "Them" (whoever the player is talking to)
            runner.AddCommandHandler<int>("DisplayImage", DisplayImage);

            optionsContainer.SetActive(false); 
        }

        void Start () 
        {
            dialogueBubblePrefab.SetActive(false);
            UpdateMessageBoxSettings();
        }

        // YarnCommand <<Me>>, but does not use YarnCommand C# attribute, registers in Awake() instead
        public void SetSenderMe() 
        {
            isRightAlignment = true;
            currentBGColor = Color.grey;
            currentBGColor.a = 0.75f;
            currentTextColor = Color.black;
        }

        // YarnCommand <<Them>> does not use YarnCommand C# attribute, registers in Awake() instead
        public void SetSenderThem() 
        {
            isRightAlignment = false;
            currentBGColor = new Color(0.1990477f, 0.2161004f, 0.5943396f);
            currentBGColor.a = 0.69f;
            currentTextColor = Color.white;
        }

        //THIS IS SUPPOSED TO BE CALLED BY THE YARN SCRIPT Yarnchatdialoge and display an image
        //;3
        void DisplayImage(int spriteID)
        {
            Sprite picToPost = spriteArray[spriteID];
            chatImageGameObject = new GameObject();
            chatImage = chatImageGameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
            chatImage.sprite = picToPost;

            chatImageGameObject.AddComponent<RectTransform>();
            chatImageGameObject.AddComponent<HorizontalLayoutGroup>();

            chatImageGameObject.transform.SetParent(parent.transform);



            var rectValue = chatImageGameObject.GetComponent<RectTransform>();

            rectValue.sizeDelta = new Vector2(415.2f, 79.48f);

            Debug.Log("Posted Image to Chat");
            //Instantiate(chatImageGameObject, dialogueBubblePrefab.transform.parent);






            //Sprite.Create(picToPost, new Rect(0.0f, 0.0f, picToPost.width, picToPost.height), new Vector2(0.5f, 0.5f), 100.0f);
            //var bg = dialogueBubblePrefab.GetComponentInChildren<Image>();
            //bg.sprite = spriteArray[spriteID];
            //Instantiate<Sprite>(spriteArray[spriteID], bg.transform.position, bg.transform.rotation);

        }

        // when we clone a new message box, re-style the message box based on whether SetSenderMe or SetSenderThem was most recently called
        void UpdateMessageBoxSettings() 
        {
            var bg = dialogueBubblePrefab.GetComponentInChildren<Image>();
            
            bg.color = currentBGColor;
            var message = dialogueBubblePrefab.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            message.text = "";
            message.color = currentTextColor;

            var layoutGroup = dialogueBubblePrefab.GetComponent<HorizontalLayoutGroup>();
            if ( isRightAlignment ) 
            {
                layoutGroup.padding.left = 32;
                layoutGroup.padding.right = 0;
                bg.transform.SetAsLastSibling();
            }
            else
            {
                layoutGroup.padding.left = 0;
                layoutGroup.padding.right = 32;
                bg.transform.SetAsFirstSibling();
            }
        }

        public void CloneMessageBoxToHistory()
        {
            // if this isn't the very first message, then clone current message box and move it up
            if ( isFirstMessage == false )
            {
                var oldClone = Instantiate( 
                    dialogueBubblePrefab, 
                    dialogueBubblePrefab.transform.position, 
                    dialogueBubblePrefab.transform.rotation, 
                    dialogueBubblePrefab.transform.parent
                );
                dialogueBubblePrefab.transform.SetAsLastSibling();
            }
            isFirstMessage = false;

            // reset message box and configure based on current settings
            dialogueBubblePrefab.SetActive(true);
            UpdateMessageBoxSettings();
        }

        Coroutine currentTypewriterEffect;

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            if (currentTypewriterEffect != null)
            {
                StopCoroutine(currentTypewriterEffect);
            }

            CloneMessageBoxToHistory();

            text.text = dialogueLine.TextWithoutCharacterName.Text;

            currentTypewriterEffect = StartCoroutine(ShowTextAndNotify());

            IEnumerator ShowTextAndNotify() {
                yield return StartCoroutine(Effects.Typewriter(text, lettersPerSecond, null));
                currentTypewriterEffect = null;
                onDialogueLineFinished();
            }
        }

        public override void RunOptions(DialogueOption[] dialogueOptions, Action<int> onOptionSelected)
        {
            foreach(Transform child in optionsContainer.transform)
            {
                Destroy(child.gameObject);
            }

            optionsContainer.SetActive(true);

            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                DialogueOption option = dialogueOptions[i];
                var optionView = Instantiate(optionPrefab);
                
                optionView.transform.SetParent(optionsContainer.transform, false);

                optionView.Option = option;

                optionView.OnOptionSelected = (selectedOption) =>
                {
                    optionsContainer.SetActive(false);
                    onOptionSelected(selectedOption.DialogueOptionID);
                };
            }
        }
    }

}
