using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Title") TitleInit();
        else if (SceneManager.GetActiveScene().name == "Menu") { }
        else InGameInit();

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Title") TitleUpdate();
        else if (SceneManager.GetActiveScene().name == "Menu") { }
        else InGameUpdate();
    }

    #region Title

    Button _start;
    Button _menu;
    Button _exit;

    void TitleInit()
    {
        _start = GameObject.Find("StartButton").GetComponent<Button>();
        _menu = GameObject.Find("MenuButton").GetComponent<Button>();
        _exit = GameObject.Find("ExitButton").GetComponent<Button>();
    }

    void TitleUpdate()
    {
        _start.onClick.AddListener(() => 
        {
            SceneManager.LoadScene("Stage01");
        });

        _menu.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Menu");
        });

        _exit.onClick.AddListener(() =>
        {
            if(Application.isEditor) UnityEditor.EditorApplication.isPlaying = false;
            else Application.Quit();
        });
    }

    #endregion

    #region InGame

    GameObject _player;
    Slider _hp;
    Image _shotSelect;
    private int shotSelect;

    public Sprite img0;
    public Sprite img1;
    public Sprite img2;
    public Sprite img3;

    void InGameInit()
    {
        _player = GameObject.Find("Player");
        _hp = GameObject.Find("HP").GetComponent<Slider>();
        _shotSelect = GameObject.Find("ShotSelect").GetComponent<Image>();
        _hp.maxValue = _player.GetComponent<Player>().getHP;
        _hp.value = _player.GetComponent<Player>().getHP;

    }

    void InGameUpdate()
    {
        _hp.value = _player.GetComponent<Player>().getHP;

        switch (_player.GetComponent<Player>().getShotSelect)
        {
            case 0:
                _shotSelect.sprite = img0;
                break;
            case 1:
                _shotSelect.sprite = img1;
                break;
            case 2:
                _shotSelect.sprite = img2;
                break;
            case 3:
                _shotSelect.sprite = img3;
                break;
        }
    }

    #endregion
}
