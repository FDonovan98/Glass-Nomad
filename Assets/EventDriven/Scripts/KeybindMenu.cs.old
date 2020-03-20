using UnityEngine;

using UnityEngine.UI;

using UnityEditor.Events;

public class KeybindMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private AgentInputHandler inputHandler;

    [SerializeField]
    private GameObject KeybindMenuElement;

    private GameObject[] menuElements;
    private Text[, ] menuElementText;

    private void Start()
    {
        InitialiseArrays();

        int i = 0;
        foreach (CommandObject elemet in inputHandler.commandList)
        {
            menuElementText[i, 0].text = elemet.ToString();
            menuElementText[i, 1].text = elemet.keycode.ToString();
            i++;
        }
    }

    private void InitialiseArrays()
    {
        inputHandler = player.GetComponent<AgentInputHandler>();
        int numberOfInputs = inputHandler.commandList.Length;

        menuElements = new GameObject[numberOfInputs];
        menuElementText = new Text[numberOfInputs, 2];

        KeybindElementFactory keybindElementFactory = new KeybindElementFactory(KeybindMenuElement, this.transform);

        for (int i = 0; i < numberOfInputs; i++)
        {
            menuElements[i] = keybindElementFactory.GetNewInstance();
            
            Text[] tempTextHolder = menuElements[i].GetComponentsInChildren<Text>();
            for (int j = 0; j < 2; j++)
            {
                menuElementText[i, j] = tempTextHolder[j];
            }

            InputField inputField = menuElements[i].GetComponentInChildren<InputField>();

            UnityEventTools.AddPersistentListener(inputField.onValueChanged, inputHandler.commandList[i].ChangeKeycode);
        }
    }
}
