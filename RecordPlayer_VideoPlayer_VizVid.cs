
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using JLChnToZ.VRC.VVMW;

namespace UdonVR.RecordPlayer
{
    public class RecordPlayer_VideoPlayer_VizVid : RecordPlayer_VideoplayerClass
    {
        public Core vizVid;
        //public FrontendHandler vizVid_handeler;
        public byte playerType;

        private void Start()
        {
            //Debug.Log("[RecordPlayer][Vizvid] Start");
            vizVid._AddListener(this);
        }

        public void _OnStop()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _OnStop");
            _End();
        }

        public void _onVideoEnd()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _onVideoEnd");
            _PlaybackEnded();
        }

        public void _onVideoStart()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _onVideoStart");
            _OnVideoLoad();
        }

        public void _OnVolumeChange()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _OnVolumeChange");
            recordPlayer.volumeSlider._Internal_UpdateValue(_GetVolume(), false);
        }

        //##############################################################################################################

        #region overrides

        public override void _Pause()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _Pause");
            if (vizVid.IsPaused)
            {
                vizVid.Play();
            }
            else
            {
                vizVid.Pause();
            }
        }
        public override void _Stop()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _Stop");
            vizVid.Stop();
        }
        public override bool _IsPaused()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _IsPaused");
            return vizVid.IsPaused;
        }
        public override void _loopCurrentUrl()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _loopCurrentUrl");
            vizVid.Progress = 0f;
        }
        public override void _LoadUrl(VRCUrl _url)
        {
            //Debug.Log($"[RecordPlayer][Vizvid] _LoadUrl {_url}");
            vizVid.PlayUrl(_url,playerType);
        }
        public override bool _CanPlayVideo()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _CanPlayVideo");
            return true;
        }
        public override bool _IsVideoplayerOwner()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _IsVideoplayerOwner");
            return Networking.IsOwner(vizVid.gameObject);
        }
        public override float _GetPlaybackTime()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _GetPlaybackTime");
            //Debug.LogError($"Active Time {vizVid.Time}/{vizVid.Duration}");
            return Remap(vizVid.Time, 0, vizVid.Duration, 0, 1);
        }
        public override float _GetPlaybackDuration_Current()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _GetPlaybackDuration_Current");
            return vizVid.Time;
        }
        public override float _GetPlaybackDuration_End()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _GetPlaybackDuration_End");
            return vizVid.Duration;
        }
        public override float _GetVolume()
        {
            //Debug.Log("[RecordPlayer][Vizvid] _GetVolume");
            return vizVid.Volume;
        }
        public override void _SetVolume(float _volume)
        {
            //Debug.Log($"[RecordPlayer][Vizvid] _SetVolume {_volume}");
            vizVid.Volume = _volume;
        }
        public override void _ChangePlaybackTime(float _time)
        {
            float newValue = Remap(_time, 0, _GetPlaybackDuration_End(), 0, 1);
            recordPlayer._Log($"[USharp] ChangePlaybackTime: {newValue}");
            vizVid.Progress = newValue;
        }

        #endregion
    }
}