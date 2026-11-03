# UdonVR Record Player Module: VizVid

Compatibility hook bridging the **UdonVR Record Player** (`RecordPlayer_VideoplayerClass`) with **JLChnToZ VizVid (VVMW)**.

## Feature Compatibility

| Capability | Status | Implementation Details |
| :--- | :---: | :--- |
| **Editor Auto-Setup** | `Full` | Supported via `RecordPlayerAutoSetup` editor script |
| **Load URL** | `Full` | Calls `vizVid.PlayUrl(_url, playerType)` |
| **External Load Detection** | `Full` | `_onVideoStart()` (and `_OnVideoStart()` alias) triggers `_OnVideoLoad()` |
| **Play / Pause / Stop** | `Full` | Uses `vizVid.Play()`, `vizVid.Pause()`, and `vizVid.Stop()` |
| **Seeking & Scrubbing** | `Full` | Remaps target time (`0..1`) and sets `vizVid.Progress` |
| **Tonearm Progress** | `Full` | Remaps `vizVid.Time` / `vizVid.Duration` (`0..1`) |
| **2-Way Volume Sync** | `Full` | Syncs on start and runtime via `_OnVolumeChange()` |
| **Permission / Lock Check** | `N/A` | VizVid Core does not use a lock system; `_CanPlayVideo()` returns `true` |
| **Album Auto-Advance & Shuffle** | `Full` | `_onVideoEnd()` (and `_OnVideoEnd()` alias) triggers `_PlaybackEnded()` |
| **Single-Track Loop** | `Full` | Resets `vizVid.Progress = 0f` via `_loopCurrentUrl()` |
| **Custom Track Titles** | `None` | Not supported by `Core.PlayUrl`; `_SetTitle()` is not overridden |

## Inspector Setup
1. Add `RecordPlayer_VideoPlayer_VizVid` to your module GameObject.
2. Assign your `RecordPlayer` instance to **`recordPlayer`**.
3. Assign your scene's VizVid `Core` component to **`vizVid`**.
4. Set **`playerType`** (`byte`) to match your desired VizVid backend player index (default `0`).

<!-- MODULE_MANIFEST_START -->
```json
{
  "schemaVersion": "1.1.0",
  "id": "VizVid",
  "displayName": "JLChnToZ VizVid",
  "repo": "[https://github.com/UdonVR/RecordPlayerModule_VizVid](https://github.com/UdonVR/RecordPlayerModule_VizVid)",
  "scriptPath": "RecordPlayer_VideoPlayer_VizVid.cs",
  "className": "UdonVR.RecordPlayer.RecordPlayer_VideoPlayer_VizVid",
  "baseClass": "UdonVR.RecordPlayer.RecordPlayer_VideoplayerClass",
  "autoSetup": true,
  "requiredNamespaces": [
    "JLChnToZ.VRC.VVMW"
  ],
  "inspectorReferences": [
    {
      "fieldName": "recordPlayer",
      "fieldType": "UdonVR.RecordPlayer.RecordPlayer",
      "required": true,
      "description": "Core RecordPlayer controller"
    },
    {
      "fieldName": "vizVid",
      "fieldType": "JLChnToZ.VRC.VVMW.Core",
      "required": true,
      "description": "VizVid Core component"
    },
    {
      "fieldName": "playerType",
      "fieldType": "System.Byte",
      "required": false,
      "description": "Target VizVid player backend index passed to PlayUrl"
    }
  ],
  "capabilities": {
    "autoSetup": "full",
    "loadUrl": "full",
    "externalLoadDetection": "full",
    "playPauseStop": "full",
    "seekTime": "full",
    "playbackProgress": "full",
    "volumeSync": "bidirectional",
    "permissionCheck": "not_applicable",
    "autoAdvanceAndShuffle": "full",
    "loopTrack": "full",
    "customTitleMetadata": "none"
  },
  "notes": [
    "Includes both lowercase (_onVideoStart, _onVideoEnd) and PascalCase (_OnVideoStart, _OnVideoEnd) event hooks for VizVid compatibility.",
    "VizVid Core does not implement player locking; _CanPlayVideo() always returns true.",
    "_SetTitle() is not implemented."
  ]
}
```
<!-- MODULE_MANIFEST_END -->
