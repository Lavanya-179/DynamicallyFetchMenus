using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using IGD2sxcBLL.TagsApp;
using IGDTheme.Components;
using IGDTheme.Components.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.HtmlControls;
using Component = IGDTheme.Components.Models.Component;

namespace IGDTheme
{
    public static class LayoutController
    {
        public static string UserString
        {
            get
            {
                UserInfo UserModel = UserController.Instance.GetCurrentUserInfo();
                var user = new
                {
                    UserModel.FirstName,
                    UserModel.LastName,
                    UserModel.UserID
                };
                return JsonConvert.SerializeObject(user);
            }
        }

        public static string PrimaryMarker
        {
            get
            {
                PortalSettings portalSettings = (PortalSettings)PortalController.Instance.GetCurrentSettings();
                string root = ((DotNetNuke.Entities.Tabs.TabInfo)portalSettings.ActiveTab.BreadCrumbs[0]).TabName.ToLower();

                IEnumerable<string> CommercialInsightRoots = Root.CommercialInsight?.Split(',').Select(x => x.ToLower());
                IEnumerable<string> SocialImpactRoots = Root.SocialImpact?.Split(',').Select(x => x.ToLower());
                IEnumerable<string> EventsRoots = Root.Events?.Split(',')?.Select(x => x.ToLower());
                IEnumerable<string> AboutRoots = Root.AboutUs?.Split(',')?.Select(x => x.ToLower());
                IEnumerable<string> LearningRoots = Root.Learning?.Split(',').ToArray();

                return CommercialInsightRoots.Contains(root) ? "CommercialInsight" :
                    SocialImpactRoots.Contains(root) ? "SocialImpact" :
                    EventsRoots.Contains(root) ? "Events" :
                    AboutRoots.Contains(root) ? "About" :
                    LearningRoots.Contains(root) ? "Learning" :
                    "";
            }
        }

        public static AreaType Area
        {
            get
            {
                var primaryMarker = PrimaryMarker;
                if (primaryMarker == "CommercialInsight")
                {
                    return AreaType.CommercialInsight;
                }
                else if (primaryMarker == "SocialImpact")
                {
                    return AreaType.SocialImpact;
                }
                else
                {
                    return AreaType.Neutral;
                }
            }
        }

        /// <summary>
        /// Retrieve the css class for the given layout
        /// </summary>
        /// <returns></returns>
        public static string LayoutClass()
        {
            switch (Area)
            {
                case AreaType.CommercialInsight:
                    return "primary";

                case AreaType.SocialImpact:
                    return "accent";

                case AreaType.Neutral:
                    return "";
            }
            return "";
        }

        public static string AdobeClientId => ConfigurationManager.AppSettings["AdobeClientId"];
        public static string LoggedIn
        {
            get
            {
                UserInfo UserModel = UserController.Instance.GetCurrentUserInfo();
                return UserModel.UserID != -1 ? "true" : "false";
            }
        }

