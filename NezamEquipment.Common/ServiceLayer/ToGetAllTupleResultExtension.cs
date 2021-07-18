using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using NezamEquipment.Common.Extension;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFSecondLevelCache;

namespace NezamEquipment.Common.ServiceLayer
{
    public static class ToGetAllTupleResultExtension
    {
        public static async Task<GetAllTupleResult<TResult>> ToGetAllTupleResult<TSource,TResult>(this IQueryable<TSource> e, GetAllTupleDto dto, IMapper mapper = null, object extraData = null) 
            where TResult : class
        {
            var result = new GetAllTupleResult<TResult>
            {
                Count = 0,
                ExtraData = extraData,
            };

            if (dto == null)
            {
                result.Count = await e.Cacheable().CountAsync();
            }
            else if (dto.DoCount)
            {
                result.Count = await e.Cacheable().CountAsync();
            }


            if (dto != null)
            {
                if (!dto.IsSortComplex && dto.ToSort != null)
                {
                    switch (dto.ToSort.Role)
                    {
                        case GetAllTupleDto.SortRole.Ascending:
                            e = e.OrderBy(dto.ToSort.PropertyName).AsQueryable();
                            break;
                        case GetAllTupleDto.SortRole.Descending:
                            e = e.OrderByDescending(dto.ToSort.PropertyName).AsQueryable();
                            break;
                    }
                }
                else if (dto.IsSortComplex && dto.ToSorts != null && dto.ToSorts.Any())
                {
                    var first = dto.ToSorts.First();
                    switch (first.Role)
                    {
                        case GetAllTupleDto.SortRole.Ascending:
                            e = e.OrderBy(first.PropertyName).AsQueryable();
                            break;
                        case GetAllTupleDto.SortRole.Descending:
                            e = e.OrderByDescending(first.PropertyName).AsQueryable();
                            break;
                    }
                    foreach (var sort in dto.ToSorts.Skip(1).ToList())
                    {
                        var d = (IOrderedQueryable<TSource>)e;
                        switch (sort.Role)
                        {
                            case GetAllTupleDto.SortRole.Ascending:
                                e = d.ThenBy(first.PropertyName).AsQueryable();
                                break;
                            case GetAllTupleDto.SortRole.Descending:
                                e = d.ThenByDescending(first.PropertyName).AsQueryable();
                                break;
                        }
                    }
                }

                if (dto.Skip != null && dto.Take != null)
                    e = e.Skip((dto.Skip.Value - 1) * dto.Take.Value).AsQueryable();

                if (dto.Take != null)
                    e = e.Take(dto.Take.Value).AsQueryable();
            }

            if (typeof(TSource) == typeof(TResult))
            {
                var d = (IQueryable<TResult>) e;
                result.List = await d.Cacheable().ToListAsync();
            }
            else
            {
                if (mapper != null)
                {
                    result.List = await e.ProjectTo<TResult>(mapper.ConfigurationProvider).Cacheable().ToListAsync();
                }
                else
                {
                    throw new Exception("اینترفیس آی مپر ارسال نشده است");
                }
            }

            return result;
        }
    }


}