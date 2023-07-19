using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        private RawImage chatImage;

        private GameObject chatImageGameObject;

        //gets where the image needs to be put
        public GameObject parent;
        public AudioManager audioManager;

        public GameObject optionsContainer;
        public OptionView optionPrefab;

        [Tooltip("This is the chat message bubble UI object (what we are cloning for each message!)... NOT the container group for all chat bubbles")]
        public GameObject dialogueBubblePrefab;
        public float lettersPerSecond = 20f;
        bool isFirstMessage = true;

        // current message bubble styling settings, modified by SetSender
        bool isRightAlignment = true;
        Color currentBGColor = Color.black, currentTextColor = Color.white;
        public Image fade;
        public Transform chatParent;

        IEnumerator WaitForSound()
        {
            //checks to see if audio is playing
            if (audioManager != null){
                yield return new WaitForEndOfFrame();
            } else{
                //wait until song stops playing then fade to black
                //sound should be changed to the current song playing
                yield return new WaitUntil(() => GameManager.game.audioManager.isCurrentSongEnded == true);
                FadeBlack();
            }

        }

        void StartWaitForSound()
        {
            StartCoroutine(WaitForSound());
        }
        

        void Awake()
        {
            runner = GetComponent<DialogueRunner>();
            runner.AddCommandHandler("Me", SetSenderMe ); // registers Yarn Command <<Me>>, which sets the current message sender to "Me"
            runner.AddCommandHandler("Them", SetSenderThem ); // registers Yarn Command <<They>>, which sets the current message sender to "Them" (whoever the player is talking to)
            runner.AddCommandHandler("DisplayImage", DisplayImage);
            runner.AddCommandHandler("DisplayMoneyshot", DisplayMoneyshot);
            runner.AddCommandHandler("FadeToBlack", FadeBlack);
            runner.AddCommandHandler("StartWaitForSound", StartWaitForSound);
            // runner.AddCommandHandler<int>("NextStage", SetNextStage); // set nextstage stage. ends dialogue and goes to next stage
            // runner.AddCommandHandler<Trigger>("CheckTrigger", OnTrigger) //taking a trigger as an argument, it will wait until the trigger is activated, and then go to the next dialogue
            chatParent = dialogueBubblePrefab.transform.parent; 
            Debug.Log(chatParent.name);
            originalY = chatParent.position.y;           
            
            // optionsContainer.SetActive(false); 
        }
        bool startFade = false;
        void FadeBlack(){
            startFade = true;
        }
        void Update(){
            //scroll up and down script

            if(startFade)
                fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a+0.0075f);
        }
        void Start () 
        {
            dialogueBubblePrefab.SetActive(false);
            UpdateMessageBoxSettings();
            scrollLim[0] = 0.0f;
            scrollLim[1] = 100.0f;
            GameManager.game.controls.mouse.MouseScroll.performed += ctx => ScrollChat();
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
        float[] scrollLim = new float[2];
        float scrollAmount = 1.0f;
        float originalY;
        void ScrollChat(){
            float scrollDir = -GameManager.game.controls.mouse.MouseScroll.ReadValue<float>();
            
            scrollLim[1] = chatParent.childCount * 200f;

            scrollAmount = Mathf.Clamp(scrollAmount+ (scrollDir * Time.deltaTime * 2.5f), scrollLim[0], scrollLim[1]);
            Debug.Log("Scroll Direction:" + scrollDir);
            Debug.Log("Scroll Amount: " + scrollAmount);
            
            chatParent.position = new Vector3(chatParent.position.x, originalY-scrollAmount, chatParent.position.z);
        }
        private Sprite picToPost;
        bool firstAfterImage = false;
        void DisplayImage() // TODO: you have to fix the texting order
        {

            Texture2D sprite = GameManager.game.drawUtils.textPic;

            // Sprite picToPost = Sprite.Create(sprite,new Rect(0,0,sprite.width,sprite.height),new Vector2(0.5f, 0.5f));
            
            chatImageGameObject = new GameObject();
            chatImage = chatImageGameObject.AddComponent<RawImage>() as RawImage;
            chatImage.texture = sprite;

            chatImageGameObject.AddComponent<HorizontalLayoutGroup>();
            
            var rectValue = chatImageGameObject.AddComponent<LayoutElement>();

            rectValue.preferredHeight = 200f;
            // rectValue.preferredWidth = 2f;

            var localScale = chatImageGameObject.GetComponent<RectTransform>();

            localScale.localScale = new Vector3 (0.35f, 0.5f, 1f) * 0.75f;
            chatImageGameObject.transform.SetParent(parent.transform);
            chatImageGameObject.transform.SetAsLastSibling();
            chatImageGameObject.SetActive(true);
            firstAfterImage = true;


            //var rectValue = chatImageGameObject.GetComponent<RectTransform>();

            //rectValue.sizeDelta = new Vector2(415.2f, 79.48f);

            Debug.Log("Posted Image to Chat");
            //Instantiate(chatImageGameObject, dialogueBubblePrefab.transform.parent);

            //Sprite.Create(picToPost, new Rect(0.0f, 0.0f, picToPost.width, picToPost.height), new Vector2(0.5f, 0.5f), 100.0f);
            //var bg = dialogueBubblePrefab.GetComponentInChildren<Image>();
            //bg.sprite = spriteArray[spriteID];
            //Instantiate<Sprite>(spriteArray[spriteID], bg.transform.position, bg.transform.rotation);

        }
        public Texture2D moneyshot;
        void DisplayMoneyshot()
        {
            // Sprite picToPost = Sprite.Create(sprite,new Rect(0,0,sprite.width,sprite.height),new Vector2(0.5f, 0.5f));
            
            chatImageGameObject = new GameObject();
            chatImage = chatImageGameObject.AddComponent<RawImage>() as RawImage;
            chatImage.texture = moneyshot;

            chatImageGameObject.AddComponent<HorizontalLayoutGroup>();
            

            var rectValue = chatImageGameObject.AddComponent<LayoutElement>();

            rectValue.preferredHeight = 200f;

            var localScale = chatImageGameObject.GetComponent<RectTransform>();

            localScale.localScale = new Vector3 (0.35f, 0.5f, 1f);
            chatImageGameObject.transform.SetParent(parent.transform);
            chatImageGameObject.transform.SetAsLastSibling();
            chatImageGameObject.SetActive(true);
            firstAfterImage = true;


            //var rectValue = chatImageGameObject.GetComponent<RectTransform>();

            //rectValue.sizeDelta = new Vector2(415.2f, 79.48f);

            Debug.Log("Posted Image to Chat");
            //Instantiate(chatImageGameObject, dialogueBubblePrefab.transform.parent);
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
                Debug.LogWarning("Set bubble as last sibling in UpdateMessageBox.");
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
                    chatParent
                );
                dialogueBubblePrefab.transform.SetAsLastSibling();
                Debug.LogAssertion("Set bubble as last sibling in CloneMessageBox.");
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
            if (firstAfterImage){
                chatImageGameObject.transform.SetAsLastSibling();
                chatImageGameObject.transform.SetSiblingIndex(chatImageGameObject.transform.GetSiblingIndex()-1);
                // chatImageGameObject.GetComponent<RectTransform>().localScale = new Vector3 (0.35f, 0.5f, 1f);
                // Debug.LogAssertion("Set bubble as last sibling in RunLine.");
                // var replacedObject = chatImageGameObject.transform.parent.GetChild(chatImageGameObject.transform.parent.childCount-1);
                // Debug.LogWarning(replacedObject.name);
                // replacedObject.SetSiblingIndex(replacedObject.GetSiblingIndex()-1);
                firstAfterImage = false;
            }
            

            text.text = dialogueLine.TextWithoutCharacterName.Text;

            currentTypewriterEffect = StartCoroutine(ShowTextAndNotify());

            IEnumerator ShowTextAndNotify() {
                yield return StartCoroutine(Effects.Typewriter(text, lettersPerSecond, null));
                currentTypewriterEffect = null;
                
                onDialogueLineFinished();
            }
            audioManager.playText = true;
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
