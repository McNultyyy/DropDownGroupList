using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DropDownGroupList
{
    public static class EnumerableExtension
    {
        public static IEnumerable<GroupedSelectListItem> ToGroupedSelectListItems<T>(
            this IEnumerable<T> list,
            Expression<Func<T, object>> groupKeyExpression,
            Expression<Func<T, object>> groupNameExpression,
            Expression<Func<T, object>> optionValueExpr,
            Expression<Func<T, object>> optionTextExpr)
        {
            return from item in list
                   let groupKey = groupKeyExpression.Compile().Invoke(item)
                   let groupName = groupNameExpression.Compile().Invoke(item)
                   let optionValue = optionValueExpr.Compile().Invoke(item)
                   let optionText = optionTextExpr.Compile().Invoke(item)
                   select new GroupedSelectListItem()
                   {
                       GroupKey = groupKey.ToString(),
                       GroupName = groupName.ToString(),
                       Text = optionText.ToString(),
                       Value = optionValue.ToString()
                   };
        }

        public static IEnumerable<GroupedSelectListItem> ToGroupedSelectListItems<T>(
            this IEnumerable<T> list,
            Expression<Func<T, object>> groupExpression,
            Expression<Func<T, object>> optionExpr)
        {
            return ToGroupedSelectListItems(list, groupExpression, groupExpression, optionExpr, optionExpr);
        }

        public static IEnumerable<GroupedSelectListItem> ToGroupedSelectListItems<T>(
            this IGrouping<object, IEnumerable<object>> grouping)
        {
            return from @group in grouping
                   from item in @group
                   select new GroupedSelectListItem()
                   {
                       GroupName = @group.ToString(),
                       GroupKey = @group.ToString(),
                       Text = item.ToString(),
                       Value = item.ToString()
                   };
        }
    }
}