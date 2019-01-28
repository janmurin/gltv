using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLTV.Data;
using GLTV.Models;
using GLTV.Models.Objects;
using GLTV.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            TvItemFile itemFile = _tvItemService.FetchTvItemFileAsync(filename).Result;
            if (itemFile != null)
            {
                WebClientLog wcl = new WebClientLog()
                {
                    Source = sourceIp,
                    TimeInserted = DateTime.Now,
                    TvItemFileId = itemFile.ID,
                    TvScreenId = KnownTvScreens.FirstOrDefault(x => x.IpAddress.Equals(sourceIp))?.ID,
                    Type = itemFile.IsVideoFile() ? WebClientLogType.VideoRequest : WebClientLogType.ImageRequest
                };

                Context.Add(wcl);
                Context.SaveChanges();
                Console.WriteLine($"adding file request from ip {sourceIp} for file: " + wcl);
            }
            else
            {
                WebClientLog wcl = new WebClientLog()
                {
                    Source = sourceIp,
                    TimeInserted = DateTime.Now,
                    Message = $"file with filename [{filename}] not found!",
                    Type = WebClientLogType.Exception
                };

                Context.Add(wcl);
                Context.SaveChanges();
                Console.WriteLine("file not found on file request: " + wcl);
            }

            return Task.CompletedTask;
        }

        public Task AddHandShakeAsync(string sourceIp, WebClientLogType type, Location location)
        {
            if (!type.Equals(WebClientLogType.ProgramRequest) && !type.Equals(WebClientLogType.ChatRequest))
            {
                throw new InvalidOperationException($"{type.ToString()} is not allowed type. Allowed types: [ProgramRequest, ChatRequest]");
            }

            DateTime now = DateTime.Now;

            // find/add known screen according to ip and update lastHandshake
            TvScreen knownScreen = KnownTvScreens.FirstOrDefault(x => x.IpAddress.Equals(sourceIp));
            if (knownScreen == null)
            {
                knownScreen = new TvScreen()
                {
                    Location = location,
                    LastHandshake = now,
                    IpAddress = sourceIp,
                    Description = "located at " + location
                };
                Console.WriteLine("adding new known screen: " + knownScreen);
                Context.Add(knownScreen);
            }
            else
            {
                knownScreen.LastHandshake = now;
                if (knownScreen.Location != location)
                {
                    // very rare and improbable case
                    Console.WriteLine($"CHANGING LOCATION FOR {knownScreen.IpAddress} FROM {knownScreen.Location} TO {location}.");
                    knownScreen.Location = location;
                }
            }

            var activeScreens = Context.TvScreenHandshake
                .Where(x => x.IsActive)
                .ToList();

            // from active screens inactivate timeouted ones
            var timeoutedScreens = activeScreens.Where(x => (now - x.LastHandshake).TotalMinutes > 10).ToList();
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

        public Task<List<TvScreen>> FetchActiveScreensAsync()
        {
            List<TvScreenHandshake> handshakes = Context.TvScreenHandshake.ToList();
            List<TvScreen> screens = Context.TvScreen.ToList();
            List<TvScreen> activeScreens = new List<TvScreen>();
            DateTime days7Ago = DateTime.Now - TimeSpan.FromDays(7);

            screens.ForEach(screen =>
            {
                //Console.WriteLine($"screen: {screen}");
                // ignore inactive screens
                if ((DateTime.Now - screen.LastHandshake).TotalDays < 30)
                {
                    screen.ScreenHandshakes = handshakes
                        .Where(s => s.TvScreenId == screen.ID && s.Type == WebClientLogType.ProgramRequest)
                        .OrderByDescending(s => s.LastHandshake)
                        .ToList();

                    screen.TotalMinutesActive = (int)screen.ScreenHandshakes.Sum(s => (s.LastHandshake - s.FirstHandshake).TotalMinutes);

                    // ignore screens with low activity
                    //Console.WriteLine($"total minutes: {screen.TotalMinutesActive}, total handshakes: {screen.ScreenHandshakes.Count}");
                    if (screen.TotalMinutesActive > 24 * 60 || screen.ScreenHandshakes.Count > 10)
                    {
                        // calculate sum of minutes active for the last 7 days
                        List<TvScreenHandshake> lastWeekHandshakes = screen.ScreenHandshakes
                            .Where(x => DateTime.Compare(x.LastHandshake, days7Ago) > 0)
                            .ToList();
                        lastWeekHandshakes.ForEach(handshake =>
                        {
                            if (DateTime.Compare(handshake.FirstHandshake, days7Ago) > 0)
                            {
                                screen.TotalMinutesActiveLast7days += (int)(handshake.LastHandshake - handshake.FirstHandshake).TotalMinutes;
                            }
                            else
                            {
                                screen.TotalMinutesActiveLast7days += (int)(handshake.LastHandshake - days7Ago).TotalMinutes;
                            }
                        });

                        activeScreens.Add(screen);
                    }
                }
            });
            Console.WriteLine("active screen Counts: " + activeScreens.Count);

            // for each screen fetch last 200 client requests
            List<int> ids = activeScreens.Select(x => x.ID).ToList();
            List<WebClientLog> clientLogs = Context.WebClientLog
                .Include(x => x.TvItemFile)
                .Where(x => x.TvScreenId != null && ids.Contains((int)x.TvScreenId))
                .ToList();

            activeScreens.ForEach(screen =>
            {
                screen.LastActivity = clientLogs
                    .Where(x => x.TvScreenId == screen.ID)
                    .OrderByDescending(log => log.TimeInserted)
                    .Where(x => DateTime.Compare(x.TimeInserted, days7Ago) > 0)
                    .ToList();

                screen.TotalNetworkUsage7Days = screen.TotalMinutesActiveLast7days * 30 * 1024; // each program requests requires 30 KB approximately transfered

                if (screen.LastActivity.Count == 0)
                {
                    screen.LastActivity = clientLogs
                        .Where(x => x.TvScreenId == screen.ID)
                        .OrderByDescending(log => log.TimeInserted)
                        .Skip(0)
                        .Take(200)
                        .ToList();
                }
                else
                {
                    screen.TotalNetworkUsage7Days += screen.LastActivity.Sum(x => x.TvItemFile.Length);

                    // if more than 200 rows, pick only last 200
                    if (screen.LastActivity.Count > 200)
                    {
                        screen.LastActivity = screen.LastActivity
                            .Skip(0)
                            .Take(200)
                            .ToList();
                    }
                }
            });

            return Task.FromResult(activeScreens);

        }

        public Task MigrateData()
        {

            //Console.WriteLine("starting data migration");
            //List<ClientEvent> clientEvents = Context.ClientEvent.OrderByDescending(x => x.ID).ToList();
            //List<ClientEvent> chatEvents = clientEvents.Where(x => x.Type.Equals(ClientEventType.ChatRequest)).ToList();
            //List<ClientEvent> programEvents = clientEvents.Where(x => x.Type.Equals(ClientEventType.ProgramRequest)).ToList();

            //// 1. create tvScreens
            //List<TvScreen> screens = Context.TvScreen.ToList();
            //if (Context.TvScreen.ToList().Count > 0
            //    || Context.TvScreenHandshake.ToList().Count > 0
            //    || Context.WebClientLog.ToList().Count > 0
            //    || Context.WebServerLog.ToList().Count > 0)
            //{
            //    String message = $"Skipping Migration: one of the tables is not empty: TvScreen, TvScreenHandshake, WebClientLog, WebServerLog.";
            //    Console.WriteLine(message);
            //    return Task.FromException(new ArgumentException(message));
            //}

            //Console.WriteLine("\nAdding TvScreens from ProgramRequests");
            //HashSet<TvScreen> tvScreens = new HashSet<TvScreen>();
            //programEvents.OrderByDescending(x => x.ID).ToList().ForEach(pe =>
            //{

            //    Location location = Location.Kosice;
            //    if (pe.Message.LastIndexOf(" ", StringComparison.Ordinal) > 0)
            //    {
            //        var loc = "unknown";
            //        loc = pe.Message.Substring(pe.Message.LastIndexOf(" ", StringComparison.Ordinal)).Trim();
            //        location = (Location)Enum.Parse(typeof(Location), loc);
            //    }

            //    TvScreen s = new TvScreen()
            //    {
            //        Location = location,
            //        IpAddress = pe.Source,
            //        LastHandshake = pe.TimeInserted,
            //        Description = "located at " + location.ToString()
            //    };
            //    bool added = tvScreens.Add(s);
            //    if (added)
            //    {
            //        Console.WriteLine("new tvscreen " + tvScreens.Count + ". => " + s);
            //        Context.Add(s);
            //    }
            //});
            //Context.SaveChanges();


            ////// 2. create handshakes from program requests
            //List<TvScreenHandshake> programHandshakes = new List<TvScreenHandshake>();
            //Console.WriteLine($"\nAdding TvScreenHandshakes from ClientEvent.ProgramRequest. Known screens size: {KnownTvScreens.Count}");
            //KnownTvScreens.ForEach(screen =>
            //{
            //    Console.WriteLine("current screen: " + screen);
            //    List<ClientEvent> screenEvents = programEvents.Where(pe => pe.Source.Equals(screen.IpAddress)).OrderBy(x => x.ID).ToList();
            //    bool isActive = false;
            //    TvScreenHandshake activeScreen = null;

            //    screenEvents.ForEach(se =>
            //    {
            //        if (!isActive)
            //        {
            //            activeScreen = new TvScreenHandshake()
            //            {
            //                TvScreen = screen,
            //                FirstHandshake = se.TimeInserted,
            //                LastHandshake = se.TimeInserted,
            //                IsActive = true,
            //                Type = WebClientLogType.ProgramRequest
            //            };
            //            isActive = true;
            //        }
            //        else
            //        {
            //            if ((se.TimeInserted - activeScreen.LastHandshake).TotalMinutes < 10)
            //            {
            //                activeScreen.LastHandshake = se.TimeInserted;
            //            }
            //            else
            //            {
            //                activeScreen.IsActive = false;
            //                programHandshakes.Add(activeScreen);

            //                activeScreen = new TvScreenHandshake()
            //                {
            //                    TvScreen = screen,
            //                    FirstHandshake = se.TimeInserted,
            //                    LastHandshake = se.TimeInserted,
            //                    IsActive = true,
            //                    Type = WebClientLogType.ProgramRequest
            //                };
            //            }
            //        }
            //    });

            //    if (activeScreen != null)
            //    {
            //        activeScreen.IsActive = false;
            //        programHandshakes.Add(activeScreen);
            //    }

            //});
            //programHandshakes.ForEach(x =>
            //{
            //    Console.WriteLine("adding new active screen: " + x);
            //    Context.Add(x);
            //});

            //// 3. create handshakes from chat requests
            //List<TvScreenHandshake> chatHandshakes = new List<TvScreenHandshake>();
            //Console.WriteLine($"\nAdding TvScreenHandshakes from ClientEvent.ChatRequest. Known screens size: {KnownTvScreens.Count}");
            //KnownTvScreens.ForEach(screen =>
            //{
            //    Console.WriteLine("current screen: " + screen);
            //    List<ClientEvent> screenEvents = chatEvents.Where(pe => pe.Source.Equals(screen.IpAddress)).OrderBy(x => x.ID).ToList();
            //    bool isActive = false;
            //    TvScreenHandshake activeScreen = null;

            //    screenEvents.ForEach(se =>
            //    {
            //        if (!isActive)
            //        {
            //            activeScreen = new TvScreenHandshake()
            //            {
            //                TvScreen = screen,
            //                FirstHandshake = se.TimeInserted,
            //                LastHandshake = se.TimeInserted,
            //                IsActive = true,
            //                Type = WebClientLogType.ChatRequest
            //            };
            //            isActive = true;
            //        }
            //        else
            //        {
            //            if ((se.TimeInserted - activeScreen.LastHandshake).TotalMinutes < 10)
            //            {
            //                activeScreen.LastHandshake = se.TimeInserted;
            //            }
            //            else
            //            {
            //                activeScreen.IsActive = false;
            //                chatHandshakes.Add(activeScreen);

            //                activeScreen = new TvScreenHandshake()
            //                {
            //                    TvScreen = screen,
            //                    FirstHandshake = se.TimeInserted,
            //                    LastHandshake = se.TimeInserted,
            //                    IsActive = true,
            //                    Type = WebClientLogType.ChatRequest
            //                };
            //            }
            //        }
            //    });

            //    if (activeScreen != null)
            //    {
            //        activeScreen.IsActive = false;
            //        chatHandshakes.Add(activeScreen);
            //    }

            //});
            //chatHandshakes.ForEach(x =>
            //{
            //    Console.WriteLine("adding new active screen: " + x);
            //    Context.Add(x);
            //});

            //// 4. import file requests
            //Console.WriteLine("\nImporting file requests from ClientEvent table");
            //List<ClientEvent> fileRequestEvents = clientEvents
            //    .Where(x => x.Type.Equals(ClientEventType.ImageRequest) || x.Type.Equals(ClientEventType.VideoRequest))
            //    .ToList();
            //fileRequestEvents.ForEach(x =>
            //{
            //    WebClientLog wcl = new WebClientLog()
            //    {
            //        Message = x.Message,
            //        Source = x.Source,
            //        TimeInserted = x.TimeInserted,
            //        TvItemFileId = x.TvItemFileId,
            //        TvScreenId = KnownTvScreens.FirstOrDefault(y => y.IpAddress.Equals(x.Source))?.ID,
            //        Type = (int)x.Type == (int)WebClientLogType.VideoRequest ? WebClientLogType.VideoRequest : WebClientLogType.ImageRequest
            //    };
            //    Context.Add(wcl);
            //});

            //// 5. migrate log events
            //Console.WriteLine("\nMigrating LogEvent into WebServerLog");
            //List<LogEvent> logEvents = Context.LogEvent.ToList();
            //logEvents.ForEach(x =>
            //{
            //    WebServerLog wcl = new WebServerLog()
            //    {
            //        Type = (WebServerLogType)x.Type,
            //        TimeInserted = x.TimeInserted,
            //        Message = x.Message,
            //        Author = x.Author,
            //        TvItemId = x.TvItemId
            //    };
            //    Console.WriteLine("adding " + wcl);
            //    Context.Add(wcl);
            //});

            //Context.SaveChanges();

            //Console.WriteLine("Migration done successfully. Drop tables ClientEvent and LogEvent.");

            //Manual cleanup of WebClientLog table
            //select TvScreenId, Type, count(*) as Count from WebClientLog group by TvScreenId, Type order by Count desc

            //select* from WebClientLog where TvScreenId in (2, 4, 5) and TimeInserted<'2019-01-01 00:00:00.000' and Type in (2, 3)

            return Task.CompletedTask;
        }
    }
}
