using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class ChangeVideo : MonoBehaviour
{
    public bool NextVideo = true;
    
    [Header("Video Player")]
    public GameObject videoPlayer;
    
    [Header("Video Clip")]
    public List<VideoClip> Videos;

    [Header("UI Element")]
    public GameObject RightArrow;
    public GameObject LeftArrow;
    public GameObject ComfirmButton;
    public GameObject TurtorialTextUI;
    
    [Header("Video Texture")]
    public RenderTexture TargetTexture;

    [Header("Sound")]
    public AudioClip OpenSound;
    public AudioClip ClickSound;

    private GameObject InstVideoPlayer;
    private AudioSource audioSource;
    private string[] TurtorialText = { "<sprite=0>發射重力彈\n\n<sprite=1>吸取物品(範圍限制)", "吸取途中再次按下  <sprite=1>能做出更大的位移"
                                        ,"  特殊的外牆能夠反彈重力彈 ! "};
    private ShowVideoUI showVideoUI;


    public GameObject GetInstVideoPlayer()
    {
        return InstVideoPlayer;
    }
    private void Awake()
    {
        showVideoUI = FindObjectOfType<ShowVideoUI>();

        /** Create videoPlayer object */
        InstVideoPlayer = Instantiate(videoPlayer);

        /** If videos number less than 1 , disable right arrow image */
        if (showVideoUI.VideoIndexQuantity == 1)
        {
            RightArrow.SetActive(false);
        }
        InstVideoPlayer.GetComponent<VideoPlayer>().clip = Videos[showVideoUI.VideoIndex[0]];
        InstVideoPlayer.GetComponent<VideoPlayer>().targetTexture = TargetTexture;
        
        /** Set video text */
        TurtorialTextUI.GetComponent<TextMeshProUGUI>().text = TurtorialText[showVideoUI.VideoIndex[0]];

        /** Play ui sound */
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(OpenSound, 1.0f);
    }
    public void ChangeVideos()
    {
        audioSource.PlayOneShot(ClickSound, 0.3f);

        if (NextVideo)
        {
            InstVideoPlayer.GetComponent<VideoPlayer>().clip = Videos[showVideoUI.VideoIndex[1]];
            InstVideoPlayer.GetComponent<VideoPlayer>().Play();

            /** Set turtorial text */
            TurtorialTextUI.GetComponent<TextMeshProUGUI>().text = TurtorialText[1];

            /** UI image active setting */
            RightArrow.SetActive(false);
            LeftArrow.SetActive(true);
            ComfirmButton.SetActive(true);

            NextVideo = false;
        }
        else
        {
            InstVideoPlayer.GetComponent<VideoPlayer>().clip = Videos[0];
            InstVideoPlayer.GetComponent<VideoPlayer>().Play();

            /** Set turtorial text */
            TurtorialTextUI.GetComponent<TextMeshProUGUI>().text = TurtorialText[0];

            RightArrow.SetActive(true);
            LeftArrow.SetActive(false);
            ComfirmButton.SetActive(false);
            
            NextVideo = true;
        }
    }

}
