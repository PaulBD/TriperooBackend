﻿using core.customers.dtos;
using core.customers.services;
using core.hotels.services;
using core.places.services;
using ServiceStack;
using System;
using System.Net;

namespace triperoo.apis.endpoints.review
{
    #region Question Endpoint

    /// <summary>
    /// Request
    /// </summary>
    [Route("/v1/question", "POST")]
    public class QuestionRequest
    {
        public string Guid { get; set; }
        public QuestionDetailDto Question { get; set; }
    }

    #endregion

    #region API logic

    public class QuestionApi : Service
    {
        private readonly IQuestionService _questionService;
        private readonly ICustomerService _customerService;
        private readonly ILocationService _locationService;

        /// <summary>
        /// Constructor
        /// </summary>
        public QuestionApi(IQuestionService questionService, ICustomerService customerService, ILocationService locationService)
        {
            _questionService = questionService;
            _customerService = customerService;
            _locationService = locationService;

        }

        #region Insert Question

        /// <summary>
        /// Insert Question
        /// </summary>
        public object Post(QuestionRequest request)
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
                
                var reference = "question:" + Guid.NewGuid();
                request.Question.QuestionReference = reference;
                request.Question.InventoryReference = request.Question.InventoryReference;
                request.Question.DateCreated = DateTime.Now;
                request.Question.CustomerReference = customer.TriperooCustomers.CustomerReference;

                _questionService.InsertNewQuestion(reference, request.Question);
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
