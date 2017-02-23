using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.Blobs;
using EPiServer.Security;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using EPiServer.Web;
using MissionSearch.Util;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace MissionSearchEpi.Util
{
    public class EpiHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte[] ReadEpiBlob(Blob binaryData)
        {
            using (var cs = binaryData.OpenRead())
            {
                using (var stream = new MemoryStream())
                {
                    byte[] buffer = new byte[2048];

                    int bytesRead;
                    while ((bytesRead = cs.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        stream.Write(buffer, 0, bytesRead);
                    }
                    byte[] result = stream.ToArray();

                    return result;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentLink"></param>
        /// <returns></returns>
        public static string GetPageTreePath(PageReference parentLink)
        {
            var repository = ServiceLocator.Current.GetInstance<IContentRepository>();

            return CreatePath(repository, parentLink);
        }

        public static string GetPageTreeIdPath(PageReference parentLink)
        {
            var repository = ServiceLocator.Current.GetInstance<IContentRepository>();

            return CreateIdPath(repository, parentLink);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentLink"></param>
        /// <returns></returns>
        public static string GetParentName(PageReference parentLink)
        {
            var repository = ServiceLocator.Current.GetInstance<IContentRepository>();

            var parent = repository.Get<PageData>(parentLink);

            return parent != null ? parent.Name : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentLink"></param>
        /// <returns></returns>
        public static string GetParentFolderName(PageReference parentLink)
        {
            var repository = ServiceLocator.Current.GetInstance<IContentRepository>();

            var parent = repository.Get<ContentFolder>(parentLink);

            return parent != null ? parent.Name : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentLink"></param>
        /// <returns></returns>
        public static string GetFolderPath(PageReference parentLink)
        {
            var repository = ServiceLocator.Current.GetInstance<IContentRepository>();

            return CreateFolderPath(repository, parentLink);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentLink"></param>
        /// <returns></returns>
        public static List<string> GetPageTreePaths(PageReference parentLink)
        {
            var paths = new List<string>();

            var repository = ServiceLocator.Current.GetInstance<IContentRepository>();

            var rootpath = CreatePath(repository, parentLink);

            var parts = rootpath.Split('/');
            var lastPath = "";

            foreach (var part in parts)
            {
                if (part == "")
                    continue;

                var path = string.Format("{0}/{1}", lastPath, part);

                if (!paths.Contains(path))
                    paths.Add(path);

                lastPath = path;
            }

            return paths;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="parentLink"></param>
        /// <returns></returns>
        private static string CreatePath(IContentRepository repository, PageReference parentLink)
        {
            try
            {
                string path = "";

                if (parentLink.ID == 1)
                    return "/Root";

                var parent = repository.Get<PageData>(parentLink);

                if (parent == null)
                    return "";

                var parentLink2 = parent.ParentLink;

                if (parentLink2 != null)
                    path += CreatePath(repository, parentLink2);

                //path += string.Format("/{0}", parent.Name.Replace(" ", ""));
                path += string.Format("/{0}", parent.Name);
                return path;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="parentLink"></param>
        /// <returns></returns>
        private static string CreateIdPath(IContentRepository repository, PageReference parentLink)
        {
            try
            {
                string path = "";

                var parent = repository.Get<PageData>(parentLink);

                if (parent == null)
                    return "";

                var parentLink2 = parent.ParentLink;

                if (parentLink2 == null)
                    return path;
                
                path += CreateIdPath(repository, parentLink2);
                path += string.Format("/{0}", parentLink2.ID);

                return path;
            }
            catch
            {
                return "";
            }
        }

        private static string CreateFolderPath(IContentRepository repository, PageReference parentLink)
        {
            try
            {
                string path = "";

                if (parentLink.ID == 1)
                    return "/Root";

                var parent = repository.Get<ContentFolder>(parentLink);

                if (parent == null)
                    return "";

                var parentLink2 = parent.ParentLink;

                if (parentLink2 != null)
                    path += CreateFolderPath(repository, parentLink2.ToPageReference());
                
                path += string.Format("/{0}", parent.Name);
                return path;
            }
            catch
            {
                return "";
            }
        }


        /// <summary>
        /// returns a list of category names from a category list
        /// </summary>
        /// <param name="categoryList"></param>
        /// <returns></returns>
        public static List<string> GetCategoryNames(CategoryList categoryList)
        {
            var categories = new List<string>();

            var categoryRepository = ServiceLocator.Current.GetInstance<CategoryRepository>();

            var rootCategory = categoryRepository.GetRoot();

            if (categoryList.Any())
            {
                foreach (var categoryId in categoryList)
                {
                    var category = FindCategory(rootCategory, categoryId);

                    if (category != null)
                    {
                        categories.Add(category.Name);
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Category> GetRootCategories()
        {
            var categories = new List<Category>();

            var categoryRepository = ServiceLocator.Current.GetInstance<CategoryRepository>();

            var rootCategory = categoryRepository.GetRoot();
            
            foreach (var category in rootCategory.Categories)
            {
                    categories.Add(category);
            }

            return categories;
        }

        /// <summary>
        ///        
        /// </summary>
        /// <param name="categoryList"></param>
        /// <returns></returns>
        public static List<string> GetCategoryPaths(CategoryList categoryList)
        {
            var categories = new List<string>();

            var categoryRepository = ServiceLocator.Current.GetInstance<CategoryRepository>();

            var rootCategory = categoryRepository.GetRoot();

            if (categoryList.Any())
            {
                foreach (var categoryId in categoryList)
                {
                    var categoryPaths = CreateCategoryPath(rootCategory, categoryId);

                    //if(categoryPaths.Any())
                    //    categories.AddRange(categoryPaths);
                    
                    var path = string.Empty;

                    foreach(var category in categoryPaths)
                    {
                        if(path == string.Empty)
                        {
                            path = category;
                        }
                        else
                        {
                            path = string.Format("{0}/{1}", path, category);
                        }

                        categories.Add(path);
                    }
                }
            }

            return categories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static List<string> CreateCategoryPath(Category root, int categoryId)
        {
            var paths = new List<string>();

            var category = FindCategory(root, categoryId);

            if (category == null)
                return paths;

            paths.Add(category.Name);

            if (category.Parent != null)
            {
                var parentPaths = CreateCategoryPath(root, category.Parent.ID);
                
                if (parentPaths.Any())
                    paths.InsertRange(0, parentPaths);
            }

            return paths;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryList"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static bool HasCategory(CategoryList categoryList, long categoryId)
        {
          
            if (categoryList.Any())
            {
                foreach (var catId in categoryList)
                {
                    if (catId == categoryId)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// return categories from category list matching category root id
        /// </summary>
        /// <param name="categoryList"></param>
        /// <param name="rootCategoryId"></param>
        /// <returns></returns>
        public static List<string> GetCategories(CategoryList categoryList, int rootCategoryId)
        {
            var categories = new List<string>();

            var categoryRepository = ServiceLocator.Current.GetInstance<CategoryRepository>();

            var rootCategory = categoryRepository.GetRoot();

            if (categoryList.Any())
            {
                foreach (var categoryId in categoryList)
                {
                    var category = FindCategory(rootCategory, categoryId);

                    if (category != null)
                    {
                        var parent = category.Parent;

                        do
                        {
                            if (parent.ID == rootCategoryId)
                            {
                                categories.Add(category.Name);
                                break;
                            }

                            parent = parent.Parent;

                        } while (parent != null);
                    }
                }
            }

            return categories;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private static Category FindCategory(Category root, int categoryId)
        {
            var category = root.Categories.FirstOrDefault(c => c.ID == categoryId);

            if (category != null)
                return category;

            foreach (var subroot in root.Categories)
            {
                var c = FindCategory(subroot, categoryId);

                if (c != null)
                {
                    return c;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetApplicationConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetApplicationConfigIntValue(string key)
        {
            return TypeParser.ParseInt(ConfigurationManager.AppSettings[key]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentLink"></param>
        /// <returns></returns>
        public static string GetSitePath(ContentReference contentLink)
        {

            var resolver = ServiceLocator.Current.GetInstance<SiteDefinitionResolver>();
            var site = resolver.GetDefinitionForContent(contentLink, false, false);

            return (site != null) ? site.SiteUrl.Host : string.Empty;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetExternalURL(PageData p)
        {
            var pageUrlBuilder = new UrlBuilder(p.LinkURL);

            Global.UrlRewriteProvider.ConvertToExternal(pageUrlBuilder, p.PageLink, Encoding.UTF8);

            return pageUrlBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsRestrictedContent(PageData p)
        {
            return p.ACL.Any(r => r.Key == "Everyone" && r.Value.Access == AccessLevel.NoAccess);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentRef"></param>
        /// <returns></returns>
        public static PageData GetPage(ContentReference contentRef)
        {
            var repository = ServiceLocator.Current.GetInstance<IContentRepository>();

            return repository.Get<PageData>(contentRef);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentRef"></param>
        /// <returns></returns>
        public static BlockData GetBlock(ContentReference contentRef)
        {
            var repository = ServiceLocator.Current.GetInstance<IContentRepository>();

            return repository.Get<BlockData>(contentRef);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<T> GetContentAreaContent<T>(IList<ContentAreaItem> items) where T : IContentData
        {
            var list = new List<T>();

            var repository = ServiceLocator.Current.GetInstance<IContentRepository>();

            foreach (var item in items)
            {
                var contentItem = repository.Get<IContentData>(item.ContentLink);

                if (contentItem is T)
                {
                    list.Add((T)contentItem);
                }
            }


            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contentArea"></param>
        /// <returns></returns>
        public static List<T> GetContentAreaContent<T>(ContentArea contentArea) where T : IContentData
        {
            var list = new List<T>();

            if (contentArea == null)
                return list;

            return GetContentAreaContent<T>(contentArea.Items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appConfigKey"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        public static List<string> GetCategories(string appConfigKey, CategoryList categories)
        {
            var id = GetApplicationConfigIntValue(appConfigKey);

            if (id == 0)
                return new List<string>();

            return GetCategories(categories, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="pageLink"></param>
        /// <returns></returns>
        public static string GetFriendlyURL(string URL, PageReference pageLink)
        {
            var url = new UrlBuilder(URL);

            Global.UrlRewriteProvider.ConvertToExternal(url, pageLink, Encoding.UTF8);

            return url.Uri.AbsoluteUri;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<ContentUsage> GetUsage<T>() where T : IContent
        {
            var contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();

            var contentModelUsage = ServiceLocator.Current.GetInstance<IContentModelUsage>();

            var contentType = contentTypeRepository.Load<T>();

            var usages = contentModelUsage.ListContentOfContentType(contentType);

            if (usages == null)
                return new List<ContentUsage>();

            return usages.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetShellPath()
        {
            var shellPath = Paths.ToShellClientResource("");

            return shellPath;
        }
    }
}