        /// <summary>
        /// Loads a given component to the given pane
        /// </summary>
        /// <param name="pane">HtmlControl where the component is to be applied to</param>
        /// <param name="component">Component which is to be placed (must have an object that inherits HtmlControl)</param>
        public static void LoadComponentControl(ref HtmlGenericControl pane, Component component)
        {
            var type = typeof(Component);
            var properties = type.GetProperty(component.ComponentType.ToString());

            switch (component.ComponentType)
            {
                case ComponentType.SocialButtons:
                    var social = (SocialButtons)properties.GetValue(component, null);
                    pane.Controls.Add(social);
                    break;
                case ComponentType.AdvertCards:
                    var advert = (AdvertCards)properties.GetValue(component, null);
                    pane.Controls.Add(advert);
                    break;
                case ComponentType.AnchorPoint:
                    var anchor = (HtmlGenericControl)properties.GetValue(component, null);
                    pane.Controls.Add(anchor);
                    break;
                case ComponentType.AuthorInfo:
                    var author = (AuthorInfo)properties.GetValue(component, null);
                    pane.Controls.Add(author);
                    break;
                case ComponentType.ContentHeader:
                    var header = (ContentHeader)properties.GetValue(component, null);
                    pane.Controls.Add(header);
                    break;
                case ComponentType.Divider:
                    var divider = (Divider)properties.GetValue(component, null);
                    pane.Controls.Add(divider);
                    break;
                case ComponentType.HeaderImage:
                    var headerImage = (HeaderImage)properties.GetValue(component, null);
                    pane.Controls.Add(headerImage);
                    break;
                case ComponentType.PageHeader:
                    var pageHeaderImage = (PageHeader)properties.GetValue(component, null);
                    pane.Controls.Add(pageHeaderImage);
                    break;
                case ComponentType.HeroButton:
                    var heroButton = (HeroButton)properties.GetValue(component, null);
                    pane.Controls.Add(heroButton);
                    break;
                case ComponentType.Media:
                    var media = (Media)properties.GetValue(component, null);
                    pane.Controls.Add(media);
                    break;
                case ComponentType.PDFViewer:
                    var pdfViewer = (PDFViewer)properties.GetValue(component, null);
                    pane.Controls.Add(pdfViewer);
                    break;
                case ComponentType.SingleImage:
                    var image = (SingleImage)properties.GetValue(component, null);
                    pane.Controls.Add(image);
                    break;
                case ComponentType.Standfirst:
                    var standfirst = (Standfirst)properties.GetValue(component, null);
                    pane.Controls.Add(standfirst);
                    break;
                case ComponentType.Text:
                    var text = (Text)properties.GetValue(component, null);
                    pane.Controls.Add(text);
                    break;
                case ComponentType.ContentsGridView:
                    var contentsGridView = (ContentsGridView)properties.GetValue(component, null);
                    pane.Controls.Add(contentsGridView);
                    break;
            }
        }

        /// <summary>
        /// Get the second level tab from current tab.
        /// </summary>
        /// <param name="tabs"></param>
        /// <returns>Second level tab</returns>
        public static TabInfo SecondLevelTab(IEnumerable<KeyValuePair<int, TabInfo>> tabs)
        {
            TabInfo currentTab = PortalSettings.Current.ActiveTab;
            while (currentTab.Level > 1)
            {
                KeyValuePair<int, TabInfo> parent = tabs.Where(t => t.Key == currentTab.ParentId).FirstOrDefault();
                if (parent.Value != null)
                {
                    currentTab = parent.Value;
                }
                else
                {
                    break;
                }
            }
            return currentTab;
        }

        /// <summary>
        /// Property for the header to be used in the secondary nav.
        /// </summary>
        public static string SecondaryNavHeader { get; set; }

        /// <summary>
        /// Get the secondary nav menu.
        /// </summary>
        /// <returns>The secondary nav menu</returns>
        public static string GetSecondaryNav()
        {
            List<KeyValuePair<int, TabInfo>> tabs = TabController.Instance.GetTabsByPortal(0).ToList();
            TabInfo secondLevelTab = SecondLevelTab(tabs);
            // If the tab is hidden from menu, return empty array.
            if (secondLevelTab.IsVisible == false)
            {
                return "[]";
            }

            SecondaryNavHeader = secondLevelTab.Title;

            // Get all third level tabs from second level tab that are visible in menu.
            List<KeyValuePair<int, TabInfo>> thirdLevelTabs = tabs.Where(t => t.Value.Level == 2 && t.Value.ParentId == secondLevelTab.KeyID && t.Value.IsVisible).ToList();

            // Get all fourth level tabs for each third level tab that is visible in menu.
            Dictionary<int, List<KeyValuePair<int, TabInfo>>> fourthLevelTabs = new Dictionary<int, List<KeyValuePair<int, TabInfo>>>();

            foreach (var tab in thirdLevelTabs)
            {
                fourthLevelTabs.Add(tab.Key, tabs.Where(t => t.Value.Level == 3 && t.Value.ParentId == tab.Key && t.Value.IsVisible).ToList());
            }

            List<SecondaryNavViewModel> menu = new List<SecondaryNavViewModel>();

            // Populate secondary nav.
            foreach (var third in thirdLevelTabs)
            {
                List<KeyValuePair<int, TabInfo>> children = fourthLevelTabs[third.Key];
                bool isMarketsOrRetailers = third.Value.Title.ToLower().Contains("retailers") || third.Value.Title.ToLower().Contains("markets");
                int type = isMarketsOrRetailers || children.Count > 0 ? 1 : 0;
                SecondaryNavViewModel nav = new SecondaryNavViewModel
                {
                    id = third.Key.ToString(),
                    text = third.Value.Title,
                    type = type,
                    hasSearch = isMarketsOrRetailers,
                    href = third.Value.FullUrl,
                    children = children.Select(c => new SecondaryNavViewModel
                    {
                        text = c.Value.Title,
                        href = c.Value.FullUrl
                    }).ToArray()
                };
                menu.Add(nav);
            }

            return JsonConvert.SerializeObject(menu);
        }

