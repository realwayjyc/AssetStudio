using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace UnityAnalyzer
{
    public class Component:UnityObject
    {
        /// <summary>
        /// 当在Datagrid上单击其所属的GameObject时，在GameObject的TAB中显示该控件的内容
        /// </summary>
        protected UserControl gameObjectComponentInfoControl;
        public UserControl GameObjectComponentInfoControl
        {
            get { return gameObjectComponentInfoControl; }
        }


        protected SerializedObjectIdentifier parentGameObjectIdentifier;
        public SerializedObjectIdentifier ParentGameObjectIdentifier
        {
            get { return parentGameObjectIdentifier; }
        }

        
       

        public override UserControl CreateObjectInfoPanel()
        {
            return null;
        }

        public virtual UserControl CreateGameObjectComponentInfoControl()
        {
            return null;
        }

        public GameObject GetGameObject()
        {
            return this.GetUnityObjectBySerializedObjectIdentifier(parentGameObjectIdentifier) as GameObject;
        }
    }
}
