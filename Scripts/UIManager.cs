using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    void Start()
    {
        //各シーンの初期化読み込み
        if (SceneManager.GetActiveScene().name == "Title") TitleInit();
        else if (SceneManager.GetActiveScene().name == "Menu") MenuInit();
        else InGameInit();

    }

    // Update is called once per frame
    void Update()
    {
        //各シーンの更新読み込み
        if (SceneManager.GetActiveScene().name == "Title") TitleUpdate();
        else if (SceneManager.GetActiveScene().name == "Menu") MenuUpdate();
        else InGameUpdate();
    }

    #region Title
    //タイトルシーン関連の処理

    //ボタン
    Button _start;
    Button _menu;
    Button _exit;

    //シーンの事前読み込み用
    AsyncOperation _stage01;

    void TitleInit()
    {
        _start = GameObject.Find("StartButton").GetComponent<Button>();
        _menu = GameObject.Find("MenuButton").GetComponent<Button>();
        _exit = GameObject.Find("ExitButton").GetComponent<Button>();

        //シーンの事前読み込み
        _stage01 = SceneManager.LoadSceneAsync("Stage01");
        _stage01.allowSceneActivation = false;
    }

    void TitleUpdate()
    {
        
        _start.onClick.AddListener(() => 
        {
            //InGame開始
            _stage01.allowSceneActivation = true;
        });

        _menu.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Menu");
        });

        _exit.onClick.AddListener(() =>
        {
            //終了処理
            if(Application.isEditor) UnityEditor.EditorApplication.isPlaying = false;
            else Application.Quit();
        });
    }

    #endregion

    #region Menu
    //メニューシーン関連の処理

    Button _back;
    AsyncOperation _titleAsync;
    void MenuInit()
    {
        _back = GameObject.Find("BackButton").GetComponent<Button>();
        _titleAsync = SceneManager.LoadSceneAsync("Title");
        _titleAsync.allowSceneActivation = false;
    }

    void MenuUpdate()
    {
        _back.onClick.AddListener(() =>
        {
            _titleAsync.allowSceneActivation = true;
        });
    }

    #endregion

    #region InGame
    //ゲームシーン関連の処理

    GameObject _player;
    Slider _hp;
    Image _shotSelect;

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
        //HPゲージの更新
        _hp.value = _player.GetComponent<Player>().getHP;

        //現在選択されてるショットの種類に応じた画像表示
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
