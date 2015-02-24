using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using System.Diagnostics;

namespace DenonCMD
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                string hostname = args[0];

                PostParamCollection postParamCollection = new PostParamCollection();
                List<string> getCollection = new List<string>();

                for (int i = 1; i < args.Length; i++)
                {
                    //ability to set the volume to the chosen value
                    //e.g. volume-40 sets the volume to -40db
                    if (args[i].Contains("vol-"))
                    {
                        getCollection.Add("/goform/formiPhoneAppVolume.xml?1+" + args[i].Substring(3));
                    }
                    else
                    {
                        switch (args[i])
                        {
                            case "powerOn":
                                postParamCollection.Add(new PostParam("cmd0", "PutZone_OnOff/ON"));
                                break;
                            case "powerOff":
                                postParamCollection.Add(new PostParam("cmd0", "PutZone_OnOff/OFF"));
                                break;
                            case "power":
                                postParamCollection.Add(switchPower(hostname));
                                break;

                            case "muteOn":
                                postParamCollection.Add(new PostParam("cmd0", "PutVolumeMute/on"));
                                break;
                            case "muteOff":
                                postParamCollection.Add(new PostParam("cmd0", "PutVolumeMute/off"));
                                break;
                            case "mute":
                                postParamCollection.Add(switchMute(hostname));
                                break;

                            case "volUp":
                                postParamCollection.Add(new PostParam("cmd0", "PutMasterVolumeBtn/>"));
                                break;
                            case "volDown":
                                postParamCollection.Add(new PostParam("cmd0", "PutMasterVolumeBtn/<"));
                                break;
                            
                            case "inputGame":
                                getCollection.Add("/goform/formiPhoneAppDirect.xml?SIGAME");
                                break;
                            case "inputCD":
                                getCollection.Add("/goform/formiPhoneAppDirect.xml?SICD");
                                break;
                            case "inputDVD":
                                getCollection.Add("/goform/formiPhoneAppDirect.xml?SIDVD");
                                break;
                            case "inputBluRay":
                                getCollection.Add("/goform/formiPhoneAppDirect.xml?SIBD");
                                break;
                            case "inputSatCbl":
                                getCollection.Add("/goform/formiPhoneAppDirect.xml?SISAT/CBL");
                                break;
                            case "inputMediaPlayer":
                                getCollection.Add("/goform/formiPhoneAppDirect.xml?SIMPLAY");
                                break;

                            case "inputTuner":
                                postParamCollection.Add(new PostParam("cmd0", "PutZone_InputFunction/TUNER"));
                                break;
                            case "tunerPresetUp":
                                getCollection.Add("/goform/formiPhoneAppTuner.xml?1+PRESETUP");
                                break;
                            case "tunerPresetDown":
                                getCollection.Add("/goform/formiPhoneAppTuner.xml?1+PRESETDOWN");
                                break;

                            case "netPlayPause":
                                postParamCollection.Add(new PostParam("cmd0", "PutNetAudioCommand/CurEnter"));
                                break;
                            case "netNextTrack":
                                postParamCollection.Add(new PostParam("cmd0", "PutNetAudioCommand/CurDown"));
                                break;
                            case "netPrevTrack":
                                postParamCollection.Add(new PostParam("cmd0", "PutNetAudioCommand/CurUp"));
                                break;                            

                            default:
                                break;
                        }
                    }
                }
                try
                {
                    if (postParamCollection.Count > 0)
                    {
                        HttpPost httpPost = new HttpPost("http://" + hostname + "/MainZone/index.put.asp");
                        foreach (PostParam param in postParamCollection)
                        {
                            httpPost.doPost(param);
                        }
                        
                    }
                    if (getCollection.Count > 0)
                    {
                        WebRequest wrGETURL;
                        foreach (string call in getCollection)
                        {
                            wrGETURL = WebRequest.Create("http://" + hostname + call);
                            wrGETURL.GetResponse();
                            wrGETURL.Abort();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[DenonCMD] " + ex.Message);
                }
            }
            else
            {
                Debug.WriteLine("[DenonCMD] " + "I need two or more arguments arguments: host command [command...]");
            }
        }

        private static XmlDocument getConfig(string hostname)
        {
            XmlDocument xm = new XmlDocument();
            try
            {
                WebRequest wrGETURL;
                wrGETURL = WebRequest.Create("http://" + hostname + "/goform/formMainZone_MainZoneXml.xml");

                Stream objStream;
                objStream = wrGETURL.GetResponse().GetResponseStream();
                StreamReader objReader = new StreamReader(objStream);

                
                xm.LoadXml(objReader.ReadToEnd());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[DenonCMD] " + ex.Message);
            }
            return xm;
        }

        private static PostParam switchPower(string hostname)
        {
            XmlDocument xm = getConfig(hostname);
            XmlNodeList power = xm.GetElementsByTagName("Power");
            if (power.Count == 1)
            {
                string currMuteStatus = power[0].InnerText;
                if (currMuteStatus == "STANDBY")
                {
                    return new PostParam("cmd0", "PutZone_OnOff/ON");
                }
                else
                {
                    return new PostParam("cmd0", "PutZone_OnOff/OFF");
                }
            }
            else return new PostParam();
        }

        private static PostParam switchMute(string hostname)
        {
            XmlDocument xm = getConfig(hostname);
            XmlNodeList mute = xm.GetElementsByTagName("Mute");
            if (mute.Count == 1)
            {
                string currMuteStatus = mute[0].InnerText;
                if (currMuteStatus == "off")
                {
                    return new PostParam("cmd0", "PutVolumeMute/on");
                }
                else
                {
                    return new PostParam("cmd0", "PutVolumeMute/off");
                }
            }
            else return new PostParam();
        }
    }
}
