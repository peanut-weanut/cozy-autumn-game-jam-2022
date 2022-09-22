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
        private Image chatImage;

        private GameObject chatImageGameObject;

        //gets where the image needs to be put
        public GameObject parent;
        public AudioManager audioManager;

        public GameObject optionsContainer;
        public OptionView optionPrefab;

        [Tooltip("This is the chat message bubble UI object (what we are cloning for each message!)... NOT the container group for all chat bubbles")]
        public GameObject dialogueBubblePrefab;
        public float lettersPerSecond = 10f;
        public delegate void OnTextSentDelegate();
        public OnTextSentDelegate OnTextSent;
        bool isFirstMessage = true;

        // current message bubble styling settings, modified by SetSender
        bool isRightAlignment = true;
        Color currentBGColor = Color.black, currentTextColor = Color.white;

        void Awake()
        {
            runner = GetComponent<DialogueRunner>();
            runner.AddCommandHandler("Me", SetSenderMe ); // registers Yarn Command <<Me>>, which sets the current message sender to "Me"
            runner.AddCommandHandler("Them", SetSenderThem ); // registers Yarn Command <<They>>, which sets the current message sender to "Them" (whoever the player is talking to)
            runner.AddCommandHandler("DisplayImage", DisplayImage);
            runner.AddCommandHandler("Moneyshot", DisplayMoneyshot);
            // runner.AddCommandHandler<int>("NextStage", SetNextStage); // set nextstage stage. ends dialogue and goes to next stage
            // runner.AddCommandHandler<Trigger>("CheckTrigger", OnTrigger) //taking a trigger as an argument, it will wait until the trigger is activated, and then go to the next dialogue

            // optionsContainer.SetActive(false); 
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
            currentBGColor = new Color(1.0f, 0.5824543f, 0.259434f);
            currentBGColor.a = 1.0f;
            currentTextColor = Color.white;
            GameManager.game.audioManager.isItMe = true;
        }

        // YarnCommand <<Them>> does not use YarnCommand C# attribute, registers in Awake() instead
        public void SetSenderThem() 
        {
            isRightAlignment = false;
            currentBGColor = new Color(1.0f, 0.284667f, 0.2588235f);
            currentBGColor.a = 1.0f;
            currentTextColor = Color.white;
            GameManager.game.audioManager.isItMe = false;
        }

        //THIS IS SUPPOSED TO BE CALLED BY THE YARN SCRIPT Yarnchatdialoge and display an image
        //;3
        void DisplayMoneyshot(){
            //send the cute drawing that cassie makes
        }
        void DisplayImage()
        {
            Texture2D sprite = GameManager.game.drawUtils.textPic;

            Sprite picToPost = Sprite.Create(sprite,new Rect(0,0,sprite.width,sprite.height),new Vector2(0.5f, 0.5f));
            chatImageGameObject = new GameObject();
            chatImage = chatImageGameObject.AddComponent<Image>() as Image;
            chatImage.sprite = picToPost;

            chatImageGameObject.AddComponent<RectTransform>();
            chatImageGameObject.AddComponent<HorizontalLayoutGroup>();
            chatImageGameObject.AddComponent<CanvasRenderer>();
            

            var rectValue = chatImageGameObject.AddComponent<LayoutElement>();

            rectValue.preferredHeight = 200f;

            var localScale = chatImageGameObject.GetComponent<RectTransform>();

            localScale.localScale = new Vector3 (0.35f, 0.5f, 1f);
            chatImageGameObject.transform.SetParent(parent.transform);
            chatImageGameObject.transform.SetAsLastSibling();



            //var rectValue = chatImageGameObject.GetComponent<RectTransform>();

            //rectValue.sizeDelta = new Vector2(415.2f, 79.48f);

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

        // Coroutine currentTypewriterEffect;

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            // if (currentTypewriterEffect != null)
            // {
            //     StopCoroutine(currentTypewriterEffect);
            // }

            CloneMessageBoxToHistory();

            text.text = dialogueLine.TextWithoutCharacterName.Text;

            // currentTypewriterEffect = StartCoroutine(ShowTextAndNotify());

            // IEnumerator ShowTextAndNotify() {
                // yield return StartCoroutine(Effects.Typewriter(text, lettersPerSecond, null));
                // currentTypewriterEffect = null;
                audioManager.playText = true;
                onDialogueLineFinished();
            // }
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
