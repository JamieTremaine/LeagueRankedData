using MingweiSamuel.Camille;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LeagueTierLevels
{
    class Api
    {    
        private long m_TotalCalls = 0;
        private RiotApi m_ApiInstance;
        private readonly object m_ApiLock = new object();

        public ApiRateLimiter rateLimiter { get; private set; }
        public static Api ApiInstance { get; private set; }
        public ApiType keyType { get; private set; }

        public Api(ApiType apiType)
        {
            this.m_ApiInstance = RiotApi.NewInstance(ApiInit.API_KEY);
            this.rateLimiter = new ApiRateLimiter(apiType);
            this.keyType = apiType;
            ApiInstance = this;
        }

        public async Task<(Tier, bool)> GetTierOfPlayers(MingweiSamuel.Camille.Enums.Region region, string tierName)
        {
            Console.WriteLine("Getting tier of players - Tier:{0}", tierName);

            Division[] divisionArray = new Division[DivisionInfo.TOTALDIVISIONS];

            for (int i = DivisionInfo.TOTALDIVISIONS - 1; i >= 0; i--)
            {
                var divisionNumber = (MingweiSamuel.Camille.Enums.Division)Enum.ToObject(typeof(MingweiSamuel.Camille.Enums.Division), i + 1);

                try
                {
                    divisionArray[i] = await GetDivisionOfPlayers(region, tierName, divisionNumber); //Want to get Division linearly
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    return (null, false);
                }

                string consoleOutput = string.Format("{0} {1} completed", tierName, i);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(consoleOutput);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Tier tier = new Tier(tierName, divisionArray);

            return (tier, true);
        }

        public async Task<Division> GetDivisionOfPlayers(MingweiSamuel.Camille.Enums.Region region, string tier, MingweiSamuel.Camille.Enums.Division divisionName)
        {
            var listOfLeagueEntryTask = new List<Task<MingweiSamuel.Camille.LeagueV4.LeagueEntry[]>>();
            var listOfSummonerTasks = new List<Task<Summoner>>();

            Console.WriteLine("Getting summoners in division: {0}", divisionName.ToString());

            int numPages = await FindNumPagesInDivision(region, tier, divisionName);


            //get everyone in division
            for (int i = 1; i <= numPages; i++)
            {
                ApiArgs args = new ApiArgs(Region:region, Tier:tier, DivisionName:divisionName, Queue: Queues.RANKED_SOLO_5x5, Page:i);

                try
                {
                    listOfLeagueEntryTask.Add(CallApi<MingweiSamuel.Camille.LeagueV4.LeagueEntry[]>(ApiCallType.LEAGUE_ENTRY, args));
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR GETTING SUMMONERS IN DIVISION");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    throw;
                }
            }

            List<Summoner> tempSummonerArray = new List<Summoner>();
            foreach (var arrayOfPlayers in await Task.WhenAll(listOfLeagueEntryTask))
            {
                //get detailed information for everyone in the division
                foreach (var player in arrayOfPlayers)
                {
                    Summoner summoner = new Summoner(player.SummonerId);
                    summoner.AddLeagueEntryData(player);

                    tempSummonerArray.Add(summoner);
                    listOfSummonerTasks.Add(GetSummonerData(summoner));
                }
            }

            Summoner[] summonersInDivision = await Task.WhenAll(listOfSummonerTasks);

            Division division = new Division(divisionName, summonersInDivision);

            return division;
        }


        public void ChangeKeyType(ApiType apiType)
        {
            this.rateLimiter = new ApiRateLimiter(apiType);
        }

        private async Task<int> FindNumPagesInDivision(MingweiSamuel.Camille.Enums.Region region, string tier, MingweiSamuel.Camille.Enums.Division division)
        {
            Console.WriteLine("finding top page for division");

            bool Found = false;

            double page = 1000; //1000 seems to be an okay start point. Anything plat+ will be lower than 1000 silver/gold tends to be above. Extreme low ranks act like high ranks
            double Top = 3500;  //most populated rank as of 08/12/21 is g4 with 2742 pages
            double low = 0;

            while (!Found)
            {
                ApiArgs args = new ApiArgs(Region:region, Tier:tier, DivisionName:division, Queue: Queues.RANKED_SOLO_5x5, Page:(int)page);

                MingweiSamuel.Camille.LeagueV4.LeagueEntry[] result;

                try
                {
                    Console.WriteLine("Searching page {0}", page.ToString());
                     result = await CallApi<MingweiSamuel.Camille.LeagueV4.LeagueEntry[]>(ApiCallType.LEAGUE_ENTRY, args);
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR FINDING TOP PAGE");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    throw;
                }
                
                if (result.Length == 0) //too high
                {
                    Top = page;
                    page = Math.Round((page + low) / 2, MidpointRounding.AwayFromZero);
                }
                else if (result.Length < 205) //is top page
                {
                    Found = true;
                }
                else if (result.Length == 205) //too low
                {
                    if (Top == page + 1) //very rare case where the top page is actually a full page
                    {
                        Found = true;
                        break; //break early so page isnt updated
                    }

                    low = page;
                    page = Math.Round((Top + page) / 2, MidpointRounding.AwayFromZero);
                }
            }

            Console.WriteLine("Found top page: {0}", page.ToString());

            return Convert.ToInt32(page);
        }

        private async Task<Summoner> GetSummonerData(Summoner summoner)
        {
            ApiArgs args = new ApiArgs(Region: Regions.EUW, ID: summoner.SummonerId);
            MingweiSamuel.Camille.SummonerV4.Summoner result;
            try
            {
                result = await CallApi<MingweiSamuel.Camille.SummonerV4.Summoner>(ApiCallType.SUMMONER, args);
            }
            catch 
            {

                Console.WriteLine("Could not get summoner data for: {0}", summoner.SummonerId);
                throw; 
            }        

            summoner.AddSummonerData(result);

            return summoner;
        }
       

        private async Task<T> CallApi<T>(ApiCallType callType, ApiArgs args)
        {
            rateLimiter.TakeSemaphore(); //wait until timer gives access
                                     //blocks calling api too fast to avoid hitting the rate limit

            m_TotalCalls++;

            Console.Write("API has been called {0} times... ", m_TotalCalls.ToString());
            T output;
            try
            {
                switch (callType)
                {
                    case ApiCallType.LEAGUE_ENTRY:
                        output = await (Task<T>)Convert.ChangeType(LeagueEntry(args), typeof(Task<T>));
                        break;
                    case ApiCallType.SUMMONER:
                        output = await (Task<T>)Convert.ChangeType(GetSummoner(args), typeof(Task<T>));
                        break;
                    default:
                        output = default(T);
                        break;
                }
                return output;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("IN CALLAPI CATCH");
                Console.ForegroundColor = ConsoleColor.Gray;
                throw; 
            }

            finally
            {
                rateLimiter.ApiCalled(); //will release semaphore on Elapsed event
            }                   
        }

        private async Task<MingweiSamuel.Camille.LeagueV4.LeagueEntry[]> LeagueEntry(ApiArgs args)
        {

            MingweiSamuel.Camille.LeagueV4.LeagueEntry[] result = default;
            try
            {
                result = await m_ApiInstance.LeagueV4.GetLeagueEntriesAsync(args.Region, args.DivisionName.ToString(), args.Tier, args.Queue, args.Page);
            }
            catch (Exception ex)
            {
                ApiErrorHandler errorHandle = new ApiErrorHandler();

                bool succesfulllyHandled = errorHandle.Handle(ex);
                if (succesfulllyHandled)
                {
                    result = await LeagueEntry(args);
                }
                else
                {
                    throw new ApiFailedExeception();
                }
            }

            Console.WriteLine("Got {0} summoners", result.Length.ToString());                      
            return result;

        }

        private async Task<MingweiSamuel.Camille.SummonerV4.Summoner> GetSummoner(ApiArgs args)
        {
            MingweiSamuel.Camille.SummonerV4.Summoner result = default;               

            try
            {
                result = await m_ApiInstance.SummonerV4.GetBySummonerIdAsync(args.Region, args.ID);
                Console.WriteLine("Got a summoners data");
            }
            catch (Exception ex)
            {
                ApiErrorHandler errorHandle = new ApiErrorHandler();

                bool succesfulllyHandled = errorHandle.Handle(ex);
                if (succesfulllyHandled)
                {                                       
                    result = await GetSummoner(args);
                }
                else
                {
                    throw new ApiFailedExeception();
                }
            }
            return result;
        }

    }    
}
