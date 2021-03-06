namespace WebLinking.Samples.SimpleApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Application;
    using Application.Data;
    using Integration.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IValueStore _valueStore;

        public ValuesController(
            IValueStore valueStore)
            => _valueStore = valueStore
                ?? throw new ArgumentNullException(nameof(valueStore));

        // GET api/values
        [HttpGet(Name = RouteNames.ValuesGetCollection)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(
            (int) HttpStatusCode.OK,
            Type = typeof(IEnumerable<ValueModel>))]
        public IActionResult Get(
            [FromQuery] GetValueCollectionParameters query)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var parameters = query ?? new GetValueCollectionParameters();
            var values = _valueStore.GetPagedCollection(
                parameters.Offset,
                parameters.Limit);

            return new PagedCollectionResult<ValueModel>(values);
        }

        // GET api/values/5
        [HttpGet(
            "{id}",
            Name = RouteNames.ValuesGetById)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType(
            (int) HttpStatusCode.OK,
            Type = typeof(ValueModel))]
        public ActionResult<ValueModel> Get(
            int id)
        {
            if (id >= 0) { return _valueStore.GetById(id); }

            ModelState.AddModelError(
                "id",
                "id must be a positive number");
            return BadRequest(ModelState);
        }
    }
}
