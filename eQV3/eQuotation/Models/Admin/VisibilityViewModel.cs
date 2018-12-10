
using eQuotation.Entities;
using eQuotation.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace eQuotation.Models.Admin
{
    public class VisibilityViewModel : ViewModelBase<Object>
    {

        public List<VisibilityItemViewModel> Elements { get; set; }

        public VisibilityViewModel(string CategoryID) : base()
        {
            this.Elements = new List<VisibilityItemViewModel>();
            if (string.IsNullOrEmpty(CategoryID))
            {
                Init();
            }else
            {
                SelectTab(CategoryID);
            }
          
        }

        public override void Init()
        {
            //get the default menu elements
            this.Elements.AddRange(SetMenu(this.UnitWork.MenuElement.Get(x => x.Active).ToList()));
        }

        public void SelectTab(string CategoryID)
        {
            //get the default menu elements
            this.Elements.AddRange(SetMenu(this.UnitWork.MenuElement.Get(x => x.Active && x.CategoryID== CategoryID).ToList()));
        }

        public override void SetValue()
        {
            throw new NotImplementedException();
        }

        public override void GetValue(object data)
        {

            if (this.Elements.Count() > 0)
            {
                //put mark on first row of category or group
                bool isNewCat = true, isNewGroup = true;
                string actCat = null, actGroup = null;

                foreach (var menu in this.Elements.OrderBy(x => x.ProcIDCat).ThenBy(x => x.ProcIDGroup).ThenBy(x => x.ProcIDElem))
                {
                    isNewGroup = actGroup != menu.GroupID ? true : false;
                    isNewCat = actCat != menu.CategoryID ? true : false;

                    menu.IsNewCategory = isNewCat;
                    menu.IsNewGroup = isNewGroup;

                    actGroup = menu.GroupID;
                    actCat = menu.CategoryID;
                }

                //sort menus based on hierarchy level
                this.Elements = this.Elements.OrderBy(x => x.ProcIDCat)
                    .ThenByDescending(x => x.IsNewCategory)
                    .ThenBy(x => x.ProcIDGroup)
                    .ThenByDescending(x => x.IsNewGroup)
                    .ThenBy(x => x.ProcIDElem).ToList();
            }
        }

        public void UpdateControl(string param)
        {
            var controls = param.Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);

            foreach(var elem in controls)
            {
                var input = elem.Split('=');
                string eleId = input[0]; bool val = Convert.ToBoolean(input[1]);

                if (!val)
                {
                    //the given element is not selected and must be deleted
                    this.UnitWork.MenuControl.DeleteAll(x => x.ElementID == eleId && x.AppName == AppContext.AppName);
                }
                else
                {
                    //check if the given element has been registered
                    var elements = this.UnitWork.MenuControl.Get(x => x.AppName == AppContext.AppName && x.ElementID == eleId);

                    if (elements.Count() == 0)
                    {
                        //insert element into MENUCONTROL table
                        var newControl = new MenuControl();
                        newControl.ID = this.UnitWork.MenuControl.NewID(x => x.ID, 4);
                        newControl.AppName = AppContext.AppName;
                        newControl.ElementID = eleId;
                        newControl.Timestamp = DateTime.Now;
                        newControl.CreatedBy = HttpContext.Current.User.Identity.Name;
                        newControl.Active = true;

                        this.UnitWork.MenuControl.Insert(newControl);
                    }
                }
            }
        }

        private List<VisibilityItemViewModel> SetMenu(List<MenuElement> elements)
        {
            var menuList = new List<VisibilityItemViewModel>();

            foreach (var menu in elements)
            {
                var newElement = new VisibilityItemViewModel();
                
                //set category
                if (menu.CategoryID  != null)
                {
                    newElement.CategoryID = menu.Category.ID;
                    newElement.Category = TextLan.Category(menu.Category);
                    newElement.ProcIDCat = menu.Category.ProcID;
                }
                else
                {
                    newElement.CategoryID = menu.Group.Category.ID;
                    newElement.Category = TextLan.Category(menu.Category);
                    newElement.ProcIDCat = menu.Group.Category.ProcID;
                }

                //set group
                newElement.Group = (menu.GroupID != null)? TextLan.Group(menu.Group): string.Empty;
                newElement.ProcIDGroup = (menu.GroupID != null) ? menu.Group.ProcID : 0;
                newElement.GroupID = (menu.GroupID != null) ? menu.Group.ID : string.Empty;

                newElement.ElementID = menu.ID;
                newElement.ProcIDElem = menu.ProcID;
                newElement.ElementName = TextLan.Element(menu);
                newElement.Default = menu.Default;
                newElement.URL = menu.ClientURL;

                //find if menu is enable or not
                if (menu.Default)
                    newElement.Enabled = true;
                else
                {
                    var mnControls = this.UnitWork.MenuControl.Get(x => x.AppName == AppContext.AppName && x.ElementID == menu.ID);
                    newElement.Enabled = mnControls.Count() > 0 ? true : false;
                }

                menuList.Add(newElement);
            }

            return menuList;
        }

        
    }

    public class TextLan
    {

        public static string Group(MenuGroup group)
        {
            var text = string.Empty;

            if (group == null) return text;

            switch (Thread.CurrentThread.CurrentCulture.Name)
            {
                case "de-DE":
                    text = group.NameDe;
                    break;
                case "zh-TW":
                    text = group.NameTw;
                    break;
                default:
                    text = group.Name;
                    break;
            }

            return text;
        }

        public static string Category(MenuCategory category)
        {
            var text = string.Empty;

            if (category == null) return text;

            switch (Thread.CurrentThread.CurrentCulture.Name)
            {
                case "de-DE":
                    break;
                case "zh-TW":
                    switch (category.Name)
                    {
                        case "System Administration":
                            text = "系統管理";
                            break;
                        case "Quotation":
                            text = "eQuotation Menu";
                            break;
                    }
                    break;
                default:
                    text = category.Name;
                    break;
            }

            return text;
        }

        public static string Element(MenuElement element)
        {
            var text = string.Empty;

            if (element == null) return text;

            switch (Thread.CurrentThread.CurrentCulture.Name)
            {
                case "de-DE":
                    break;
                case "zh-TW":
                    switch (element.Name)
                    {
                        case "User Profile":
                            text = "使用者設定";
                            break;
                        case "User Manager":
                            text = "使用者管理";
                            break;
                        case "Role Manager":
                            text = "角色管理";
                            break;
                        case "Visibility Control":
                            text = "顯示控制";
                            break;
                        case "My Team Quotation":
                            text = "My Team's Quotation";
                            break;
                        case "My Quotation":
                            text = "My Quotation";
                            break;
                        case "Create New Quotation":
                            text = "Create New Quotation";
                            break;
                        case "Approve/Reject Quotation":
                            text = "Approve/Reject Quotation";
                            break;
                    }
                    break;
                default:
                    text = element.Name;
                    break;
            }

            return text;
        } 

    }

}