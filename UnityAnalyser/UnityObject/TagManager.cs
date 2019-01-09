using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UnityAnalyzer
{

    public class SortingLayerEntry
    {
        private string name;
        public string Name
        {
            get { return name; }
        }

        private uint userID;
        public uint UserID
        {
            get { return userID; }
        }

        private uint uniqueID;
        public uint UniqueID
        {
            get { return uniqueID; }
        }
        public SortingLayerEntry(string aName,uint aUserID,uint auniqueID)
        {
            name=aName;
            userID = aUserID;
            uniqueID = auniqueID;
        }
    };

    public class TagManager:UnityObject
    {
        private List<string> tags=new List<string>();
        public List<string> Tags
        {
            get { return tags; }
        }

        private List<string> layers=new List<string>();
        public List<string> Layers
        {
            get { return layers; }
        }

        private List<SortingLayerEntry> sortingLayers = new List<SortingLayerEntry>();
        public List<SortingLayerEntry> SortingLayers
        {
            get { return sortingLayers; }
        }



        public static TagManager Create(ObjectInfo objectInfo, byte[] content, int objectOffset)
        {
            TagManager ret = new TagManager();
            int index = objectOffset + objectInfo.ByteStart;

            int tagCount = BitConverter.ToInt32(content, index);
            index += 4;
            for (int i = 0; i < tagCount; i++)
            {
                ret.tags.Add(Util.readStringAndAlign(content, objectOffset, ref index));
            }

            int layerCount = 32;
            if (objectInfo.UnityFileVersion[0] == 5 &&
                objectInfo.UnityFileVersion[1] == 3)
            {
                layerCount = BitConverter.ToInt32(content, index);
                index += 4;
            }


            for (int i = 0; i < layerCount; i++)
            {
                ret.layers.Add(Util.readStringAndAlign(content, objectOffset, ref index));
            }

            int sortingLayerCount = BitConverter.ToInt32(content, index);
            index += 4;
            for (int i = 0; i < sortingLayerCount; i++)
            {
                string name = Util.readStringAndAlign(content, objectOffset, ref index);
                uint userid = BitConverter.ToUInt32(content, index);
                index += 4;
                uint uniqueid = 0;
                if (objectInfo.UnityFileVersion[0] == 4 && objectInfo.UnityFileVersion[1] == 6)
                {
                     uniqueid= BitConverter.ToUInt32(content, index);
                     index += 4;
                }
                ret.sortingLayers.Add(new SortingLayerEntry(name,userid,uniqueid));
            }
            return ret;
        }

        public override UserControl CreateObjectInfoPanel()
        {
            if (objectInfoPanel == null)
            {
                if (System.Windows.Application.Current != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        TagManagerPanel panel = new TagManagerPanel();
                        objectInfoPanel = panel;
                        panel.SetUnityObject(this);
                    });
                }
            }
            return objectInfoPanel;
        }

        public SortingLayerEntry GetSortingLayerEntryByUniqueID(uint uniqueID)
        {
            foreach (SortingLayerEntry s in sortingLayers)
            {
                if (objectInfo.UnityFileVersion[0] == 5 && objectInfo.UnityFileVersion[1] == 3)
                {
                    if (s.UserID == uniqueID)
                    {
                        return s;
                    }
                }
                else if (objectInfo.UnityFileVersion[0] == 4 && objectInfo.UnityFileVersion[1] == 6)
                {
                    if (s.UniqueID == uniqueID)
                    {
                        return s;
                    }
                }
            }
            return null;
        }
    }
}
