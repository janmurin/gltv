using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Models;
using GLTV.Models.Objects;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace GLTV.Services
{
    public class EventService : ServiceBase, IEventService
    {
        private readonly ITvItemService _tvItemService;

        public EventService(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, ITvItemService tvItemService)
            : base(context, signInManager)
        {
            _tvItemService = tvItemService;
        }

        public Task AddWebServerLogAsync(string author, WebServerLogType type, string message, int? itemId)
        {
            WebServerLog webServerLog = new WebServerLog();
            webServerLog.Author = author;
            webServerLog.Message = message;
            webServerLog.TvItemId = itemId;
            webServerLog.TimeInserted = DateTime.Now;
            webServerLog.Type = type;

            Context.Add(webServerLog);
            Context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<List<WebServerLog>> FetchWebServerActivitiesAsync()
        {
            List<WebServerLog> events = Context.WebServerLog
                .Include(x => x.TvItem)
                .OrderByDescending(x => x.TimeInserted)
                .ToList();

            foreach (WebServerLog logEvent in events)
            {
                if (logEvent.Type == WebServerLogType.ItemDelete)
                {
                    logEvent.Message = $"User deleted item [{logEvent.TvItem.GetDetailHyperlink(false)}] with id [{logEvent.TvItemId}].";
                }

                if (logEvent.Type == WebServerLogType.ItemUpdate)
                {
                    logEvent.Message = $"User updated item [{logEvent.TvItem.GetDetailHyperlink(false)}] with id [{logEvent.TvItemId}].";
                }

                if (logEvent.Type == WebServerLogType.ItemDeleteFiles)
                {
                    logEvent.Message = $"User deleted files for item [{logEvent.TvItem.GetDetailHyperlink(false)}] with id [{logEvent.TvItemId}].";
                }

                if (logEvent.Type == WebServerLogType.ItemInsert)
                {
                    logEvent.Message = $"User inserted new item [{logEvent.TvItem.GetDetailHyperlink(false)}] with id [{logEvent.TvItemId}].";
                }

                if (logEvent.Type == WebServerLogType.AnonymousDetails)
                {
                    logEvent.Message = $"Displayed anonymous details for item [{logEvent.TvItem.GetDetailHyperlink(false)}] with id [{logEvent.TvItemId}].";
                }
            }

            return Task.FromResult(events);
        }

        public Task AddFileRequestEventAsync(string sourceIp, string filename)
        {
            //TvItemFile itemFile = _tvItemService.FetchTvItemFileAsync(filename).Result;
            //if (itemFile != null)
            //{
            //    AddClientEventAsync(
            //        sourceIp,
            //        itemFile.IsVideoFile() ? WebClientLogType.VideoRequest : WebClientLogType.ImageRequest,
            //        "",
            //        itemFile.ID, );
            //}

            return Task.CompletedTask;
        }

        public Task AddHandShakeAsync(string sourceIp, WebClientLogType type, Location location)
        {
            if (!type.Equals(WebClientLogType.ProgramRequest) && !type.Equals(WebClientLogType.ChatRequest))
            {
                throw new InvalidOperationException($"{type.ToString()} is not allowed type. Allowed types: [ProgramRequest, ChatRequest]");
            }

            DateTime now = DateTime.Now;

            // find/add known screen according to ip and location and update lastHandshake
            TvScreen knownScreen = KnownTvScreens.FirstOrDefault(x => x.Location.Equals(location) && x.IpAddress.Equals(sourceIp));
            if (knownScreen == null)
            {
                knownScreen = new TvScreen()
                {
                    Location = location,
                    LastHandshake = now,
                    IpAddress = sourceIp
                };
                Console.WriteLine("adding new known screen: " + knownScreen);
                Context.Add(knownScreen);
            }
            else
            {
                knownScreen.LastHandshake = now;
            }

            var activeScreens = Context.TvScreenHandshake
                .Where(x => x.IsActive)
                .ToList();

            // from active screens inactivate timeouted ones
            var timeoutedScreens = activeScreens.Where(x => (now - x.LastHandshake).TotalMinutes > 5).ToList();
            var timeoutIds = timeoutedScreens.Select(x => x.ID).ToList();
            activeScreens = activeScreens.Where(x => !timeoutIds.Contains(x.ID)).ToList();

            if (timeoutedScreens.Count > 0)
            {
                // inactivate all timeouted screens
                Console.WriteLine("inactivating clientHandshakes with ids: " + string.Join(",", timeoutIds));
                timeoutedScreens.ForEach(x =>
                {
                    x.IsActive = false;
                    Context.Update(x);
                });
            }

            // from active screens find the one with the same ip and location, if not present, create new active screen
            TvScreenHandshake tvScreenHandshake = activeScreens.FirstOrDefault(x => x.TvScreen.Equals(knownScreen) && x.Type.Equals(type));
            if (tvScreenHandshake == null)
            {
                TvScreenHandshake tsh = new TvScreenHandshake()
                {
                    FirstHandshake = now,
                    LastHandshake = now,
                    IsActive = true,
                    TvScreen = knownScreen,
                    Type = type
                };

                Console.WriteLine("adding new active screen: " + tsh);
                Context.Add(tsh);
            }
            else
            {
                tvScreenHandshake.LastHandshake = now;
                Context.Update(tvScreenHandshake);
            }

            Context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<List<WebClientLog>> FetchWebClientLogsAsync()
        {
            List<WebClientLog> events = Context.WebClientLog
                .Include(x => x.TvItemFile)
                .Where(x => x.Type != WebClientLogType.ProgramRequest)
                .OrderByDescending(x => x.TimeInserted)
                .Skip(0)
                .Take(200)
                .ToList();

            foreach (WebClientLog logEvent in events)
            {
                if (logEvent.Type == WebClientLogType.ImageRequest || logEvent.Type == WebClientLogType.VideoRequest)
                {
                    logEvent.Message = $"Request for file {logEvent.TvItemFile.GetDetailHyperlink()}";
                }
            }

            return Task.FromResult(events);
        }

        public Task<List<WebClientLog>> FetchClientsLastProgramRequestAsync()
        {
            List<WebClientLog> events = Context.WebClientLog
                .Include(x => x.TvItemFile)
                .Where(x => x.Type == WebClientLogType.ProgramRequest)
                .OrderByDescending(x => x.TimeInserted)
                .Skip(0)
                .Take(20000)
                .ToList();

            IEnumerable<string> sources = events.Select(x => x.Source).ToList().Distinct();
            List<WebClientLog> requestEvents = new List<WebClientLog>();

            foreach (string source in sources)
            {
                WebClientLog webClientLog = events.First(x => x.Source.Equals(source));
                var location = "unknown";
                if (webClientLog.Message.LastIndexOf(" ", StringComparison.Ordinal) > 0)
                {
                    location = webClientLog.Message.Substring(
                        webClientLog.Message.LastIndexOf(" ", StringComparison.Ordinal)).Trim();
                }
                webClientLog.Source += " (" + location + ")";
                requestEvents.Add(webClientLog);
            }


            return Task.FromResult(requestEvents);

        }
    }
}
