﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> _logger;
        private readonly IMocks _mocks;

        public DefaultController(ILogger<DefaultController> logger, IMocks mocks)
        {
            _logger = logger;
            _mocks = mocks;
        }

        [HttpDelete]
        [HttpGet]
        [HttpHead]
        [HttpOptions]
        [HttpPatch]
        [HttpPost]
        [HttpPut]
        [Route("/{*.}")]
        public ActionResult Handle()
        {
            _logger.LogInformation(Request.Method);
            _logger.LogInformation(Request.Path);
            _logger.LogInformation(Request.Path + Request.QueryString.ToString());
            _logger.LogInformation(Request.QueryString.ToString());

            var r = _mocks.GetResponse(Request.Path + Request.QueryString.ToString(), Request.Method);
            
            foreach (var header in r.Headers.EnumerateArray())
            {
                var h = header.EnumerateObject();
                Response.Headers.Add(h.FirstOrDefault().Name, h.FirstOrDefault().Value.ToString());
            }

            var response = new ContentResult();
            response.Content = r.Content;
            response.StatusCode = r.StatusCode;

            return response;
        }

        [HttpGet]
        [Route("/mockserverconfig")]
        public ActionResult Config()
        {
            return Ok(_mocks.GetCollection());
        }
    }
}
