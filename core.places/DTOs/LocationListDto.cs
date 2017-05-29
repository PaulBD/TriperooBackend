﻿using System.Collections.Generic;

namespace core.places.dtos
{

	public class SourceData
	{
		public string ForesquareId { get; set; }
	}

	public class ContactDetails
	{
		public string phone { get; set; }
		public string formattedPhone { get; set; }
		public object twitter { get; set; }
		public object facebook { get; set; }
		public object facebookUsername { get; set; }
		public object facebookName { get; set; }
		public object instagram { get; set; }
	}

	public class PhotoList
	{
		public string prefix { get; set; }
		public string suffix { get; set; }
		public int width { get; set; }
		public int height { get; set; }
	}

	public class Photos
	{
		public int photoCount { get; set; }
		public List<PhotoList> photoList { get; set; }
	}

	public class LocationCoordinatesDto
	{
		public double Latitude { get; set; }
		public double Longitude { get; set; }
	}

	public class SummaryDto
	{
		public string En { get; set; }
		public string Fr { get; set; }
		public string De { get; set; }
		public string Es { get; set; }
		public string It { get; set; }
	}

	public class StatsDto
	{
		public int LikeCount { get; set; }
		public int ReviewCount { get; set; }
		public double AverageReviewScore { get; set; }
	}

	public class LocationDto
    {
        public LocationDto()
        {
            SourceData = new SourceData();
            FormattedAddress = new List<string>();
            Tags = new List<string>();
            ContactDetails = new ContactDetails();
            Photos = new Photos();
            Summary = new SummaryDto();
            Stats = new StatsDto();
            LocationCoordinates = new LocationCoordinatesDto();
        }

        public int AverageReviewScore { get; set; }
        public string Doctype { get; set; }
        public string Image { get; set; }
        public double Latitude { get; set; }
        public string LetterIndex { get; set; }
        public int LikeCount { get; set; }
        public int ListingPriority { get; set; }
        public double Longitude { get; set; }
        public int ParentRegionID { get; set; }
        public string ParentRegionName { get; set; }
        public string ParentRegionNameLong { get; set; }
        public string ParentRegionType { get; set; }
        public int RegionID { get; set; }
        public string RegionName { get; set; }
        public string RegionNameLong { get; set; }
        public string RegionType { get; set; }
        public string RelativeSignificance { get; set; }
        public int ReviewCount { get; set; }
        public int SearchPriority { get; set; }
        public string SubClass { get; set; }
        public SummaryDto Summary { get; set; }
        public string Url { get; set; }
        public string ParentUrl
        {
            get
            {
                return "/" + ParentRegionID + "/visit/" + ParentRegionNameLong.Replace(",", "").Replace(" ", "-");
            }
        }

        public SourceData SourceData { get; set; }
		public List<string> FormattedAddress { get; set; }
		public List<string> Tags { get; set; }
		public ContactDetails ContactDetails { get; set; }
		public Photos Photos { get; set; }
		public StatsDto Stats { get; set; }
		public LocationCoordinatesDto LocationCoordinates { get; set; }
	}

    public class LocationListDto
    {
        public LocationListDto()
        {
            Locations = new List<LocationDto>();
        }

        public IList<LocationDto> Locations { get; set; }
		public int LocationCount { get; set; }
    }
}
