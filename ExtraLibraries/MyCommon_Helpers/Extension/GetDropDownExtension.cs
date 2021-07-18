using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyCommon.Helpers.Extension
{
    internal static class GetDropDownExtension
    {
        internal static SelectList ByNumber(string title, int start, int end)
        {
            var selectlist = new List<SelectListItem>();

            for (var i = start; i <= end; i++)
            {
                selectlist.Add(new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }

            selectlist.Insert(0, new SelectListItem
            {
                Text = title,
                Value = ""
            });

            return new SelectList(selectlist, "Value", "Text");
        }

        internal static SelectList ByBoolean(string onTrue, string onFalse, string title = "انتخاب کنید")
        {
            var selectlist = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = onTrue,
                    Value = true.ToString(),
                },
                new SelectListItem()
                {
                    Text = onFalse,
                    Value = false.ToString(),
                }
            };

            selectlist.Insert(0, new SelectListItem
            {
                Text = title,
                Value = ""
            });

            return new SelectList(selectlist, "Value", "Text");
        }


        internal static SelectList ByDictionary(IDictionary<string, string> items, string title = "انتخاب کنید")
        {
            var selectlist = items.Select(x => new SelectListItem()
            {
                Text = x.Value,
                Value = x.Key
            }).ToList();

            selectlist.Insert(0, new SelectListItem
            {
                Text = title,
                Value = ""
            });

            return new SelectList(selectlist, "Value", "Text");
        }

        internal static SelectList ByDictionary(IDictionary<bool, string> items, string title = "انتخاب کنید")
        {
            var selectlist = items.Select(x => new SelectListItem()
            {
                Text = x.Value,
                Value = x.Key.ToString()
            }).ToList();

            selectlist.Insert(0, new SelectListItem
            {
                Text = title,
                Value = ""
            });

            return new SelectList(selectlist, "Value", "Text");
        }

        internal static SelectList ByDictionary(IDictionary<int, string> items, string title = "انتخاب کنید")
        {
            var selectlist = items.Select(x => new SelectListItem()
            {
                Text = x.Value,
                Value = x.Key.ToString()
            }).ToList();

            selectlist.Insert(0, new SelectListItem
            {
                Text = title,
                Value = ""
            });

            return new SelectList(selectlist, "Value", "Text");
        }

        internal static SelectList ByDictionary(IDictionary<long, string> items, string title = "انتخاب کنید")
        {
            var selectlist = items.Select(x => new SelectListItem()
            {
                Text = x.Value,
                Value = x.Key.ToString()
            }).ToList();

            selectlist.Insert(0, new SelectListItem
            {
                Text = title,
                Value = ""
            });

            return new SelectList(selectlist, "Value", "Text");
        }

        internal static SelectList ByList(IList<string> items, string title = "انتخاب کنید")
        {
            var selectlist = items.Select(x => new SelectListItem()
            {
                Text = x,
                Value = x
            }).ToList();

            selectlist.Insert(0, new SelectListItem
            {
                Text = title,
                Value = ""
            });

            return new SelectList(selectlist, "Value", "Text");
        }

    }
}