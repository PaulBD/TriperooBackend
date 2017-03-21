﻿using core.customers.dtos;
using core.customers.services;
using core.hotels.services;
using core.places.services;
using ServiceStack;
using System;
using System.Net;

namespace triperoo.apis.endpoints.review
{
    #region Review Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/review", "POST")]
    public class ReviewRequest
    {
        public string Guid { get; set; }
        public ReviewDetailDto Review { get; set; }
    }

    #endregion

    #region API logic

    public class ReviewApi : Service
    {
        private readonly IReviewService _reviewService;
        private readonly ICustomerService _customerService;
        private readonly ILocationService _locationService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReviewApi(IReviewService reviewService, ICustomerService customerService, ILocationService locationService)
        {
            _reviewService = reviewService;
            _customerService = customerService;
            _locationService = locationService;

        }

        #region Insert Review

        /// <summary>
        /// Insert Review
        /// </summary>
        public object Post(ReviewRequest request)
        {
            try
            {
                var token = Request.Headers["token"];

                if (string.IsNullOrEmpty(token))
                {
                    return new HttpResult("Token not found", HttpStatusCode.Unauthorized);
                }

                var customer = _customerService.ReturnCustomerByToken(token);

                if (customer == null)
                {
                    return new HttpResult("Customer not found" + token, HttpStatusCode.Unauthorized);
                }

                var location = _locationService.ReturnLocationById(request.Review.InventoryReference);

                if (location == null)
                {
                    return new HttpResult("Location not found" + token, HttpStatusCode.Unauthorized);
                }

                if (location != null)
                {
                    request.Review.Place.NameShort = location.RegionName;
                    request.Review.Place.Name = location.RegionNameLong;
                    request.Review.Place.Address = location.RegionNameLong.Replace(location.RegionName + ",", "").Trim();
                    request.Review.Place.ProfileUrl = location.Url;
                    request.Review.Place.ImageUrl = location.Image;
                    request.Review.Place.Type = location.RegionType;
                }

                var reference = "review:" + Guid.NewGuid();
                request.Review.ReviewReference = reference;
                request.Review.DateCreated = DateTime.Now;
                request.Review.CustomerReference = customer.TriperooCustomers.CustomerReference;

                _reviewService.InsertNewReview(reference, request.Review);

                location.ReviewCount += 1;
                location.AverageReviewScore = (location.AverageReviewScore + request.Review.StarRating) / location.ReviewCount;

                var key = "location:" + location.RegionID;
                _locationService.UpdateLocation(key, location);
                base.Cache.Add(key, location);
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(HttpStatusCode.OK);
        }

        #endregion
    }

    #endregion
}
