using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Ink.Runtime;

/// <summary>
/// Met lien Ink, l'UI et le PNJ
/// </summary>
[DisallowMultipleComponent]
public class DialogueManager : MonoBehaviour
{
    // La seule et unique instance de dialogue Manager
    public static DialogueManager instance;

    private GameObject _player;
    private DialogueController _NPC;

    [SerializeField, Tooltip("Intervalle de temps entre 2 charact�res"), Min(0)]
    private float _dialogueSpeed = 0.02f;

    //Ink
    #region Ink
    private Story _story;
    private const string INKTAG_PORTRAIT = "portrait";
    private const string INKTAG_ADDFONCTION = "add";

    #endregion

    // Tout les �l�ments utile de l'UI de dialogue
    #region UIcomponent
    [SerializeField] private GameObject _dialoguePanel;

    [SerializeField] private Text _dialogueText;
    [SerializeField] private Button _nextButton;

    [SerializeField] private Image _playerPortrait;
    [SerializeField] private Text _playerName;

    [SerializeField] private Image _PNJPortrait;
    [SerializeField] private Text _PNJName;

    [SerializeField] private List<Button> _answerButtons;
    #endregion

    private void Start()
    {
        // Permet de v�rifier qu'il n'y a qu'une seule instance de la classe dans la sc�ne
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de DialogueManager dans la scene");
            return;
        }
        instance = this;

        // On r�cup�re le Player
        _player = FindObjectOfType<PlayerController>().gameObject;

        _dialoguePanel.SetActive(false);

