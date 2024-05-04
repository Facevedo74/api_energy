using System;
using Microsoft.AspNetCore.Mvc;
using prueba_redarbor.Service;

namespace prueba_redarbor.Controllers
{
    [Route("api/redarbor")]
    public class UserController : Controller
    {
        private IUserService userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this.userService = userService;
            this._logger = logger;
        }

        /// <summary>
        /// Gets the state of the helper.
        /// </summary>
        /// <remarks>
        /// This method returns the state of the helper.
        /// </remarks>
        /// <returns>1 if exist elements</returns>
        [HttpGet("HelperStatus")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult HelperStatus()
        {
            try
            {
                return Ok(userService.HelperStatus());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}

