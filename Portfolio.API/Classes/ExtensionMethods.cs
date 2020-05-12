using Microsoft.SharePoint.Client;
using Portfolio.Shared.DataModels;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Portfolio.API.Classes
{
    public static class ExtensionMethods
    {
        public static int GetLookupFieldValue(this FieldLookupValue lookupField)
        {
            return lookupField != null ? lookupField.LookupId : -1;
        }

        public static FlexComponentReference GetFlexComponentReference(this FieldLookupValue lookupField)
        {
            if (lookupField == null)
            {
                return null;
            }
                
            else
            {
                return new FlexComponentReference
                {
                    FlexId = lookupField.LookupId,
                    FlexName = lookupField.LookupValue
                };
            }
        }

        public static async Task<string> GetImageAsBase64String(this ClientContext ctx, string serverRelativeUrl)
        {
            var file = ctx.Web.GetFileByServerRelativeUrl(serverRelativeUrl);
            var data = file.OpenBinaryStream();

            ctx.Load(file);
            await ctx.ExecuteQueryAsync();

            using MemoryStream mStream = new MemoryStream();

            if (data != null)
            {
                data.Value.CopyTo(mStream);
                byte[] imageArray = mStream.ToArray();
                return string.Format($"data:image;base64,{Convert.ToBase64String(imageArray)}");
            }
            else
            {
                return string.Empty;
            }
        }

        public static async Task<ListItem> GetHeroByIdAsync(this ClientContext ctx, int id)
        {
            var heroList = ctx.Web.Lists.GetByTitle("HeroComponents");
            var heroItems = ctx.LoadQuery(heroList.GetItems(CamlQuery.CreateAllItemsQuery())
                                                  .QueryByHeroId(id));
            await ctx.ExecuteQueryAsync();

            return heroItems.FirstOrDefault();
        }

        public static string GetHtmlContent(this string originalHtml)
        {
            if (string.IsNullOrEmpty(originalHtml))
                return null;

            var strippedHtml = Regex.Replace(originalHtml, "<.*?>", string.Empty);

            // If HTML content is empty the length is still 2
            var isEmptyContent = strippedHtml.Length <= 2;

            return !isEmptyContent ? originalHtml : null;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string StripAndTruncateHtml(this string originalHtml, int truncate)
        {
            if (string.IsNullOrEmpty(originalHtml))
                return null;

            var strippedHtml = Regex.Replace(originalHtml, "<.*?>", string.Empty);
            var polishedHtml = strippedHtml.Truncate(truncate).TrimEnd();

            return polishedHtml;
        }
    }
}
