using System;

namespace GitFlowVS.Extension.ViewModels
{

    public class ListItem
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as ListItem;
            if (item != null)
            {
                return String.Equals(Name, item.Name);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}