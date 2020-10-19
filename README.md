# BeatFollowerPlugin
A Beat Saber plugin to record activities and song recommendations to your [BeatFollower.com](https://www.beatfollower.com) profile.  Twitch Extensions and other potential streamer tools are planned, along with external API's for tools to use.

Please raise issues for both the website and the plugin in the issues section.

To get started with BeatFollower you will need to install the BeatFollower Plugin to Beat Saber. Hopefully, in the future, the mod will be approved on BeatMods and appear in ModAssistant. However, for the moment you can grab it from the releases section of the GitHub Repository.

## Prerequisites
Please make sure to have the following plugins installed:
* BeatSaberMarkupLanguage (v1.3.2 was tested, newer may work)
* SongCore (v2.9.1 was tested, newer may work)
* BS Utils v1.4.9 (v1.4.9 or greater is required, 1.4.8 will not work!)

## Installation Instructions
1. If you have installed the plugin with ModAssistant, you can skip to step 3.  Start by downloading and extracting the latest BeatFollower.dll from the releases section of this GitHub repository
1. Second, you should copy the BeatFollower.dll to your Beat Saber Plugins folder
1. Next, you can start your game, but you should wait just a moment before putting your VR headset on..
1. You now need to generate an API Key by visiting the Plugin Details tab, on your profile at [BeatFollower](https://www.beatfollower.com).
	1. Make sure to click Generate Key. It's important not to share this key with anyone, if you do please go and generate a new one the follow the steps below.
1. Copy your API Key and then open up the BeatFollower.ini in your prefered text editor. This file will be in the UserData folder, inside your Beat Saber installation folder (Note: you must start the game, with the plugin installed, for this file to get generated)
1. Highlight the default API Key in the file (it should be a series of zero's). Then, paste the API Key you copied from this website. Make sure to save the file.
1. That's it! You are good to go, you don't even need to restart the game.

