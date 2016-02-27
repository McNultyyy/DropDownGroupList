using System.Web.Mvc;

namespace DropDownGroupList
{
    public class GroupedSelectListItem : SelectListItem
    {
        public string GroupKey { get; set; }

        public string GroupName { get; set; }

        public bool Disabled { get; set; }
    }
}