        /// <summary>
        /// Get all retailers from tags.
        /// </summary>
        /// <returns>List of retailers</returns>
        public static string GetRetailers()
        {
            // Get the retailer tab.
            KeyValuePair<int, TabInfo> retailerTab = TabController.Instance.GetTabsByPortal(0)
                .Where(t => t.Value.Title.ToLower().Contains("retailers")).FirstOrDefault();

            // Convert the tags to a List Tag.
            TagsHelper tagHelper = new TagsHelper();
            IEnumerable<Tag> tags = tagHelper.ConvertList(new DataController().Tags);

            // Get the retailer tag.
            Tag retailerTag = tags.Where(t => t.TagName.ToLower() == "retailers").FirstOrDefault();

            // Get all retailer child tags.
            List<Tag> retailerTags = tags.Where(t => t.ParentTagGUID == retailerTag.EntityGuid).ToList();

            // Populate the retailer view model from the retailer child tags.
            List<RetailerMarketViewModel> retailers = new List<RetailerMarketViewModel>();
            foreach (var tag in retailerTags)
            {
                retailers.Add(new RetailerMarketViewModel
                {
                    title = tag.TagDisplayName.Replace('"', '\u0027'),
                    hyperlink = retailerTab.Value.FullUrl + "/" + tag.TagDisplayName.Trim().Replace(' ', '-').Replace('\'', '\u0027')
                });
            }

            return JsonConvert.SerializeObject(retailers);
        }

