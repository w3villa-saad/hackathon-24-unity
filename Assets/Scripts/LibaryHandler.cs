using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace W3Labs
{
    public class LibaryHandler : MonoBehaviour
    {
        public GameObject videoPrefab; // Reference to your image prefab
        public string videoFolderPath = "Assets/Videos"; // Path to your video folder
        public Transform gridParent; // Parent object to hold the prefabs

        void Start()
        {
            InstantiatePrefabsForVideos();
        }

        void InstantiatePrefabsForVideos()
        {
            // Get all video files in the folder
            string[] videoFiles = Directory.GetFiles(videoFolderPath, "*.mp4");

            foreach (string videoFile in videoFiles)
            {
                // Instantiate the prefab for each video
                GameObject videoThumbnail = Instantiate(videoPrefab, gridParent);

                // Set the video path or handle any other initialization
                VideoPlayer thumbnailComponent = videoThumbnail.GetComponent<VideoPlayer>();
                if (thumbnailComponent != null)
                {
                    thumbnailComponent.url = videoFile;
                    thumbnailComponent.Play();
                }
            }
        }
    }



}
