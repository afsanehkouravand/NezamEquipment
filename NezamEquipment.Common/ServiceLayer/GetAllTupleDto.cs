using System.Collections.Generic;

namespace NezamEquipment.Common.ServiceLayer
{
    public class GetAllTupleDto
    {
        public GetAllTupleDto()
        {
            DoCount = true;
        }

        public static GetAllTupleDto Get(int? skip = null, int? take = null, string sortPropertyName = null, SortRole? sortRole = null,
            bool? doCount = true)
        {
            var dto = new GetAllTupleDto
            {
                Skip = skip,
                Take = take,
                ToSort = new Sort
                {
                    PropertyName = !string.IsNullOrWhiteSpace(sortPropertyName) ? sortPropertyName : "Id",
                    Role = sortRole ?? SortRole.Ascending,
                },
                DoCount = doCount ?? true,
            };

            return dto;
        }

        public bool DoCount { get; set; }

        public int? Skip { get; set; }
        public int? Take { get; set; }

        public bool IsSortComplex { get; set; }
        public Sort ToSort { get; set; }
        public List<Sort> ToSorts { get; set; }
        public class Sort
        {
            public string PropertyName { get; set; }
            public SortRole Role { get; set; }
        }
        public enum SortRole
        {
            Ascending,
            Descending,
        }
    }
}