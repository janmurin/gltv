using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Extensions;
using GLTV.Models;
using GLTV.Models.Objects;
using GLTV.Models.ViewModels;
using GLTV.Services;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GLTV.Controllers
{
    [Authorize]
    [Route("[controller]/[action]/{id?}")]
    public class UtilityController : Controller
    {
        private readonly IFileService _fileService;
        private readonly ITvItemService _tvItemService;
        private readonly IEmailSender _emailSender;
        private readonly IEventService _eventService;

        public UtilityController(IFileService fileService, ITvItemService tvItemService, IEmailSender emailSender, IEventService eventService)
        {
            _fileService = fileService;
            _tvItemService = tvItemService;
            _emailSender = emailSender;
            _eventService = eventService;
        }

        public async Task<IActionResult> ActiveScreens()
        {
            List<TvScreen> activeTvScreens = await _eventService.FetchActiveScreensAsync();

            ClientEventsViewModel model = new ClientEventsViewModel();
            model.ClientEvents = new List<WebClientLog>(); // empty list so far
            model.ActiveTvScreens = activeTvScreens;

            return View(model);
        }

        public async Task<IActionResult> ServerLog()
        {
            List<WebServerLog> tvItems = await _eventService.FetchWebServerActivitiesAsync();

            return View(tvItems);
        }

        public async Task<IActionResult> DeletedItems()
        {
            DeletedViewModel model = new DeletedViewModel();
            model.TvItems = await _tvItemService.FetchTvItemsAsync(true);
            model.ZombieFiles = await _fileService.FindZombieFilesAsync();
            model.TotalUndeletedFileSize = model.TvItems.Sum(x => Utils.GetTotalFileSizeLong(x));

            return View(model);
        }

        [HttpPost, ActionName("DeleteFiles")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFiles(int id)
        {
            TvItem item = await _tvItemService.FetchTvItemAsync(id);

            bool success = await _fileService.DeleteFilesAsync(item.Files);
            if (success)
            {
                await _eventService.AddWebServerLogAsync(User.Identity.Name, WebServerLogType.ItemDeleteFiles, "", id);
            }

            return RedirectToAction(nameof(DeletedItems));
        }

        [HttpPost, ActionName("DeleteZombieFile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteZombieFile([FromForm]string fileName)
        {
            bool success = await _fileService.DeleteZombieFileAsync(fileName);
            if (success)
            {
                await _eventService.AddWebServerLogAsync(User.Identity.Name, WebServerLogType.ItemDeleteZombieFile, $"User deleted zombie file[{fileName}].", null);
            }
            else
            {
                // if file deletion is not successful, it will stay in the file system as a zombie file
                await _eventService.AddWebServerLogAsync(User.Identity.Name, WebServerLogType.Exception, $"User NOT SUCCESSFULLY deleted zombie file[{fileName}].", null);
            }

            return RedirectToAction(nameof(DeletedItems));
        }
    }
}