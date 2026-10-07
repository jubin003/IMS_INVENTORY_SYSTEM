using System;
using System.Web;

using Entity.Components;
using Entity.Framework;
using Service.Components;
using NCCSEncryption;
using System.Web.Caching;

namespace PhyeGanSiteMap
{
    public class SiteMapDataProvider : StaticSiteMapProvider
    {
        Encryption MyEncr = new Encryption();
        private readonly object _siteMapLock = new object();
        private SiteMapNode _siteMapRoot;

        public override SiteMapNode BuildSiteMap()
        {
            if (_siteMapRoot == null || HttpContext.Current.Session["AlreadySet"] == "NotSet")
            {

                // Use a lock to provide thread safety            
                lock (_siteMapLock)
                {
                    base.Clear();
                    CreateSiteMapRoot();
                    CreateSiteMapNodes();
                    HttpContext.Current.Session["AlreadySet"] = "Set";
                    return _siteMapRoot;
                }
            }
            else
            {
                return _siteMapRoot;
            }
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            return BuildSiteMap();
        }

        private void CreateSiteMapRoot()
        {
            _siteMapRoot = new SiteMapNode(this, "Root", "~/Default.aspx", "Root");
            AddNode(_siteMapRoot);
        }
        public void OnSiteMapChanged(string key, object item, CacheItemRemovedReason reason)
        {
            //Clear();

        }

