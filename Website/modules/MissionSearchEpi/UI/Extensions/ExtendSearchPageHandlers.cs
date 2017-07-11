using MissionSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using MissionSearchEpi.UI.Blocks;
using System.Text.RegularExpressions;
using MissionSearch.Util;

namespace MissionSearchEpi.Extensions
{
    public static partial class ExtendSearchPage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateFacetBlock"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private static DateRangeFacet ProcessDateFacetBlock(DateRangeFacetBlock dateFacetBlock, int order)
        {
            var dateFacet = new DateRangeFacet(dateFacetBlock.FieldName, dateFacetBlock.Label)
            {
                Order = order,
            };
            
            dateFacet.Ranges = new List<DateRange>();

            var now = DateTime.Today;

            var startYear = now.Year;

            var gapStrs = new List<string>();

            if (!string.IsNullOrEmpty(dateFacetBlock.RangeGap))
            {
                var sep = dateFacetBlock.RangeGap.Contains((',')) ? ',' : ' ';
                gapStrs = dateFacetBlock.RangeGap.Split(sep).ToList();
            }

            foreach (var gap in gapStrs)
            {
                var match = Regex.Match(gap, @"(\d*)(\w)");

                if (!match.Success)
                    continue;

                var range = TypeParser.ParseInt(match.Groups[1].ToString());
                var dateType = match.Groups[2].ToString();

                DateTime start, start2;
                DateTime end, end2;
                string label, label2;

                switch (dateType)
                {
                    case "D":
                        start = now.AddDays(-range);
                        end = now.AddDays((1));
                        label = string.Format("Last {0} days", range);

                        start2 = now.AddDays(1);
                        end2 = start2.AddDays((range));
                        label2 = string.Format("Next {0} days", range);
                        break;

                    case "M":
                        start = new DateTime(now.Year, now.Month, 1).AddMonths(-range);
                        end = now;
                        label = string.Format("Last {0} Months", range);

                        start2 = now.AddDays(1);
                        end2 = new DateTime(now.Year, now.Month, 1).AddMonths(range);
                        label2 = string.Format("Next {0} Months", range);
                        break;
                    
                    default:
                        start = new DateTime(startYear, 1, 1).AddYears(-(range - 1));
                        end = new DateTime(startYear, 12, 31);
                        label = string.Format("Last {0} Years", range);

                        end2 = new DateTime(startYear, 1, 1).AddYears((range - 1));
                        start2 = new DateTime(startYear, 12, 31);
                        label2 = string.Format("Next {0} Years", range);

                        startYear = startYear - 1;
                        break;
                     
                }

                dateFacet.Ranges.Add(new DateRange()
                {
                    Lower = start.Date,
                    Upper = end.Date,
                    Label = label,
                });

                dateFacet.Ranges.Add(new DateRange()
                {
                    Lower = start2.Date,
                    Upper = end2.Date,
                    Label = label2,
                });
            }

            // start generating years from startyear plus max range and count backwards
            startYear = startYear + dateFacetBlock.MaxRange;

            var rangeEnd = dateFacetBlock.MaxRange * 2 - 1;
            for (var i = 0; i < rangeEnd; i++)
            {
                dateFacet.Ranges.Add(new DateRange()
                {
                    Lower = new DateTime(startYear, 1, 1),
                    Upper = new DateTime(startYear, 12, 31),
                    Label = string.Format("{0}", startYear),
                });

                startYear = startYear - 1;
            }

            if (dateFacetBlock.MaxRange > 0)
            {
                dateFacet.Ranges.Add(new DateRange()
                {
                    Lower = null,
                    Upper = new DateTime(startYear, 12, 31),
                    //Label = string.Format("{0} or before", startYear),
                });
            }

            return dateFacet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rangeFacetBlock"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private static NumRangeFacet ProcessRangeFacetBlock(RangeFacetBlock rangeFacetBlock, int order)
        {

            var gapStrs = rangeFacetBlock.RangeGap.Split(',');

            var lastGap = rangeFacetBlock.Start;

            var rangeFacet = new NumRangeFacet(rangeFacetBlock.FieldName, rangeFacetBlock.Label)
            {
                NumericFormat = rangeFacetBlock.NumericFormat,
                Order = order,
            };
            
            rangeFacet.Range = new List<NumRange>();

            foreach (var gap in gapStrs)
            {
                var endGap = TypeParser.ParseDouble(gap) - 1;

                rangeFacet.Range.Add(new NumRange()
                {
                    Lower = lastGap,
                    Upper = endGap,
                });

                lastGap = endGap + 1;
            }

            rangeFacet.Range.Add(new NumRange()
            {
                Lower = lastGap,
                Upper = null,

            });

            return rangeFacet;
        }
        
    }
}