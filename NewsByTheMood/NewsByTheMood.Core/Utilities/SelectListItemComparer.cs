using System.Web.Mvc;

namespace NewsByTheMood.Core.Utilities
{
    public class SelectListItemComparer : IEqualityComparer<SelectListItem>
    {
        public bool Equals(SelectListItem x, SelectListItem y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;
            return x.Value == y.Value;
        }

        public int GetHashCode(SelectListItem obj)
        {
            return obj.Value == null ? 0 : obj.Value.GetHashCode();
        }
    }
}
