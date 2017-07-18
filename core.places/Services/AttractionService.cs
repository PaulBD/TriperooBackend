﻿using core.places.dtos;
using library.couchbase;
using System.Collections.Generic;

namespace core.places.services
{
    public class AttractionService : IAttractionService
    {
        private CouchBaseHelper _couchbaseHelper;
        private readonly string _bucketName = "TriperooCommon";
        private string _query;

        public AttractionService()
        {
            _couchbaseHelper = new CouchBaseHelper();
			_query = "SELECT doctype, image, letterIndex, listingPriority, locationCoordinates.latitude as latitude, locationCoordinates.longitude as longitude, parentRegionID, parentRegionName, parentRegionNameLong, parentRegionType, regionID, regionName, regionNameLong, regionType, relativeSignificance, searchPriority, stats.averageReviewScore as averageReviewScore, stats.likeCount as likeCount, stats.reviewCount as reviewCount, subClass, url, formattedAddress, contactDetails, tags, photos, locationCoordinates, summary, stats FROM " + _bucketName;
		}

        /// <summary>
        /// Return attractions by location Id
        /// </summary>
        public List<LocationDto> ReturnAttractionsByParentId(int parentLocationId)
        {
            var q = _query + " WHERE parentRegionID = " + parentLocationId + " AND regionType = 'Point of Interest Shadow'";

            return ProcessQuery(q);
		}

        /// <summary>
        /// Return attractions by location Id and category
        /// </summary>
        public List<LocationDto> ReturnAttractionsByParentIdAndCategory(int parentLocationId, string category)
        {
            var q = _query + " WHERE parentRegionID = " + parentLocationId + " AND regionType = 'Point of Interest Shadow' AND subClass = '" + category + "'";

            return ProcessQuery(q);
        }

        /// <summary>
        /// Process Query
        /// </summary>
        private List<LocationDto> ProcessQuery(string q)
        {
            var result = _couchbaseHelper.ReturnQuery<LocationDto>(q, _bucketName);

            if (result.Count > 0)
            {
                return result;
            }

            return null;
        }
    }
}
