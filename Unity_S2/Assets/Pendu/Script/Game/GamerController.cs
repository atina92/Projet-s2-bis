using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine;
using Util;
using WebSocketSharp;
using Random = System.Random;

namespace Game
{
    public class GamerController : MonoBehaviour
    {
        public Text wordIndicator;

        public Text scoreIndicator;
        public Text letterIndicator;
        
        public InputField Letter_input;
        
        private static string _inputOfPlayer;

       // private HangmanController hangman;
        private string word;
        private char[] revealed;
        private int score;
        private bool completed;

        private int nb_errors;

        [SerializeField] 
        private GameObject YouLost;
        
        [SerializeField] 
        private GameObject YouWon;
        
        [SerializeField] 
        private GameObject Buttons;

        // Start is called before the first frame update
        void Start()
        {
            //hangman = GameObject.FindGameObjectsWithTag("Player").GetComponent<HangmanController>();
            reset();
        }

        public void Update()
        {
            Debug.Log(word);
            
            if(Letter_input.text != null)
                 _inputOfPlayer = Letter_input.text;

            if (nb_errors == 7)
            {
                this.gameObject.SetActive(false);
                YouLost.SetActive(true);
                Buttons.SetActive(true);
                for (int i = 0; i < revealed.Length; i++)
                {
                    revealed[i] = word[i];
                }
                UpdateWorldIndicator();
            }

            if (completed)
            {
                this.gameObject.SetActive(false);
                YouWon.SetActive(true);
                Buttons.SetActive(true);
            }
        }
        // Update is called once per frame
        public void OnValidateCLick()
        {
            if (completed)
            {
                if (Input.anyKeyDown)
                    next();
            }
            char? c = Pendu_main.GetInput().ToString().ToUpper()[0];
            if (c != null && TextUtils.isAlpha((char) c))
            {
                check(c);
                //if (!check(s).ToUpper)
                //hangman.punish();
                /*
                 * if (hangman.isDead)
                 * worldIndicator.text =word;
                 * completed=true;
                 */
            }

        }

        private bool check(char? c)
        {
            bool ret = false;
            int complete = 0;
            int score = 0;
            for (int i = 0; i < revealed.Length; i++)
            { 
                if (c == word[i])
                {
                    ret = true;
                    if (revealed[i] == 0)
                    {
                        revealed[i] = (char) c;
                        score += 100;
                    }
                }
                if (revealed[i] != 0)
                    complete++;
               
            }

            if (score != 0)
            {
                this.score += score;
                if (complete == revealed.Length)
                {
                    this.completed = true;
                    this.score += revealed.Length * 100;
                }
                UpdateWorldIndicator();
                updateIndicatorScore();
            }

            if (!ret && !letterIndicator.text.Contains((char) c))
            {
                letterIndicator.text += " ";
                letterIndicator.text += c;
                nb_errors++;
            }
            return ret;
        }

        private void UpdateWorldIndicator()
        {
            string displayed = "";
            
            
            for (int i = 0; i < word.Length; i++)
            {
                char c = revealed[i];

                if (c == 0 )
                {
                    if (word[i] >= 'A' && word[i]<= 'Z' || word[i] >= 'a' && word[i] <= 'z')
                    {
                        c = '_';
                    }
                    else
                    {
                        c = word[i];
                    }
                }

                displayed += ' ';
                displayed += c;

            }

            wordIndicator.text = displayed;
        }

        private void updateIndicatorScore()
        {
            scoreIndicator.text = "Score: " + score;
        }

        private void setWorld(string world)
        {
            world = world.ToUpper();
            this.word = world;
            revealed = new char[world.Length];
            letterIndicator.text =  " Letter: "+'\n';
            UpdateWorldIndicator();

        }
        public void next()
        {
            word = GetWord();
            setWorld(word);
            //setWorld("A-tester*!é");
        }
        public static string GetWord(string path = "Assets/Pendu/Script/word_bank.txt")
        {
            if (!File.Exists(path))
                throw new ArgumentException("Loader: couldn't load word bank at " + path);
            
            try
            {
                var index = new Random().Next(File.ReadLines(path).Count());
                return File.ReadLines(path).Skip(index).Take(1).First();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void reset()
        {
            nb_errors = 0;
            completed = false;
            YouLost.SetActive(false);
            YouWon.SetActive(false);
            Buttons.SetActive(false);
            this.gameObject.SetActive(true);
            updateIndicatorScore();
            next();
        }

        public void OnRestartClick()
        {
            reset();
        }

        public void OnExitCLick()
        {
            PhotonNetwork.LoadLevel(0);
            PhotonNetwork.LeaveRoom();
        }
    }
}