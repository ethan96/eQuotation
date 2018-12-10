
using Advantech.Myadvantech.DataAccess;
using eQuotation.Entities;
using eQuotation.Models.Admin;
using eQuotation.Models.Home;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace eQuotation.Models.Home
{
    public class NavigationViewModel : ViewModelBase<Object>
    {
        public List<LeafNode> ActiveMenus { get; set; }

        public VisibilityViewModel MenuControl { get; set; }

        public List<Sop> Sops { get; set; } 

        //private const string _wsId = "003";
        //private const string _wipId = "004";

        public NavigationViewModel(string CategoryID)
            : base()
        {
            this.MenuControl = new VisibilityViewModel(CategoryID);
            this.ActiveMenus = new List<LeafNode>();
            this.Sops = new List<Sop>();
            this.Init();
        }

        public override void Init()
        {
            this.MenuControl.GetValue(null);

            // get SOP name and url 
            this.Sops = GetSOPs();

            //filter only enabled menu to be displayed
            this.MenuControl.Elements = this.MenuControl.Elements.Where(x => x.Enabled).ToList();
        }

        public override void SetValue()
        {
            throw new NotImplementedException();
        }

        public override void GetValue(object data)
        {
            var activeCat = new LeafNode();
            var activeGroup = new LeafNode();

            //put mark on first row of category or group
            bool isNewCat = true, isNewGroup = true;
            string actCat = null, actGroup = null;

            foreach (var menu in this.MenuControl.Elements.OrderBy(x => x.ProcIDCat).ThenBy(x => x.ProcIDGroup).ThenBy(x => x.ProcIDElem))
            {
                isNewGroup = actGroup != menu.GroupID ? true : false;
                isNewCat = actCat != menu.CategoryID ? true : false;

                menu.IsNewCategory = isNewCat;
                menu.IsNewGroup = isNewGroup;

                actGroup = menu.GroupID;
                actCat = menu.CategoryID;
            }

            //sort menu controls
           this.MenuControl.Elements = this.MenuControl.Elements.OrderBy(x => x.ProcIDCat)
                                            .ThenByDescending(x => x.IsNewCategory)
                                            .ThenBy(x => x.ProcIDGroup)
                                            .ThenByDescending(x => x.IsNewGroup)
                                            .ThenBy(x => x.ProcIDElem).ToList();

            //get only enabled menu
            foreach (var menu in this.MenuControl.Elements)
            {
                //create tree-node for category
                if (menu.IsNewCategory)
                {
                    var nodeCat = new LeafNode()
                    {
                        id = string.Format("C_{0}", menu.CategoryID),
                        icon = "folder",
                        label = menu.Category,
                        inode = true,
                        open = false,
                        branch = new List<LeafNode>()
                    };

                    activeCat = nodeCat;
                    activeGroup = null;

                    //add category to treeview
                    this.ActiveMenus.Add(activeCat);
                }

                //create tree-node for group
                if (menu.IsNewGroup && !string.IsNullOrEmpty(menu.Group))
                {
                    var nodeGrp = new LeafNode()
                    {
                        id = string.Format("G_{0}_{1}", menu.CategoryID, menu.GroupID),
                        icon = "folder",
                        label = menu.Group,
                        inode = true,
                        open = false,
                        branch = new List<LeafNode>()
                    };

                    activeGroup = nodeGrp;

                    //add group to active category node
                    activeCat.branch.Add(activeGroup);
                }

                //create tree-node for element
                var node = new LeafNode()
                {
                    id = menu.URL,
                    icon = "file",
                    label = menu.ElementName,
                    inode = false,
                    open = false
                };

                if (activeGroup != null)
                    activeGroup.branch.Add(node);
                else
                    activeCat.branch.Add(node);
            }
        }

        private List<Sop> GetSOPs()
        {
            var SOPs = new List<Sop>();
            var cnSOPUrl = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "ChineseSOP", "");
            if (cnSOPUrl.Any())
                SOPs.Add(new Sop() { Name = "Chinese SOP", Url = cnSOPUrl });
            var enSOPUrl = eQuotationDAL.GetRegionParameterValue(AppContext.AppRegion, "", "EnglishSOP", "");
            if (enSOPUrl.Any())
                SOPs.Add(new Sop() { Name = "English SOP", Url = enSOPUrl });

            return SOPs;
        }
    }
}