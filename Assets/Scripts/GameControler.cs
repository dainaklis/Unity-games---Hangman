using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GameControler : MonoBehaviour
{
    public static GameControler gamecontrol;

    // ------------- from 5val tutorialsEU -------------------------

    [SerializeField] private Text timeField;
    [SerializeField] private Text worldToFindField;
    [SerializeField] private GameObject WinText;
    [SerializeField] private GameObject LoseText;
    [SerializeField] public GameObject startButton;
    [SerializeField] private GameObject replayButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] GameObject[] hangMan;

    private float time;
    private string[] wolrdsLocal = {"KRISTINA","JANINA","ROBERTAS","MARIJA","DAINIUS","JORIS","DENAS","TOMAS","ARNOLDAS","EVELINA","KAROLIS"};
    private string[] words = File.ReadAllLines(@"Assets/Words.txt");
    private string chosenWorld;
    private string hiddenWorld;
    private int fails;
    private bool gameEnd = false;

    [SerializeField] private Animator cordAnim;


    void Awake()
    {
        gamecontrol = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
        cordAnim.Play("cordAnimations");
        
        ChoseRandomWorld();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnd == false)
        {
            TimeFunction();
        }
        

    }

// --------------------------------------------------------------------------------------------------------------
    private void TimeFunction(){
        time += Time.deltaTime;
        timeField.text = time.ToString();
    }

    private void ChoseRandomWorld(){

        // Chosen Random World from ALL and hidden letter.

        chosenWorld = wolrdsLocal[Random.Range(0, wolrdsLocal.Length)].ToUpper();

        // ----------------------From  TXT documents--------------------------------------
        // chosenWorld = words[Random.Range(0, words.Length)].ToUpper();

        Debug.Log(chosenWorld);

        for (int i = 0; i < chosenWorld.Length; i++)
        {
            char letter = chosenWorld[i];

            if (char.IsWhiteSpace(letter))
            {
                hiddenWorld += " ";
            }
            else
            {
                hiddenWorld += "*";
            }

            
        }
        
        worldToFindField.text = hiddenWorld;

    }

// -------------------------------------------------------------------------------------------------------------------------
    private void OnGUI() {
        
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.keyCode.ToString().Length == 1)
        {
            string pressedLetter = e.keyCode.ToString().ToUpper();

            Debug.Log("You pressed: "+ pressedLetter);

            if (chosenWorld.Contains(pressedLetter))
            {
                int i = chosenWorld.IndexOf(pressedLetter);
                while (i != -1)
                {
                    hiddenWorld = hiddenWorld.Substring(0, i) + pressedLetter + hiddenWorld.Substring(i + 1);

                    chosenWorld = chosenWorld.Substring(0, i) + "_" + chosenWorld.Substring(i + 1);

                    i = chosenWorld.IndexOf(pressedLetter);
                }
                
                worldToFindField.text = hiddenWorld;
            }
            else
            {
                hangMan[fails].SetActive(true);
                cordAnim.Play("cordStop");
                fails++;
            }
            
            //----------------------- LOSE and WIN text--------------------------

            if (fails == hangMan.Length)
            {
                LoseText.SetActive(true);
                replayButton.SetActive(true);
                exitButton.SetActive(true);
                gameEnd = true;
            }
            if (!hiddenWorld.Contains("*"))
            {
                WinText.SetActive(true);
                replayButton.SetActive(true);
                exitButton.SetActive(true);
                gameEnd = true;
            }

            

        }
    }
}