        //On assigne le bouton next � la fonction en question
        _nextButton.onClick.AddListener(DisplayNextSentence);
    }

    /// <summary>
    /// Lance un dialogue avec l'UI de dialogue
    /// </summary>
    /// <param name="inkJson">Le TextAsset du dialogue</param>
    /// <param name="inkJsonState">L'�tat du dialogue comportant de potentielles modifications par rapport au TextAsset</param>
    /// <param name="NPC">Le personnage</param>
    public void StartDialogue(TextAsset inkJson, string inkJsonState, DialogueController NPC)
    {
        _NPC= NPC;

        _dialoguePanel.SetActive(true);

        // On d�sactive les input inutiles en dialogue du Player
        _player.GetComponent<PlayerController>().StopMoveInput();

        // Place le controller sur le boutton Continue
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_nextButton.gameObject);

        //On cr�e une nouvelle Story
        _story = new Story(inkJson.text);

        // Si le dialogue a d�j� �t� utilis�, alors on charge les potentielles modifications dues aux pr�c�dents utilisations
        if(inkJsonState != "")
        {
            _story.state.LoadJson(inkJsonState);
        }

        // On approche le panel avec l'anim'
        //_animator.SetBool("IsOpen", true);

        // On affiche le nom du PNJ et du perso
        _playerName.text = "ThePlayer";
        _PNJName.text = _NPC.Character.Name;

        // On affiche les portraits par d�faut des personnages
        _PNJPortrait.sprite = _NPC.Character.NormalFace;


        // On affiche la premi�re phrase du knot Start
        _story.ChoosePathString("Start");
        DisplayNextSentence();
    }

    /// <summary>
    /// Affiche la phrase suivante du dialogue si il y en a
    /// </summary>
    public void DisplayNextSentence()
    {
        // On arr�te la coroutine qui affichait le texte (si elle tournait encore)  //StopAllCoroutines();
        StopCoroutine(nameof(TypeSentence));
        
        // Si le blabla a une suite
        if (_story.canContinue)
        {
            //On lance la coroutine d'affichage (tout le texte jusqu'au prochain choix)
            StartCoroutine(TypeSentence(_story.Continue()));
        }
        else
        {
            //Sinon fin du dialogue
            EndDialogue();
            return;
        }

        //Gestion des tags
        TagsManagement(_story.currentTags);

        // On d�saffiche et nettoye tout les boutons de r�ponse
        foreach(Button button in _answerButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }

        //Si il y a un choix � faire
        if(_story.currentChoices.Count > 0)
        {
            // On masque le bouton next
            _nextButton.gameObject.SetActive(false);

            //Pour tout les choix actuels
            for (int i = 0; i < _story.currentChoices.Count; i++)
            {
                //Si il y a assez de boutons de r�ponses
                if (_answerButtons.Count > i)
                {
                    Choice choice = _story.currentChoices[i];
                    // On affiche un nouveau bouton d�di�
                    _answerButtons[i].gameObject.SetActive(true);

                    //On affiche dans le bouton le texte du choix d�di�
                    Text buttonText = _answerButtons[i].GetComponentInChildren<Text>();
                    buttonText.text = _story.currentChoices[i].text;

                    //On assigne au bouton le choix d�di� en lui ajoutant l'event de choix et comme parametre le choix d�di�
                    _answerButtons[i].onClick.AddListener(delegate { ChooseStoryChoice(choice); });
                }
                else
                {
                    Debug.LogWarning($"Le dialogue \"{_story.currentText}\" contient plus de r�ponse possible que d'onglets de r�ponse disponibles ");
                    break;
                }
            }
        }
        else
        {
            // Sinon on affiche le bouton next si il �tait masqu�
            _nextButton.gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// Gestion des diff�rents Tags afin de modifier UI et data en fonction de la story
    /// </summary>
    /// <param name="tags">Liste des tags actuels</param>
    private void TagsManagement(List<string> tags)
    {
        // Pour tout les tags
        foreach (string tag in tags)
        {
            // On brise le tag en 2 au niveau du ":"
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogWarning($"Le tag \"{tag}\" n'est pas fonctionnel");
                continue;
            }

            // On r�cup�re la cl� (type d'action � faire) et la valeur du tag
            string tagKey = splitTag[0].Trim().ToLower();
            string tagValue = splitTag[1].Trim().ToLower();

            // On teste la cl� afin de savoir quelle action effectuer
            switch (tagKey)
            {
                case INKTAG_PORTRAIT:
                    Debug.Log("Emotion : "+ tagValue);
                    switch (tagValue)
                    {
                        case "normal":
                            _PNJPortrait.sprite = _NPC.Character.NormalFace;
                            break;
                        case "angry":
                            _PNJPortrait.sprite = _NPC.Character.AngryFace;
                            break;
                        case "happy":
                            _PNJPortrait.sprite = _NPC.Character.HappyFace;
                            break;
                        case "sad":
                            _PNJPortrait.sprite = _NPC.Character.SadFace;
                            break;
                        default:
                            Debug.LogWarning($"Le portrait \"{tagValue}\" n'est pas reconnu");
                            break;
                    }
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
    /// Fonction pour les boutons, qui fait continuer la story selon le choix
    /// </summary>
    /// <param name="choice">Le choix associ� au bouton</param>
    public void ChooseStoryChoice(Choice choice)
    {
        // Dit au Ink de suivre ce chemin
        _story.ChooseChoiceIndex(choice.index);

        DisplayNextSentence();
    }

    /// <summary>
    /// Met fin au dialogue et cache l'UI
    /// </summary>
    private void EndDialogue()
    {
        // On sauvegarde dans le PNJ les nouvelles potentielles modifications de son dialogue
        _NPC.InkJsonState = _story.state.ToJson();

        _story = null;
        _NPC = null;

        //_animator.SetBool("IsOpen", false);

        // Place le controller sur aucun bouton
        EventSystem.current.SetSelectedGameObject(null);

        // On r�active les input inutiles en dialogue du Player
        _player.GetComponent<PlayerController>().ResumeMoveInput();

        _dialoguePanel.SetActive(false);

        // On dit au PNJ d'arr�ter de parler
        //_NPCtalking.IsTalking = false;
    }

    /// <summary>
    /// Affiche la phrase charact�re par charact�re
    /// </summary>
    /// <param name="sentence">La phrase � afficher</param>
    IEnumerator TypeSentence(string sentence)
    {
        // On clean le texte
        _dialogueText.text = "";
        
        // Affiche un par un chaque charact�re de la sentence
        foreach (char letter in sentence.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(_dialogueSpeed);
        }
    }
}