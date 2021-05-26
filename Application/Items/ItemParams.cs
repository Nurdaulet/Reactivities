using System;
using System.ComponentModel.DataAnnotations;
using Application.Core;

namespace Application.Items
{
    public class ItemParams: PagingParams
    {
        private const string DecimalMaxValue = "79228162514264337593543950335";

        public string Title { get; set; }

        public string UserId { get; set; }

        [Range(typeof(decimal), "0.01", DecimalMaxValue)]
        public decimal? MinPrice { get; set; }

        [Range(typeof(decimal), "0.01", DecimalMaxValue)]
        public decimal? MaxPrice { get; set; }

        public bool? GetLiveItems { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [Range(1, int.MaxValue)]
        public int? MinimumPicturesCount { get; set; }

        public Guid SubCategoryId { get; set; }
    }
}