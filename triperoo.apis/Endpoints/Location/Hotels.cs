﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using core.hotels.dtos;
using core.hotels.services;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace triperoo.apis.endpoints.location
{
	#region Hotels By Location Id

	/// <summary>
	/// Request
	/// </summary>
    [Route("/v1/location/{id}/hotels", "GET")]
	public class HotelRequest
	{
		public int Id { get; set; }
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
        
	}

	/// <summary>
	/// Validator
	/// </summary>
	public class HotelRequestValidator : AbstractValidator<HotelRequest>
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public HotelRequestValidator()
		{
			// Get
			RuleSet(ApplyTo.Get, () =>
			{
				RuleFor(r => r.Id).NotNull().WithMessage("Supply a valid location id");
				RuleFor(r => r.PageSize).GreaterThan(0).WithMessage("Invalid page size has been supplied");
				RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(0).WithMessage("Invalid page number has been supplied");
			});
		}
	}

	#endregion

    #region Hotels By Location Proximity

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/location/hotels", "GET")]
    public class HotelProximityRequest
    {
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double Distance { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        
    }

    /// <summary>
    /// Validator
    /// </summary>
    public class HotelProximityRequestValidator : AbstractValidator<HotelProximityRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HotelProximityRequestValidator()
        {
            // Get
            RuleSet(ApplyTo.Get, () =>
            {
				RuleFor(r => r.Latitude).NotNull().WithMessage("Supply a valid location latitude");
				RuleFor(r => r.Longitude).NotNull().WithMessage("Supply a valid location longitude");
				RuleFor(r => r.Distance).NotNull().WithMessage("Supply a valid location distance");
                RuleFor(r => r.PageSize).GreaterThan(0).WithMessage("Invalid page size has been supplied");
                RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(0).WithMessage("Invalid page number has been supplied");
            });
        }
    }

    #endregion

	#region API logic

	public class HotelRequestApi : Service
	{
		private readonly IHotelService _hotelService;

		/// <summary>
		/// Constructor
		/// </summary>
		public HotelRequestApi(IHotelService hotelService)
		{
			_hotelService = hotelService;
		}

        #region Get Hotels By Location

        /// <summary>
        /// Get Hotels By location
        /// </summary>
        public object Get(HotelRequest request)
        {
            string cacheName = "hotels:" + request.Id;
            HotelListDto response;

            try
            {
                response = Cache.Get<HotelListDto>(cacheName);

                if (response == null)
                {
                    response = new HotelListDto();
                    response.HotelList = _hotelService.ReturnHotelsByPlaceId(request.Id);
                    response.HotelCount = response.HotelList.Count;
                }

                if (response.HotelCount > request.PageSize)
                {
                    response.HotelList = response.HotelList.Skip(request.PageSize * request.PageNumber).Take(request.PageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

		#endregion


        #region Get Hotels By Location Proximity

        /// <summary>
        /// Get Hotels By location
        /// </summary>
        public object Get(HotelProximityRequest request)
        {
            string cacheName = "hotels:" + request.Latitude + ":" + request.Longitude;
            HotelListDto response;

            try
            {
                response = Cache.Get<HotelListDto>(cacheName);

                if (response == null)
                {
                    response = new HotelListDto();
                    response.HotelList = _hotelService.ReturnHotelsByProximity(request.Longitude, request.Latitude, request.Distance);
                    response.HotelCount = response.HotelList.Count;
                }

                if (response.HotelCount > request.PageSize)
                {
                    response.HotelList = response.HotelList.Skip(request.PageSize * request.PageNumber).Take(request.PageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
            }

            return new HttpResult(response, HttpStatusCode.OK);
        }

        #endregion
	}

	#endregion
}