        /// <summary>
        /// Get all markets from tags.
        /// </summary>
        /// <returns>List of markets</returns>
        public static string GetMarkets()
        {
            // Get the markets tab.
            KeyValuePair<int, TabInfo> marketTab = TabController.Instance.GetTabsByPortal(0)
                .Where(t => t.Value.Title.ToLower().Contains("markets")).FirstOrDefault();

            // Convert the tags to a List Tag.
            TagsHelper tagHelper = new TagsHelper();
            IEnumerable<Tag> tags = tagHelper.ConvertList(new DataController().Tags);

            // Get the countries tag.
            Tag marketsTag = tags.Where(t => t.TagName.ToLower() == "countries").FirstOrDefault();
            List<Tag> marketTags = tags.Where(t => t.ParentTagGUID == marketsTag.EntityGuid).ToList();

            // Populate the market view model from the markets child tags.
            List<RetailerMarketViewModel> markets = new List<RetailerMarketViewModel>();
            foreach (var tag in marketTags)
            {
                markets.Add(new RetailerMarketViewModel
                {
                    title = tag.TagDisplayName.Replace('"', '\u0027'),
                    hyperlink = marketTab.Value.FullUrl + "/" + tag.TagDisplayName.Trim().Replace(' ', '-').Replace('\'', '\u0027')
                });
            }

            return JsonConvert.SerializeObject(markets);
        }
        /// <summary>
        /// Get the primary nav menu.
        /// </summary>
        /// <returns>The primary nav menu</returns>
        public static string GetPrimaryNav()
        {
            List<TabInfo> tabs = TabController.Instance.GetTabsByPortal(0).Select(s => s.Value).ToList();
            List<TabInfo> topLevelTabs = tabs.Where(t => t.IsVisible && t.Level == 0).ToList();
            var nav = new List<PrimaryNavViewModel>();

            foreach (var tab in topLevelTabs)
            {
                var children = tabs.Where(t => t.IsVisible && t.Level == 1 && t.ParentId == tab.TabID).Take(9).ToList();
                nav.Add(new PrimaryNavViewModel
                {
                    id = tab.TabID.ToString(),
                    text = tab.Title,
                    children = children.Select(s => new PrimaryNavViewModel
                    {
                        id = tab.TabID.ToString(),
                        text = tab.Title,
                        description = s.Description
                    }).ToArray(),
                    subTitle = tab.Title.Contains("commercial") ? "We connect our industry." : tab.Title.Contains("impact") ? "Uniting and inspiring our industry." : "",
                    description = tab.Title.Contains("commercial") ? "Our products" : tab.Title.Contains("impact") ? "Our focus areas" : "",
                });
            }
            return JsonConvert.SerializeObject(nav);
        }
    }

public static class Root
    {
        public static string CommercialInsight
        {
            get
            {
                string roots = ConfigurationManager.AppSettings["AreaRoots"];
                if (string.IsNullOrEmpty(roots))
                {
                    return "commercial-insight,commercialinsight,commercial insight";
                }
                return roots.Split(':').Where(c => c.Split('=')[0] == "CommercialInsight").Select(x => x.Split('=')[1]).FirstOrDefault();
            }
        }
        public static string SocialImpact
        {
            get
            {
                string roots = ConfigurationManager.AppSettings["AreaRoots"];
                if (string.IsNullOrEmpty(roots))
                {
                    return "social-impact,socialimpact,social impact";
                }
                return roots.Split(':').Where(c => c.Split('=')[0] == "SocialImpact").Select(x => x.Split('=')[1]).FirstOrDefault();
            }
        }
        public static string Events
        {
            get
            {
                string roots = ConfigurationManager.AppSettings["AreaRoots"];
                if (string.IsNullOrEmpty(roots))
                {
                    return "events";
                }
                return roots.Split(':').Where(c => c.Split('=')[0] == "Events").Select(x => x.Split('=')[1]).FirstOrDefault();
            }
        }
        public static string AboutUs
        {
            get
            {
                string roots = ConfigurationManager.AppSettings["AreaRoots"];
                if (string.IsNullOrEmpty(roots))
                {
                    return "about-us,aboutus,about us";
                }
                return roots.Split(':').Where(c => c.Split('=')[0] == "AboutUs").Select(x => x.Split('=')[1]).FirstOrDefault();
            }
        }
        public static string Learning
        {
            get
            {
                string roots = ConfigurationManager.AppSettings["AreaRoots"];
                if (string.IsNullOrEmpty(roots))
                {
                    return "learning";
                }
                return roots.Split(':').Where(c => c.Split('=')[0] == "Learning").Select(x => x.Split('=')[1]).FirstOrDefault();
            }
        }
    }

    public enum MediaType
    {
        Default = 0,
        Soundcloud = 1,
        YouTube = 2,
        Vimeo = 3
    }

    public enum AreaType
    {
        SocialImpact = 0,
        CommercialInsight = 1,
        Neutral = 2
    }

    /// <summary>
    /// View model of the secondary nav.
    /// </summary>
    public class SecondaryNavViewModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public int type { get; set; }
        public bool hasSearch { get; set; }
        public string href { get; set; }
        public SecondaryNavViewModel[] children { get; set; }
    }

    public class PrimaryNavViewModel
    {
        public string id { get; set; }
        public string text { get; set; }
        public PrimaryNavViewModel[] children { get; set; }
        public string description { get; set; }
        public string subTitle { get; set; }
    }

    /// <summary>
    /// View model of the retailers and markets.
    /// </summary>
    public class RetailerMarketViewModel
    {
        public string title { get; set; }
        public string hyperlink { get; set; }
    }
}