        private PageLinkNodesEntity setRole(PageLinkNodesEntity ent)
        {
            string permission = ent.Permission;
            Boolean Read =false;
            Boolean Add =false;
            Boolean Modify =false;
            Boolean Delete =false;
            Boolean Full =false;
            Boolean VChecker = false;
            Boolean VPasser = false;
            Boolean VApprover = false;
            Read = permission.Substring(4, 1)=="1"?true:false;
            Add = permission.Substring(5, 1) == "1" ? true : false;
            Modify = permission.Substring(6, 1) == "1" ? true : false;
            Delete = permission.Substring(7, 1) == "1" ? true : false;
            Full = permission.Substring(4, 4) == "1111" ? true : false;
            VChecker = permission.Substring(0, 1) == "1" ? true : false;
            VPasser = permission.Substring(1, 1) == "1" ? true : false;
            VApprover = permission.Substring(2, 1) == "1" ? true : false;

            HelperFunction hf=new HelperFunction();
            if (Full)
                ent.Role = PageLinkNodesEntity.UserRights.Full;
            else if (Read == true && Add == true && Modify == true & Delete == false)
                ent.Role = PageLinkNodesEntity.UserRights.AddModifyView;
            else if (Read == true && Add == true && Modify == false & Delete == true)
                ent.Role = PageLinkNodesEntity.UserRights.AddViewDelete;
            else if (Read == true && Add == false && Modify == true & Delete == true)
                ent.Role = PageLinkNodesEntity.UserRights.ViewModifyDelete;
            else if (Read == false && Add == true && Modify == true & Delete == true)
                ent.Role = PageLinkNodesEntity.UserRights.AddModifyDelete;
            else if (Read == false && Add == false && Modify == true & Delete == true)
                ent.Role = PageLinkNodesEntity.UserRights.ModifyDelete;
            else if (Read == false && Add == true && Modify == false& Delete == true)
                ent.Role = PageLinkNodesEntity.UserRights.AddDelete;
            else if (Read == false && Add == true && Modify == true & Delete == false)
                ent.Role = PageLinkNodesEntity.UserRights.AddModify;
            else if (Read ==true&& Add == true&& Modify == false& Delete == false)
                ent.Role = PageLinkNodesEntity.UserRights.ViewAdd;
            else if (Read ==true&& Add == false&& Modify ==false& Delete == true)
                ent.Role = PageLinkNodesEntity.UserRights.ViewDelete;
            else if (Read == true&& Add == false && Modify == true& Delete == false)
                ent.Role = PageLinkNodesEntity.UserRights.ModifyView;
            else if (Read == false&& Add == false&& Modify == false& Delete == true)
                ent.Role = PageLinkNodesEntity.UserRights.Delete;
            else if (Read == false&& Add == false && Modify == true & Delete == false)
                ent.Role = PageLinkNodesEntity.UserRights.ModifyView;
            else if (Read == false && Add == true&& Modify == false & Delete == false)
                ent.Role = PageLinkNodesEntity.UserRights.Add;
            else if (Read == true && Add == false && Modify == false& Delete == false)
                ent.Role = PageLinkNodesEntity.UserRights.View;
            else if (Read == false&& Add == false && Modify == false & Delete == false)
                ent.Role = PageLinkNodesEntity.UserRights.NoAccess;

            if (VChecker == true && VPasser == true && VApprover == true)
                ent.AccountAccess = PageLinkNodesEntity.UserRights.VCheckerPasserApprover;
            else if (VChecker == true && VPasser == true)
                ent.AccountAccess = PageLinkNodesEntity.UserRights.VCheckerPasser;
            else if (VChecker == true)
                ent.AccountAccess = PageLinkNodesEntity.UserRights.VChecker;
            else if (VPasser == true && VApprover == true)
                ent.AccountAccess = PageLinkNodesEntity.UserRights.VPasserApprover;
            else if (VPasser == true)
                ent.AccountAccess = PageLinkNodesEntity.UserRights.VPasser;
            else if (VApprover == true)
                ent.AccountAccess = PageLinkNodesEntity.UserRights.VApprover;

            return ent;

        }
        private void CreateSiteMapNodes()
        {
            UserProfileEntity userProfileEnt = new UserProfileEntity();
            userProfileEnt = (UserProfileEntity)HttpContext.Current.Session["UserProfile"];
            string CurrentModule = HttpContext.Current.Session["UserModule"].ToString();
            ModuleSubModuleService srv = new ModuleSubModuleService();
            EntityList entList = new EntityList();
            entList = srv.GetValideModules(userProfileEnt, CurrentModule);
            SiteMapNode node = null;
            int i = 0;
            foreach (ModuleSubmoduleEntity ent in entList)
            {
               
                //if (ent.Moduleid.ToString() == CurrentModule)
                //{
                    node = new SiteMapNode(this, string.Format(ent.Name, i), string.Format("", i), string.Format(ent.Name, i));
                    EntityList entListNodes = new EntityList();
                    //entList = srv.GetValideModules(userProfileEnt, CurrentModule);

                    entListNodes = new EntityList();
                    entListNodes = srv.GetLinkNodes(ent, userProfileEnt.UserGroupID.ToString());
                    SiteMapNode nodeLink = null;
                    int j = 0;
                    foreach (PageLinkNodesEntity entNode in entListNodes)
                    {
                        PageLinkNodesEntity entPageLInk=setRole(entNode);
                        if (entNode.Permission != "00000000")
                        {
                            string Pageurl = entNode.LinkUrl + "?";
                            string QueryParamters = "";
                            QueryParamters = "Access=267b&";
                            QueryParamters = QueryParamters + "Role=" + entPageLInk.Role;
                            QueryParamters = QueryParamters + "&Account=" + entPageLInk.AccountAccess;
                            QueryParamters = MyEncr.Encrypt(QueryParamters, "!#$a54?3");
                            Pageurl = Pageurl + QueryParamters;
                            nodeLink = new SiteMapNode(this, string.Format(entNode.Pagename, j), string.Format(Pageurl, j), string.Format(entNode.Pagename, j));
                            try
                            {
                                AddNode(nodeLink, node);
                            }
                            catch { }

                            nodeLink = null;
                        }
                        j = j+1;
                    }
                    try
                    {
                        AddNode(node, _siteMapRoot);
                    }
                    catch {  }
                    i = i+1;
                //}
            }
        }
    }
}