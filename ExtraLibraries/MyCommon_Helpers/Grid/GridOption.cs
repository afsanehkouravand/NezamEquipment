using System.Collections.Generic;
using System.Web.Routing;
using MyCommon.Helpers.Security;

namespace MyCommon.Helpers.Grid
{
    public class GridOption
    {
        public GridOption()
        {
            GridColumns = new List<GridColumn>();
            ListOfData = new List<object>();
            Buttons = new List<GridButton>();
            HiddenInputKeys = new List<string>();
            EnablePaging = true;
            EnablePageSize = true;
            GridFooter = new List<GridColumn>();
            ShowEmptyResult = true;
            FooterButtons = new List<GridButton>();
        }

        internal string TableLoadingRandom { get; set; }

        internal bool IsSetTableFooter { get; set; }
        internal List<GridColumn> GridFooter { get; set; }

        internal int PlusPlusNConter { get; set; }
        internal string HiddenInputKey { get; set; }
        internal IList<string> HiddenInputKeys { get; set; }

        internal RequestContext RequestContext { get; set; }

        internal bool IsRowColorRoles { get; set; }
        internal IDictionary<string, EGridColorType> RowColorRoles { get; set; }
        internal string RowColorRolesPropertyName { get; set; }

        internal bool EnabledHaveAccessTo { get; set; }
        internal object HaveAccessTo { get; set; }
        internal bool HaveAccessToIsAdmin { get; set; }
        internal IList<RoleAccessDto> HaveAccessToRoleAccess { get; set; }

        internal bool ShowEmptyResult { get; set; }
        internal string EmptyResultMessage { get; set; }

        internal string TableTitle { get; set; }

        internal EGridColorType TableColor { get; set; }

        internal List<GridColumn> GridColumns { get; set; }

        internal IEnumerable<object> ListOfData { get; set; }

        internal bool EnablePaging { get; set; }
        internal int PageTotal { get; set; }
        internal int PageSize { get; set; }
        internal int PageNumber { get; set; }
        internal bool EnablePageSize { get; set; }

        internal List<GridButton> Buttons { get; set; }
        internal List<GridButton> FooterButtons { get; set; }
        
        internal GridForm Form { get; set; }

        internal bool HideHeaderTag { get; set; }

        internal bool HideFooterIfEmpty { get; set; }
    }
}