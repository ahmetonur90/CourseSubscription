using AutoMapper;
using CourseSubscription.Business.Abstract;
using CourseSubscription.Business.BusinessFlow.Subscription;
using CourseSubscription.Core.DTOs;
using CourseSubscription.Core.Util;
using CourseSubscription.Core.Util.Result.ServiceException;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Subscription.API.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using static Subscription.API.Helpers.ObjectActionResult;

namespace Subscription.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "Subscription APIs")]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/subscription/v{version:apiversion}/[controller]")]
    public class OperationsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISubscriptionService _subscriptionService;
        private readonly ITrainingsService _trainingsService;
        private readonly ICoursesServices _coursesServices;
        private readonly IMapper _mapper;

        public OperationsController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ISubscriptionService subscriptionService,
            ITrainingsService trainingsService,
            ICoursesServices coursesServices,
            IMapper mapper
            )
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _subscriptionService = subscriptionService;
            _trainingsService = trainingsService;
            _coursesServices = coursesServices;
            _mapper = mapper;
        }

        [HttpGet("getusers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {

                var response = await new UserFlow(_configuration, _httpClientFactory).Execute(null);

                return new ObjectActionResult(
                      success: true,
                      statusCode: HttpStatusCode.OK,
                      message: new MsgObject()
                      {
                          Statu = Constants.Successful
                      },
                      data: response);

            }
            catch (InvalidDataException ex)
            {
                return new ObjectActionResult(
                    success: false,
                    statusCode: HttpStatusCode.BadRequest,
                    message:
                        new MsgObject
                        {
                            Header = "InvalidDataException",
                            Statu = Constants.Fail,
                            Content = ex.Message
                        }
                    );
            }
            catch (ServiceException ex)
            {
                return new ObjectActionResult(
                  success: false,
                  statusCode: (HttpStatusCode)ex.ErrorCode,
                  message:
                      new MsgObject
                      {
                          Header = ex.Message,
                          Statu = Constants.Fail,
                          Content = ex.ErrorContent
                      }
                  );
            }
            catch (Exception ex)
            {
                return new ObjectActionResult(
                    success: false,
                    statusCode: HttpStatusCode.BadGateway,
                    message: new MsgObject
                    {
                        Header = string.Empty,
                        Statu = Constants.Fail,
                        Content = string.Empty
                    });
            }
        }

        [HttpPost("createsubscription")]
        public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionRequestDto req)
        {
            try
            {
                if (!ModelState.IsValid)
                    return new ObjectActionResult(
                        success: false,
                        statusCode: HttpStatusCode.BadGateway,
                        message:
                            new MsgObject()
                            {
                                Header = Constants.InvalidRequest,
                                Statu = Constants.Fail,
                                Content = ModelState.Values.Where(x => x.Errors.Count > 0)
                                         .SelectMany(x => x.Errors)
                                         .Select(x => x.ErrorMessage)
                                         .ToList()
                            },
                        data: req);

                var response = await new SubscriptionInsertFlow(_mapper, _configuration, _httpClientFactory, _subscriptionService, _trainingsService, _coursesServices).Execute(req);


                return new ObjectActionResult(
                      success: true,
                      statusCode: HttpStatusCode.OK,
                      message: new MsgObject()
                      {
                          Statu = Constants.Successful
                      },
                      data: response);

            }
            catch (InvalidDataException ex)
            {
                return new ObjectActionResult(
                    success: false,
                    statusCode: HttpStatusCode.BadRequest,
                    message:
                        new MsgObject
                        {
                            Header = "InvalidDataException",
                            Statu = Constants.Fail,
                            Content = ex.Message
                        }
                    );
            }
            catch (ServiceException ex)
            {
                return new ObjectActionResult(
                  success: false,
                  statusCode: (HttpStatusCode)ex.ErrorCode,
                  message:
                      new MsgObject
                      {
                          Header = ex.Message,
                          Statu = Constants.Fail,
                          Content = ex.ErrorContent
                      }
                  );
            }
            catch (Exception ex)
            {
                return new ObjectActionResult(
                    success: false,
                    statusCode: HttpStatusCode.BadGateway,
                    message: new MsgObject
                    {
                        Header = string.Empty,
                        Statu = Constants.Fail,
                        Content = string.Empty
                    });
            }
        }

        [HttpGet("getsubscriptions")]
        public async Task<IActionResult> GetSubscriptions([FromQuery] SubscriptionFilterDto req)
        {
            try
            {
                var response = await new SubscriptionListFlow(_mapper, _configuration, _httpClientFactory, _subscriptionService, _trainingsService, _coursesServices).Execute(req);

                var meta = (PagedList<SubscriptionListDto>)response;
                var metadata = new
                {
                    meta.TotalCount,
                    meta.PageSize,
                    meta.CurrentPage,
                    meta.TotalPages,
                    meta.HasNext,
                    meta.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return new ObjectActionResult(
                      success: true,
                      statusCode: HttpStatusCode.OK,
                      message: new MsgObject()
                      {
                          Statu = Constants.Successful
                      },
                      data: response);

            }
            catch (InvalidDataException ex)
            {
                return new ObjectActionResult(
                    success: false,
                    statusCode: HttpStatusCode.BadRequest,
                    message:
                        new MsgObject
                        {
                            Header = "InvalidDataException",
                            Statu = Constants.Fail,
                            Content = ex.Message
                        }
                    );
            }
            catch (ServiceException ex)
            {
                return new ObjectActionResult(
                  success: false,
                  statusCode: (HttpStatusCode)ex.ErrorCode,
                  message:
                      new MsgObject
                      {
                          Header = ex.Message,
                          Statu = Constants.Fail,
                          Content = ex.ErrorContent
                      }
                  );
            }
            catch (Exception ex)
            {
                return new ObjectActionResult(
                    success: false,
                    statusCode: HttpStatusCode.BadGateway,
                    message: new MsgObject
                    {
                        Header = string.Empty,
                        Statu = Constants.Fail,
                        Content = string.Empty
                    });
            }
        }

        [HttpGet("getsubscriptiondetails")]
        public async Task<IActionResult> GetSubscriptionsDetails([FromQuery] SubscriptionDetailFilterDto req)
        {
            try
            {
                var response = await new SubscriptionsDetailsFlow(_mapper, _configuration, _httpClientFactory, _subscriptionService).Execute(req);

                var meta = (PagedList<SubscriptionDetailDto>)response;
                var metadata = new
                {
                    meta.TotalCount,
                    meta.PageSize,
                    meta.CurrentPage,
                    meta.TotalPages,
                    meta.HasNext,
                    meta.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return new ObjectActionResult(
                      success: true,
                      statusCode: HttpStatusCode.OK,
                      message: new MsgObject()
                      {
                          Statu = Constants.Successful
                      },
                      data: response);

            }
            catch (InvalidDataException ex)
            {
                return new ObjectActionResult(
                    success: false,
                    statusCode: HttpStatusCode.BadRequest,
                    message:
                        new MsgObject
                        {
                            Header = "InvalidDataException",
                            Statu = Constants.Fail,
                            Content = ex.Message
                        }
                    );
            }
            catch (ServiceException ex)
            {
                return new ObjectActionResult(
                  success: false,
                  statusCode: (HttpStatusCode)ex.ErrorCode,
                  message:
                      new MsgObject
                      {
                          Header = ex.Message,
                          Statu = Constants.Fail,
                          Content = ex.ErrorContent
                      }
                  );
            }
            catch (Exception ex)
            {
                return new ObjectActionResult(
                    success: false,
                    statusCode: HttpStatusCode.BadGateway,
                    message: new MsgObject
                    {
                        Header = string.Empty,
                        Statu = Constants.Fail,
                        Content = string.Empty
                    });
            }
        }
    }
}
