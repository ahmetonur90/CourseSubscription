using AutoMapper;
using CourseSubscription.Business.Abstract;
using CourseSubscription.Business.BusinessFlow.Training;
using CourseSubscription.Core.DTOs;
using CourseSubscription.Core.Util;
using CourseSubscription.Core.Util.Result.ServiceException;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Training.API.Helpers;
using static Training.API.Helpers.ObjectActionResult;

namespace Training.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "Training APIs")]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/training/v{version:apiversion}/[controller]")]
    public class OperationsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITrainingsService _trainingsService;
        private readonly ICoursesServices _coursesServices;
        private readonly IMapper _mapper;

        public OperationsController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ITrainingsService trainingsService,
            ICoursesServices coursesServices,
            IMapper mapper
            )
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _trainingsService = trainingsService;
            _coursesServices = coursesServices;
            _mapper = mapper;
        }

        [HttpGet("gettrainings")]
        public async Task<IActionResult> GetTrainings([FromQuery] TrainingFilterDto req)
        {
            try
            {

                var response = await new TrainingListFlow().Execute(req);

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

        [HttpPost("createtraining")]
        public async Task<IActionResult> CreateTraining([FromBody] CreateTrainingRequestDto req)
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

                var response = await new TrainingInsertFlow(_mapper, _trainingsService, _coursesServices).Execute(req);

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

        [HttpPost("updatetraining")]
        public async Task<IActionResult> UpdateTraining([FromBody] UpdateTrainingRequestDto req)
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

                var response = await new TrainingUpdateFlow(_mapper, _trainingsService, _coursesServices).Execute(req);

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
