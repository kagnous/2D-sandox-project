using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UI;
using System;

public class InkScriptTest : MonoBehaviour
{
    public TextAsset inkJson;
    private Story _story;

    public Text textPrefab;
    public Button buttonPrefab;

    private const string INKTAG_PORTRAIT = "portrait";
    private const string INKTAG_ADDFONCTION = "add";

    void Start()
    {
        _story = new Story(inkJson.text);

        RefreshUI();
    }

    void RefreshUI()
    {
        EraseUI();

        // Crée un texte
        Text storyText = Instantiate(textPrefab);
        storyText.transform.SetParent(transform, false);
        // Assigne a ce texte la suite du blabla jusqu'au prochain choix
        storyText.text = LoadStoryChunk();

        // Tags
        TagsManagement(_story.currentTags);

        // Pour tout les choix actuels du blabla
        foreach ( Choice choice in _story.currentChoices )
        {
            // On crée un boutton
            Button choiceButton = Instantiate(buttonPrefab);
            choiceButton.transform.SetParent(transform, false);

            // On récupère le texte et on y assigne le texte du choix en courns de création
            Text choiceText = choiceButton.GetComponentInChildren<Text>();
            choiceText.text = choice.text;

            // On dit au bouton de lancer la suite si il est choisi, avec son propre index pour chaque boutton
            choiceButton.onClick.AddListener( delegate { ChooseStroyChoice(choice); } );
        }
    }

    /// <summary>
    /// Gestion des différents Tags afin de modifier UI et data en fonction de la story
    /// </summary>
    /// <param name="tags">Liste des tags actuels</param>
    private void TagsManagement(List<string> tags)
    {
        // Pour tout les tags
        foreach(string tag in tags )
        {
            // On brise le tag en 2 au niveau du ":"
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError($"Le tag \"{tag}\" n'est pas fonctionnel");
                continue;
            }

            // On récupère la clé (type d'action à faire) et la valeur du tag
            string tagKey = splitTag[0].Trim().ToLower();
            string tagValue = splitTag[1].Trim().ToLower();

            // On teste la clé afin de savoir quelle action effectuer
            switch(tagKey)
            {
                case INKTAG_PORTRAIT:
                    Debug.Log("Emotion : " + tagValue);
                    break;
                case INKTAG_ADDFONCTION:
                    Debug.Log("Ajout de l'item " + tagValue);
                    break;
                default:
                    Debug.LogWarning(tagKey + " non renonnu comme tag");
                    break;
            }
        }
    }

    /// <summary>
    /// Fonction assignée au boutton
    /// </summary>
    /// <param name="choice">Choix assigné au bouton</param>
    void ChooseStroyChoice(Choice choice)
    {
        // Dit au Ink de suivre ce chemin
        _story.ChooseChoiceIndex(choice.index);

        RefreshUI();
    }


    // Detruit tout
    void EraseUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    string LoadStoryChunk()
    {
        string text = "";

        // Si le blabla a une suite
        if(_story.canContinue)
        {
            // Alors on retourne le blabla jusqu'au prochain choix
            text = _story.ContinueMaximally();
        }
        
        return text;
    }
}