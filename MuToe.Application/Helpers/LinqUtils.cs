﻿

using MuTote.Domain.Commons;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace MuTote.Application.Utilities
{
    public static class LinqUtils
    {
        public static IQueryable<TEntity> DynamicFilter<TEntity>(this IQueryable<TEntity> source, TEntity entity)
        {
            foreach (PropertyInfo property in entity.GetType().GetProperties())
            {
                if (!(entity.GetType().GetProperty(property.Name) == null))
                {
                    object data = entity.GetType().GetProperty(property.Name)
                        ?.GetValue((object)entity, (object[])null);
                    if (data != null &&
                        !property.CustomAttributes.Any(
                            (Func<CustomAttributeData, bool>)(a => a.AttributeType == typeof(SkipAttribute))))
                    {
                        if (property.CustomAttributes.Any(
                                (Func<CustomAttributeData, bool>)(a => a.AttributeType == typeof(StringAttribute))))
                        {
                            source = source.Where<TEntity>(property.Name + ".ToLower().Contains(@0)",
                                (object)data.ToString().ToLower());
                        }
                        else if (property.CustomAttributes.Any(
                                (Func<CustomAttributeData, bool>)(a => a.AttributeType == typeof(IntAttribute))))
                        {
                            source = source.Where<TEntity>(property.Name + " == @0",
                                (object)data);
                        }
                        else if (property.CustomAttributes.Any(a => a.AttributeType == typeof(BooleanAttribute)))
                        {
                            source = source.Where<TEntity>(property.Name + "== @0", (object)data);
                        }
                        else if (property.CustomAttributes.Any(
                                     (Func<CustomAttributeData, bool>)(a =>
                                        a.AttributeType == typeof(ChildAttribute))))
                        {
                            foreach (PropertyInfo propertyChild in property.PropertyType.GetProperties())
                            {
                                object dataChild = data.GetType().GetProperty(propertyChild.Name)
                                    ?.GetValue(data, (object[])null);
                                if (dataChild != null)
                                {
                                    source = source.Where<TEntity>(string.Format("{0}.{1}=\"{2}\"", property.Name,
                                        propertyChild.Name, dataChild));
                                }
                            }
                        }
                        else if (property.CustomAttributes.Any(
                                     (Func<CustomAttributeData, bool>)(a =>
                                        a.AttributeType == typeof(DateRangeAttribute))))
                        {
                            // string operate1 = data.ToString().Substring(1);
                            // string operare2 = data.ToString().Substring(data.ToString().Length);
                            
                            IQueryable<TEntity> source2 = source;
                            string predicate = null;
                            if (property.Name.Equals("StartDate"))
                            {
                                predicate = property.Name + " <= @0  &&  EndDate >= @0" ;
                                DateTime date = DateTime.Today;
                                object dateRange = (object)date.Date;
                                date = date.Date;
                                source = source2.Where<TEntity>(predicate, dateRange);
                            }
                            else
                            {
                                DateTime date = (DateTime)data;
                                 predicate = property.Name + " >= @0 && " + property.Name + " =< @1";
                                object[] dateRange = new object[2]
                                {
                                (object) date.Date,
                                null
                                };
                                date = date.Date;
                                dateRange[1] = date.AddDays(1.0);
                                source = source2.Where<TEntity>(predicate, dateRange);
                            }
                           
                        }
                        else if (property.CustomAttributes.Any(
                                     (Func<CustomAttributeData, bool>)(a => a.AttributeType == typeof(SortAttribute))))
                        {
                            string[] sort = data.ToString().Split(", ");
                            if (sort.Length == 2)
                            {
                                if (sort[1].Equals("asc"))
                                {
                                    source = source.OrderBy(sort[0]);
                                }

                                if (sort[1].Equals("desc"))
                                {
                                    source = source.OrderBy(sort[0] + " descending");
                                }
                            }
                            else
                            {
                                source = source.OrderBy(sort[0]);
                            }
                        }
                    }
                }
            }

            return source;
        }
    }
}
