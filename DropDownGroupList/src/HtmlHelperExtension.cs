using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DropDownGroupList
{
    public static class HtmlHelperExtension
    {
        public static MvcHtmlString DropDownGroupList(this HtmlHelper htmlHelper, string name)
        {
            return DropDownListHelper(htmlHelper, name, null, null, null);
        }

        public static MvcHtmlString DropDownGroupList(this HtmlHelper htmlHelper, string name, IEnumerable<GroupedSelectListItem> selectList)
        {
            return DropDownListHelper(htmlHelper, name, selectList, null, null);
        }

        public static MvcHtmlString DropDownGroupList(this HtmlHelper htmlHelper, string name, string optionLabel)
        {
            return DropDownListHelper(htmlHelper, name, null, optionLabel, null);
        }

        public static MvcHtmlString DropDownGroupList(this HtmlHelper htmlHelper, string name, IEnumerable<GroupedSelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return DropDownListHelper(htmlHelper, name, selectList, null, htmlAttributes);
        }

        public static MvcHtmlString DropDownGroupList(this HtmlHelper htmlHelper, string name, IEnumerable<GroupedSelectListItem> selectList, object htmlAttributes)
        {
            return DropDownListHelper(htmlHelper, name, selectList, null, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString DropDownGroupList(this HtmlHelper htmlHelper, string name, IEnumerable<GroupedSelectListItem> selectList, string optionLabel)
        {
            return DropDownListHelper(htmlHelper, name, selectList, optionLabel, null);
        }

        public static MvcHtmlString DropDownGroupList(this HtmlHelper htmlHelper, string name, IEnumerable<GroupedSelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            return DropDownListHelper(htmlHelper, name, selectList, optionLabel, htmlAttributes);
        }

        public static MvcHtmlString DropDownGroupList(this HtmlHelper htmlHelper, string name, IEnumerable<GroupedSelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            return DropDownListHelper(htmlHelper, name, selectList, optionLabel, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString DropDownGroupListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<GroupedSelectListItem> selectList)
        {
            return htmlHelper.DropDownGroupListFor(expression, selectList, null, null);
        }

        public static MvcHtmlString DropDownGroupListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<GroupedSelectListItem> selectList, object htmlAttributes)
        {
            return htmlHelper.DropDownGroupListFor(expression, selectList, null, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString DropDownGroupListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<GroupedSelectListItem> selectList, IDictionary<string, object> htmlAttributes)
        {
            return htmlHelper.DropDownGroupListFor(expression, selectList, null, htmlAttributes);
        }

        public static MvcHtmlString DropDownGroupListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<GroupedSelectListItem> selectList, string optionLabel)
        {
            return htmlHelper.DropDownGroupListFor(expression, selectList, optionLabel, null);
        }

        public static MvcHtmlString DropDownGroupListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<GroupedSelectListItem> selectList, string optionLabel, object htmlAttributes)
        {
            return htmlHelper.DropDownGroupListFor(expression, selectList, optionLabel, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString DropDownGroupListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<GroupedSelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
                throw new ArgumentNullException(nameof(expression));
            if (htmlAttributes != null)
            {
                foreach (var keyValuePair in htmlAttributes.ToList())
                    htmlAttributes.ChangeKey(keyValuePair.Key, keyValuePair.Key.Replace('_', '-'));
            }
            return DropDownListHelper(htmlHelper, ExpressionHelper.GetExpressionText(expression), selectList, optionLabel, htmlAttributes, typeof(TProperty).IsEnumerable());
        }

        public static bool ChangeKey<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey oldKey, TKey newKey)
        {
            TValue obj;
            if (!dict.TryGetValue(oldKey, out obj))
                return false;
            dict.Remove(oldKey);
            dict[newKey] = obj;
            return true;
        }

        private static MvcHtmlString DropDownListHelper(HtmlHelper htmlHelper, string expression, IEnumerable<GroupedSelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes, bool isMultiple = false)
        {
            return htmlHelper.SelectInternal(optionLabel, expression, selectList, isMultiple, htmlAttributes);
        }

        private static IEnumerable<GroupedSelectListItem> GetSelectData(this HtmlHelper htmlHelper, string name)
        {
            object obj = null;
            if (htmlHelper.ViewData != null)
                obj = htmlHelper.ViewData.Eval(name);
            if (obj == null)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Missing Select Data"));
            var enumerable = obj as IEnumerable<GroupedSelectListItem>;
            if (enumerable == null)
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Wrong Select DataType"));
            return enumerable;
        }

        internal static string ListItemToOption(GroupedSelectListItem item)
        {
            var tagBuilder = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode(item.Text)
            };
            if (item.Value != null)
                tagBuilder.Attributes["value"] = item.Value;
            if (item.Selected)
                tagBuilder.Attributes["selected"] = "selected";
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

        private static MvcHtmlString SelectInternal(this HtmlHelper htmlHelper, string optionLabel, string name, IEnumerable<GroupedSelectListItem> selectList, bool allowMultiple, IDictionary<string, object> htmlAttributes)
        {
            name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Null Or Empty", nameof(name));
            var flag1 = false;
            if (selectList == null)
            {
                selectList = htmlHelper.GetSelectData(name);
                flag1 = true;
            }
            var obj = allowMultiple ? htmlHelper.GetModelStateValue(name, typeof(string[])) : htmlHelper.GetModelStateValue(name, typeof(string));
            if (!flag1 && obj == null)
                obj = htmlHelper.ViewData.Eval(name);
            if (obj != null)
            {
                IEnumerable source;
                if (!allowMultiple)
                    source = new object[1]
                    {
                        obj
                    };
                else
                    source = obj as IEnumerable;
                var hashSet = new HashSet<string>(source.Cast<object>().Select(value => Convert.ToString(value, CultureInfo.CurrentCulture)), StringComparer.OrdinalIgnoreCase);
                var list = new List<GroupedSelectListItem>();
                foreach (var groupedSelectListItem in selectList)
                {
                    groupedSelectListItem.Selected = groupedSelectListItem.Value != null ? hashSet.Contains(groupedSelectListItem.Value) : hashSet.Contains(groupedSelectListItem.Text);
                    list.Add(groupedSelectListItem);
                }
                selectList = list;
            }
            var stringBuilder1 = new StringBuilder();
            if (optionLabel != null)
            {
                var stringBuilder2 = stringBuilder1;
                var groupedSelectListItem = new GroupedSelectListItem
                {
                    Text = optionLabel,
                    Value = string.Empty,
                    Selected = false
                };
                var str = ListItemToOption(groupedSelectListItem);
                stringBuilder2.AppendLine(str);
            }
            using (var enumerator = selectList.GroupBy(i => i.GroupKey).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var group = enumerator.Current;
                    var str = selectList.Where(i => i.GroupKey == @group.Key).Select(it => it.GroupName).FirstOrDefault();
                    var flag2 = selectList.Where(i => i.GroupKey == @group.Key).Select(it => it.Disabled).FirstOrDefault();
                    stringBuilder1.AppendLine(string.Format(@"<optgroup label=""{0}"" value=""{1}"" {2}>", str, @group.Key, flag2 ? "disabled" : ""));
                    foreach (var groupedSelectListItem in @group)
                        stringBuilder1.AppendLine(ListItemToOption(groupedSelectListItem));
                    stringBuilder1.AppendLine("</optgroup>");
                }
            }
            var tagBuilder = new TagBuilder("select")
            {
                InnerHtml = stringBuilder1.ToString()
            };
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("name", name, true);
            tagBuilder.GenerateId(name);
            if (allowMultiple)
                tagBuilder.MergeAttribute("multiple", "multiple");
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(name, out modelState) && modelState.Errors.Count > 0)
                tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        internal static object GetModelStateValue(this HtmlHelper helper, string key, Type destinationType)
        {
            ModelState modelState;
            if (helper.ViewData.ModelState.TryGetValue(key, out modelState) && modelState.Value != null)
                return modelState.Value.ConvertTo(destinationType, null);
            return null;
        }
    }
}