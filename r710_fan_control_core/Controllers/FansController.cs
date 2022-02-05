using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using r710_fan_control_core.Services;
using System.Threading.Tasks;

namespace r710_fan_control_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FansController : ControllerBase
    {
        private readonly IModeService _modeService;

        public FansController(IModeService modeService)
        {
            _modeService = modeService;
        }

        [Route("auto")]
        [HttpGet]
        public void Auto()
        {
            _modeService.Auto();
        }

        [Route("autolow")]
        [HttpGet]
        public void AutoLow()
        {
            _modeService.AutoLow();
        }

        [Route("manual")]
        [HttpGet]
        public void Manual(int speedPercent)
        {
            _modeService.Manual(speedPercent);
        }
    }
}
