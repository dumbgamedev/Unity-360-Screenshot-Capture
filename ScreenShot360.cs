// Custom Action by DumbGameDev
// www.dumbgamedev.com

using UnityEngine;
using System.IO;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("Custom")]
    [Tooltip("Take a 360 screen shot image.")]
    public class ScreenShot360 : FsmStateAction
    {
        [RequiredField]
        [Tooltip("Nav agent game object.")]
        public FsmOwnerDefault camera;

        [Tooltip("This number should be variables of 2. Such as 1024, 2048, etc.")]
        public FsmInt imageWidth;

        [Tooltip("If set to false, a PNG image will be used")]
        public FsmBool useJPEG;

        [Tooltip("Set a unique string name for the photo save file. Such as Photo + time")]
        public FsmString photoSaveName;

        // private variables

        private Camera _camera;

        public override void Reset()

        {
            camera = null;
            imageWidth = 1024;
            useJPEG = true;
            photoSaveName = "photo";
        }

        public override void OnEnter()
        {
            var go = Fsm.GetOwnerDefaultTarget(camera);
            _camera = go.GetComponent<Camera>();

            TakePhoto();
            Finish();
        }

        void TakePhoto()
        {
            var go = Fsm.GetOwnerDefaultTarget(camera);
            if (go == null)
            {
                return;
            }

            if (imageWidth.Value < 1)
            {
                return;
            }

            if (_camera == null)
            {
                Debug.LogError("Playmaker error: No camera component for 360 screen shot found on " + go.name);
                return;
            }

            // take a photo
            byte[] bytes = I360Render.Capture(imageWidth.Value, useJPEG.Value, _camera);
            if (bytes != null)
            {
                string path = Path.Combine(Application.persistentDataPath, photoSaveName + (useJPEG.Value ? ".jpeg" : ".png"));
                File.WriteAllBytes(path, bytes);
                Debug.Log("360 render saved to " + path);
            }
        }
    }
}