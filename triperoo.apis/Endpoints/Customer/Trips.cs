﻿using core.customers.dtos;
using core.customers.services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Net;

namespace triperoo.apis.endpoints.customer
{
	#region Trip Endpoint

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/customer/trip/{tripId}", "GET")]
	[Route("/v1/customer/trip/{tripId}", "DELETE")]
	[Route("/v1/customer/trip/", "POST")]
	public class TripRequest
	{
		public int TripId { get; set; }
		public TripDto Trip { get; set; }
	}

	#endregion

	#region Favourites Endpoint

	/// <summary>
	/// Request
	/// </summary>
	[Route("/v1/customer/trips", "GET")]
	public class CustomerTripsRequest
	{
	}

	#endregion

	#region API logic

	public class TripApi : Service
	{
		private readonly ITripService _tripService;

		/// <summary>
		/// Constructor
		/// </summary>
		public TripApi(ITripService tripService)
		{
			_tripService = tripService;
		}

		#region Return Customer Trips By Token

		/// <summary>
		/// Return Customer Trips By Token
		/// </summary>
		public object Get(CustomerTripsRequest request)
		{
			var response = new List<TripDto>();

			try
			{
				var token = Request.Headers.Get("token");

				if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
				}

				response = _tripService.ReturnTripsByToken(token);

				if (response == null)
				{
					throw HttpError.NotFound("Customer details cannot be found");
				}
			}
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(response, HttpStatusCode.OK);
		}

		#endregion

		#region Return Trip By Id

		/// <summary>
		/// Return Trip By Id
		/// </summary>
		public object Get(TripRequest request)
		{
			var response = new TripDto();

			try
			{
				var token = Request.Headers["token"];

				if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
				}

				response = _tripService.ReturnTripById(request.TripId, token);

				if (response == null)
				{
					throw HttpError.NotFound("Customer details cannot be found");
				}
			}
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(response, HttpStatusCode.OK);
		}

		#endregion

		#region Insert Trip

		/// <summary>
		/// Insert New Trip
		/// </summary>
		public object Post(TripRequest request)
		{
            var tripId = 0;
			try
			{
				var token = Request.Headers["token"];

				if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
				}

				tripId = _tripService.InsertNewTrip(token, request.Trip);
			}
			catch (Exception ex)
			{
				throw new HttpError(ex.ToStatusCode(), "Error", ex.Message);
			}

			return new HttpResult(tripId, HttpStatusCode.OK);
		}

		#endregion

		#region Archive Existing Trip

		/// <summary>
		/// Archive Existing Trip
		/// </summary>
		public object Delete(TripRequest request)
		{
			try
			{
				var token = Request.Headers["token"];

				if (token == null)
				{
					throw HttpError.Unauthorized("You are unauthorized to access this page");
				}

				_tripService.ArchiveExistingTrip(request.TripId, token);
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
